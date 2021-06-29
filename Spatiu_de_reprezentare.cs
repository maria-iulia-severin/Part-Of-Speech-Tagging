using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;

namespace POS_Tagging
{
    public partial class Spatiu_de_reprezentare : Form
    {
        string[] noun = { "nn", "nns", "np", "nps", "nrs" };
        string[] verb = { "be", "bed", "bedz", "beg", "bem", "ben", "ber", "bez", "do", "dod", "doz", "hv", "hvd", "hvg", "hvn", "hvz", "md", "vb", "vbd", "vbg", "vbn", "vbz" };
        string[] adjective = { "jj", "jjr", "jjs", "jjt" };
        string[] adverb = {"rb", "rbr", "rbt", "rn", "rp", "wrb", "nr" };
        string[] pronoun = { "pn", "pp", "ppl", "ppls", "ppo", "pps", "ppss", "wp", "wpo", "wps" };
        string[] conjunction = { "cs", "cc" };
        string[] article = { "at" };
        string[] preposition = { "in", "to" };
        string[] other = { "uh", "abl", "abn", "abx", "ap", "cd", "dt", "dti", "dts", "dtx", "ex", "fw", "od", "ql", "qlp", "wdt", "wql", "nc", "hl", "tl" };

        List<String> partsOfSpeech = new List<string> { "noun", "conjunction", "other", "verb", "preposition", "article", "adjective", "pronoun", "adverb" };
        List<String> wordTrainArray = new List<string>();
        List<String> tagTrainArray = new List<string>();
        List<String> predictionArray = new List<string>();

        List<WordTags> wordTagTestArray = new List<WordTags>();
        List<WordTags> wordTagViterbiArray = new List<WordTags>();

        private List<ViterbiPath> viterbiPaths = new List<ViterbiPath>();

        const int PHRASE_LENGHT = 10;

        int[] contorTP = new int[9];
        int[] contorTN = new int[9];
        int[] contorFP = new int[9];
        int[] contorFN = new int[9];
        int[,] emissionIntegerTempMatrix;
        double[,] emissionMatrix;
        int[,] transitionIntegerTempMatrix;
        double[,] transitionMatrix;

        int numberOfWordsInPhrase;
        int countTotalWordsTrain = 0;
        int countTotalNumberOfTagsInTrain = 0; // numar total etichetari pt a dat 1 pe linie

        char[] separators = new char[] { ' ', '\r', '\n', '\t' };
        string[] phraseSeparators = new string[] { ",/,-hl", "./.-hl", "!/.-hl", "?/.-hl", ";/.-hl", ",/,", "./.", "!/.", "?/.", ";/." };
        char[] viterbiSeparators = new char[] { '?', '!', '-', '+', '*', ' ', '`', '\'', '.', ',', ':', ';', '(', ')', '$', '\r', '\n', '\t' };
        char[] spaceSeparator = new char[] { ' ' };
        public Spatiu_de_reprezentare()
        {
            InitializeComponent();
            chartAcuratete.Hide();
            listPredictie.Hide();
            panelLeft.Height = btnReadCorpus.Height;
            panelLeft.Top = btnReadCorpus.Top;
        }
        double[] initialState = new double[9];
        double[] currentProbability = new double[9];
        double[] pastProbability = new double[9];
        double[] transitions = new double[9];

