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
            this.components = new System.ComponentModel.Container();
            this.lbRecipes = new System.Windows.Forms.ListBox();
            this.tbPantry = new System.Windows.Forms.TextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnSaveHtml = new System.Windows.Forms.Button();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbRecipes
            // 
            this.lbRecipes.Location = new System.Drawing.Point(10, 10);
            this.lbRecipes.Size = new System.Drawing.Size(350, 300);
            // 
            // tbPantry
            // 
            this.tbPantry.Location = new System.Drawing.Point(10, 345);
            this.tbPantry.Size = new System.Drawing.Size(350, 180);
            this.tbPantry.Multiline = true;
            this.tbPantry.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbPantry.Text = "uova, cioccolato, mascarpone, farina, lievito";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(380, 10);
            this.btnGenerate.Size = new System.Drawing.Size(180, 30);
            this.btnGenerate.Text = "Genera lista della spesa";
            this.btnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
            // 
            // btnSaveHtml
            // 
            this.btnSaveHtml.Location = new System.Drawing.Point(380, 50);
            this.btnSaveHtml.Size = new System.Drawing.Size(180, 30);
            this.btnSaveHtml.Text = "Salva HTML";
            this.btnSaveHtml.Click += new System.EventHandler(this.BtnSaveHtml_Click);
            // 
            // tbResult
            // 
            this.tbResult.Location = new System.Drawing.Point(380, 90);
            this.tbResult.Size = new System.Drawing.Size(390, 435);
            this.tbResult.Multiline = true;
            this.tbResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbResult.ReadOnly = true;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Text = "Pasticceria Ciccio e Renata";
            this.Controls.Add(this.lbRecipes);
            this.Controls.Add(this.tbPantry);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnSaveHtml);
            this.Controls.Add(this.tbResult);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
