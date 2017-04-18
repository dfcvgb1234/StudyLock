using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SheetsQuickstart;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class ListOfProcesses : Form
    {
        public ListOfProcesses()
        {
            InitializeComponent();
        }

        private void ListOfProcesses_Load(object sender, EventArgs e)
        {
            string[] proc = Program.gamesList.Where(x => x != null)
                       .Select(x => x.ToString())
                       .ToArray();
            string[] check = Program.checkedState.Where(x => x != null)
                       .Select(x => x.ToString())
                       .ToArray();
            for (int i = 0; i < proc.Length; i++)
            {
                if (check[i] == "TRUE")
                {
                    checkedListBox1.Items.Add(proc[i], true);
                }
                else
                {
                    checkedListBox1.Items.Add(proc[i], false);
                }
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            List<object> tmp = new List<object>();
            Console.WriteLine(tmp.Count);
            for(int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if(checkedListBox1.CheckedItems.Contains(checkedListBox1.Items[i]))
                {
                    tmp.Add("TRUE");
                }
                if(!checkedListBox1.CheckedItems.Contains(checkedListBox1.Items[i]))
                {
                    tmp.Add("FALSE");
                }
            }
            Program.UpdateSheet(tmp, "Ark1!C2:C999");
            MessageBox.Show(this, "Genstart programmet for at ændringerne træder i kraft", "ADVARSEL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void CheckAll_click(object sender, EventArgs e)
        {
            string[] proc = Program.gamesList.Where(x => x != null)
                      .Select(x => x.ToString())
                      .ToArray();
            checkedListBox1.Items.Clear();
            for (int i = 0; i < proc.Length; i++)
            {
                checkedListBox1.Items.Add(proc[i], true);
            }
        }

        private void DeSelectAll_Click(object sender, EventArgs e)
        {
            string[] proc = Program.gamesList.Where(x => x != null)
          .Select(x => x.ToString())
          .ToArray();
            checkedListBox1.Items.Clear();
            for (int i = 0; i < proc.Length; i++)
            {
                checkedListBox1.Items.Add(proc[i], false);
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            var new_prog = new AddProgram();
            new_prog.Show();
        }
        public void AddToList(string gameName)
        {
            checkedListBox1.Focus();
            checkedListBox1.Items.Clear();
            Console.WriteLine("add!");
        }
    }
}
