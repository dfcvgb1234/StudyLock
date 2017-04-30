﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{

    public partial class ListOfWebsites : Form
    {
        public static int progressWeb = 0;
        public ListOfWebsites()
        {
            InitializeComponent();
            textBox1.KeyPress += TextBox1_KeyPress;
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                Add.PerformClick();
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(textBox1.Text);
            Form1.UpdateHostFile(textBox1.Text, false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressWeb = 20;            
            listBox1.Items.Remove(textBox1.Text);
            listBox1.Items.Remove(listBox1.SelectedItem);
            if (listBox1.Items.Count > 0)
            {
                Form1.UpdateHostFile(listBox1.Items[0].ToString(), true);
            }
            if(listBox1.Items.Count == 0)
            {
                File.Delete(@"C:\Windows\System32\drivers\etc\hosts");
            }
            int inc = listBox1.Items.Count / 50;
            if (listBox1.Items.Count > 1)
            {
                for (int i = 1; i < listBox1.Items.Count; i++)
                {
                    Form1.UpdateHostFile(listBox1.Items[i].ToString(), false);
                    progressWeb = progressWeb + inc;
                }
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            string path = @"C:\Windows\System32\drivers\etc\host.begeba";
            if(File.Exists(path))
            {
                File.Delete(path);
            }
            File.Create(path).Close();

            using (StreamWriter w = File.AppendText(path))
            {
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    w.WriteLine(listBox1.Items[i] + ",");
                }
            }
        }

        private void ListOfWebsites_Load(object sender, EventArgs e)
        {
            readFile();
            foreach (string h in readFile())
            {
                if (String.IsNullOrWhiteSpace(h) == false)
                {
                    Console.WriteLine(h);
                    listBox1.Items.Add(h);
                }
            }
        }
        public string[] readFile()
        {
            string path = @"C:\Windows\System32\drivers\etc\host.begeba";
            if (File.Exists(path))
            {
                string commatext = File.ReadAllText(path);
                return commatext.Split(',');
            }
            else
                return null;
        }
    }
}
