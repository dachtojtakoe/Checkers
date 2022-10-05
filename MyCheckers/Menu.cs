using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyCheckers
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void StartGame(object sender, EventArgs e)
        {
            Form1 aa = new Form1();
            this.Visible = false;
            aa.ShowDialog();
            this.Visible = true;
            aa.Dispose();
        }
        private void OpenInfo(object sender, EventArgs e)
        {
            Info aa = new Info();
            this.Visible = false;
            aa.ShowDialog();
            this.Visible = true;
            aa.Dispose();
        }

        private void Exit(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
