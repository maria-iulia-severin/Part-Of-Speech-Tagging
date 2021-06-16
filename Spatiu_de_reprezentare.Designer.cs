
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Spatiu_de_reprezentare));
            this.btnReadCorpus = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.loadMatrixFile = new System.Windows.Forms.Button();
            this.btnPredict = new System.Windows.Forms.Button();
            this.btn_Viterbi = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.customTextBox1 = new POS_Tagging.CustomTextBox();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnReadCorpus
            // 
            this.btnReadCorpus.FlatAppearance.BorderSize = 0;
            this.btnReadCorpus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReadCorpus.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReadCorpus.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.btnReadCorpus.Image = ((System.Drawing.Image)(resources.GetObject("btnReadCorpus.Image")));
            this.btnReadCorpus.Location = new System.Drawing.Point(2, 130);
            this.btnReadCorpus.Margin = new System.Windows.Forms.Padding(2);
            this.btnReadCorpus.Name = "btnReadCorpus";
            this.btnReadCorpus.Size = new System.Drawing.Size(182, 88);
            this.btnReadCorpus.TabIndex = 0;
            this.btnReadCorpus.Text = "Read Corpus";
            this.btnReadCorpus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnReadCorpus.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnReadCorpus.UseVisualStyleBackColor = true;
            this.btnReadCorpus.Click += new System.EventHandler(this.btnReadCorpus_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // loadMatrixFile
            // 
            this.loadMatrixFile.FlatAppearance.BorderSize = 0;
            this.loadMatrixFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loadMatrixFile.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadMatrixFile.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.loadMatrixFile.Image = ((System.Drawing.Image)(resources.GetObject("loadMatrixFile.Image")));
            this.loadMatrixFile.Location = new System.Drawing.Point(1, 218);
            this.loadMatrixFile.Margin = new System.Windows.Forms.Padding(2);
            this.loadMatrixFile.Name = "loadMatrixFile";
            this.loadMatrixFile.Size = new System.Drawing.Size(184, 81);
            this.loadMatrixFile.TabIndex = 1;
            this.loadMatrixFile.Text = "Load Matrix";
            this.loadMatrixFile.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.loadMatrixFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.loadMatrixFile.UseVisualStyleBackColor = true;
            this.loadMatrixFile.Click += new System.EventHandler(this.loadMatrixFile_Click);
            // 
            // btnPredict
            // 
            this.btnPredict.FlatAppearance.BorderSize = 0;
            this.btnPredict.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPredict.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPredict.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.btnPredict.Image = ((System.Drawing.Image)(resources.GetObject("btnPredict.Image")));
            this.btnPredict.Location = new System.Drawing.Point(1, 299);
            this.btnPredict.Margin = new System.Windows.Forms.Padding(2);
            this.btnPredict.Name = "btnPredict";
            this.btnPredict.Size = new System.Drawing.Size(184, 87);
            this.btnPredict.TabIndex = 2;
            this.btnPredict.Text = "Predict";
            this.btnPredict.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPredict.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnPredict.UseVisualStyleBackColor = true;
            this.btnPredict.Click += new System.EventHandler(this.btnPredict_Click);
            // 
            // btn_Viterbi
            // 
            this.btn_Viterbi.FlatAppearance.BorderSize = 0;
            this.btn_Viterbi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Viterbi.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Viterbi.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.btn_Viterbi.Image = ((System.Drawing.Image)(resources.GetObject("btn_Viterbi.Image")));
            this.btn_Viterbi.Location = new System.Drawing.Point(1, 386);
            this.btn_Viterbi.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Viterbi.Name = "btn_Viterbi";
            this.btn_Viterbi.Size = new System.Drawing.Size(184, 84);
            this.btn_Viterbi.TabIndex = 3;
            this.btn_Viterbi.Text = "Viterbi";
            this.btn_Viterbi.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_Viterbi.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btn_Viterbi.UseVisualStyleBackColor = true;
            this.btn_Viterbi.Click += new System.EventHandler(this.btn_Viterbi_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.btnReadCorpus);
            this.panel1.Controls.Add(this.btn_Viterbi);
            this.panel1.Controls.Add(this.btnPredict);
            this.panel1.Controls.Add(this.loadMatrixFile);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(186, 551);
            this.panel1.TabIndex = 4;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.panel6.Location = new System.Drawing.Point(190, 237);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(7, 72);
            this.panel6.TabIndex = 7;
            // 
            // panel4
            // 
            this.panel4.Location = new System.Drawing.Point(186, 130);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(10, 100);
            this.panel4.TabIndex = 6;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(118)))), ((int)(((byte)(121)))));
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(186, 100);
            this.panel3.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(183, 58);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(186, 340);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(949, 211);
            this.panel2.TabIndex = 5;
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(118)))), ((int)(((byte)(121)))));
            this.panelLeft.Location = new System.Drawing.Point(186, 139);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(7, 72);
            this.panelLeft.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(206, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 21);
            this.label1.TabIndex = 8;
            this.label1.Text = "Introduceti o propoziție:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(206, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 21);
            this.label2.TabIndex = 9;
            this.label2.Text = "Predictie:";
            // 
            // customTextBox1
            // 
            this.customTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(238)))), ((int)(((byte)(239)))));
            this.customTextBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(118)))), ((int)(((byte)(121)))));
            this.customTextBox1.BorderSize = 2;
            this.customTextBox1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(120)))), ((int)(((byte)(138)))));
            this.customTextBox1.Location = new System.Drawing.Point(210, 44);
            this.customTextBox1.Margin = new System.Windows.Forms.Padding(5);
            this.customTextBox1.Name = "customTextBox1";
            this.customTextBox1.Padding = new System.Windows.Forms.Padding(7);
            this.customTextBox1.Size = new System.Drawing.Size(386, 36);
            this.customTextBox1.TabIndex = 7;
            this.customTextBox1.UnderlinedStyle = false;
            // 
            // Spatiu_de_reprezentare
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(238)))), ((int)(((byte)(239)))));
            this.ClientSize = new System.Drawing.Size(1135, 551);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.customTextBox1);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(120)))), ((int)(((byte)(138)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Spatiu_de_reprezentare";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Part-of-Speech Tagging";
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReadCorpus;
        private System.Windows.Forms.Button loadMatrixFile;
        private System.Windows.Forms.Button btnPredict;
        private System.Windows.Forms.Button btn_Viterbi;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.PictureBox pictureBox1;
        private CustomTextBox customTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

