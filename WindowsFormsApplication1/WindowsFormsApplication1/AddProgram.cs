using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SheetsQuickstart;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class AddProgram : Form
    {
        public AddProgram()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            ListOfProcesses lop = new ListOfProcesses();
            if(!String.IsNullOrWhiteSpace(ProgramName.Text) && !String.IsNullOrWhiteSpace(textBox2.Text))
            {
                List<object> gameName = new List<object>();
                List<object> processName = new List<object>();
                List<object> check = new List<object>();
                gameName.Add(ProgramName.Text);
                processName.Add(textBox2.Text);
                check.Add(true);
                int count = Program.range + Program.increment;
                Program.UpdateSheet(gameName, "Ark1!A"+count);
                Program.UpdateSheet(processName, "Ark1!B" + count);
                Program.UpdateSheet(check, "Ark1!C" + count);
                Program.increment++;
                lop.AddToList(ProgramName.Text);
                this.Close();
            }
            else
            {
                MessageBox.Show(this, "Du skal udfylde alle felter!", "ADVARSEL", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void ProgramName_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
