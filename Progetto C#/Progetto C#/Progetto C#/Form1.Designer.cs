namespace Progetto_C_
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private System.Windows.Forms.ListBox lbRecipes;
        private System.Windows.Forms.TextBox tbPantry;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.TextBox tbResult;
        private System.Windows.Forms.Button btnSaveHtml;

        private void InitializeComponent()
        {
            lbRecipes = new ListBox();
            tbPantry = new TextBox();
            btnGenerate = new Button();
            btnSaveHtml = new Button();
            tbResult = new TextBox();
            SuspendLayout();
            // 
            // lbRecipes
            // 
            lbRecipes.ItemHeight = 15;
            lbRecipes.Location = new Point(10, 10);
            lbRecipes.Name = "lbRecipes";
            lbRecipes.Size = new Size(350, 289);
            lbRecipes.TabIndex = 0;
            // 
            // tbPantry
            // 
            tbPantry.Location = new Point(10, 345);
            tbPantry.Multiline = true;
            tbPantry.Name = "tbPantry";
            tbPantry.ScrollBars = ScrollBars.Vertical;
            tbPantry.Size = new Size(350, 180);
            tbPantry.TabIndex = 1;
            tbPantry.Text = "uova, cioccolato, mascarpone, farina, lievito";
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(380, 10);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(180, 30);
            btnGenerate.TabIndex = 2;
            btnGenerate.Text = "Genera lista della spesa";
            btnGenerate.Click += BtnGenerate_Click;
            // 
            // btnSaveHtml
            // 
            btnSaveHtml.Location = new Point(380, 50);
            btnSaveHtml.Name = "btnSaveHtml";
            btnSaveHtml.Size = new Size(180, 30);
            btnSaveHtml.TabIndex = 3;
            btnSaveHtml.Text = "Salva HTML";
            btnSaveHtml.Click += BtnSaveHtml_Click;
            // 
            // tbResult
            // 
            tbResult.Location = new Point(380, 90);
            tbResult.Multiline = true;
            tbResult.Name = "tbResult";
            tbResult.ReadOnly = true;
            tbResult.ScrollBars = ScrollBars.Vertical;
            tbResult.Size = new Size(390, 435);
            tbResult.TabIndex = 4;
            // 
            // Form1
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 600);
            Controls.Add(lbRecipes);
            Controls.Add(tbPantry);
            Controls.Add(btnGenerate);
            Controls.Add(btnSaveHtml);
            Controls.Add(tbResult);
            Name = "Form1";
            Text = "Pasticceria Ciccio e Renata";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
