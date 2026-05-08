using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Progetto_C_
{
    public partial class Form1 : Form
    {
        private List<string> recipes;

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

        private void InitializeControls()
        {
            lbRecipes = new ListBox
            {
                Location = new Point(10, 10),
                Size = new Size(350, 300)
            };

            // Fill listbox with recipe names (part before first dot)
            foreach (var r in recipes)
            {
                var name = ExtractRecipeName(r);
                lbRecipes.Items.Add(name);
            }
            lbRecipes.SelectedIndex = 0;

            var lblPantry = new Label
            {
                Text = "Dispensa (separa gli ingredienti con virgola o newline):",
                Location = new Point(10, 320),
                AutoSize = true
            };

            tbPantry = new TextBox
            {
                Location = new Point(10, 345),
                Size = new Size(350, 180),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Text = "uova, cioccolato, mascarpone, farina, lievito"
            };

            btnGenerate = new Button
            {
                Text = "Genera lista della spesa",
                Location = new Point(380, 10),
                Size = new Size(180, 30)
            };
            btnGenerate.Click += BtnGenerate_Click;

            btnSaveHtml = new Button
            {
                Text = "Salva HTML",
                Location = new Point(380, 50),
                Size = new Size(180, 30),
                Enabled = true
            };
            btnSaveHtml.Click += BtnSaveHtml_Click;

            tbResult = new TextBox
            {
                Location = new Point(380, 90),
                Size = new Size(390, 435),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true
            };

            Controls.Add(lbRecipes);
            Controls.Add(lblPantry);
            Controls.Add(tbPantry);
            Controls.Add(btnGenerate);
            Controls.Add(btnSaveHtml);
            Controls.Add(tbResult);
        }

        private void BtnGenerate_Click(object? sender, EventArgs e)
        {
            if (lbRecipes.SelectedIndex < 0) return;

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

            tbResult.Text = sb.ToString();
        }

        private void BtnSaveHtml_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbResult.Text))
            {
                MessageBox.Show("Generare prima la lista della spesa premendo 'Genera lista della spesa'.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var recipe = recipes[Math.Max(0, lbRecipes.SelectedIndex)];
            var name = ExtractRecipeName(recipe);
            var ingredients = ExtractIngredients(recipe);
            var pantryItems = ParsePantry(tbPantry.Text);
            var missing = ingredients.Where(i => !pantryItems.Contains(i, StringComparer.OrdinalIgnoreCase)).ToList();

            var html = BuildHtml(name, ingredients, pantryItems, missing);

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
    }
}
