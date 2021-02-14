using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_Tagging
{
    public partial class Spatiu_de_reprezentare : Form
    {
        private List<string> predicted = new List<string>();
        private List<KeyValuePair<string, string>> training_data = new List<KeyValuePair<string, string>>();
        //StreamReader file = new StreamReader("ca01.txt");

        public Spatiu_de_reprezentare()
        {
            InitializeComponent();
            ReadCorpus();
        }

        private void button_Predict_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, string> item in training_data)
            {
                //Console.WriteLine("Key = {0}, Value = {1}", item.Key, item.Value);
                predicted.Add(Predict(item));
            }
            predicted.ToList().ForEach(Console.WriteLine);
        }
        private string Predict(KeyValuePair<string, string> word)
        {
            return "noun";
        }
        public void ReadCorpus()
        {
            string rootPath = @"C:\Users\iulia.severin\source\repos\POS-Tagging\bin\Debug\brown";
            var files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
            string[] pos_tag;

            foreach (string file in files)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    string lines = reader.ReadToEnd();
                    char[] separators = new char[] { '/', ' ' };
                    pos_tag = lines.Split(separators, StringSplitOptions.RemoveEmptyEntries); //Split

                    for (int i = 0; i < pos_tag.Length-1; i += 2)
                    {
                        training_data.Add(new KeyValuePair<string, string>(pos_tag[i + 1], pos_tag[i]));
                    }
                }
                //Console.WriteLine(Path.GetFileName(file));
            }

            //afisare perechi parte de vorbire - tag
            //foreach (KeyValuePair<string, string> item in training_data.Where(item => item.Key == "nn"))
            /* foreach (KeyValuePair<string, string> item in training_data)
            {
                Console.WriteLine("Key = {0}, Value = {1}", item.Key, item.Value);
            } */
        }
    }
}

//Dictionary
//private Dictionary<string, string> training_data = new Dictionary<string, string>();
/*   

//dupa ce am facut line.Split - verific ce am in pos_tag
if (pos_tag.Length < 2) // If we get less than 2 results, discard them
      continue;
  else if (pos_tag.Length == 2) // If there are 2 results, add them to the dictionary
      training_data.Add(pos_tag[0].Trim(), pos_tag[1].Trim());
  else if (pos_tag.Length > 2)
      Split_pos_tag(pos_tag, training_data); //If there are more than 2 results
}}

public void Split_pos_tag(string[] stringInput, Dictionary<string, string> dictionaryInput)
{
StringBuilder sb = new StringBuilder();
List<string> temporaryList = new List<string>(); // This list will hold the keys and values as we find them
bool hasFirstValue = false;
foreach (string s in stringInput)
{
    foreach (char c in s) // Iterate through each character in the input
    {
        if (c != '/')  // Keep building the string until we reach a "/"
            sb.Append(c);
        else if (c == '/' && !hasFirstValue)
        {
            temporaryList.Add(sb.ToString().Trim());
            sb.Clear();
            hasFirstValue = true;
        }
        else if (c == '/' && hasFirstValue)
        {
            string[] pos_tag_pair = sb.ToString()
                                    .Trim()
                                    .Split(new string[] { "  " },
                                            StringSplitOptions.RemoveEmptyEntries);

            // Add both results to the list
            temporaryList.Add(pos_tag_pair[0].Trim());
            //  temporaryList.Add(pos_tag_pair[1].Trim());
            sb.Clear();
        }
    }
}
temporaryList.Add(sb.ToString().Trim()); // Add the last result to the list

for (int i = 0; i < temporaryList.Count; i += 2)
{

    //adauga in lista cat timp nu exista duplicates
    if (!dictionaryInput.ContainsKey(temporaryList[i+1]))
    {
        dictionaryInput.Add(temporaryList[i+1], temporaryList[i]);
    }

}

foreach (KeyValuePair<string, string> kvp in dictionaryInput)
{
    Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
}
}*/