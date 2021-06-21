
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.btnReadCorpus = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.loadMatrixFile = new System.Windows.Forms.Button();
            this.btnPredict = new System.Windows.Forms.Button();
            this.btn_Viterbi = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.selectAlgoritm = new System.Windows.Forms.ComboBox();
            this.btnStatistici = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chartAcuratete = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listPredictie = new System.Windows.Forms.ListView();
            this.Cuvant = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBoxSentence = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartAcuratete)).BeginInit();
            this.SuspendLayout();
            // 
            // btnReadCorpus
            // 
            this.btnReadCorpus.FlatAppearance.BorderSize = 0;
            this.btnReadCorpus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReadCorpus.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReadCorpus.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.btnReadCorpus.Image = ((System.Drawing.Image)(resources.GetObject("btnReadCorpus.Image")));
            this.btnReadCorpus.Location = new System.Drawing.Point(2, 93);
            this.btnReadCorpus.Margin = new System.Windows.Forms.Padding(2);
            this.btnReadCorpus.Name = "btnReadCorpus";
            this.btnReadCorpus.Size = new System.Drawing.Size(182, 66);
            this.btnReadCorpus.TabIndex = 0;
            this.btnReadCorpus.Text = "Citire Corpus";
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
            this.loadMatrixFile.Location = new System.Drawing.Point(2, 403);
            this.loadMatrixFile.Margin = new System.Windows.Forms.Padding(2);
            this.loadMatrixFile.Name = "loadMatrixFile";
            this.loadMatrixFile.Size = new System.Drawing.Size(182, 68);
            this.loadMatrixFile.TabIndex = 1;
            this.loadMatrixFile.Text = "Antrenare";
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
            this.btnPredict.Location = new System.Drawing.Point(1, 233);
            this.btnPredict.Margin = new System.Windows.Forms.Padding(2);
            this.btnPredict.Name = "btnPredict";
            this.btnPredict.Size = new System.Drawing.Size(184, 69);
            this.btnPredict.TabIndex = 2;
            this.btnPredict.Text = "Predictie Fisiere Test";
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
            this.btn_Viterbi.Location = new System.Drawing.Point(1, 477);
            this.btn_Viterbi.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Viterbi.Name = "btn_Viterbi";
            this.btn_Viterbi.Size = new System.Drawing.Size(184, 70);
            this.btn_Viterbi.TabIndex = 3;
            this.btn_Viterbi.Text = "Viterbi";
            this.btn_Viterbi.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_Viterbi.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btn_Viterbi.UseVisualStyleBackColor = true;
            this.btn_Viterbi.Click += new System.EventHandler(this.btn_Viterbi_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.selectAlgoritm);
            this.panel1.Controls.Add(this.btnStatistici);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.btnPredict);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.btnReadCorpus);
            this.panel1.Controls.Add(this.loadMatrixFile);
            this.panel1.Controls.Add(this.btn_Viterbi);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(186, 551);
            this.panel1.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label5.Location = new System.Drawing.Point(28, 202);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 23);
            this.label5.TabIndex = 10;
            this.label5.Text = "Algoritm utilizat";
            // 
            // selectAlgoritm
            // 
            this.selectAlgoritm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(238)))), ((int)(((byte)(239)))));
            this.selectAlgoritm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(120)))), ((int)(((byte)(138)))));
            this.selectAlgoritm.FormattingEnabled = true;
            this.selectAlgoritm.Items.AddRange(new object[] {
            "Algoritmul Viterbi",
            "Predictor Frecvente"});
            this.selectAlgoritm.Location = new System.Drawing.Point(8, 170);
            this.selectAlgoritm.Name = "selectAlgoritm";
            this.selectAlgoritm.Size = new System.Drawing.Size(168, 31);
            this.selectAlgoritm.TabIndex = 13;
            this.selectAlgoritm.Tag = "";
            // 
            // btnStatistici
            // 
            this.btnStatistici.FlatAppearance.BorderSize = 0;
            this.btnStatistici.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStatistici.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStatistici.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.btnStatistici.Image = ((System.Drawing.Image)(resources.GetObject("btnStatistici.Image")));
            this.btnStatistici.Location = new System.Drawing.Point(1, 306);
            this.btnStatistici.Margin = new System.Windows.Forms.Padding(2);
            this.btnStatistici.Name = "btnStatistici";
            this.btnStatistici.Size = new System.Drawing.Size(184, 69);
            this.btnStatistici.TabIndex = 12;
            this.btnStatistici.Text = "Afisare Acuratete";
            this.btnStatistici.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnStatistici.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnStatistici.UseVisualStyleBackColor = true;
            this.btnStatistici.Click += new System.EventHandler(this.btnStatistici_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 383);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 23);
            this.label4.TabIndex = 11;
            this.label4.Text = "Testare:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 23);
            this.label3.TabIndex = 10;
            this.label3.Text = "Statistici:";
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
            this.panel3.Size = new System.Drawing.Size(186, 58);
            this.panel3.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(185, 58);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chartAcuratete);
            this.panel2.Location = new System.Drawing.Point(502, 299);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(633, 252);
            this.panel2.TabIndex = 5;
            // 
            // chartAcuratete
            // 
            this.chartAcuratete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(238)))), ((int)(((byte)(239)))));
            chartArea2.Name = "ChartArea1";
            this.chartAcuratete.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartAcuratete.Legends.Add(legend2);
            this.chartAcuratete.Location = new System.Drawing.Point(3, 1);
            this.chartAcuratete.Name = "chartAcuratete";
            this.chartAcuratete.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Acuratetea de predictie";
            series2.YValuesPerPoint = 6;
            this.chartAcuratete.Series.Add(series2);
            this.chartAcuratete.Size = new System.Drawing.Size(627, 248);
            this.chartAcuratete.TabIndex = 10;
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(118)))), ((int)(((byte)(121)))));
            this.panelLeft.Location = new System.Drawing.Point(186, 87);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(7, 72);
            this.panelLeft.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(206, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(238, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "Introduceti o propoziție:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(206, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 23);
            this.label2.TabIndex = 9;
            this.label2.Text = "Predictie:";
            // 
            // listPredictie
            // 
            this.listPredictie.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(238)))), ((int)(((byte)(239)))));
            this.listPredictie.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listPredictie.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Cuvant});
            this.listPredictie.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listPredictie.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(120)))), ((int)(((byte)(138)))));
            this.listPredictie.GridLines = true;
            this.listPredictie.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listPredictie.HideSelection = false;
            this.listPredictie.Location = new System.Drawing.Point(210, 299);
            this.listPredictie.Name = "listPredictie";
            this.listPredictie.Size = new System.Drawing.Size(200, 240);
            this.listPredictie.TabIndex = 10;
            this.listPredictie.UseCompatibleStateImageBehavior = false;
            this.listPredictie.View = System.Windows.Forms.View.Details;
            // 
            // Cuvant
            // 
            this.Cuvant.Text = "Cuvant  Predictie";
            this.Cuvant.Width = 200;
            // 
            // textBoxSentence
            // 
            this.textBoxSentence.BackColor = System.Drawing.Color.White;
            this.textBoxSentence.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxSentence.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(120)))), ((int)(((byte)(138)))));
            this.textBoxSentence.Location = new System.Drawing.Point(210, 53);
            this.textBoxSentence.Name = "textBoxSentence";
            this.textBoxSentence.Size = new System.Drawing.Size(761, 25);
            this.textBoxSentence.TabIndex = 11;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(120)))), ((int)(((byte)(138)))));
            this.panel5.Location = new System.Drawing.Point(210, 79);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(761, 10);
            this.panel5.TabIndex = 12;
            // 
            // btnExit
            // 
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.Location = new System.Drawing.Point(1076, 0);
            this.btnExit.Margin = new System.Windows.Forms.Padding(2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(56, 55);
            this.btnExit.TabIndex = 14;
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // Spatiu_de_reprezentare
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(238)))), ((int)(((byte)(239)))));
            this.ClientSize = new System.Drawing.Size(1135, 551);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.textBoxSentence);
            this.Controls.Add(this.listPredictie);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
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
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartAcuratete)).EndInit();
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnStatistici;
        private System.Windows.Forms.ComboBox selectAlgoritm;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartAcuratete;
        private System.Windows.Forms.ListView listPredictie;
        private System.Windows.Forms.ColumnHeader Cuvant;
        private System.Windows.Forms.TextBox textBoxSentence;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnExit;
    }
}

