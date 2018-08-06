using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HexagonalMinesweeper
{
    public partial class Form3 : Form
    {
        Form1 form;
        public Form3(String time, Form1 form, bool isWinner)
        {
            InitializeComponent();
            this.form = form;
            if (isWinner)
            {
                label1.Text = "Gratulacje! Pole minowe \r\nzostało zabezpieczone w czasie: \r\n" + time + " sekund";
                labelWin.Visible = true;
            }
            else
            {
                label1.Text = "Przez twoją decyzję wybuchło \r\ncałe pole minowe!";
                labelLose.Visible = true;
            }
        }

        private void playAgainButton_Click(object sender, EventArgs e)
        {
            var t = new Thread(() => Application.Run(new Form2()));
            t.Start();
            form.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form.Dispose();
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            form.Dispose();
        }
    }
}
