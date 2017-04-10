using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class ListOfWebsites : Form
    {
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
        }
    }
}
