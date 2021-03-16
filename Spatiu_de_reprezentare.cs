using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace POS_Tagging
{

    public partial class Spatiu_de_reprezentare : Form
    {
        //ingore symbols - filter
        private bool IgnoredSymbols(string word)
        {
            string[] ignored_symbols = { "''" , ",", ":", ".", "(", ")", "``", "?", "!", ";", "--" };
            int length = ignored_symbols.Length;

            for (int i = 0; i < length; i++)
            {
                //if contains this simbole, ignore all ? pt --hl --tl
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
        }

        string[] word_array = new string[60000];
        string[] tag_array = new string[600];
        int word_count = 0;
        int tag_count = 0;
        public void ReadCorpus()
        {
            string rootPath = @"C:\Users\iulia.severin\source\repos\POS-Tagging\bin\Debug\brown";
            var files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
            string[] word_tag_pair; //
            char[] separators = new char[] { ' ', '\r', '\n', '\t' };

            foreach (string file in files)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    string text_in_file = reader.ReadToEnd();

                    word_tag_pair = text_in_file.Split(separators, StringSplitOptions.RemoveEmptyEntries); //Split
                    
                    for(int i = 0; i < word_tag_pair.Length; i++)
                    {
                        string[] word_tag_split = word_tag_pair[i].Split('/');
                        //add words
                        AddWord(word_tag_split[0]);
                        //add tags
                        AddTag(word_tag_split[1]);

                    }
                }
            }
        }

        private void AddWord(string word)
        {
            bool exists_in_array = false;
            for (int j = 0; j < word_count; j++)
            {
                if (word_array[j] == word)
                {
                    exists_in_array = true;
                    break;
                }
            }
            if (!exists_in_array && !IgnoredSymbols(word))
            {
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
            if (!exists_in_array && !IgnoredSymbols(tag))
            {
                tag_array[tag_count] = tag;
                tag_count++;
            }

        }
    }
}
