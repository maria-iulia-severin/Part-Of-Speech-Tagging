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
        string[] adverb = { "rb", "rbr", "rbt", "rn", "rp", "wrb", "nr" };
        string[] pronoun = { "pn", "pp", "ppl", "ppls", "ppo", "pps", "ppss", "wp", "wpo", "wps" };
        string[] conjunction = { "cs", "cc" };
        string[] article = { "at" };
        string[] preposition = { "in", "to" };
        string[] other = { "uh", "abl", "abn", "abx", "ap", "cd", "dt", "dti", "dts", "dtx", "ex", "fw", "od", "ql", "qlp", "wdt", "wql", "nc", "hl", "tl" };

        List<String> partsOfSpeech = new List<string> { "noun", "conjunction", "other", "verb", "preposition", "article", "adjective", "pronoun", "adverb" };
        List<String> wordTrainArray = new List<string>();
        List<String> tagTrainArray = new List<string>();

        List<WordTags> wordTagTestArray = new List<WordTags>();
        int[] contorTP = new int[9];
        int[] contorTN = new int[9];
        int[] contorFP = new int[9];
        int[] contorFN = new int[9];
        int[,] emissionIntegerTempMatrix;
        double[,] emissionMatrix;
        int[,] transitionIntegerTempMatrix;
        double[,] transitionMatrix;

        int countTotalWordsTrain = 0;
        int countTotalNumberOfTagsInTrain = 0; // numar total etichetari pt a dat 1 pe linie

        char[] separators = new char[] { ' ', '\r', '\n', '\t' };
        char[] spaceSeparator = new char[] { ' ' };
        public Spatiu_de_reprezentare()
        {
            InitializeComponent();
            panelLeft.Height = btnReadCorpus.Height;
            panelLeft.Top = btnReadCorpus.Top;
        }
        double[] initialState = new double[9];
        double[] currentProbability = new double[9];
        double[] pastProbability = new double[9];
        double[] transitions = new double[9];

        // string[] testViterbi = { "Sara", "had", "dinner", "quickly" };
        int distanceUnit = 1;
        private void btnReadCorpus_Click(object sender, EventArgs e)
        {
            panelLeft.Height = btnReadCorpus.Height;
            panelLeft.Top = btnReadCorpus.Top;
            ReadCorpus();
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
            panelLeft.Height = btnPredict.Height;
            panelLeft.Top = btnPredict.Top;
            ReadBrownTest();

            foreach (WordTags wordTag in wordTagTestArray) // for .... wordTag[i].word
            {
                //string predictedTag = PredictNoun(wordTag.word);
                string predictedTag = Predict(wordTag.word);
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
        string[] testViterbi = new string[100];
        int index = 1;
        string[] lineSplit= new string[100];
        private void btn_Viterbi_Click(object sender, EventArgs e)
        {
            panelLeft.Height = btn_Viterbi.Height;
            panelLeft.Top = btn_Viterbi.Top;

            if (index < testViterbi.Length)
            {
                testViterbi[index] = Convert.ToString(customTextBox1.Text);
                index++;
            }

            for (int i = 0; i < testViterbi.Count(); i++)
            {
                 lineSplit = testViterbi[i].Split(spaceSeparator, StringSplitOptions.RemoveEmptyEntries);
            }

            for (int i = 0; i < lineSplit.Count(); i++)
            {

                string test = HiddenMarkovModel(lineSplit[i], i);


                CircularButton circle = new CircularButton();
                this.Controls.Add(circle);
                distanceUnit += 1;
                if (test == "noun")
                {
                    circle.BackColor = Color.FromArgb(87, 127, 180);
                }
                else if (test == "verb")
                {
                    circle.BackColor = Color.FromArgb(0, 194, 203);
                }
                else if (test == "adjective")
                {
                    circle.BackColor = Color.FromArgb(56, 182, 255);
                }
                else if (test == "adverb")
                {
                    circle.BackColor = Color.FromArgb(242, 209, 201);
                }
                else if (test == "pronoun")
                {
                    circle.BackColor = Color.FromArgb(175, 179, 247);
                }
                else if (test == "conjunction")
                {
                    circle.BackColor = Color.FromArgb(145, 215, 223);
                }
                else if (test == "article")
                {
                    circle.BackColor = Color.FromArgb(162, 250, 163);
                }
                else if (test == "preposition")
                {
                    circle.BackColor = Color.FromArgb(188, 217, 121);
                }
                else
                {
                    circle.BackColor = Color.FromArgb(91, 108, 93);
                }

                circle.Enabled = false;
                circle.FlatAppearance.BorderSize = 0;
                circle.FlatStyle = FlatStyle.Flat;
                circle.ForeColor = Color.White;
                circle.Font = new Font("Century Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                // circle.Location = new System.Drawing.Point(251, 183);
                circle.Name = "circularButton1";
                circle.Size = new Size(124, 116);
                circle.TabIndex = 10;
                circle.Text = test;
                //  circle.UseVisualStyleBackColor = false;
                circle.Top = 177;
                circle.Left = distanceUnit * 130;
            }
        }
        private string HiddenMarkovModel(string word, int position)
        {
            int pos;
            int word0;
            int wordn;
            if (position == 0)
            {
                for (int i = 0; i < partsOfSpeech.Count; i++)
                {
                    pos = GetTagPosition(partsOfSpeech[i]);
                    word0 = GetWordPosition(word);
                    currentProbability[i] = ViterbiS0(word0, pos);
                }
                CopyCurrentProbabilityToLastProbability();
                Console.WriteLine(word + " " + GetPoSFromCurrentProbability(currentProbability.Max()));
                return GetPoSFromCurrentProbability(currentProbability.Max());
            }
            else
            {
                wordn = GetWordPosition(word);
                for (int j = 0; j < partsOfSpeech.Count; j++)
                {
                    pos = GetTagPosition(partsOfSpeech[j]);
                    currentProbability[j] = ViterbiSn(wordn, pos);
                }
                CopyCurrentProbabilityToLastProbability();
                Console.WriteLine(word + " " + GetPoSFromCurrentProbability(currentProbability.Max()));
                return GetPoSFromCurrentProbability(currentProbability.Max());
            }
        }
        private string GetPoSFromCurrentProbability(double probability)
        {
            for (int i = 0; i < partsOfSpeech.Count; i++)
            {
                if (currentProbability[i] == probability)
                {
                    return tagTrainArray[i];
                }
            }
            return "unfound";
        }
        private double ViterbiS0(int word, int posCurrent)
        {
            //currentProbability[pos] 
            //ce fac cand nu am cuvantul ?
            return emissionMatrix[word, posCurrent] * initialState[posCurrent];
        }
        private double ViterbiSn(int word, int posCurrent)
        {
            int posPast;

            foreach (string pos2 in partsOfSpeech)
            {
                posPast = GetTagPosition(pos2);
                transitions[posPast] = emissionMatrix[word, posCurrent] * transitionMatrix[posPast, posCurrent];
            }

            double[] tempProbbility = new double[9];
            for (int i = 0; i < partsOfSpeech.Count; i++)
            {
                tempProbbility[i] = pastProbability[i] * transitions[i];
            }
            return tempProbbility.Max();
        }
        private void CopyCurrentProbabilityToLastProbability()
        {
            for (int i = 0; i < partsOfSpeech.Count; i++)
            {
                pastProbability[i] = currentProbability[i];
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

            //VITERBI 
            foreach (string file in files)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    string textInFile = reader.ReadToEnd();

                    wordTagPair = textInFile.Split(separators, StringSplitOptions.RemoveEmptyEntries);

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
                        AddWordTagTestArray(wordTagSplit);
                    }
                }
            }

            for (int i = 0; i < tagTrainArray.Count; i++)
            {
                initialState[i] = GetFrequence(tagTrainArray[i]);
            }

            transitionIntegerTempMatrix = new int[tagTrainArray.Count, tagTrainArray.Count];

            for (int i = 0; i < wordTagTestArray.Count - 1; i++)
            {
                transitionIntegerTempMatrix[GetTagPosition(wordTagTestArray[i].tags[0]), GetTagPosition(wordTagTestArray[i + 1].tags[0])]++;
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
            //MATRICE PROBABILITATI DE EMISIE - 
            //VALUESUM - suma pe coloana - numarul total de cuvinte care au aparut ca substantiv
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
            //string fileName = "Noun-Prediction-Statistics" +
            string fileName = "Frequence-Prediction-Statistics" +
               DateTime.Now.Day + "D" +
               DateTime.Now.Month + "M" +
               DateTime.Now.Hour + "h" +
               DateTime.Now.Minute + "min-" +
               150 + ".txt";

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

                tw.WriteLine("Initial State");
                for (int i = 0; i < tagTrainArray.Count; i++)
                {
                    tw.Write(initialState[i] + " ");
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
    }
}


