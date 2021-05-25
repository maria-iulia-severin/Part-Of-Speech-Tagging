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
        }
        //Functia aceasta ia perechea word-tag si modifica tag-ul cu cele 9 categ.
        private void AddWordTagTestArray(WordTags wordTag)
        {
            WordTags finalWordTag = new WordTags();
            finalWordTag.word = wordTag.word;
            finalWordTag.tags = new List<string>();

            foreach (string tag in wordTag.tags)
            {
                string getTagPoS = GetPartOfSpeech(tag);

                if (!finalWordTag.tags.Contains(getTagPoS))
                {
                    finalWordTag.tags.Add(getTagPoS);
                    //predictie
                }
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
                    countTotalWordsTrain += word_tag_pair.Length;

                    for (int i = 0; i < word_tag_pair.Length; i++)
                    {
                        WordTags word_tag_split = SpitPair(word_tag_pair[i]);

                        AddWordInTrain(word_tag_split.word);

                        for (int j = 0; j < word_tag_split.tags.Count; j++)
                        {
                            AddTagInTrain(word_tag_split.tags[j]);
                        }
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
        private string Predict(string word)
        {
            int wordPosition = GetWordPosition(word);
            int maxFrequence = -1;
            int maxFrequencePosition = 0;

            for (int i = 0; i < tagTrainArray.Count; i++)
            {
                if (wordPosition == -1)
                {
                    return "unfound";
                }

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

                /*tw.WriteLine("Verb Percentage: " + GetFrequence(verb, nameof(verb)));
                tw.WriteLine("Adjective Percentage: " + GetFrequence(adjective, nameof(adjective)));
                tw.WriteLine("Article Percentage: " + GetFrequence(article, nameof(article)));
                tw.WriteLine("Adverb Percentage: " + GetFrequence(adverb, nameof(adverb)));
                tw.WriteLine("Conjuction Percentage: " + GetFrequence(conjunction, nameof(conjunction)));
                tw.WriteLine("Preposition Percentage: " + GetFrequence(preposition, nameof(preposition)));
                tw.WriteLine("Pronoun Percentage: " + GetFrequence(pronoun, nameof(pronoun)));
                tw.WriteLine("Other Percentage: " + GetFrequence(other, nameof(other)));*/
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
                    AddTagInTrain(lineSplit[i]);
                }
            }
        }
        private void btnPredict_Click(object sender, EventArgs e)
        {
            int[] contorTP = new int[partsOfSpeech.Count];
            int[] contorTN = new int[partsOfSpeech.Count];
            int[] contorFP = new int[partsOfSpeech.Count];
            int[] contorFN = new int[partsOfSpeech.Count];

            // int ctwordTag = 0;
            ReadBrownTest();
            foreach (WordTags wordTag in wordTagTestArray)
            {
                // Console.WriteLine("{0}: {1}", word, Predict(word));
                string predictedTag = PredictNoun(wordTag.word);
                Console.WriteLine("{0}: {1}", wordTag.word, PredictNoun(wordTag.word));

                foreach (string realTag in wordTag.tags)
                {
                    if(predictedTag == realTag)
                    {
                        contorTP[partsOfSpeech.IndexOf(realTag)]++;
                        //foreach part of Speach ++ TN 
                    }
                    else
                    {
                        contorFN[partsOfSpeech.IndexOf(predictedTag)]++;
                        contorFP[partsOfSpeech.IndexOf(realTag)]++;
                    }

                    //if ((predictionNoun == "noun") & (predictionNoun == tag))
                    //{
                    //    contorTP++;
                    //}
                    //if ((predictionNoun != "noun") & (predictionNoun != tag))
                    //{
                    //    contorTN++;
                    //}
                    //if ((predictionNoun == "noun") & (predictionNoun != tag))
                    //{
                    //    contorFP++;
                    //}
                    //if ((predictionNoun != "noun") & (predictionNoun == tag))
                    //{
                    //    contorFN++;
                    //}
                }
            }

            MessageBox.Show("Total True Positive Nouns: " + contorTP.ToString() + "\n"
                            + "Total False Negative Nouns: " + contorFN.ToString() + "\n"
                            + "Total False Positive Nouns: " + contorFP.ToString() + "\n"
                            + "Total True Negative Nouns: " + contorTN.ToString());

        }
    }
}
