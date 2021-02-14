
namespace POS_Tagging
{
    partial class Spatiu_de_reprezentare
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_Predict = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_Predict
            // 
            this.button_Predict.Location = new System.Drawing.Point(644, 384);
            this.button_Predict.Name = "button_Predict";
            this.button_Predict.Size = new System.Drawing.Size(128, 54);
            this.button_Predict.TabIndex = 0;
            this.button_Predict.Text = "Predict";
            this.button_Predict.UseVisualStyleBackColor = true;
            this.button_Predict.Click += new System.EventHandler(this.button_Predict_Click);
            // 
            // Spatiu_de_reprezentare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_Predict);
            this.Name = "Spatiu_de_reprezentare";
            this.Text = "Part-of-Speech Tagging";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Predict;
    }
}

