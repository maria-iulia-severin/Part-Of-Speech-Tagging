using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

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

        string[] word_array = new string[60000];
        string[] tag_array = new string[600];
        int[,] matrix = new int[60000, 600];
        int word_count = 0;
        int tag_count = 0;
        int count_words = 0;

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
            Console.WriteLine("Prediction:" + Predict("me"));
        }
        public void ReadCorpus()
        {
            string rootPath = @"C:\Users\iulia.severin\source\repos\POS-Tagging\bin\Debug\brown";
            var files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
            string[] word_tag_pair;
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
            Console.WriteLine("Noun Percentage: " + GetNounFrequence());
            Console.WriteLine("\n");
            Console.WriteLine("Verb Percentage: " + GetVerbFrequence());
            Console.WriteLine("\n");
            Console.WriteLine("Adjective Percentage: " + GetAdjectiveFrequence());
            Console.WriteLine("\n");
            Console.WriteLine("Article Percentage: " + GetArticleFrequence());
            Console.WriteLine("\n");
            Console.WriteLine("Adverb Percentage: " + GetAdverbFrequence());
            Console.WriteLine("\n");
            Console.WriteLine("Conjuction Percentage: " + GetConjunctionFrequence());
            Console.WriteLine("\n");
            Console.WriteLine("Preposition Percentage: " + GetPrepositionFrequence());
            Console.WriteLine("\n");
            Console.WriteLine("Pronoun Percentage: " + GetPronounFrequence());
            Console.WriteLine("\n");
            Console.WriteLine("Other Percentage: " + GetOtherFrequence());
            for (int i = 0; i < tag_array.Length; i++)
            {
                if (tag_array[i] != "")
                    Console.WriteLine(tag_array[i]); //ar trebui sa nu imi mai afiseze nimic dupa ce scap de tot ce nu am nevoie din ala de tag
            }

            //afisare matrice
            /* for (int i = 0; i < word_array.Length; i++)
             {
                 for (int j = 0; j < tag_array.Length; j++)
                 {
                     Console.Write(matrix[i, j] + " ");
                 }
                 Console.Write("\n");
             }
            //afisare tags
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
            word_tag_return.tags = new List<string>(tagGroup.Split(new char[] { '-', '+', '*', ' ', '`', '\'', '.', ',', ':', '(', ')', '$' }, StringSplitOptions.RemoveEmptyEntries));

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
        //NOUN
        private double GetNounFrequence()
        {
            int[] nounPositions = GetTagIndexes(noun);
            int nounsSum = 0;
            for (int i = 0; i < word_array.Length; i++)
            {
                for (int j = 0; j < nounPositions.Length; j++)
                {
                    nounsSum += matrix[i, nounPositions[j]];
                }
            }
            Console.WriteLine("No. Appearances nouns:" + nounsSum);
            Console.WriteLine("Total number of words:" + count_words);
            return 1.0 * nounsSum / count_words;
        }
        private int[] GetTagIndexes(string[] value)
        {
            int[] positions = new int[value.Length];

            for (int i = 0; i < value.Length; i++)
            {
                for (int j = 0; j < tag_array.Length; j++)
                {
                    if (value[i] == tag_array[j])
                    {
                        positions[i] = j;
                        tag_array[j] = "";
                        break;
                    }
                }
            }
            return positions;
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
        //VERB
        private double GetVerbFrequence()
        {
            int[] verbPositions = GetTagIndexes(verb);
            int verbsSum = 0;
            for (int i = 0; i < word_array.Length; i++)
            {
                for (int j = 0; j < verbPositions.Length; j++)
                {
                    verbsSum += matrix[i, verbPositions[j]];
                }
            }
            Console.WriteLine("No. Appearances verbs:" + verbsSum);
            return 1.0 * verbsSum / count_words;
        }
        private int[] GetVerbIndexes()
        {
            int[] positions = new int[verb.Length];

            for (int i = 0; i < verb.Length; i++)
            {
                for (int j = 0; j < tag_array.Length; j++)
                {
                    if (verb[i] == tag_array[j])
                    {
                        positions[i] = j;
                        break;
                    }
                }
            }
            return positions;
        }
        //ADJECTIVE
        private double GetAdjectiveFrequence()
        {
            int[] adjectivePositions = GetTagIndexes(adjective);
            int adjectivesSum = 0;
            for (int i = 0; i < word_array.Length; i++)
            {
                for (int j = 0; j < adjectivePositions.Length; j++)
                {
                    adjectivesSum += matrix[i, adjectivePositions[j]];
                }
            }
            Console.WriteLine("No. Appearances adjectives:" + adjectivesSum);
            return 1.0 * adjectivesSum / count_words;
        }
        private int[] GetAdjectiveIndexes()
        {
            int[] positions = new int[adjective.Length];

            for (int i = 0; i < adjective.Length; i++)
            {
                for (int j = 0; j < tag_array.Length; j++)
                {
                    if (adjective[i] == tag_array[j])
                    {
                        positions[i] = j;
                        break;
                    }
                }
            }
            return positions;
        }
        //ADVERB
        private double GetAdverbFrequence()
        {
            int[] adverbPositions = GetTagIndexes(adverb);
            int adverbsSum = 0;
            for (int i = 0; i < word_array.Length; i++)
            {
                for (int j = 0; j < adverbPositions.Length; j++)
                {
                    adverbsSum += matrix[i, adverbPositions[j]];
                }
            }
            Console.WriteLine("No. Appearances adverbs:" + adverbsSum);
            return 1.0 * adverbsSum / count_words;
        }
        private int[] GetAdverbIndexes()
        {
            int[] positions = new int[adverb.Length];

            for (int i = 0; i < adverb.Length; i++)
            {
                for (int j = 0; j < tag_array.Length; j++)
                {
                    if (adverb[i] == tag_array[j])
                    {
                        positions[i] = j;
                        break;
                    }
                }
            }
            return positions;
        }
        //PRONOUN
        private double GetPronounFrequence()
        {
            int[] pronounPositions = GetTagIndexes(pronoun);
            int pronounsSum = 0;
            for (int i = 0; i < word_array.Length; i++)
            {
                for (int j = 0; j < pronounPositions.Length; j++)
                {
                    pronounsSum += matrix[i, pronounPositions[j]];
                }
            }
            Console.WriteLine("No. Appearances pronouns:" + pronounsSum);
            return 1.0 * pronounsSum / count_words;
        }
        private int[] GetPronounIndexes()
        {
            int[] positions = new int[pronoun.Length];

            for (int i = 0; i < pronoun.Length; i++)
            {
                for (int j = 0; j < tag_array.Length; j++)
                {
                    if (pronoun[i] == tag_array[j])
                    {
                        positions[i] = j;
                        break;
                    }
                }
            }
            return positions;
        }
        //CONJUNCTION
        private double GetConjunctionFrequence()
        {
            int[] conjunctionPositions = GetTagIndexes(conjunction);
            int conjunctionsSum = 0;
            for (int i = 0; i < word_array.Length; i++)
            {
                for (int j = 0; j < conjunctionPositions.Length; j++)
                {
                    conjunctionsSum += matrix[i, conjunctionPositions[j]];
                }
            }
            Console.WriteLine("No. Appearances conjunctions:" + conjunctionsSum);
            return 1.0 * conjunctionsSum / count_words;
        }
        private int[] GetConjunctionIndexes()
        {
            int[] positions = new int[conjunction.Length];

            for (int i = 0; i < conjunction.Length; i++)
            {
                for (int j = 0; j < tag_array.Length; j++)
                {
                    if (conjunction[i] == tag_array[j])
                    {
                        positions[i] = j;
                        break;
                    }
                }
            }
            return positions;
        }
        //ARTICLE 
        private double GetArticleFrequence()
        {
            int[] articlePositions = GetTagIndexes(article);
            int articlesSum = 0;
            for (int i = 0; i < word_array.Length; i++)
            {
                for (int j = 0; j < articlePositions.Length; j++)
                {
                    articlesSum += matrix[i, articlePositions[j]];
                }
            }
            Console.WriteLine("No. Appearances articles:" + articlesSum);
            return 1.0 * articlesSum / count_words;
        }
        private int[] GetArticleIndexes()
        {
            int[] positions = new int[article.Length];

            for (int i = 0; i < article.Length; i++)
            {
                for (int j = 0; j < tag_array.Length; j++)
                {
                    if (article[i] == tag_array[j])
                    {
                        positions[i] = j;
                        break;
                    }
                }
            }
            return positions;
        }
        //PREPOSITION 
        private double GetPrepositionFrequence()
        {
            int[] prepositionPositions = GetTagIndexes(preposition);
            int prepositionsSum = 0;
            for (int i = 0; i < word_array.Length; i++)
            {
                for (int j = 0; j < prepositionPositions.Length; j++)
                {
                    prepositionsSum += matrix[i, prepositionPositions[j]];
                }
            }
            Console.WriteLine("No. Appearances prepositions:" + prepositionsSum);
            return 1.0 * prepositionsSum / count_words;
        }
        private int[] GetPrepositionIndexes()
        {
            int[] positions = new int[preposition.Length];

            for (int i = 0; i < preposition.Length; i++)
            {
                for (int j = 0; j < tag_array.Length; j++)
                {
                    if (preposition[i] == tag_array[j])
                    {
                        positions[i] = j;
                        break;
                    }
                }
            }
            return positions;
        }
        //OTHER 
        private double GetOtherFrequence()
        {
            int[] otherPositions = GetTagIndexes(other);
            int othersSum = 0;
            for (int i = 0; i < word_array.Length; i++)
            {
                for (int j = 0; j < otherPositions.Length; j++)
                {
                    othersSum += matrix[i, otherPositions[j]];
                }
            }
            Console.WriteLine("No. Appearances others:" + othersSum);
            return 1.0 * othersSum / count_words;
        }
        private int[] GetOtherIndexes()
        {
            int[] positions = new int[other.Length];

            for (int i = 0; i < other.Length; i++)
            {
                for (int j = 0; j < tag_array.Length; j++)
                {
                    if (other[i] == tag_array[j])
                    {
                        positions[i] = j;
                        break;
                    }
                }
            }
            return positions;
        }
    }
}
