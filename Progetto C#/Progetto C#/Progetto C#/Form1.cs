using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Progetto_C_
{
    public partial class Form1 : Form
    {
        private List<string> recipes;
        // Last generated data (populated when user clicks "Genera lista della spesa")
        private string? lastName;
        private List<string>? lastIngredients;
        private List<string>? lastPantryItems;
        private List<string>? lastMissing;
        private string? lastResultText;

        public Form1()
        {
            InitializeComponent();
            InitializeData();

            // populate the recipes listbox created in the designer
            lbRecipes.Items.Clear();
            foreach (var r in recipes)
            {
                lbRecipes.Items.Add(ExtractRecipeName(r));
            }
            if (lbRecipes.Items.Count > 0) lbRecipes.SelectedIndex = 0;
        }

        private void InitializeData()
        {
            // Ten example recipes following the specified format
            recipes = new List<string>
            {
                "Torta Al Cioccolato e Panna. Ingredienti: cioccolato, farina, uova, zucchero, panna. Preparazione: sciogliere il cioccolato, mescolare gli ingredienti, cuocere in forno.",
                "Tiramisù. Ingredienti: zucchero, uova, biscotti, mascarpone, caffè. Preparazione: unire le uova con lo zucchero, aggiungere il mascarpone, bagnare i biscotti nel caffè e stratificare.",
                "Bavarese alle Fragole. Ingredienti: fragole, panna, zucchero, gelatina, latte. Preparazione: frullare le fragole, montare la panna, unire e rassodare in frigorifero.",
                "Croissant. Ingredienti: farina, lievito, burro, zucchero, latte. Preparazione: impastare, laminare con il burro, formare i cornetti e cuocere.",
                "Crostata alla Marmellata. Ingredienti: farina, burro, zucchero, uova, marmellata. Preparazione: preparare la frolla, stenderla, aggiungere la marmellata e cuocere.",
                "Cheesecake. Ingredienti: biscotti, burro, formaggio fresco, zucchero, uova. Preparazione: sbriciolare i biscotti, mescolare con il burro, preparare la crema e cuocere o raffreddare.",
                "Millefoglie. Ingredienti: pasta sfoglia, panna, zucchero, vaniglia. Preparazione: stendere la sfoglia, cuocere e assemblare con la crema o panna montata.",
                "Profiteroles. Ingredienti: farina, uova, burro, acqua, panna, cioccolato. Preparazione: preparare la pasta choux, formare i bignè, farcire e glassare.",
                "Panettone. Ingredienti: farina, lievito, uova, burro, zucchero, uvetta. Preparazione: impastare a lungo, far lievitare, inserire l'uvetta e cuocere.",
                "Tartufo al Cioccolato. Ingredienti: cioccolato, panna, burro, cacao. Preparazione: sciogliere il cioccolato, unire la panna, far raffreddare e formare le palline.",
            };
        }

        // Note: UI controls and layout are defined in the Designer file. No manual control creation here.

        private void BtnGenerate_Click(object? sender, EventArgs e)
        {
            var data = BuildSelectedRecipeData();
            if (data == null) return;
            // Save last generated values
            lastName = data.Value.name;
            lastIngredients = data.Value.ingredients;
            lastPantryItems = data.Value.pantry;
            lastMissing = data.Value.missing;
            lastResultText = data.Value.resultText;

            tbResult.Text = lastResultText;
        }

        // Restituisce i dati costruiti per la ricetta selezionata (nome, ingredienti, dispensa, mancanti, testo risultato)
        private (string name, List<string> ingredients, List<string> pantry, List<string> missing, string resultText)? BuildSelectedRecipeData()
        {
            if (lbRecipes.SelectedIndex < 0) return null;

            var recipe = recipes[lbRecipes.SelectedIndex];
            var name = ExtractRecipeName(recipe);
            var ingredients = ExtractIngredients(recipe);
            var pantryItems = ParsePantry(tbPantry.Text);

            var missing = ingredients.Where(i => !pantryItems.Contains(i, StringComparer.OrdinalIgnoreCase)).ToList();
            var present = ingredients.Where(i => pantryItems.Contains(i, StringComparer.OrdinalIgnoreCase)).ToList();

            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Ricetta: {name}");
            sb.AppendLine();
            sb.AppendLine("Ingredienti richiesti:");
            foreach (var ing in ingredients) sb.AppendLine("- " + ing);
            sb.AppendLine();
            sb.AppendLine("Ingredienti presenti in dispensa:");
            if (present.Any()) foreach (var p in present) sb.AppendLine("- " + p);
            else sb.AppendLine("(nessuno)");
            sb.AppendLine();
            sb.AppendLine("Lista della spesa (mancanti):");
            if (missing.Any()) foreach (var m in missing) sb.AppendLine("- " + m);
            else sb.AppendLine("(nulla da comprare)");

            return (name, ingredients, pantryItems, missing, sb.ToString());
        }

        private void BtnSaveHtml_Click(object? sender, EventArgs e)
        {
            // Use the last generated data. If user didn't generate yet, ask to generate first.
            if (string.IsNullOrEmpty(lastResultText) || lastName == null || lastIngredients == null || lastPantryItems == null || lastMissing == null)
            {
                MessageBox.Show("Devi prima generare la lista premendo 'Genera lista della spesa' per poter salvare l'HTML.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var html = BuildHtml(lastName, lastIngredients, lastPantryItems, lastMissing);

            if (string.IsNullOrEmpty(html))
            {
                MessageBox.Show(
                    "Impossibile generare l'HTML perché manca il file 'template.html' nella cartella dell'eseguibile o si è verificato un errore durante la lettura.\r\n" +
                    "Aggiungi 'template.html' al progetto e imposta 'Copy to Output Directory' su 'Copy if newer' o 'Copy always', poi riprova.",
                    "Template mancante", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Save the HTML file to the user's Desktop
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var path = Path.Combine(desktop, "lista_spesa.html");
            File.WriteAllText(path, html);

            MessageBox.Show($"File HTML salvato sul Desktop:\r\n{path}", "Salvato", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static string ExtractRecipeName(string recipe)
        {
            var idx = recipe.IndexOf('.');
            if (idx <= 0) return recipe.Trim();
            return recipe.Substring(0, idx).Trim();
        }

        private static List<string> ExtractIngredients(string recipe)
        {
            var startKey = "Ingredienti:";
            var prepKey = "Preparazione:";
            var si = recipe.IndexOf(startKey, StringComparison.OrdinalIgnoreCase);
            if (si < 0) return new List<string>();
            si += startKey.Length;
            var ei = recipe.IndexOf(prepKey, si, StringComparison.OrdinalIgnoreCase);
            string ingPart;
            if (ei < 0)
                ingPart = recipe.Substring(si);
            else
                ingPart = recipe.Substring(si, ei - si);

            return SplitAndClean(ingPart, ',');
        }

        private static List<string> ParsePantry(string text)
        {
            return SplitAndClean(text, ',', '\n', '\r');
        }

        // Helper: split by given separators, trim whitespace and trailing dots, remove empties
        private static List<string> SplitAndClean(string text, params char[] separators)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            return text.Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim().TrimEnd('.'))
                .Where(p => !string.IsNullOrEmpty(p))
                .ToList();
        }

        private static string? BuildHtml(string name, List<string> ingredients, List<string> pantry, List<string> missing)
        {
            // Cerca il file template.html nella cartella dell'eseguibile
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var templatePath = Path.Combine(baseDir, "template.html");

            if (!File.Exists(templatePath))
            {
                return null; // template mancante
            }

            string template;
            try
            {
                template = File.ReadAllText(templatePath);
            }
            catch
            {
                return null; // errore lettura
            }

            // Funzione semplice per costruire una lista HTML dagli ingredienti
            static string BuildListHtml(List<string> items)
            {
                if (items == null || items.Count == 0)
                    return "<p>(nessuno)</p>";
                var sb = new System.Text.StringBuilder();
                sb.AppendLine("<ul>");
                foreach (var it in items)
                {
                    sb.AppendLine($"  <li>{System.Net.WebUtility.HtmlEncode(it)}</li>");
                }
                sb.AppendLine("</ul>");
                return sb.ToString();
            }

            var ingHtml = BuildListHtml(ingredients);
            var pantryHtml = BuildListHtml(pantry);
            var missingHtml = (missing != null && missing.Count > 0) ? BuildListHtml(missing) : "<p>(nulla da comprare)</p>";

            // Sostituisci i segnaposto nel template
            template = template.Replace("{{NAME}}", System.Net.WebUtility.HtmlEncode(name ?? string.Empty));
            template = template.Replace("{{INGREDIENTS}}", ingHtml);
            template = template.Replace("{{PANTRY}}", pantryHtml);
            template = template.Replace("{{MISSING}}", missingHtml);

            return template;
        }

        // Removed unused event handlers.
    }
}
