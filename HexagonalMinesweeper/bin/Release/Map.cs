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

        public void DrawHexMap(Graphics g, Pen pen, float height)
        {
            PointF[] hexPoints;
            for (int row = 0; row < field.GetLength(1); row++)
            {
                for (int col = 0; col < field.GetLength(0); col++)
                {
                    hexPoints = HexToCoords(height, row, col);
                    g.DrawPolygon(pen, hexPoints);                   
                }
            }
        }
        public void DrawBombs(Graphics g, Brush brush, float height)
        {
            PointF[] hexPoints;
            for (int row = 0; row < field.GetLength(1); row++)
            {
                for (int col = 0; col < field.GetLength(0); col++)
                {
                    hexPoints = HexToCoords(height, row, col);
                    if (field[col, row] == 1)
                        g.FillPolygon(brush, hexPoints);
                }
            }

        }
        public void DrawRevealed(Graphics g, Brush brush, float height)
        {
            PointF[] hexPoints;
            for (int row = 0; row < field.GetLength(1); row++)
            {
                for (int col = 0; col < field.GetLength(0); col++)
                {
                    hexPoints = HexToCoords(height, row, col);
                    if (field[col, row] == -1)
                    {
                        g.FillPolygon(brush, hexPoints);
                        if (neighbourField[col, row] == "1")
                            g.DrawImage(numbers[1], hexPoints[1].X - 20, hexPoints[2].Y - 10);
                        else if (neighbourField[col, row] == "2")
                            g.DrawImage(numbers[2], hexPoints[1].X - 20, hexPoints[1].Y - 10);
                        else if (neighbourField[col, row] == "3")
                            g.DrawImage(numbers[3], hexPoints[1].X - 20, hexPoints[1].Y - 10);
                        else if (neighbourField[col, row] == "4")
                            g.DrawImage(numbers[4], hexPoints[1].X - 20, hexPoints[1].Y - 10);
                        else if (neighbourField[col, row] == "5")
                            g.DrawImage(numbers[5], hexPoints[1].X - 20, hexPoints[1].Y - 10);
                        else if (neighbourField[col, row] == "6")
                            g.DrawImage(numbers[6], hexPoints[1].X - 20, hexPoints[1].Y - 10);
                    };
                }
            }
        }
        public void DrawFlags(Graphics g, Brush brush, float height)
        {
            PointF[] hexPoints;
            for (int row = 0; row < flagField.GetLength(1); row++)
            {
                for (int col = 0; col < flagField.GetLength(0); col++)
                {
                    hexPoints = HexToCoords(height, row, col);
                    if (flagField[col, row] == 1)
                    {
                        g.FillPolygon(brush, hexPoints);
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


        public void RevealNeighbours(int col, int row)
        {
            if (CheckFieldValue(col, row) == 1 || CheckFlagFieldValue(col, row) == 1 ) //field with bomb neighbouring -> stop function
                return;
            else
            {
                if (col % 2 == 1 ) //when the collumn is odd
                {
                    if (CheckFieldValue(col, row) == 0) // 0 - unrevealed yet
                    {
                        if (!neighbourField[col,row].Equals("0"))
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

                        if (!neighbourField[col, row].Equals("0"))
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


        public float HexWidth(float hexHeight)
        {
            return (float)(4 * (hexHeight / 2 / Math.Sqrt(3)));
        }


        public void MouseToHexPoint(float x, float y, float hexHeight,
             out int column, out int row) // getting coordinates from mouse point
        {
            float hexWidth = HexWidth(hexHeight);
            column = (int)(x / (hexWidth * 0.75f));

            if (column % 2 == 0)
                row = (int)(y / hexHeight);
            else
                row = (int)((y - hexHeight / 2) / hexHeight);

            float areaX = column * hexWidth * 0.75f;
            float areaY = row * hexHeight;
            if (column % 2 == 1)
                areaY = areaY + hexHeight / 2;

            bool isHexAbove = false, isHexBelow = false;
            float subX = x - areaX;
            if (subX < hexWidth / 4)
            {
                float subY = y - (areaY + hexHeight / 2);
                if (subX < 0.001)
                {
                    if (subY < 0)
                        isHexAbove = true;
                    if (subY > 0)
                        isHexBelow = true;
                }
                else if (subY < 0 && -subY / subX > Math.Sqrt(3))
                        isHexAbove = true;
                else
                {
                    if (subY / subX > Math.Sqrt(3))
                        isHexBelow = true;
                }
            }

            if (isHexAbove)
            {
                if (column % 2 == 0)
                    row--;
                column--;
            }
            else if (isHexBelow)
            {
                if (column % 2 == 1)
                    row++;
                column--;
            }
        }

        private PointF[] HexToCoords(float height, float row, float column)
        {
            float width = HexWidth(height);
            float y = height / 2;
            float x;
            y = y + row * height;

            if (column % 2 == 1)
                y = y + height / 2;

            x = column * (width * 0.75f);

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
