using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace WordFilter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadFilterWords(); //Load saved filter words
            textBox1.Multiline = true;
            textBox1.ScrollBars = ScrollBars.Vertical;
        }

        private void LoadFilterWords()
        {
            string path = Application.StartupPath + "\\words.txt";  //path to save file

            if (File.Exists(path)) //if file exists
            {
                StreamReader reader = new StreamReader(path, Encoding.Default); //make new streamwriter instance

                string word;
                while ((word = reader.ReadLine()) != null) //while save file has not empty line, read it and add it to listbox
                {
                    listBox1.Items.Add(word);
                }

                reader.Close();
                RemoveEmptyInFilters(); //re´move empty items in listbox
            }
        }

        private void RemoveEmptyInFilters()
        {
            int count = listBox1.Items.Count; //count of items in listbox
            for (int i = count - 1; i >= 0; i--) //iterate thru all items in listbox and remove empty ones
            {
                if (listBox1.Items[i].ToString() == "")
                {
                    listBox1.Items.RemoveAt(i);
                }
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(txtbox_addword.Text); //add filter word to listbox
            txtbox_addword.Clear(); //clear text field
            RemoveEmptyInFilters(); //remove empty items in listbox
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit(); //exit app
        }

        private void btn_remove_Click(object sender, EventArgs e)
        {
            listBox1.Items.RemoveAt(listBox1.SelectedIndex); //remove item from listbox
        }

        private void btn_filter_Click(object sender, EventArgs e)
        {
            string[] splittedText = textBox1.Text.Split(' '); //split textbox text into single words

            foreach (string filterWord in listBox1.Items) //iterate thru words and check if it matches to a filter word
            {
                Regex reg = new Regex(filterWord.ToString());

                for (int i = 0; i < splittedText.Length; i++)
                {
                    if (reg.IsMatch(splittedText[i]))
                    {
                        splittedText[i] = new string('*', splittedText[i].Length); //if match replace word with ****
                    }
                }

            }

            textBox2.Clear(); //clear textbox

            foreach (string str in splittedText) //add filtered text to textbox
            {
                textBox2.Text += str;
                textBox2.Text += " ";
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            string[] filterWords = listBox1.Items.OfType<string>().ToArray(); //make an array from filter words

            string path = Application.StartupPath + "\\words.txt"; //path to save file

            if (filterWords.Length > 0) //check if there is actually a word to save
            {
                File.WriteAllLines(path, filterWords); //write word to save file
            }

            else
            {
                MessageBox.Show("No filter words in list, data not saved."); //message box if there is no filter words to be saved
            }
        }
    }
}
