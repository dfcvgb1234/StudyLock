using System;
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
        }

        private void button1_Click(object sender, EventArgs e)
        {          
            listBox1.Items.Remove(textBox1.Text);
            listBox1.Items.Remove(listBox1.SelectedItem);
            if(listBox1.Items.Count == 0)
            {
                File.Delete(@"C:\Windows\System32\drivers\etc\hosts");
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
                    w.Write(listBox1.Items[i] + ";");
                }
            }
        }

        private void ListOfWebsites_Load(object sender, EventArgs e)
        {
            string path = @"C:\Windows\System32\drivers\etc\host.begeba";
            string fileText = File.ReadAllText(path);
            string[] splitStrings = fileText.Split(';');
            foreach (string s in splitStrings)
            {
                if (String.IsNullOrWhiteSpace(s) == false)
                {
                    listBox1.Items.Add(s);
                }
            }
        }
    }
}
