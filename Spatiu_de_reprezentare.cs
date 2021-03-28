using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace POS_Tagging
{
    public partial class Spatiu_de_reprezentare : Form
    {
        string[] noun = { "nn", "nn$", "nns", "nns$", "np", "np$", "nps", "nps$", "nrs" };
        string[] verb = { " " };
        string[] adjective = { " " };
        string[] adverb = { " " };
        string[] pronoun = { " " };
        string[] conjunction = { " " };
        string[] article = { " " };
        string[] preposition = { " " };
        string[] other = { " " };
        //ingore symbols - filter
        private bool IgnoredSymbols(string word)
        {
            string[] ignored_symbols = { "''", "'", ",", ":", ".", "(", ")", "``", "?", "!", ";", "--" };
            int length = ignored_symbols.Length;

            for (int i = 0; i < length; i++)
            {
                //if contains this symbol, ignore all ? pt --hl --tl
                if (word == ignored_symbols[i])
                {
                    return true;
                }
            }
            return false;
        }

        public Spatiu_de_reprezentare()
        {
            InitializeComponent();
            ReadCorpus();
            Console.WriteLine(Predict("shelter"));
        }

        string[] word_array = new string[60000];
        string[] tag_array = new string[600];
        int[,] matrix = new int[60000, 600];
        int word_count = 0;
        int tag_count = 0;
        int count_words = 0;
        public void ReadCorpus()
        {
            string rootPath = @"C:\Users\iulia.severin\source\repos\POS-Tagging\bin\Debug\brown";
            var files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
            string[] word_tag_pair; //
            char[] separators = new char[] { ' ', '\r', '\n', '\t' };

            //Tag - Words in array
            foreach (string file in files)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    string text_in_file = reader.ReadToEnd();

                    word_tag_pair = text_in_file.Split(separators, StringSplitOptions.RemoveEmptyEntries); //Split
                    count_words += word_tag_pair.Length;

                    for (int i = 0; i < word_tag_pair.Length; i++)
                    {
                        //string[] word_tag_split = word_tag_pair[i].Split('/');
                        WordTags word_tag_split = SpitPair(word_tag_pair[i]);

                        //add words
                        AddWord(word_tag_split.word);
                        for (int j = 0; j < word_tag_split.tags.Count; j++)
                        {
                            //add tags
                            AddTag(word_tag_split.tags[j]);
                        }

                    }
                }
            }
            Console.WriteLine(count_words);


            //Tag - Words in Matrix
            foreach (string file in files)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    string text_in_file = reader.ReadToEnd();

                    word_tag_pair = text_in_file.Split(separators, StringSplitOptions.RemoveEmptyEntries); //Split

                    for (int i = 0; i < word_tag_pair.Length; i++)
                    {
                        //string[] word_tag_split = word_tag_pair[i].Split('/');
                        WordTags word_tag_split = SpitPair(word_tag_pair[i]);
                        for (int j = 0; j < word_tag_split.tags.Count; j++)
                        {
                            matrix[GetWordPosition(word_tag_split.word), GetTagPosition(word_tag_split.tags[j])]++;
                        }
                    }
                }
            }


            Console.WriteLine(GetNounFrequence());
            /* for (int i = 0; i < word_array.Length; i++)
             {
                 for (int j = 0; j < tag_array.Length; j++)
                 {
                     Console.Write(matrix[i, j] + " ");
                 }
                 Console.Write("\n");
             }
             for (int i = 0; i < tag_array.Length; i++)
             {
                 Console.WriteLine(tag_array[i]);
             }*/
        }
        private string Predict(string word)
        {
            int wordPosition = GetWordPosition(word);
            int maxFrequence = -1;
            int maxFrequencePosition = 0;
            for (int i = 0; i < tag_array.Length; i++)
            {
                if (matrix[wordPosition, i] > maxFrequence)
                {
                    maxFrequence = matrix[wordPosition, i];
                    maxFrequencePosition = i;
                }
            }
            return tag_array[maxFrequencePosition];
        }
        private double GetNounFrequence()
        {
            int[] nounPositions = GetNounIndexes();
            int nounsSum = 0;
            for (int i = 0; i < word_array.Length; i++)
            {
                for (int j = 0; j < nounPositions.Length; j++)
                {
                    nounsSum += matrix[i, nounPositions[j]];
                }
            }
            Console.WriteLine(nounsSum);
            Console.WriteLine(count_words);
            return 1.0 * nounsSum / count_words;
        }

        private int[] GetNounIndexes()
        {
            int[] positions = new int[noun.Length];

            for (int i = 0; i < noun.Length; i++)
            {
                for (int j = 0; j < tag_array.Length; j++)
                {
                    if (noun[i] == tag_array[j])
                    {
                        positions[i] = j;
                        break;
                    }
                }
            }
            return positions;
        }

        private int GetTagPosition(string tag)
        {
            for (int i = 0; i < tag_array.Length; i++)
            {
                if (tag == tag_array[i])
                {
                    return i;
                }
            }
            return -1;
        }
        private int GetWordPosition(string word)
        {
            for (int i = 0; i < word_array.Length; i++)
            {
                if (word == word_array[i])
                {
                    return i;
                }
            }
            return -1;
        }
        private static WordTags SpitPair(string word_tag_pair)
        {

            //string[] word_tag_return = new string[2];
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
            word_tag_return.tags = new List<string>(tagGroup.Split(new char[] { '-', '+', '*', ' ' }, StringSplitOptions.RemoveEmptyEntries));

            return word_tag_return;
        }
        private void AddWord(string word)
        {
            bool exists_in_array = false;
            //char[] charsToTrim = { '\'', '$', ' ', '.', '-' };
            for (int j = 0; j < word_count; j++)
            {
                if (word_array[j] == word)
                {
                    exists_in_array = true;
                    break;
                }
            }
            if (!exists_in_array)
            // && !IgnoredSymbols(word))
            {

                //string word_trim = word.TrimEnd('\'');
                word_array[word_count] = word;
                word_count++;
            }
        }

        private void AddTag(string tag)
        {
            bool exists_in_array = false;
            for (int j = 0; j < tag_count; j++)
            {
                if (tag_array[j] == tag)
                {
                    exists_in_array = true;
                    break;
                }
            }
            if (!exists_in_array)
            // && !IgnoredSymbols(tag))
            {
                tag_array[tag_count] = tag;
                tag_count++;
            }

        }
    }
}
