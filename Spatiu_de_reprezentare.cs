using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;

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

        List<String> partsOfSpeech = new List<string> { "noun", "verb", "adjective", "adverb", "pronoun", "conjunction", "article", "preposition", "other" };

        List<String> wordTrainArray = new List<string>();
        List<String> tagTrainArray = new List<string>();

        List<String> wordTestArray = new List<string>();
        List<String> tagTestArray = new List<string>();

        List<WordTags> wordTagTestArray = new List<WordTags>();
        int[] contorTP = new int[9];
        int[] contorTN = new int[9];
        int[] contorFP = new int[9];
        int[] contorFN = new int[9];
        int[,] matrix;
        // int wordCount = 0; // de sters
        //int tagCount = 0; // de sters
        int countTotalWordsTrain = 0;
        int countTotalWordsTest = 0;

        public Spatiu_de_reprezentare()
        {
            InitializeComponent();
            //ReadCorpus();
            //Console.WriteLine("Prediction:" + Predict("me"));
        }
        private void btnReadCorpus_Click(object sender, EventArgs e)
        {
            ReadCorpus();
        }
        private void ReadBrownTest()
        {
            string rootPath = @"C:\Users\iulia.severin\source\repos\POS-Tagging\bin\Debug\Brown_Test";
            var files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
            string[] wordTagPair;
            char[] separators = new char[] { ' ', '\r', '\n', '\t' };
            int ct = 0;
            //Add pair Word-Tags in array
            foreach (string file in files)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    string textInFile = reader.ReadToEnd();

                    wordTagPair = textInFile.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    countTotalWordsTest += wordTagPair.Length;

                    for (int i = 0; i < wordTagPair.Length; i++)
                    {
                        WordTags wordTagSplit = SpitPair(wordTagPair[i]);
                        ct++;
                        //wordTagTestArray.Add(wordTagSplit);
                        AddWordTagTestArray(wordTagSplit);

                        /*                        AddWordInTest(wordTagSplit.word);

                                                for (int j = 0; j < wordTagSplit.tags.Count; j++)
                                                {
                                                    AddTagInTest(wordTagSplit.tags[j]);
                                                }*/
                    }

                }

            }


            Console.WriteLine(ct);
        }

        //Functia aceasta ia perechea word-tag si modifica tag-ul cu cele 9 categ apoi le adauga .Word si .Tag[i]
        private void AddWordTagTestArray(WordTags wordTag)
        {
            WordTags finalWordTag = new WordTags();
            finalWordTag.word = wordTag.word;
            finalWordTag.tags = new List<string>();

            foreach (string tag in wordTag.tags)
            {
                string realTag = GetPartOfSpeech(tag);

                if (!finalWordTag.tags.Contains(realTag))
                {
                    finalWordTag.tags.Add(realTag);

                }
            }

            //cuvinte fara tag - ex punct virgula
            if (finalWordTag.tags.Count == 0)
            {
                return;
            }

            wordTagTestArray.Add(finalWordTag);
        }
        private void ReadCorpus()
        {
            //string rootPath = @"C:\Users\iulia.severin\source\repos\POS-Tagging\bin\Debug\Brown";
            string rootPath = @"C:\Users\iulia.severin\source\repos\POS-Tagging\bin\Debug\Brown_Train";
            //string rootPath = @"C:\Users\iulia.severin\source\repos\POS-Tagging\bin\Debug\Brown_Test";
            var files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
            string[] word_tag_pair;
            char[] separators = new char[] { ' ', '\r', '\n', '\t' };

            //Add pair Word-Tags in array
            foreach (string file in files)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    string text_in_file = reader.ReadToEnd();

                    word_tag_pair = text_in_file.Split(separators, StringSplitOptions.RemoveEmptyEntries); //Split
                    

                    for (int i = 0; i < word_tag_pair.Length; i++)
                    {
                        WordTags word_tag_split = SpitPair(word_tag_pair[i]);

                        //Decomenteaza cand generezi statistici pentru Brown_Test
                        //if (word_tag_split.tags.Count == 0)
                        //{
                        //    continue;
                        //}

                        AddWordInTrain(word_tag_split.word);

                        for (int j = 0; j < word_tag_split.tags.Count; j++)
                        {
                            AddTagInTrain(word_tag_split.tags[j]);
                        }
                        countTotalWordsTrain++;
                    }
                }
            }

            matrix = new int[wordTrainArray.Count, tagTrainArray.Count];

            //Add Tag - Words in Matrix
            foreach (string file in files)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    string text_in_file = reader.ReadToEnd();
                    word_tag_pair = text_in_file.Split(separators, StringSplitOptions.RemoveEmptyEntries); //Split

                    for (int i = 0; i < word_tag_pair.Length; i++)
                    {
                        WordTags word_tag_split = SpitPair(word_tag_pair[i]);

                        //Decomenteaza cand generezi statistici pentru Brown_Test
                        //if (word_tag_split.tags.Count == 0)
                        //{
                        //    //Console.WriteLine(word_tag_split.word + " " + word_tag_pair[i]);
                        //    continue;
                        //}

                        for (int j = 0; j < word_tag_split.tags.Count; j++)
                        {
                            matrix[GetWordPosition(word_tag_split.word), GetTagPosition(word_tag_split.tags[j])]++;
                        }
                    }
                }
            }

            WriteStatistics(files.Length);
            WriteMatrixToFile(files.Length);
        }
        private void AddWordInTrain(string word)
        {
            bool existsInArray = false;
            //char[] charsToTrim = { '\'', '$', ' ', '.', '-' };

            for (int j = 0; j < wordTrainArray.Count; j++)
            {
                if (wordTrainArray[j] == word)
                {
                    existsInArray = true;
                    break;
                }
            }
            if (!existsInArray)
            // && !IgnoredSymbols(word))
            {

                //string word_trim = word.TrimEnd('\'');
                //word_array[word_count] = word;
                wordTrainArray.Add(word);
            }
        }
        private void AddTagInTrain(string tag)
        {
            bool existsInArray = false;
            tag = GetPartOfSpeech(tag);

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
        //la Load, deja tag-urile sunt schimbate si trebuia sa evit un pas din functia de mai sus
        private void AddTagInTrainLoad(string tag)
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
        private void AddWordInTest(string word)
        {
            bool existsInArray = false;

            for (int j = 0; j < wordTestArray.Count; j++)
            {
                if (wordTestArray[j] == word)
                {
                    existsInArray = true;
                    break;
                }
            }
            if (!existsInArray)
            {
                wordTestArray.Add(word);
            }
        }
        private void AddTagInTest(string tag)
        {
            bool existsInArray = false;
            tag = GetPartOfSpeech(tag);

            for (int j = 0; j < tagTestArray.Count; j++)
            {
                if (tagTestArray[j] == tag)
                {
                    existsInArray = true;
                    break;
                }
            }

            if (!existsInArray)
            {
                tagTestArray.Add(tag);
            }
        }
        private int GetTagPosition(string tag)
        {
            tag = GetPartOfSpeech(tag);
            for (int i = 0; i < tagTrainArray.Count; i++)
            {
                if (tag == tagTrainArray[i])
                {
                    return i;
                }
            }
            return -1;
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
        int ctunfound = 0;
        private string Predict(string word)
        {
            int wordPosition = GetWordPosition(word);
            int maxFrequence = -1;
            int maxFrequencePosition = 0;

            if (wordPosition == -1)
            {
                ctunfound++;
                return "noun";
            }

            for (int i = 0; i < tagTrainArray.Count; i++)
            {
                if (matrix[wordPosition, i] > maxFrequence)
                {
                    maxFrequence = matrix[wordPosition, i];
                    maxFrequencePosition = i;
                }
            }
         
            return tagTrainArray[maxFrequencePosition];
        }
        private string PredictNoun(string word)
        {
            return "noun";
        }
        private static WordTags SpitPair(string word_tag_pair)
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

        //Functie care ia pozitia tag-ului din tag_array.
        private int GetTagIndexes(string value)
        {
            int positions = -1;

            for (int i = 0; i < value.Length; i++)
            {
                for (int j = 0; j < tagTrainArray.Count; j++)
                {
                    if (value == tagTrainArray[j])
                    {
                        positions = j;
                        //   tag_array[j] = "";
                        break;
                    }
                }
            }
            return positions;
        }
        private double GetFrequence(string value)
        {

            int valuePositions = GetTagIndexes(value);
            int valueSum = 0;

            for (int i = 0; i < wordTrainArray.Count; i++)
            {
                valueSum += matrix[i, valuePositions];
            }

            Console.WriteLine("No. Appearances {0}: {1}", value, valueSum);
            return 1.0 * valueSum / countTotalWordsTrain;
        }
        private void WriteStatistics(int noOfFiles)
        {
            string fileName = "Statistics-" +
               DateTime.Now.Day + "D" +
               DateTime.Now.Month + "M" +
               DateTime.Now.Hour + "h" +
               DateTime.Now.Minute + "min-" +
               noOfFiles + ".txt";

            using (TextWriter tw = new StreamWriter(fileName))
            {
                tw.WriteLine("Number of total pairs tag-word:" + countTotalWordsTrain);
                for (int i = 0; i < partsOfSpeech.Count; i++)
                {
                    tw.WriteLine("{0} Percentage: {1}", partsOfSpeech[i], GetFrequence(partsOfSpeech[i]));
                }
            }
        }
        private void WritePredictionStatistics(int sumAll)
        {
            string fileName = "Noun-Prediction-Statistics" +
            //string fileName = "Frequence-Prediction-Statistics" +
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

                        tw.Write(matrix[i, j] + " ");
                    }
                    tw.WriteLine();
                }
            }
        }
        private void readMatrix(StreamReader reader)
        {
            char[] separators = new char[] { ' ' };
            matrix = new int[wordTrainArray.Count, tagTrainArray.Count];

            for (int i = 0; i < wordTrainArray.Count; i++)
            {
                string line = reader.ReadLine();
                string[] lineSplit = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < tagTrainArray.Count; j++)
                {
                    matrix[i, j] = Convert.ToInt32(lineSplit[j]);
                }
            }
        }
        private void readWordTagArray(StreamReader reader, bool isWord)
        {
            char[] separators = new char[] { ' ' };
            string line = reader.ReadLine();
            string[] lineSplit = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

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
                    AddTagInTrainLoad(lineSplit[i]);
                }
            }
        }
        private void loadMatrixFile_Click(object sender, EventArgs e)
        {
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
                    }
                }
            }

            MessageBox.Show("File loaded succesfully!");
        }
        private void btnPredict_Click(object sender, EventArgs e)
        {
            ReadBrownTest();

            foreach (WordTags wordTag in wordTagTestArray)
            {
                string predictedTag = PredictNoun(wordTag.word);
                //string predictedTag = Predict(wordTag.word);
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

            Console.WriteLine(wordTagTestArray.Count);
            WritePredictionStatistics(wordTagTestArray.Count);
            Console.WriteLine("Contor unfound: " + ctunfound);
        }
    }
}


