
namespace POS_Tagging
{
    partial class CustomTextBox
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxPropozitie = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxPropozitie
            // 
            this.textBoxPropozitie.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxPropozitie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPropozitie.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(120)))), ((int)(((byte)(138)))));
            this.textBoxPropozitie.Location = new System.Drawing.Point(7, 7);
            this.textBoxPropozitie.Name = "textBoxPropozitie";
            this.textBoxPropozitie.Size = new System.Drawing.Size(486, 25);
            this.textBoxPropozitie.TabIndex = 0;
            // 
            // CustomTextBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(238)))), ((int)(((byte)(239)))));
            this.Controls.Add(this.textBoxPropozitie);
            this.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(120)))), ((int)(((byte)(138)))));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "CustomTextBox";
            this.Padding = new System.Windows.Forms.Padding(7);
            this.Size = new System.Drawing.Size(500, 30);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxPropozitie;
    }
}
