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
        List<string> CardNames;
        List<string> CardNamesLower;
        List<int> Amount;

        AutoCompleteStringCollection source;

        public Form1()
        {
            InitializeComponent();

            CardNames = new List<string>();
            CardNamesLower = new List<string>();
            Amount = new List<int>();

            StreamReader streamReader = new StreamReader("cards.csv");
            string[] totalData = new string[File.ReadAllLines("cards.csv").Length];
            totalData = streamReader.ReadLine().Split(';');
            while (!streamReader.EndOfStream)
            {
                totalData = streamReader.ReadLine().Split(';');
                CardNames.Add(totalData[0]);
                CardNamesLower.Add(totalData[0].ToLower());
                Amount.Add(Int32.Parse(totalData[1]));
            }

            redraw();

            source = new AutoCompleteStringCollection();
            source.AddRange(CardNames.ToArray());

            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox1.AutoCompleteCustomSource = source;
        }

        private void ownedCardsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            int index = CardNamesLower.IndexOf(text.ToLower());

            if (index != -1)
            {
                Amount[index] += Int32.Parse(textBox2.Text);
                redraw();
            }
            textBox1.Text = "";
            textBox2.Text = "1";
        }

        public void redraw()
        {
            ownedCardsListBox.BeginUpdate();
            ownedCardsListBox.Items.Clear();
            for (int i = 0; i < Amount.Count; i++)
            {
                if (Amount[i] > 0)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(Amount[i]);
                    builder.Append("\t");
                    builder.Append(CardNames[i]);
                    ownedCardsListBox.Items.Add(builder);
                }
            }
            ownedCardsListBox.EndUpdate();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            int selected = ownedCardsListBox.SelectedIndex;
            if (selected != -1)
            {
                string selectedString = ownedCardsListBox.Items[selected].ToString();
                string[] text = selectedString.Split('\t');

                int index = CardNamesLower.IndexOf(text[1].ToString().ToLower());

                if (index != -1)
                {
                    --Amount[index];
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

                int index = CardNamesLower.IndexOf(text[1].ToString().ToLower());

                if (index != -1)
                {
                    Amount[index] = 0;
                    redraw();
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            StringBuilder csv = new StringBuilder();

            for (int i = 0; i < Amount.Count; i++)
            {
                string cardName = CardNames[i].ToString();
                string amount = Amount[i].ToString();
                var newLine = string.Format("{0};{1}", cardName, amount);
                csv.AppendLine(newLine);

            }

            File.WriteAllText("cards.csv", csv.ToString());
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int index = CardNamesLower.IndexOf(textBox1.ToString().ToLower());
            if (index != -1)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(Amount[index]);
                builder.Append("\t");
                builder.Append(CardNames[index]);
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
