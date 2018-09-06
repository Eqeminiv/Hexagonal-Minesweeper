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
    public partial class GameOver : Form
    {
        Game form;
        decimal rows, columns, bombs;
        public GameOver(String time, Game form, bool isWinner, decimal _rows, decimal _columns, decimal _bombs)
        {
            InitializeComponent();
            this.form = form;
            rows = _rows;
            columns = _columns;
            bombs = _bombs;
            if (isWinner)
            {
                label1.Text = "Gratulacje! Pole minowe \r\nzostało zabezpieczone \r\nw czasie: \r\n" + time + " sekund";
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
            var t = new Thread(() => Application.Run(new Options(rows, columns, bombs)));
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
