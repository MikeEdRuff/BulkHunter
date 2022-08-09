using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagicTheGatheringDatabase
{
    public partial class Form1 : Form
    {

        Dictionary<string, string[]> cardDict;

        AutoCompleteStringCollection source;

        public Form1()
        {
            InitializeComponent();

            cardDict = new Dictionary<string, string[]>();

            List<string> cardNames = new List<string>();

            StreamReader streamReader = new StreamReader("cards.csv");
            string[] totalData = new string[File.ReadAllLines("cards.csv").Length];
            totalData = streamReader.ReadLine().Split(';');
            while (!streamReader.EndOfStream)
            {
                totalData = streamReader.ReadLine().Split(';');
                string[] cardData = { totalData[0], totalData[1] };
                cardDict.Add(totalData[0].ToLower(), cardData);
                cardNames.Add(totalData[0]);
            }

            redraw();

            source = new AutoCompleteStringCollection();
            source.AddRange(cardNames.ToArray());

            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox1.AutoCompleteCustomSource = source;
        }

        public void redraw()
        {
            ownedCardsListBox.BeginUpdate();
            ownedCardsListBox.Items.Clear();
            foreach (string[] cardData in cardDict.Values)
            {
                int amount = Int32.Parse(cardData[1]);
                if (amount > 0)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(cardData[1]);
                    builder.Append("\t");
                    builder.Append(cardData[0]);
                    ownedCardsListBox.Items.Add(builder);
                }
            }
            ownedCardsListBox.EndUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text.ToLower();
            if(cardDict.ContainsKey(text))
            {
                string[] cardData = cardDict[text];
                int amount = Int32.Parse(cardData[1]);
                amount += Int32.Parse(textBox2.Text);
                cardData[1] = amount.ToString();
                cardDict[text] = cardData;
                redraw();
            }

            textBox1.Text = "";
            textBox2.Text = "1";
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            int selected = ownedCardsListBox.SelectedIndex;
            if (selected != -1)
            {
                string selectedString = ownedCardsListBox.Items[selected].ToString();
                string[] text = selectedString.Split('\t');

                if (cardDict.ContainsKey(text[1].ToLower()))
                {
                    string[] cardData = cardDict[text[1].ToLower()];
                    int amount = Int32.Parse(cardData[1]);
                    --amount;
                    cardData[1] = amount.ToString();
                    cardDict[text[1].ToLower()] = cardData;
                    redraw();
                }

                
            }
        }

        private void removeAllButton_Click(object sender, EventArgs e)
        {
            int selected = ownedCardsListBox.SelectedIndex;
            if (selected != -1)
            {
                string selectedString = ownedCardsListBox.Items[selected].ToString();
                string[] text = selectedString.Split('\t');

                if (cardDict.ContainsKey(text[1].ToLower()))
                {
                    string[] cardData = cardDict[text[1].ToLower()];
                    cardData[1] = "0";
                    cardDict[text[1].ToLower()] = cardData;
                    redraw();
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            StringBuilder csv = new StringBuilder();

            foreach(string[] cardData in cardDict.Values)
            {
                var newLine = string.Format("{0};{1}", cardData[0], cardData[1]);
                csv.AppendLine(newLine);
            }

            File.WriteAllText("cards.csv", csv.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (cardDict.ContainsKey(textBox1.Text.ToLower()))
            {
                string[] cardData = cardDict[textBox1.Text.ToLower()];
                StringBuilder builder = new StringBuilder();
                builder.Append(cardData[1]);
                builder.Append("\t");
                builder.Append(cardData[0]);
                int indexList = ownedCardsListBox.FindString(builder.ToString());
                ownedCardsListBox.SetSelected(indexList, true);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2();
            newForm.Show();
        }
    }
}
