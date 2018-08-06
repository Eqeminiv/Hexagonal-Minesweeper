using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HexagonalMinesweeper
{
    class Map
    {
        private int[,] field, flagField;
        private String[,] neighbourField;
        private int bombCount;
        private Image[] numbers;
        private Image flagImg;
        private int remainingTiles;



        bool isDefined = false;
        bool shouldDraw = false;

        private void loadImages()
        {
            numbers[0] = null;
            numbers[1] = HexagonalMinesweeper.Properties.Resources._1;
            numbers[2] = HexagonalMinesweeper.Properties.Resources._2;
            numbers[3] = HexagonalMinesweeper.Properties.Resources._3;
            numbers[4] = HexagonalMinesweeper.Properties.Resources._4;
            numbers[5] = HexagonalMinesweeper.Properties.Resources._5;
            numbers[6] = HexagonalMinesweeper.Properties.Resources._6;
        }

        public void setField(int[,] field, int bombCount)
        {
            flagImg = HexagonalMinesweeper.Properties.Resources.ikonka_test;
            numbers = new Image[7];
            loadImages();
            this.field = field;
            this.flagField = new int[field.GetLength(0), field.GetLength(1)];
            remainingTiles = this.field.GetLength(0) * this.field.GetLength(1) - bombCount;

            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    field[i, j] = 0;
                    flagField[i, j] = 0;
                }

            }
            this.bombCount = bombCount;
            placeBombs();
        }
        public void setIsDefined()
        {
            isDefined = true;
        }
        public bool getIsDefined()
        {
            return isDefined;
        }
        public void setBombCount(int bombCount)
        {
            this.bombCount = bombCount;
        }
        public bool getShouldDraw()
        {
            return shouldDraw;
        }
        public int getRemainingTiles()
        {
            return remainingTiles;
        }
        private void placeBombs()
        {
            Random r1 = new Random();
            int counter = 0;
            int width, height;
            while (counter < bombCount)
            {
                width = r1.Next(0, field.GetLength(0));
                height = r1.Next(0, field.GetLength(1));
                if (field[width, height] == 0)
                {
                    field[width, height] = 1;
                    counter++;
                }
            }

            neighbourField = new String[field.GetLength(0), field.GetLength(1)];
            for (int row = 0; row < field.GetLength(1); row++)
            {
                for (int col = 0; col < field.GetLength(0); col++)
                {
                    neighbourField[col, row] = CountBombsSurrounding(col, row);
                }
            }
        }

        public void DrawHexGrid(Graphics g, Pen pen,
            float xmin, float xmax, float ymin, float ymax,
            float height)
        {
            // Loop until a hexagon won't fit.
            for (int row = 0; row < field.GetLength(1); row++)
            {
                PointF[] points = HexToPoints(height, row, 0);

                if (points[4].Y > ymax) break;

                for (int col = 0; col < field.GetLength(0); col++)
                {
                    points = HexToPoints(height, row, col);

                    if (points[3].X > xmax) break;

                    if (points[4].Y <= ymax)
                    {
                        g.DrawPolygon(pen, points);

                    }
                }
            }
        }

        public void DrawBombs(Graphics g, Brush brush, float height)
        {
            PointF[] points;
            for (int row = 0; row < field.GetLength(1); row++)
            {
                for (int col = 0; col < field.GetLength(0); col++)
                {
                    points = HexToPoints(height, row, col);
                    if (field[col, row] == 1)
                        g.FillPolygon(brush, points);
                }
            }

        }
        public void DrawRevealed(Graphics g, Brush brush, float height)
        {
            PointF[] points;
            for (int row = 0; row < field.GetLength(1); row++)
            {
                for (int col = 0; col < field.GetLength(0); col++)
                {
                    points = HexToPoints(height, row, col);
                    if (field[col, row] == -1)
                    {
                        g.FillPolygon(brush, points);
                        if (neighbourField[col, row] == "1")
                            g.DrawImage(numbers[1], points[1].X - 20, points[2].Y - 10);
                        else if (neighbourField[col, row] == "2")
                            g.DrawImage(numbers[2], points[1].X - 20, points[1].Y - 10);
                        else if (neighbourField[col, row] == "3")
                            g.DrawImage(numbers[3], points[1].X - 20, points[1].Y - 10);
                        else if (neighbourField[col, row] == "4")
                            g.DrawImage(numbers[4], points[1].X - 20, points[1].Y - 10);
                        else if (neighbourField[col, row] == "5")
                            g.DrawImage(numbers[5], points[1].X - 20, points[1].Y - 10);
                        else if (neighbourField[col, row] == "6")
                            g.DrawImage(numbers[6], points[1].X - 20, points[1].Y - 10);

                        //g.DrawString(neighbourField[col, row], SystemFonts.MenuFont, Brushes.Black, points[4].X, points[4].Y - (height / 2));
                    };
                }
            }
        }

        public void DrawFlags(Graphics g, Brush brush, float height)
        {
            PointF[] points;
            for (int row = 0; row < flagField.GetLength(1); row++)
            {
                for (int col = 0; col < flagField.GetLength(0); col++)
                {
                    points = HexToPoints(height, row, col);
                    if (flagField[col, row] == 1)
                    {
                        g.FillPolygon(brush, points);
                        //g.DrawImage(flagImg, points[1].X - 3, points[2].Y + 5);
                        


                        //g.DrawString(neighbourField[col, row], SystemFonts.MenuFont, Brushes.Black, points[4].X, points[4].Y - (height / 2));
                    };
                }
            }
        }

        public void AlterField(int col, int row)
        {
            field[col, row] = -1;
            remainingTiles--;
        }
        public int CheckFieldValue(int col, int row)
        {
            return field[col, row];
        }
        public int CheckFlagFieldValue(int col, int row)
        {
            return flagField[col, row];
        }
        public void placeOrTakeFlag(int col, int row)
        {
            if (flagField[col, row] == 0)
            {
                flagField[col, row] = 1; //1 - postawiona flaga
            }
            else
            {
                flagField[col, row] = 0; //0 - niepostawiona flaga
            }
        }

        public String CountBombsSurrounding(int col, int row)
        {
            int counter = 0;
            if (col % 2 == 1)
            {
                if (row == 0)
                {
                    if (field.GetLength(0) % 2 == 0 && col == field.GetLength(0) - 1)
                    {
                        if (CheckFieldValue(col - 1, row) == 1)
                            counter++;
                        if (CheckFieldValue(col - 1, row + 1) == 1)
                            counter++;
                        if (CheckFieldValue(col, row + 1) == 1)
                            counter++;
                    }
                    else
                    {
                        if (CheckFieldValue(col - 1, row) == 1)
                            counter++;
                        if (CheckFieldValue(col - 1, row + 1) == 1)
                            counter++;
                        if (CheckFieldValue(col, row + 1) == 1)
                            counter++;
                        if (CheckFieldValue(col + 1, row + 1) == 1)
                            counter++;
                        if (CheckFieldValue(col + 1, row) == 1)
                            counter++;
                    }
                }
                else if (row == field.GetLength(1) - 1)
                {
                    if (field.GetLength(0) % 2 == 0 && col == field.GetLength(0) - 1)
                    {
                        if (CheckFieldValue(col - 1, row) == 1)
                            counter++;
                        if (CheckFieldValue(col, row - 1) == 1)
                            counter++;
                    }
                    else
                    {
                        if (CheckFieldValue(col - 1, row) == 1)
                            counter++;
                        if (CheckFieldValue(col + 1, row) == 1)
                            counter++;
                        if (CheckFieldValue(col, row - 1) == 1)
                            counter++;
                    }
                }
                else
                {
                    if (field.GetLength(0) % 2 == 0 && col == field.GetLength(0) - 1)
                    {
                        if (CheckFieldValue(col - 1, row) == 1)
                            counter++;
                        if (CheckFieldValue(col - 1, row + 1) == 1)
                            counter++;
                        if (CheckFieldValue(col, row + 1) == 1)
                            counter++;
                        if (CheckFieldValue(col, row - 1) == 1)
                            counter++;
                    }
                    else
                    {
                        if (CheckFieldValue(col - 1, row) == 1)
                            counter++;
                        if (CheckFieldValue(col - 1, row + 1) == 1)
                            counter++;
                        if (CheckFieldValue(col, row + 1) == 1)
                            counter++;
                        if (CheckFieldValue(col + 1, row + 1) == 1)
                            counter++;
                        if (CheckFieldValue(col + 1, row) == 1)
                            counter++;
                        if (CheckFieldValue(col, row - 1) == 1)
                            counter++;
                    }
                }

            }

            else
            {
                if (row == 0)
                {
                    if (col == 0)
                    {
                        if (CheckFieldValue(col, row + 1) == 1)
                            counter++;
                        if (CheckFieldValue(col + 1, row) == 1)
                            counter++;
                    }
                    else if (field.GetLength(0) % 2 == 1 && col == field.GetLength(0) - 1)
                    {
                        if (CheckFieldValue(col - 1, row) == 1)
                            counter++;
                        if (CheckFieldValue(col, row + 1) == 1)
                            counter++;
                    }
                    else
                    {
                        if (CheckFieldValue(col - 1, row) == 1)
                            counter++;
                        if (CheckFieldValue(col, row + 1) == 1)
                            counter++;
                        if (CheckFieldValue(col + 1, row) == 1)
                            counter++;
                    }
                }
                else if (row == field.GetLength(1) - 1)
                {
                    if (col == 0)
                    {
                        if (CheckFieldValue(col + 1, row - 1) == 1)
                            counter++;
                        if (CheckFieldValue(col + 1, row) == 1)
                            counter++;
                        if (CheckFieldValue(col, row - 1) == 1)
                            counter++;
                    }
                    else if (field.GetLength(0) % 2 == 1 && col == field.GetLength(0) - 1)
                    {
                        if (CheckFieldValue(col - 1, row - 1) == 1)
                            counter++;
                        if (CheckFieldValue(col - 1, row) == 1)
                            counter++;
                        if (CheckFieldValue(col, row - 1) == 1)
                            counter++;
                    }
                    else
                    {
                        if (CheckFieldValue(col - 1, row - 1) == 1)
                            counter++;
                        if (CheckFieldValue(col - 1, row) == 1)
                            counter++;
                        if (CheckFieldValue(col + 1, row - 1) == 1)
                            counter++;
                        if (CheckFieldValue(col + 1, row) == 1)
                            counter++;
                        if (CheckFieldValue(col, row - 1) == 1)
                            counter++;
                    }
                }
                else
                {
                    if (col == 0)
                    {
                        if (CheckFieldValue(col, row + 1) == 1)
                            counter++;
                        if (CheckFieldValue(col + 1, row - 1) == 1)
                            counter++;
                        if (CheckFieldValue(col + 1, row) == 1)
                            counter++;
                        if (CheckFieldValue(col, row - 1) == 1)
                            counter++;
                    }
                    else if (field.GetLength(0) % 2 == 1 && col == field.GetLength(0) - 1)
                    {
                        if (CheckFieldValue(col - 1, row - 1) == 1)
                            counter++;
                        if (CheckFieldValue(col - 1, row) == 1)
                            counter++;
                        if (CheckFieldValue(col, row + 1) == 1)
                            counter++;
                        if (CheckFieldValue(col, row - 1) == 1)
                            counter++;
                    }
                    else
                    {
                        if (CheckFieldValue(col - 1, row - 1) == 1)
                            counter++;
                        if (CheckFieldValue(col - 1, row) == 1)
                            counter++;
                        if (CheckFieldValue(col, row + 1) == 1)
                            counter++;
                        if (CheckFieldValue(col + 1, row - 1) == 1)
                            counter++;
                        if (CheckFieldValue(col + 1, row) == 1)
                            counter++;
                        if (CheckFieldValue(col, row - 1) == 1)
                            counter++;
                    }
                }
            }
            return counter.ToString();

        }

        public bool isBombOnSurrounding(int col, int row)
        {
            if (col % 2 == 1)
            {
                if (row == 0)
                {
                    if (field.GetLength(0) % 2 == 0 && col == field.GetLength(0) - 1 && (CheckFieldValue(col - 1, row) == 1 ||
                        CheckFieldValue(col - 1, row + 1) == 1 || CheckFieldValue(col, row + 1) == 1))
                        return true;
                    else if (field.GetLength(0) % 2 == 0 && col != field.GetLength(0) - 1)
                    {
                        if (CheckFieldValue(col - 1, row) == 1 || CheckFieldValue(col - 1, row + 1) == 1 || CheckFieldValue(col, row + 1) == 1 ||
                            CheckFieldValue(col + 1, row + 1) == 1 || CheckFieldValue(col + 1, row) == 1)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else if (row == field.GetLength(1) - 1)
                {
                    if (field.GetLength(0) % 2 == 0 && col == field.GetLength(0) - 1 && (CheckFieldValue(col - 1, row) == 1 ||
                        CheckFieldValue(col, row - 1) == 1))
                        return true;
                    else if (field.GetLength(0) % 2 == 0 && col != field.GetLength(0) - 1)
                    {
                        if (CheckFieldValue(col - 1, row) == 1 || CheckFieldValue(col + 1, row) == 1 ||
                            CheckFieldValue(col, row - 1) == 1)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                {
                    if (field.GetLength(0) % 2 == 0 && col == field.GetLength(0) - 1 &&
                     (CheckFieldValue(col - 1, row) == 1 || CheckFieldValue(col - 1, row + 1) == 1 ||
                      CheckFieldValue(col, row + 1) == 1 || CheckFieldValue(col, row - 1) == 1))
                        return true;
                    else if (field.GetLength(0) % 2 == 0 && col != field.GetLength(0) - 1)
                    {
                        if (CheckFieldValue(col - 1, row) == 1 || CheckFieldValue(col - 1, row + 1) == 1 ||
                            CheckFieldValue(col, row + 1) == 1 || CheckFieldValue(col + 1, row + 1) == 1 ||
                            CheckFieldValue(col + 1, row) == 1 || CheckFieldValue(col, row - 1) == 1)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }



            }
            else
            {
                if (row == 0)
                {
                    if (col == 0)
                    {
                        if (CheckFieldValue(col, row + 1) == 1)
                            return true;
                        if (CheckFieldValue(col + 1, row) == 1)
                            return true;
                        return false;
                    }
                    else if (field.GetLength(0) % 2 == 1 && col == field.GetLength(0) - 1)
                    {
                        if (CheckFieldValue(col - 1, row) == 1)
                            return true;
                        if (CheckFieldValue(col, row + 1) == 1)
                            return true;
                        return false;
                    }
                    else
                    {
                        if (CheckFieldValue(col - 1, row) == 1)
                            return true;
                        if (CheckFieldValue(col, row + 1) == 1)
                            return true;
                        if (CheckFieldValue(col + 1, row) == 1)
                            return true;
                        return false;
                    }
                }
                else if (row == field.GetLength(1) - 1)
                {
                    if (col == 0)
                    {
                        if (CheckFieldValue(col + 1, row - 1) == 1)
                            return true;
                        if (CheckFieldValue(col + 1, row) == 1)
                            return true;
                        if (CheckFieldValue(col, row - 1) == 1)
                            return true;
                        return false;
                    }
                    else if (field.GetLength(0) % 2 == 1 && col == field.GetLength(0) - 1)
                    {
                        if (CheckFieldValue(col - 1, row - 1) == 1)
                            return true;
                        if (CheckFieldValue(col - 1, row) == 1)
                            return true;
                        if (CheckFieldValue(col, row - 1) == 1)
                            return true;
                        return false;
                    }
                    else
                    {
                        if (CheckFieldValue(col - 1, row - 1) == 1)
                            return true;
                        if (CheckFieldValue(col - 1, row) == 1)
                            return true;
                        if (CheckFieldValue(col + 1, row - 1) == 1)
                            return true;
                        if (CheckFieldValue(col + 1, row) == 1)
                            return true;
                        if (CheckFieldValue(col, row - 1) == 1)
                            return true;
                        return false;
                    }
                }
                else
                {
                    if (col == 0)
                    {
                        if (CheckFieldValue(col, row + 1) == 1)
                            return true;
                        if (CheckFieldValue(col + 1, row - 1) == 1)
                            return true;
                        if (CheckFieldValue(col + 1, row) == 1)
                            return true;
                        if (CheckFieldValue(col, row - 1) == 1)
                            return true;
                        return false;
                    }
                    else if (field.GetLength(0) % 2 == 1 && col == field.GetLength(0) - 1)
                    {
                        if (CheckFieldValue(col - 1, row - 1) == 1)
                            return true;
                        if (CheckFieldValue(col - 1, row) == 1)
                            return true;
                        if (CheckFieldValue(col, row + 1) == 1)
                            return true;
                        if (CheckFieldValue(col, row - 1) == 1)
                            return true;
                        return false;
                    }
                    else
                    {
                        if (CheckFieldValue(col - 1, row - 1) == 1)
                            return true;
                        if (CheckFieldValue(col - 1, row) == 1)
                            return true;
                        if (CheckFieldValue(col, row + 1) == 1)
                            return true;
                        if (CheckFieldValue(col + 1, row - 1) == 1)
                            return true;
                        if (CheckFieldValue(col + 1, row) == 1)
                            return true;
                        if (CheckFieldValue(col, row - 1) == 1)
                            return true;
                        return false;
                    }
                }
            }
        }

        public void RevealNeighbours(int col, int row)
        {
            if (CheckFieldValue(col, row) == 1 || CheckFlagFieldValue(col, row) == 1) //field with bomb neighbouring -> stop function
                return;
            else
            {
                if (col % 2 == 1) //when the collumn is odd
                {
                    if (CheckFieldValue(col, row) == 0) // 0 - unrevealed yet
                    {
                        if (isBombOnSurrounding(col, row))
                        {
                            AlterField(col, row);
                            return;
                        }
                        if (row == 0)
                        {
                            if (field.GetLength(0) % 2 == 0 && col == field.GetLength(0) - 1)
                            {
                                AlterField(col, row);
                                RevealNeighbours(col - 1, row);
                                RevealNeighbours(col - 1, row + 1);
                                RevealNeighbours(col, row + 1);
                            }
                            else
                            {
                                AlterField(col, row);
                                RevealNeighbours(col - 1, row);
                                RevealNeighbours(col - 1, row + 1);
                                RevealNeighbours(col, row + 1);
                                RevealNeighbours(col + 1, row + 1);
                                RevealNeighbours(col + 1, row);
                            }
                        }
                        else if (row == field.GetLength(1) - 1)
                        {
                            if (field.GetLength(0) % 2 == 0 && col == field.GetLength(0) - 1)
                            {
                                AlterField(col, row);
                                RevealNeighbours(col - 1, row);
                                RevealNeighbours(col, row - 1);
                            }
                            else
                            {
                                AlterField(col, row);
                                RevealNeighbours(col - 1, row);
                                RevealNeighbours(col + 1, row);
                                RevealNeighbours(col, row - 1);
                            }
                        }
                        else
                        {
                            if (field.GetLength(0) % 2 == 0 && col == field.GetLength(0) - 1)
                            {
                                AlterField(col, row);
                                RevealNeighbours(col - 1, row);
                                RevealNeighbours(col - 1, row + 1);
                                RevealNeighbours(col, row + 1);
                                RevealNeighbours(col, row - 1);
                            }
                            else
                            {
                                AlterField(col, row);
                                RevealNeighbours(col - 1, row);
                                RevealNeighbours(col - 1, row + 1);
                                RevealNeighbours(col, row + 1);
                                RevealNeighbours(col + 1, row + 1);
                                RevealNeighbours(col + 1, row);
                                RevealNeighbours(col, row - 1);
                            }
                        }



                    }
                    else
                        return;

                }
                else // when the collumn is even
                {
                    if (CheckFieldValue(col, row) == 0) // 0 - unrevealed yet
                    {

                        if (isBombOnSurrounding(col, row))
                        {
                            AlterField(col, row);
                            return;
                        }

                        if (row == 0)
                        {
                            if (col == 0)
                            {
                                AlterField(col, row);
                                RevealNeighbours(col, row + 1);
                                RevealNeighbours(col + 1, row);
                            }
                            else if (field.GetLength(0) % 2 == 1 && col == field.GetLength(0) - 1)
                            {
                                AlterField(col, row);
                                RevealNeighbours(col - 1, row);
                                RevealNeighbours(col, row + 1);
                            }
                            else
                            {
                                AlterField(col, row);
                                RevealNeighbours(col - 1, row);
                                RevealNeighbours(col, row + 1);
                                RevealNeighbours(col + 1, row);
                            }
                        }
                        else if (row == field.GetLength(1) - 1)
                        {
                            if (col == 0)
                            {
                                AlterField(col, row);
                                RevealNeighbours(col + 1, row - 1);
                                RevealNeighbours(col + 1, row);
                                RevealNeighbours(col, row - 1);
                            }
                            else if (field.GetLength(0) % 2 == 1 && col == field.GetLength(0) - 1)
                            {
                                AlterField(col, row);
                                RevealNeighbours(col - 1, row - 1);
                                RevealNeighbours(col - 1, row);
                                RevealNeighbours(col, row - 1);
                            }
                            else
                            {
                                AlterField(col, row);
                                RevealNeighbours(col - 1, row - 1);
                                RevealNeighbours(col - 1, row);
                                RevealNeighbours(col + 1, row - 1);
                                RevealNeighbours(col + 1, row);
                                RevealNeighbours(col, row - 1);
                            }
                        }
                        else
                        {
                            if (col == 0)
                            {
                                AlterField(col, row);
                                RevealNeighbours(col, row + 1);
                                RevealNeighbours(col + 1, row - 1);
                                RevealNeighbours(col + 1, row);
                                RevealNeighbours(col, row - 1);
                            }
                            else if (field.GetLength(0) % 2 == 1 && col == field.GetLength(0) - 1)
                            {
                                AlterField(col, row);
                                RevealNeighbours(col - 1, row - 1);
                                RevealNeighbours(col - 1, row);
                                RevealNeighbours(col, row + 1);
                                RevealNeighbours(col, row - 1);
                            }
                            else
                            {
                                AlterField(col, row);
                                RevealNeighbours(col - 1, row - 1);
                                RevealNeighbours(col - 1, row);
                                RevealNeighbours(col, row + 1);
                                RevealNeighbours(col + 1, row - 1);
                                RevealNeighbours(col + 1, row);
                                RevealNeighbours(col, row - 1);
                            }
                        }
                    }
                    else
                        return;
                }
            }
        }


        public float HexWidth(float height)
        {
            return (float)(4 * (height / 2 / Math.Sqrt(3)));
        }


        public void PointToHex(float x, float y, float height,
            out int row, out int col)
        {

            float width = HexWidth(height);
            col = (int)(x / (width * 0.75f));

            if (col % 2 == 0)
                row = (int)(y / height);
            else
                row = (int)((y - height / 2) / height);

            float testx = col * width * 0.75f;
            float testy = row * height;
            if (col % 2 == 1) testy += height / 2;

            bool is_above = false, is_below = false;
            float dx = x - testx;
            if (dx < width / 4)
            {
                float dy = y - (testy + height / 2);
                if (dx < 0.001)
                {
                    if (dy < 0) is_above = true;
                    if (dy > 0) is_below = true;
                }
                else if (dy < 0)
                {
                    if (-dy / dx > Math.Sqrt(3)) is_above = true;
                }
                else
                {
                    if (dy / dx > Math.Sqrt(3)) is_below = true;
                }
            }

            if (is_above)
            {
                if (col % 2 == 0) row--;
                col--;
            }
            else if (is_below)
            {
                if (col % 2 == 1) row++;
                col--;
            }
        }

        private PointF[] HexToPoints(float height, float row, float col)
        {
            float width = HexWidth(height);
            float y = height / 2;
            float x = 0;

            y += row * height;

            if (col % 2 == 1)
                y += height / 2;

            x += col * (width * 0.75f);

            return new PointF[]
                {
                    new PointF(x, y),
                    new PointF(x + width * 0.25f, y - height / 2),
                    new PointF(x + width * 0.75f, y - height / 2),
                    new PointF(x + width, y),
                    new PointF(x + width * 0.75f, y + height / 2),
                    new PointF(x + width * 0.25f, y + height / 2),
                };
        }



    }
}
