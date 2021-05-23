
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
            this.btnReadCorpus = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.loadMatrixFile = new System.Windows.Forms.Button();
            this.btnPredict = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnReadCorpus
            // 
            this.btnReadCorpus.Location = new System.Drawing.Point(455, 99);
            this.btnReadCorpus.Name = "btnReadCorpus";
            this.btnReadCorpus.Size = new System.Drawing.Size(236, 121);
            this.btnReadCorpus.TabIndex = 0;
            this.btnReadCorpus.Text = "Read Corpus";
            this.btnReadCorpus.UseVisualStyleBackColor = true;
            this.btnReadCorpus.Click += new System.EventHandler(this.btnReadCorpus_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // loadMatrixFile
            // 
            this.loadMatrixFile.Location = new System.Drawing.Point(455, 296);
            this.loadMatrixFile.Name = "loadMatrixFile";
            this.loadMatrixFile.Size = new System.Drawing.Size(236, 117);
            this.loadMatrixFile.TabIndex = 1;
            this.loadMatrixFile.Text = "Load Matrix";
            this.loadMatrixFile.UseVisualStyleBackColor = true;
            this.loadMatrixFile.Click += new System.EventHandler(this.loadMatrixFile_Click);
            // 
            // btnPredict
            // 
            this.btnPredict.Location = new System.Drawing.Point(173, 118);
            this.btnPredict.Name = "btnPredict";
            this.btnPredict.Size = new System.Drawing.Size(161, 102);
            this.btnPredict.TabIndex = 2;
            this.btnPredict.Text = "Predict";
            this.btnPredict.UseVisualStyleBackColor = true;
            this.btnPredict.Click += new System.EventHandler(this.btnPredict_Click);
            // 
            // Spatiu_de_reprezentare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnPredict);
            this.Controls.Add(this.loadMatrixFile);
            this.Controls.Add(this.btnReadCorpus);
            this.Name = "Spatiu_de_reprezentare";
            this.Text = "Part-of-Speech Tagging";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnReadCorpus;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button loadMatrixFile;
        private System.Windows.Forms.Button btnPredict;
    }
}

