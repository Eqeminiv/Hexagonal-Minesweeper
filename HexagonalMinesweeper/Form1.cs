using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HexagonalMinesweeper
{
    

    public partial class Form1 : Form
    {
        Graphics g;
        double second;
        Map def = new Map();
        const int hexSize = 60;
        int[,] board;

        public Form1(decimal _rows, decimal _columns, decimal _bombs)
        {
            
            InitializeComponent();
            
            progres.Text = _bombs.ToString();
            second = 0;
            seconds.Text = second.ToString();
            this.Size = new System.Drawing.Size((int)_columns * (int)def.HexWidth(hexSize) * 3 / 4 + (int)def.HexWidth(hexSize), 
                (int)_rows * hexSize + 3 * hexSize);
            restartButton.Location = new System.Drawing.Point(ClientSize.Width / 2 - restartButton.Width / 2, restartButton.Location.Y);
            seconds.Location = new System.Drawing.Point(restartButton.Location.X - seconds.Width - 15, seconds.Location.Y);
            czas.Location = new System.Drawing.Point(restartButton.Location.X - seconds.Width + 8, czas.Location.Y);
            label1.Location = new System.Drawing.Point(restartButton.Location.X + restartButton.Width + 13, label1.Location.Y);
            progres.Location = new System.Drawing.Point(restartButton.Location.X + restartButton.Width + 15, progres.Location.Y);
            
            //board = new int[getMaxHexCols(), pictureBox1.Height / hexSize]; //TODO
            board = new int[(int)_columns, (int)_rows];
            def.setField(board, (int)_bombs);
            def.setIsDefined();
            
        }
       

        int saveCol, saveRow;


        //debug
        /* float hexWidth;
         int hexWidth2;
         float ilosc;
         int szerokoscPic;*/
        //debug

        bool wasted = false;
        //int maxHexagons = 0;
        float zuzyta;



        private int getMaxHexCols()
        {
            int maxHexagons = 0;
            zuzyta = def.HexWidth(hexSize);
            while (true)
            {
                zuzyta = zuzyta + (def.HexWidth(hexSize) * 3 / 4);
                maxHexagons++;
                if (zuzyta >= pictureBox1.ClientSize.Width)
                    return maxHexagons;

            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;


            def.DrawHexGrid(e.Graphics, Pens.Black,
                0, pictureBox1.Width,
                0, pictureBox1.Height,
                hexSize);

            //if(columns.Enabled == false)
            def.DrawRevealed(e.Graphics, Brushes.LightBlue, hexSize);
            def.DrawFlags(e.Graphics, Brushes.DarkCyan, hexSize);

            if (wasted)
                def.DrawBombs(e.Graphics, Brushes.DarkRed, hexSize);

        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        // Display the row and column under the mouse.
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            def.PointToHex(e.X, e.Y, hexSize, out int row, out int col);

            if (checkBox1.Checked == true)
            {
                toolTip1.Active = true;
                toolTip1.SetToolTip(pictureBox1, ("(" + col + ", " + row + ")"));
            }
            else
                toolTip1.Active = false;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            checkBox1.Location = new System.Drawing.Point(pictureBox1.Width-100, checkBox1.Location.Y);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (second > 998)
                timer1.Enabled = false;
            second = Math.Round((second + 0.1), 1);
            seconds.Text = second.ToString();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int row, col;
            def.PointToHex(e.X, e.Y, hexSize, out row, out col);
            if (col >= 0 && col < board.GetLength(0) && row >= 0 && row <board.GetLength(1)
                && e.Button == MouseButtons.Left && def.CheckFlagFieldValue(col,row) == 0)
            {

                    if (def.CheckFieldValue(col, row) == 1)
                        gameOver();

                    else if (def.CheckFieldValue(col, row) == 0)
                        def.RevealNeighbours(col, row);      
                
            }
            else if (col >= 0 && col < board.GetLength(0) && row >= 0 && row < board.GetLength(1)
                && e.Button == MouseButtons.Right)
            {
                if (def.CheckFieldValue(col, row) != -1)
                {
                    def.placeOrTakeFlag(col, row);
                    if (def.CheckFlagFieldValue(col, row) == 1)
                        progres.Text = (Int32.Parse(progres.Text) - 1).ToString();
                    else
                        progres.Text = (Int32.Parse(progres.Text) + 1).ToString();
                }
                
            }
            pictureBox1.Refresh();

            if(def.getRemainingTiles() == 0)
            {
                winner();
            }
        }


        private void restartButton_Click(object sender, EventArgs e)
        {
            var t = new Thread(() => Application.Run(new Form2()));
            t.Start();
            this.Close();
        }

        private void gameOver()
        {
            Form3 f;
            wasted = true;
            pictureBox1.Enabled = false;
            timer1.Enabled = false;
            f = new Form3(seconds.Text, this, false);
            f.ShowDialog();

        }


        private void winner()
        {
            Form3 f;
            //wasted = true;
            
            pictureBox1.Enabled = false;
            timer1.Enabled = false;
            f = new Form3(seconds.Text, this, true);
            f.ShowDialog();
           // this.Dispose();
            
        }



    }
}
