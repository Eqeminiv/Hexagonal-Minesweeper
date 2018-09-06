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
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
            bombs.Maximum = rows.Value * columns.Value - 1;

        }
        public Options(decimal _rows, decimal _columns, decimal _bombs)
        {
            InitializeComponent();
            rows.Value = _rows;
            columns.Value = _columns;
            bombs.Maximum = _rows * _columns - 1;
            bombs.Value = _bombs;


        }

        private void startButton_Click(object sender, EventArgs e)
        {
            var t = new Thread(() => Application.Run(new Game(rows.Value, columns.Value, bombs.Value)));
            t.Start();
            this.Close();
        }

        private void rows_ValueChanged(object sender, EventArgs e)
        {
            bombs.Maximum = rows.Value * columns.Value - 1;
        }

        private void columns_ValueChanged(object sender, EventArgs e)
        {
            bombs.Maximum = rows.Value * columns.Value - 1;
        }
    }
}
