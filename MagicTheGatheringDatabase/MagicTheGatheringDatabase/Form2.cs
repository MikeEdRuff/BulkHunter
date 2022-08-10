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
    public partial class Form2 : Form
    {
        public Dictionary<string, string[]> formTwoDict;
        Dictionary<string, string[]> cardsInListDict;
        Dictionary<string, string[]> deckList;

        public Form2(Dictionary<string, string[]> newCardDict)
        {
            InitializeComponent();
            formTwoDict = new Dictionary<string, string[]>();
            foreach(string cardData in newCardDict.Keys)
                formTwoDict.Add(cardData, newCardDict[cardData]);
        }

        public void redraw()
        {
            cardsInListListBox.BeginUpdate();
            cardsInListListBox.Items.Clear();
            foreach (string[] cardData in cardsInListDict.Values)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(cardData[1]);
                builder.Append("\t");
                builder.Append(cardData[0]);
                cardsInListListBox.Items.Add(builder);
            }
            cardsInListListBox.EndUpdate();
        }

        
        private void button8_Click(object sender, EventArgs e)
        {
            cardsInListDict = new Dictionary<string, string[]>();
            deckList = new Dictionary<string, string[]>();

            string[] tempDeckList = textBox1.Text.Split('\n');
            for (int i = 0; i < tempDeckList.Length; i++)
            {
                string[] temp = tempDeckList[i].Split(' ');
                if (temp[0] != "" && temp[0] != "\n" && temp[0] != "\r")
                {
                    if (!deckList.ContainsKey(tempDeckList[i].Substring(2).Trim().ToLower()))
                    {
                        string[] cardData = { tempDeckList[i].Substring(2).Trim(), temp[0] };
                        deckList.Add(tempDeckList[i].Substring(2).Trim().ToLower(), cardData);
                    }
                    else
                    {
                        string[] cardData = deckList[tempDeckList[i].Substring(2).Trim().ToLower()];
                        int amount = Int32.Parse(temp[0]) + Int32.Parse(cardData[1]);
                        cardData[1] = amount.ToString();
                        deckList[tempDeckList[i].Substring(2).Trim().ToLower()] = cardData;
                    }
                }
            }

            foreach (string[] cardData in deckList.Values)
            {
                if (formTwoDict.ContainsKey(cardData[0].ToLower()))
                {
                    if (Int32.Parse(formTwoDict[cardData[0].ToLower()][1]) > 0)
                    {
                        cardsInListDict.Add(formTwoDict[cardData[0].ToLower()][0].ToLower(), formTwoDict[cardData[0].ToLower()]);
                    }
                }

            }

            redraw();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int selected = cardsInListListBox.SelectedIndex;
            if (selected != -1)
            {
                string selectedString = cardsInListListBox.Items[selected].ToString();
                string[] text = selectedString.Split('\t');

                string[] cardData = cardsInListDict[text[1].ToLower()];
                if (Int32.Parse(cardData[1]) == 1)
                {
                    cardsInListDict.Remove(text[1].ToLower());
                    cardData[1] = "0";
                    formTwoDict[text[1].ToLower()] = cardData;
                }
                else
                {
                    int amount = Int32.Parse(cardData[1]) - 1;
                    cardData[1] = amount.ToString();
                    cardsInListDict[text[1].ToLower()] = cardData;
                    formTwoDict[text[1].ToLower()] = cardData;
                }

                redraw();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int selected = cardsInListListBox.SelectedIndex;
            if (selected != -1)
            {
                string selectedString = cardsInListListBox.Items[selected].ToString();
                string[] text = selectedString.Split('\t');

                string[] cardData = formTwoDict[text[1].ToLower()];
                cardData[1] = "0";
                formTwoDict[text[1].ToLower()] = cardData;

                cardsInListDict.Remove(text[1].ToLower());

                redraw();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StringBuilder csv = new StringBuilder();

            foreach (string[] cardData in formTwoDict.Values)
            {
                var newLine = string.Format("{0};{1}", cardData[0], cardData[1]);
                csv.AppendLine(newLine);
            }

            File.WriteAllText("cards.csv", csv.ToString());
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
