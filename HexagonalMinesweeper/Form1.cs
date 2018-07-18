using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HexagonalMinesweeper
{
    public partial class Form1 : Form
    {
        Graphics g;

        public Form1()
        {
            InitializeComponent();
            rows.Value = pictureBox1.ClientSize.Height / hexSize ;
            columns.Value = getMaxHexCols();
            
        }
       
        
        // The height of a hexagon.
        Map def = new Map();
        const int hexSize = 60;
        int[,] board;
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

        // Redraw the grid.
        private void startButton_Click(object sender, EventArgs e)
        {
            
            startButton.Dispose();     
            board = new int[getMaxHexCols(), pictureBox1.Height / hexSize]; //TODO
            def.setField(board, (int)numericUpDown1.Value);
            def.setIsDefined();
            columns.Enabled = false;
            rows.Enabled = false;
    
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;


            def.DrawHexGrid(e.Graphics, Pens.Black,
                0, pictureBox1.Width,
                0, pictureBox1.Height,
                hexSize);

            if(columns.Enabled == false)
                def.DrawRevealed(e.Graphics, Brushes.LightBlue, hexSize);         

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

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            
            if (def.getIsDefined())
            {
                int row, col;
                def.PointToHex(e.X, e.Y, hexSize, out row, out col);
                if (def.CheckFieldValue(col, row) == 1)
                {
                    wasted = true; //game over
                    // pictureBox1.Enabled = false; 
                }
                else if (def.CheckFieldValue(col, row) == 0)
                {
                    def.RevealNeighbours(col, row);
                }
                pictureBox1.Refresh();
            }
        }


        //TODO
        private void rows_ValueChanged(object sender, EventArgs e)
        {
            
            this.Size = new System.Drawing.Size(this.Size.Width, (int)rows.Value*hexSize+3*hexSize);
            //pictureBox1.Size = new System.Drawing.Size(this.Size.Width, (int)rows.Value * hexSize);
      
        }

        private void columns_ValueChanged(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size((int)columns.Value*(int)def.HexWidth(hexSize)*3/4 + (int)def.HexWidth(hexSize), this.Size.Height);
        }
    }
}
