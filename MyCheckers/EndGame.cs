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
    public partial class EndGame : Form
    {
        public Point aa;
        public string ltext;

        Form1 f1_l = new Form1();

        public EndGame()
        {
            InitializeComponent();
        }

        private void EndGame_Load(object sender, EventArgs e)
        {
            label1.Text = ltext;
            label1.Location = aa;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
            f1_l.ShowDialog();
            //f1_l.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