        string[] testViterbi;
        double distanceUnit = 1;
        List<CircularButton> circles = new List<CircularButton>();
        private void btnStatistici_Click(object sender, EventArgs e)
        {
            panelLeft.Height = btnStatistici.Height;
            panelLeft.Top = btnStatistici.Top;


            chartAcuratete.Series["Acuratetea de predictie"].Points.AddXY("Viterbi", 0.99);
            chartAcuratete.Series["Acuratetea de predictie"].Points.AddXY("Frecventa", 0.96);
            chartAcuratete.Show();

        }
        private void btnReadCorpus_Click(object sender, EventArgs e)
        {
            msg m = new msg();
            panelLeft.Height = btnReadCorpus.Height;
            panelLeft.Top = btnReadCorpus.Top;
            ReadCorpus();
            m.Show();
        }
        private void loadMatrixFile_Click(object sender, EventArgs e)
        {
            msg m = new msg();

            panelLeft.Height = loadMatrixFile.Height;
            panelLeft.Top = loadMatrixFile.Top;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"C:\Users\iulia.severin\source\repos\POS-Tagging\bin\Debug";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        readWordTagArray(reader, isWord: true);
                        readWordTagArray(reader, isWord: false);
                        readMatrix(reader);
                        reader.ReadLine();
                        reader.ReadLine();
                        readInitialState(reader);
                        reader.ReadLine();
                        reader.ReadLine();
                        readTransitionMatrix(reader);
                        reader.ReadLine();
                        reader.ReadLine();
                        readEmissionMatrix(reader);
                    }
                }
            }
            m.Show();

            /* for (int i = 0; i < tagTrainArray.Count; i++)
             {

                 Console.WriteLine(initialState[i] + " ");
             }

             Console.WriteLine();
             Console.WriteLine("Matricea de Transitie");
             for (int i = 0; i < tagTrainArray.Count; i++)
             {
                 for (int j = 0; j < tagTrainArray.Count; j++)
                 {
                     Console.Write(transitionMatrix[i, j] + " ");
                 }
                 Console.WriteLine();
             }

             Console.WriteLine();
             Console.WriteLine("Matricea de eimisie");
             for (int i = 0; i < wordTrainArray.Count; i++)
             {
                 for (int j = 0; j < tagTrainArray.Count; j++)
                 {
                     Console.Write(emissionMatrix[i, j] + " ");
                 }
                 Console.WriteLine();
             }*/
        }
        private void btnPredict_Click(object sender, EventArgs e)
        {
            string predictedTag;
            panelLeft.Height = btnPredict.Height;
            panelLeft.Top = btnPredict.Top;

            if (selectAlgoritm.SelectedItem == "Predictor Frecvente")
            {
                var watch = new System.Diagnostics.Stopwatch();
                ReadBrownTest();
                watch.Start();
                foreach (WordTags wordTag in wordTagTestArray)
                {
                    predictedTag = Predict(wordTag.word);
                    //predictedTag = PredictNoun(wordTag.word);

                    predictionArray.Add(wordTag.word + " " + predictedTag);
                    Console.WriteLine("{0}: {1}", wordTag.word, predictedTag);

                    for (int i = 0; i < partsOfSpeech.Count; i++)
                    {
                        if (wordTag.tags.Contains(partsOfSpeech[i]))
                        {
                            if (predictedTag.Equals(partsOfSpeech[i]))
                            {
                                contorTP[i]++;
                            }
                            else
                            {
                                contorFN[i]++;
                            }
                        }
                        else
                        {
                            if (predictedTag.Equals(partsOfSpeech[i]))
                            {
                                contorFP[i]++;
                            }
                            else
                            {
                                contorTN[i]++;
                            }
                        }
                    }
                }
                watch.Stop();
                Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

                listPredictie.Items.Clear();
                foreach (string prediction in predictionArray)
                {
                    var row = new string[] { prediction };
                    var lvi = new ListViewItem(row);
                    lvi.Tag = prediction;
                    listPredictie.Items.Add(lvi);
                }
                listPredictie.Show();

                for (int i = 0; i < partsOfSpeech.Count; i++)
                {
                    Console.WriteLine("Total True Positive {0}: {1} ", partsOfSpeech[i], contorTP[i].ToString());
                    Console.WriteLine("Total False Negative {0}: {1} ", partsOfSpeech[i], contorFN[i].ToString());
                    Console.WriteLine("Total False Positive {0}: {1} ", partsOfSpeech[i], contorFP[i].ToString());
                    Console.WriteLine("Total True Negative {0}: {1} ", partsOfSpeech[i], contorTN[i].ToString());
                    Console.WriteLine("SUMA {0}: {1} ", partsOfSpeech[i], (contorTN[i] + contorTP[i] + contorFN[i] + contorFP[i]).ToString());
                }

                Console.WriteLine("Count wordTagTestArray " + wordTagTestArray.Count);
                WritePredictionStatistics(wordTagTestArray.Count);
            }
            else
            {
                ReadBrownViterbiTest();
            }
        }
        List<List<ViterbiNode>> viterbiList = new List<List<ViterbiNode>>();
        private void btn_Viterbi_Click(object sender, EventArgs e)
        {

            panelLeft.Height = btn_Viterbi.Height;
            panelLeft.Top = btn_Viterbi.Top;

            ClearCircles();

            string viterbiSentence = textBoxSentence.Text;
            testViterbi = viterbiSentence.Split(viterbiSeparators, StringSplitOptions.RemoveEmptyEntries);

            int topValue = 145;
            int leftValue = 200;
            double distancePass = 5 / testViterbi.Count();
            int circleSize = 450 / testViterbi.Count();

            viterbiList.Clear();
            viterbiPaths.Clear();
            numberOfWordsInPhrase = testViterbi.Count();

            for (int i = 0; i < testViterbi.Count(); i++)
            {
                viterbiList.Add(HiddenMarkovModel(testViterbi[i], i, viterbiList.Count == 0 ? null : viterbiList.Last()));
            }

            foreach (ViterbiNode lastNodeFromPhrase in viterbiList.Last())
            {
                ViterbiPathRec(new ViterbiPath(), lastNodeFromPhrase, 1);
            }

            viterbiPaths = viterbiPaths.Where(p => p.nodes.Count == testViterbi.Count()).ToList();
            double bestPathValue = viterbiPaths.Max(y => y.value);
            ViterbiPath bestPath = viterbiPaths.First(x => x.value == bestPathValue);
            bestPath.nodes.Reverse();

            foreach (ViterbiNode node in bestPath.nodes)
            {
                circles.Add(new CircularButton());
                this.Controls.Add(circles.Last());

                switch (node.partOfSpeech)
                {
                    case "noun":
                        circles.Last().BackColor = Color.FromArgb(87, 127, 180);
                        break;
                    case "verb":
                        circles.Last().BackColor = Color.FromArgb(0, 194, 203);
                        break;
                    case "adjective":
                        circles.Last().BackColor = Color.FromArgb(56, 182, 255);
                        break;
                    case "adverb":
                        circles.Last().BackColor = Color.FromArgb(242, 209, 201);
                        break;
                    case "pronoun":
                        circles.Last().BackColor = Color.FromArgb(175, 179, 247);
                        break;
                    case "conjunction":
                        circles.Last().BackColor = Color.FromArgb(145, 215, 223);
                        break;
                    case "article":
                        circles.Last().BackColor = Color.FromArgb(162, 250, 163);
                        break;
                    case "preposition":
                        circles.Last().BackColor = Color.FromArgb(188, 217, 121);
                        break;
                    default:
                        circles.Last().BackColor = Color.FromArgb(91, 108, 93);
                        break;
                }

                circles.Last().Enabled = false;
                circles.Last().FlatAppearance.BorderSize = 0;
                circles.Last().FlatStyle = FlatStyle.Flat;
                circles.Last().ForeColor = Color.White;
                circles.Last().Font = new Font("Century Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                circles.Last().Name = "circularButton1";
                circles.Last().Size = new Size(circleSize, circleSize);
                circles.Last().TabIndex = 10;
                circles.Last().Text = node.partOfSpeech;
                circles.Last().Location = new Point((int)(distanceUnit * leftValue), topValue);
                distanceUnit += distancePass;
            }
        }
        private void ClearCircles()
        {
            foreach (CircularButton button in circles)
            {
                this.Controls.Remove(button);
            }
            circles.Clear();
            distanceUnit = 1;
        }
        private void ViterbiPathRec(ViterbiPath path, ViterbiNode node, double transitionEdge)
        {
            //Console.WriteLine(node.probability * transitionEdge);
            if (node == null)
                return;

            path.value *= (node.probability * transitionEdge);
            path.nodes.Add(node);

            if (node.probability == 0.0)
            {
                return;
            }

            if (node.lastNodes.Count == 0)
            {
                if (path.nodes.Count == numberOfWordsInPhrase)
                    viterbiPaths.Add(path);
                return;
            }
            else
            {
                foreach (LastNode lastNode in node.lastNodes)
                {
                    ViterbiPathRec(new ViterbiPath(path), lastNode.node, lastNode.transitionEdge);
                }
            }
        }
        private List<ViterbiNode> HiddenMarkovModel(string word, int wordPositionInPhrase, List<ViterbiNode> prevNodes)
        {
            List<ViterbiNode> viterbiNodes = new List<ViterbiNode>();
            int pos;
            int word0;
            int wordn;
            if (wordPositionInPhrase == 0)
            {
                for (int i = 0; i < tagTrainArray.Count; i++)
                {
                    pos = GetTagPosition(tagTrainArray[i]);
                    word0 = GetWordPosition(word);
                    currentProbability[i] = ViterbiS0(word0, pos);

                    ViterbiNode viterbiNode = new ViterbiNode(wordPositionInPhrase, tagTrainArray[i], currentProbability[i]);
                    viterbiNodes.Add(viterbiNode);
                }

                CopyCurrentProbabilityToLastProbability();
            }
            else
            {
                wordn = GetWordPosition(word);
                for (int i = 0; i < tagTrainArray.Count; i++)
                {
                    List<LastNode> lastNodes = new List<LastNode>();

                    pos = GetTagPosition(tagTrainArray[i]);
                    currentProbability[i] = ViterbiSn(wordn, pos, lastNodes, prevNodes);

                    ViterbiNode viterbiNode = new ViterbiNode(wordPositionInPhrase, tagTrainArray[i], currentProbability[i], lastNodes);
                    viterbiNodes.Add(viterbiNode);

                }

                CopyCurrentProbabilityToLastProbability();
            }
            return viterbiNodes;
        }
        private double ViterbiS0(int word, int posCurrent)
        {
            if (word == -1)
            {
                //si poz curenta ca substantiv - AICI POATE NU E CHIAR CORECT SI TREBUIA 1/NR TOTAL APP SUB
                return posCurrent == 0 ? initialState[posCurrent] : 0;
            }
            else
            {
                return emissionMatrix[word, posCurrent] * initialState[posCurrent];
            }
        }
        private double ViterbiSn(int word, int posCurrent, List<LastNode> lastNodes, List<ViterbiNode> prevNodes)
        {
            int posPast;
            prevNodes = prevNodes.Where(x => x.probability > 0).ToList();
            //cuvant pe care nu il stiu
            if (word == -1)
            {
                //si poz curenta ca substantiv
                if (posCurrent == GetTagPosition("noun"))
                {
                    for (int i = 0; i < tagTrainArray.Count; i++)
                    {
                        transitions[i] = 1 * transitionMatrix[i, posCurrent]; // 1 * transitionMatrix[i, posCurrent]
                    }
                }
                else
                {
                    for (int i = 0; i < tagTrainArray.Count; i++)
                    {
                        transitions[i] = 0; // 0 * transitionMatrix[posPast, posCurrent]
                    }
                }
            }
            else
            {
                foreach (string pos2 in tagTrainArray)
                {
                    posPast = GetTagPosition(pos2);
                    transitions[posPast] = emissionMatrix[word, posCurrent] * transitionMatrix[posPast, posCurrent];
                }
            }

            double[] tempProbbility = new double[9];
            for (int i = 0; i < tagTrainArray.Count; i++)
            {
                if (transitions[i] != 0 && prevNodes.Count != 0)
                {
                    lastNodes.Add(new LastNode(tagTrainArray[i], transitions[i], prevNodes));
                }

                tempProbbility[i] = pastProbability[i] * transitions[i];
            }
            return tempProbbility.Max();
        }
        private void CopyCurrentProbabilityToLastProbability()
        {
            for (int i = 0; i < tagTrainArray.Count; i++)
            {
                pastProbability[i] = currentProbability[i];
            }
        }
        private void ReadBrownViterbiTest()
        {
            int sumAllWords = 0;
            string rootPath = @"C:\Users\iulia.severin\source\repos\POS-Tagging\bin\Debug\Brown_Test";
            var files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
            string[] wordTagPair;
            //string[] line;
            string line;
            var watch = new System.Diagnostics.Stopwatch();

            foreach (string file in files)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    while ((line = reader.ReadLine()) != null)
                    {

                        if (line == "")
                        {
                            continue;
                        }

                        //line = line.Split(phraseSeparators, StringSplitOptions.RemoveEmptyEntries);

                       // for (int k = 0; k < line.Count(); k++)
                       // {
                            wordTagPair = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                           
                            for (int m = 0; m <= wordTagPair.Length / PHRASE_LENGHT; m++)
                            {
                                wordTagTestArray.Clear();
                                for (int i = 0; i < PHRASE_LENGHT && (m * PHRASE_LENGHT + i < wordTagPair.Length); i++)
                                {
                                    WordTags wordTagSplit = SplitPair(wordTagPair[m * PHRASE_LENGHT + i]);

                                    for (int j = 0; j < wordTagSplit.tags.Count; j++)
                                    {
                                        wordTagSplit.tags[j] = GetPartOfSpeech(wordTagSplit.tags[j]);
                                    }

                                    if (wordTagSplit.tags.Count == 0)
                                    {
                                        continue;
                                    }

                                    wordTagSplit.tags = wordTagSplit.tags.Distinct().ToList();
                                    RemoveOtherTag(wordTagSplit);
                                    AddWordTagTestArray(wordTagSplit);
                                }
                                //-------------------------------

                                viterbiList.Clear();
                                viterbiPaths.Clear();
                                if (wordTagTestArray.Count() == 0)
                                {
                                    continue;
                                }
                                numberOfWordsInPhrase = wordTagTestArray.Count();
                                for (int j = 0; j < wordTagTestArray.Count(); j++)
                                {
                                    // string predictedTag = HiddenMarkovModel(wordTagTestArray[j].word, j);


                                    viterbiList.Add(HiddenMarkovModel(wordTagTestArray[j].word, j, viterbiList.Count == 0 ? null : viterbiList.Last()));
                                }

                                foreach (ViterbiNode lastNodeFromPhrase in viterbiList.Last())
                                {
                                    ViterbiPathRec(new ViterbiPath(), lastNodeFromPhrase, 1);
                                    viterbiPaths = viterbiPaths.Where(p => p.nodes.Count == wordTagTestArray.Count()).ToList();

                                }

                                viterbiPaths = viterbiPaths.Where(p => p.nodes.Count == wordTagTestArray.Count()).ToList();
                                double bestPathValue = viterbiPaths.Max(y => y.value);
                                ViterbiPath bestPath = viterbiPaths.First(x => x.value == bestPathValue);
                                //  bestPath.nodes.Reverse();

                                //List<string> predictedTag = bestPath.nodes.Select(x => x.partOfSpeech).ToList();


                                for (int j = 0; j < wordTagTestArray.Count(); j++)
                                {
                                    ViterbiNode node = bestPath.nodes.Where(x => x.wordPositionInPhrase == j).First();
                                    string predictedTag = node.partOfSpeech;

                                    predictionArray.Add(wordTagTestArray[j].word + " " + predictedTag);
                                    Console.WriteLine("Predictie {0}: {1}", wordTagTestArray[j].word, predictedTag);
                                    Console.WriteLine("Tag din fisier {0}: {1}", wordTagTestArray[j].word, wordTagTestArray[j].tags[0]);

                                    for (int i = 0; i < partsOfSpeech.Count; i++)
                                    {
                                        if (wordTagTestArray[j].tags.Contains(partsOfSpeech[i]))
                                        {
                                            if (predictedTag.Equals(partsOfSpeech[i]))
                                            {
                                                contorTP[i]++;
                                            }
                                            else
                                            {
                                                contorFN[i]++;
                                            }
                                        }
                                        else
                                        {
                                            if (predictedTag.Equals(partsOfSpeech[i]))
                                            {
                                                contorFP[i]++;
                                            }
                                            else
                                            {
                                                contorTN[i]++;
                                            }
                                        }
                                    }
                                }

                                sumAllWords += wordTagTestArray.Count();
                          //  }
                        }
                    }
                }
            }
            Console.WriteLine("Numarul total de cuvinte Viterbi Test:" + sumAllWords);

            listPredictie.Items.Clear();
            foreach (string prediction in predictionArray)
            {
                var row = new string[] { prediction };
                var lvi = new ListViewItem(row);
                lvi.Tag = prediction;
                listPredictie.Items.Add(lvi);
            }
            listPredictie.Show();

            WritePredictionStatistics(sumAllWords);

            for (int i = 0; i < partsOfSpeech.Count; i++)
            {
                Console.WriteLine("Total True Positive {0}: {1} ", partsOfSpeech[i], contorTP[i].ToString());
                Console.WriteLine("Total False Negative {0}: {1} ", partsOfSpeech[i], contorFN[i].ToString());
                Console.WriteLine("Total False Positive {0}: {1} ", partsOfSpeech[i], contorFP[i].ToString());
                Console.WriteLine("Total True Negative {0}: {1} ", partsOfSpeech[i], contorTN[i].ToString());
                Console.WriteLine("SUMA {0}: {1} ", partsOfSpeech[i], (contorTN[i] + contorTP[i] + contorFN[i] + contorFP[i]).ToString());
            }
        }
        private void ReadBrownTest()
        {
            string rootPath = @"C:\Users\iulia.severin\source\repos\POS-Tagging\bin\Debug\Brown_Test";
            var files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
            string[] wordTagPair;

            //Add pair Word-Tags in array
            foreach (string file in files)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    string textInFile = reader.ReadToEnd();

                    wordTagPair = textInFile.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < wordTagPair.Length; i++)
                    {
                        WordTags wordTagSplit = SplitPair(wordTagPair[i]);

                        //categ pe cele 9 tag uri
                        for (int j = 0; j < wordTagSplit.tags.Count; j++)
                        {
                            wordTagSplit.tags[j] = GetPartOfSpeech(wordTagSplit.tags[j]);
                        }

                        if (wordTagSplit.tags.Count == 0)
                        {
                            continue;
                        }

                        wordTagSplit.tags = wordTagSplit.tags.Distinct().ToList();
                        RemoveOtherTag(wordTagSplit);
                        AddWordTagTestArray(wordTagSplit);
                    }
                }
            }
        }
        private void AddWordTagTestArray(WordTags wordTag)
        {

            WordTags finalWordTag = new WordTags();
            WordTags firstWordTag = new WordTags();
            WordTags secondWordTag = new WordTags();
            WordTags exceptionWordTag = new WordTags();

            finalWordTag.word = wordTag.word;
            finalWordTag.tags = wordTag.tags;

            string[] newWord;
            firstWordTag.tags = new List<string>();
            secondWordTag.tags = new List<string>();
            exceptionWordTag.tags = new List<string>();

            if (wordTag.tags.Count >= 2)
            {
                newWord = finalWordTag.word.Split('\'');

                if (newWord.Length == 2)
                {
                    //primul cuvant
                    firstWordTag.word = newWord[0];
                    firstWordTag.tags.Add(wordTag.tags[0]);
                    wordTagTestArray.Add(firstWordTag);

                    //al doilea cuvant
                    secondWordTag.word = newWord[1];
                    secondWordTag.tags.Add(wordTag.tags[1]);
                    wordTagTestArray.Add(secondWordTag);
                }
                else
                {
                    exceptionWordTag.word = wordTag.word;
                    exceptionWordTag.tags.Add(wordTag.tags[0]);
                    wordTagTestArray.Add(exceptionWordTag);
                }
            }
            else
            {
                wordTagTestArray.Add(finalWordTag);
            }
        }
        private void AddWordTagViterbiArray(WordTags wordTag)
        {

            WordTags finalWordTag = new WordTags();
            WordTags firstWordTag = new WordTags();
            WordTags secondWordTag = new WordTags();
            WordTags exceptionWordTag = new WordTags();

            finalWordTag.word = wordTag.word;
            finalWordTag.tags = wordTag.tags;

            string[] newWord;
            firstWordTag.tags = new List<string>();
            secondWordTag.tags = new List<string>();
            exceptionWordTag.tags = new List<string>();

            if (wordTag.tags.Count >= 2)
            {
                newWord = finalWordTag.word.Split('\'');

                if (newWord.Length == 2)
                {
                    //primul cuvant
                    firstWordTag.word = newWord[0];
                    firstWordTag.tags.Add(wordTag.tags[0]);
                    wordTagViterbiArray.Add(firstWordTag);

                    //al doilea cuvant
                    secondWordTag.word = newWord[1];
                    secondWordTag.tags.Add(wordTag.tags[1]);
                    wordTagViterbiArray.Add(secondWordTag);
                }
                else
                {
                    exceptionWordTag.word = wordTag.word;
                    exceptionWordTag.tags.Add(wordTag.tags[0]);
                    wordTagViterbiArray.Add(exceptionWordTag);
                }
            }
            else
            {
                wordTagViterbiArray.Add(finalWordTag);
            }
        }
        private void ReadCorpus()
        {
            //string rootPath = @"C:\Users\iulia.severin\source\repos\POS-Tagging\bin\Debug\Brown";
            string rootPath = @"C:\Users\iulia.severin\source\repos\POS-Tagging\bin\Debug\Brown_Train";
            //string rootPath = @"C:\Users\iulia.severin\source\repos\POS-Tagging\bin\Debug\Brown_Test";
            var files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
            string[] wordTagPair;

            //Add pair Word-Tags in array
            foreach (string file in files)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    string textInFile = reader.ReadToEnd();
                    wordTagPair = textInFile.Split(separators, StringSplitOptions.RemoveEmptyEntries); //Split

                    for (int i = 0; i < wordTagPair.Length; i++)
                    {
                        WordTags wordTagSplit = SplitPair(wordTagPair[i]);

                        for (int j = 0; j < wordTagSplit.tags.Count; j++)
                        {
                            wordTagSplit.tags[j] = GetPartOfSpeech(wordTagSplit.tags[j]);
                        }

                        if (wordTagSplit.tags.Count == 0)
                        {
                            continue;
                        }

                        wordTagSplit.tags = wordTagSplit.tags.Distinct().ToList();
                        RemoveOtherTag(wordTagSplit);

                        if (wordTagSplit.tags.Count == 1)
                        {
                            AddWordInTrain(wordTagSplit.word);
                            AddTagInTrain(wordTagSplit.tags[0]);
                            countTotalWordsTrain++;
                        }
                        else
                        {
                            SplitDoubleTaggedWords(wordTagSplit);
                        }
                    }
                }
            }

            emissionIntegerTempMatrix = new int[wordTrainArray.Count, tagTrainArray.Count];

            //Add Tag - Words in Matrix
            foreach (string file in files)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    string text_in_file = reader.ReadToEnd();
                    wordTagPair = text_in_file.Split(separators, StringSplitOptions.RemoveEmptyEntries); //Split

                    for (int i = 0; i < wordTagPair.Length; i++)
                    {
                        WordTags word_tag_split = SplitPair(wordTagPair[i]);
                        for (int j = 0; j < word_tag_split.tags.Count; j++)
                        {
                            word_tag_split.tags[j] = GetPartOfSpeech(word_tag_split.tags[j]);
                        }

                        if (word_tag_split.tags.Count == 0)
                        {
                            continue;
                        }

                        word_tag_split.tags = word_tag_split.tags.Distinct().ToList();
                        RemoveOtherTag(word_tag_split);

                        if (word_tag_split.tags.Count == 1)
                        {
                            emissionIntegerTempMatrix[GetWordPosition(word_tag_split.word), GetTagPosition(word_tag_split.tags[0])]++;
                            countTotalNumberOfTagsInTrain++;
                        }
                        else
                        {   // He's/pronoun-verb
                            string[] newWord = word_tag_split.word.Split('\'');

                            if (newWord.Length == 2)
                            {
                                if (newWord[0] != "")
                                {
                                    //primul cuvant
                                    emissionIntegerTempMatrix[GetWordPosition(newWord[0]), GetTagPosition(word_tag_split.tags[0])]++;
                                    countTotalNumberOfTagsInTrain++;
                                }

                                //al doilea cuvant
                                emissionIntegerTempMatrix[GetWordPosition(newWord[1]), GetTagPosition(word_tag_split.tags[1])]++;
                                countTotalNumberOfTagsInTrain++;
                            }
                            else
                            {
                                emissionIntegerTempMatrix[GetWordPosition(word_tag_split.word), GetTagPosition(word_tag_split.tags[0])]++;
                                countTotalNumberOfTagsInTrain++;
                            }
                        }
                    }
                }
            }

            string line;
            //VITERBI 
            foreach (string file in files)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line == "")
                        {
                            continue;
                        }

                        wordTagPair = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                        for (int i = 0; i < wordTagPair.Length; i++)
                        {
                            WordTags wordTagSplit = SplitPair(wordTagPair[i]);

                            for (int j = 0; j < wordTagSplit.tags.Count; j++)
                            {
                                wordTagSplit.tags[j] = GetPartOfSpeech(wordTagSplit.tags[j]);
                            }

                            if (wordTagSplit.tags.Count == 0)
                            {
                                continue;
                            }

                            wordTagSplit.tags = wordTagSplit.tags.Distinct().ToList();
                            RemoveOtherTag(wordTagSplit);
                            AddWordTagViterbiArray(wordTagSplit);

                            if (i == 0)
                            {
                                initialState[GetTagPosition(wordTagSplit.tags[0])]++;
                            }
                        }
                    }
                }
            }

            transitionIntegerTempMatrix = new int[tagTrainArray.Count, tagTrainArray.Count];

            for (int i = 0; i < wordTagViterbiArray.Count - 1; i++)
            {
                transitionIntegerTempMatrix[GetTagPosition(wordTagViterbiArray[i].tags[0]), GetTagPosition(wordTagViterbiArray[i + 1].tags[0])]++;
            }

            WriteStatistics(files.Length);
            WriteMatrixToFile(files.Length);
        }
        private void SplitDoubleTaggedWords(WordTags word_tag_split)
        {
            string[] newWord = word_tag_split.word.Split('\'');

            if (newWord.Length == 2)
            {
                if (newWord[0] != "")
                {
                    //primul cuvant
                    AddWordInTrain(newWord[0]);
                    AddTagInTrain(word_tag_split.tags[0]);
                    countTotalWordsTrain++;
                }

                //al doilea cuvant
                AddWordInTrain(newWord[1]);
                AddTagInTrain(word_tag_split.tags[1]);
                countTotalWordsTrain++;
            }
            else
            {
                AddWordInTrain(word_tag_split.word);
                AddTagInTrain(word_tag_split.tags[0]);
                countTotalWordsTrain++;
            }
        }
        private void RemoveOtherTag(WordTags word_tag_split)
        {
            if (word_tag_split.tags.Count >= 2)
            {
                for (int j = 0; j < word_tag_split.tags.Count; j++)
                {
                    if (word_tag_split.tags[j] == "other")
                    {
                        word_tag_split.tags.Remove(word_tag_split.tags[j]);
                        j--;
                    }
                }
            }
        }
        private void AddWordInTrain(string word)
        {
            bool existsInArray = false;

            for (int j = 0; j < wordTrainArray.Count; j++)
            {
                if (wordTrainArray[j] == word)
                {
                    existsInArray = true;
                    break;
                }
            }
            if (!existsInArray)
            {
                wordTrainArray.Add(word);
            }

        }
        private void AddTagInTrain(string tag)
        {
            bool existsInArray = false;

            for (int j = 0; j < tagTrainArray.Count; j++)
            {
                if (tagTrainArray[j] == tag)
                {
                    existsInArray = true;
                    break;
                }
            }

            if (!existsInArray)
            {
                tagTrainArray.Add(tag);
            }
        }
        private string GetPartOfSpeech(string tag)
        {
            if (noun.Contains(tag))
            {
                return "noun";
            }
            else if (verb.Contains(tag))
            {
                return "verb";
            }
            else if (adjective.Contains(tag))
            {
                return "adjective";
            }
            else if (adverb.Contains(tag))
            {
                return "adverb";
            }
            else if (pronoun.Contains(tag))
            {
                return "pronoun";
            }
            else if (conjunction.Contains(tag))
            {
                return "conjunction";
            }
            else if (article.Contains(tag))
            {
                return "article";
            }
            else if (preposition.Contains(tag))
            {
                return "preposition";
            }
            else
            {
                return "other";
            }
        }
        private int GetWordPosition(string word)
        {
            for (int i = 0; i < wordTrainArray.Count; i++)
            {
                if (word == wordTrainArray[i])
                {
                    return i;
                }
            }
            return -1;
        }
        private string Predict(string word)
        {
            int wordPosition = GetWordPosition(word);
            int maxFrequence = -1;
            int maxFrequencePosition = 0;

            if (wordPosition == -1)
            {
                return "noun";
            }

            for (int i = 0; i < tagTrainArray.Count; i++)
            {
                if (emissionIntegerTempMatrix[wordPosition, i] > maxFrequence)
                {
                    maxFrequence = emissionIntegerTempMatrix[wordPosition, i];
                    maxFrequencePosition = i;
                }
            }

            return tagTrainArray[maxFrequencePosition];
        }
        private string PredictNoun(string word)
        {
            return "noun";
        }
        private static WordTags SplitPair(string word_tag_pair)
        {
            WordTags word_tag_return = new WordTags();
            string tagGroup;
            char[] charArray = word_tag_pair.ToCharArray();

            int charLength = charArray.Length - 1;
            int i = charLength;
            char[] tag = new char[20];

            while (charArray[i] != '/')
            {
                tag[charLength - i] = charArray[i];
                i--;
            }

            word_tag_return.word = word_tag_pair.Substring(0, i);
            Array.Reverse(tag);
            tagGroup = new string(tag).Trim('\0');
            word_tag_return.tags = new List<string>
                (tagGroup.Split(new char[] { '-', '+', '*', ' ', '`', '\'', '.', ',', ':', '(', ')', '$' },
                StringSplitOptions.RemoveEmptyEntries));

            return word_tag_return;
        }
        private int GetTagPosition(string tag)
        {
            for (int i = 0; i < tagTrainArray.Count; i++)
            {
                if (tag == tagTrainArray[i])
                {
                    return i;
                }
            }
            return -1;
        }
        private double GetFrequence(string value)
        {
            int valuePositions = GetTagPosition(value);
            int valueSum = 0;

            for (int i = 0; i < wordTrainArray.Count; i++)
            {
                valueSum += emissionIntegerTempMatrix[i, valuePositions];
            }

            Console.WriteLine("No. Appearances {0}: {1}", value, valueSum);
            return 1.0 * valueSum / countTotalNumberOfTagsInTrain;
            //return 1.0 * valueSum / countTotalWordsTrain;
        }
        private double GetTransitionMatrixFrequence(string value)
        {
            int valuePositions = GetTagPosition(value);
            int valueSum = 0;
            double lineSum = 0;

            for (int i = 0; i < tagTrainArray.Count; i++)
            {
                valueSum += transitionIntegerTempMatrix[valuePositions, i];
            }

            for (int i = 0; i < tagTrainArray.Count; i++)
            {
                transitionMatrix[valuePositions, i] = 1.0 * transitionIntegerTempMatrix[valuePositions, i] / valueSum;
                lineSum += transitionMatrix[valuePositions, i];
            }
            Console.WriteLine("Suma rand Transition Matrix {0}: {1}", value, valueSum);
            return 1.0 * lineSum;
            //return 1.0 * valueSum / countTotalWordsTrain;
        }
        int[] valueSum = new int[600000];
        private double GetEmissionMatrixFrequence(string value)
        {
            int valuePositions = GetTagPosition(value);
            int valueSum = 0;
            double columnSum = 0;


            for (int i = 0; i < wordTrainArray.Count; i++)
            {
                valueSum += emissionIntegerTempMatrix[i, valuePositions];
            }

            for (int i = 0; i < wordTrainArray.Count; i++)
            {
                emissionMatrix[i, valuePositions] = 1.0 * emissionIntegerTempMatrix[i, valuePositions] / valueSum;
                columnSum += emissionMatrix[i, valuePositions];
            }
            Console.WriteLine("Suma coloana Emission Matrix {0}: {1}", value, valueSum);
            return 1.0 * columnSum;

            //MATRICE PROBABILITATI DE EMISIE  
            //VALUESUM - suma pe linie - cuvantul apple care a aparut de mai multe ori cu parti de vorbire diferite
            /*int valuePositions = GetTagPosition(value);

            double columnSum = 0;

            for (int i = 0; i < wordTrainArray.Count; i++)
            {
                for (int j = 0; j < tagTrainArray.Count; j++)
                {
                    valueSum[i] += emissionIntegerTempMatrix[i, j];
                }

           // for (int i = 0; i < wordTrainArray.Count; i++)
            //{
                emissionMatrix[i, valuePositions] = 1.0 * emissionIntegerTempMatrix[i, valuePositions] / valueSum[i];
                columnSum += emissionMatrix[i, valuePositions];
            }
            Console.WriteLine("Suma coloana Emission Matrix {0}: {1}", value, valueSum);
            return 1.0 * columnSum; */
        }
        private void WriteStatistics(int noOfFiles)
        {
            double sumFrequence = 0;
            transitionMatrix = new double[tagTrainArray.Count, tagTrainArray.Count];
            emissionMatrix = new double[wordTrainArray.Count, tagTrainArray.Count];

            string fileName = "Statistics-" +
               DateTime.Now.Day + "D" +
               DateTime.Now.Month + "M" +
               DateTime.Now.Hour + "h" +
               DateTime.Now.Minute + "min-" +
               noOfFiles + ".txt";

            using (TextWriter tw = new StreamWriter(fileName))
            {
                tw.WriteLine("Number of total pairs tag-word:" + countTotalWordsTrain);
                tw.WriteLine("Number of total tags used for frq:" + countTotalNumberOfTagsInTrain);
                for (int i = 0; i < tagTrainArray.Count; i++)
                {
                    tw.WriteLine("{0} Percentage: {1}", tagTrainArray[i], GetFrequence(tagTrainArray[i]));
                    sumFrequence += GetFrequence(tagTrainArray[i]);
                }

                tw.WriteLine("Suma Frecvente: {0}", sumFrequence);
                tw.WriteLine();

                tw.WriteLine("Verificare suma pe fiecare linie - Matrice Tranzitie:");
                for (int i = 0; i < tagTrainArray.Count; i++)
                {
                    tw.WriteLine("{0} Suma pe rand: {1}", tagTrainArray[i], GetTransitionMatrixFrequence(tagTrainArray[i]));
                }
                tw.WriteLine();

                tw.WriteLine("Verificare suma pe fiecare linie/ coloana in cazul meu - Matrice Probabilitati?:");
                for (int i = 0; i < tagTrainArray.Count; i++)
                {
                    tw.WriteLine("{0} Suma pe coloana: {1}", tagTrainArray[i], GetEmissionMatrixFrequence(tagTrainArray[i]));
                }
            }
        }
        private void WritePredictionStatistics(int sumAll)
        {
            string fileName;
            if (selectAlgoritm.SelectedItem == "Predictor Frecvente")
            {
                //fileName = "Noun-Prediction-Statistics" +
                fileName = "Frequence-Prediction-Statistics" +
                            DateTime.Now.Day + "D" +
                            DateTime.Now.Month + "M" +
                            DateTime.Now.Hour + "h" +
                            DateTime.Now.Minute + "min-" +
                            150 + ".txt";
            }
            else
            {
                fileName = "Viterbi-Prediction-Statistics" +
                            DateTime.Now.Day + "D" +
                            DateTime.Now.Month + "M" +
                            DateTime.Now.Hour + "h" +
                            DateTime.Now.Minute + "min-" +
                            150 + ".txt";
            }

            double maAccuracy = 0;
            double maPrecision = 0;
            double maRecall = 0;
            double maFMeasure = 0;
            int sumTP = 0;


            using (TextWriter tw = new StreamWriter(fileName))
            {
                for (int i = 0; i < partsOfSpeech.Count; i++)
                {
                    double accuracy = ((contorTP[i] + contorFN[i] + contorFP[i] + contorTN[i]) == 0) ? 0 : 1.0 * (contorTP[i] + contorTN[i]) / (contorTP[i] + contorFN[i] + contorFP[i] + contorTN[i]);
                    double precision = ((contorTP[i] + contorFP[i]) == 0) ? 0 : 1.0 * contorTP[i] / (contorTP[i] + contorFP[i]);
                    double recall = ((contorTP[i] + contorFN[i]) == 0) ? 0 : 1.0 * contorTP[i] / (contorTP[i] + contorFN[i]);
                    double fMeasure = ((precision + recall) == 0) ? 0 : 2 * (precision * recall / (precision + recall));
                    maAccuracy += accuracy;
                    maPrecision += precision;
                    maRecall += recall;
                    maFMeasure += fMeasure;
                    sumTP += contorTP[i];
                    sumAll = contorTN[i] + contorTP[i] + contorFN[i] + contorFP[i];

                    tw.WriteLine("Accuracy {0}, {1} ", partsOfSpeech[i], accuracy.ToString());
                    tw.WriteLine("Precision {0}: {1} ", partsOfSpeech[i], precision.ToString());
                    tw.WriteLine("Recall {0}: {1} ", partsOfSpeech[i], recall.ToString());
                    tw.WriteLine("F-Measure {0}: {1} ", partsOfSpeech[i], fMeasure.ToString());
                    tw.WriteLine();
                }

                tw.WriteLine("Media Aritmetica Accuracy: " + (1.0 * maAccuracy / partsOfSpeech.Count).ToString());
                tw.WriteLine("Media Aritmetica Precision: " + (1.0 * maPrecision / partsOfSpeech.Count).ToString());
                tw.WriteLine("Media Aritmetica Recall: " + (1.0 * maRecall / partsOfSpeech.Count).ToString());
                tw.WriteLine("Media Aritmetica F-Measure: " + (1.0 * maFMeasure / partsOfSpeech.Count).ToString());
                tw.WriteLine("Accuracy 2: " + (1.0 * sumTP / sumAll).ToString());
            }
        }
        private void WriteMatrixToFile(int noOfFiles)
        {
            double sumInitialState = 0;
            string fileName = "Matrix-" +
               DateTime.Now.Day + "D" +
               DateTime.Now.Month + "M" +
               DateTime.Now.Hour + "h" +
               DateTime.Now.Minute + "min-" +
               noOfFiles + ".txt";

            using (TextWriter tw = new StreamWriter(fileName))
            {
                for (int i = 0; i < wordTrainArray.Count; i++)
                {
                    tw.Write(wordTrainArray[i] + " ");
                }
                tw.WriteLine();

                for (int i = 0; i < tagTrainArray.Count; i++)
                {
                    tw.Write(tagTrainArray[i] + " ");
                }
                tw.WriteLine();

                for (int i = 0; i < wordTrainArray.Count; i++)
                {
                    for (int j = 0; j < tagTrainArray.Count; j++)
                    {

                        tw.Write(emissionIntegerTempMatrix[i, j] + " ");
                    }
                    tw.WriteLine();
                }
                tw.WriteLine();

                for (int i = 0; i < tagTrainArray.Count; i++)
                {
                    sumInitialState += initialState[i];
                }

                tw.WriteLine("Initial State");
                for (int i = 0; i < tagTrainArray.Count; i++)
                {
                    tw.Write(1.0 * initialState[i] / sumInitialState + " ");
                }
                tw.WriteLine();
                tw.WriteLine();

                tw.WriteLine("Matricea de Transitie");
                for (int i = 0; i < tagTrainArray.Count; i++)
                {
                    for (int j = 0; j < tagTrainArray.Count; j++)
                    {
                        tw.Write(transitionMatrix[i, j] + " ");
                    }
                    tw.WriteLine();
                }

                tw.WriteLine();

                tw.WriteLine("Matricea de Emisie");
                for (int i = 0; i < wordTrainArray.Count; i++)
                {
                    for (int j = 0; j < tagTrainArray.Count; j++)
                    {
                        tw.Write(emissionMatrix[i, j] + " ");
                    }
                    tw.WriteLine();
                }
            }
        }
        private void readMatrix(StreamReader reader)
        {
            emissionIntegerTempMatrix = new int[wordTrainArray.Count, tagTrainArray.Count];

            for (int i = 0; i < wordTrainArray.Count; i++)
            {
                string line = reader.ReadLine();
                string[] lineSplit = line.Split(spaceSeparator, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < tagTrainArray.Count; j++)
                {
                    emissionIntegerTempMatrix[i, j] = Convert.ToInt32(lineSplit[j]);
                }
            }
        }
        private void readWordTagArray(StreamReader reader, bool isWord)
        {
            string line = reader.ReadLine();
            string[] lineSplit = line.Split(spaceSeparator, StringSplitOptions.RemoveEmptyEntries);

            if (isWord)
            {
                for (int i = 0; i < lineSplit.Length; i++)
                {
                    AddWordInTrain(lineSplit[i]);
                }
            }
            else
            {
                for (int i = 0; i < lineSplit.Length; i++)
                {
                    AddTagInTrain(lineSplit[i]);
                }
            }
        }
        private void readInitialState(StreamReader reader)
        {
            string line = reader.ReadLine();
            string[] lineSplit = line.Split(spaceSeparator, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lineSplit.Length; i++)
            {
                initialState[i] = Convert.ToDouble(lineSplit[i]);
            }

        }
        private void readTransitionMatrix(StreamReader reader)
        {
            transitionMatrix = new double[tagTrainArray.Count, tagTrainArray.Count];

            for (int i = 0; i < tagTrainArray.Count; i++)
            {
                string line = reader.ReadLine();
                string[] lineSplit = line.Split(spaceSeparator, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < tagTrainArray.Count; j++)
                {
                    transitionMatrix[i, j] = Convert.ToDouble(lineSplit[j]);
                }
            }
        }
        private void readEmissionMatrix(StreamReader reader)
        {
            emissionMatrix = new double[wordTrainArray.Count, tagTrainArray.Count];

            for (int i = 0; i < wordTrainArray.Count; i++)
            {
                string line = reader.ReadLine();
                string[] lineSplit = line.Split(spaceSeparator, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < tagTrainArray.Count; j++)
                {
                    emissionMatrix[i, j] = Convert.ToDouble(lineSplit[j]);
                }
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}


