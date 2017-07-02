using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PianoTrainer
{
    public partial class FormMain : Form
    {

        public class MultipleRow : List<Row>
        {

        }


        public class Row : List<Accord>
        {

        }

        public class Accord : List<int>
        {
        }

        public MultipleRow multipleRow = new MultipleRow();

        void generateMultipleRow(int quAccord)
        {
            multipleRow.Clear();

            for (int i = 0; i < 2; i++)
            {
                Row row = new Row();
                multipleRow.Add(row);

                for (int j = 0; j < quAccord; j++)
                {

                    Accord accord = new Accord();
                    int startNote = rnd.Next(4, 16);
                    accord.Add(startNote);
                    accord.Add(startNote + rnd.Next(1, 4));
                    accord.Add(startNote - rnd.Next(1, 4));
                    accord.Sort();
                    row.Add(accord);
                }

            }

        }

        public FormMain()
        {
            InitializeComponent();
            rnd = new Random();

            initGraphics();
        }

        void initGraphics()
        {
            graphics = panel.CreateGraphics();
            draw();
        }

        Random rnd;
        Graphics graphics;

        //Шаг между нотами
        const int noteStep = 120;
        //Расстояние между нотоносцами
        const int rowHeight = 15;
        //Верхнее поле
        const int xStart = 100;
        //Левое поле
        const int yStart = 100;
        //Высота ноты
        const int noteHeight = rowHeight - 4;
        //Ширина ноты
        const int noteWidth = (int)(noteHeight * 1.3);
        //Сдвиг между рядами нотоносцев
        const int shift = 180;
        //Сдвиг ноты относительно верхнего нотоносца
        int shiftFistNote = 6;

        //Длина нотоноссца
        int rowLen
        {
            get
            {
                return panel.Width - 2 * xStart;
            }
        }
        //Количество нот
        int quNote
        {
            get
            {
                return rowLen / noteStep;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        int quNoteInMultipleRow
        {
            get
            {
                if (multipleRow.Count == 0)
                {
                    return 0;
                }
                else
                {
                    return multipleRow[0].Count;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        void draw()
        {
            graphics.Clear(Color.White);


            Pen myPen = new Pen(Color.Black, 2);
            SolidBrush myBrush = new SolidBrush(System.Drawing.Color.Black);

            //Рисуем нотоносцы
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 5; i++)
                {
                    graphics.DrawLine(myPen,
                        xStart,
                        yStart + i * rowHeight + j * shift,
                        xStart + rowLen,
                        yStart + i * rowHeight + j * shift
                        );
                }
            }

            //Рисуем ноты
            if (quNoteInMultipleRow > 0)
            {
                const int noteShift = 2;
                int fistNoteShift = (rowLen - ((quNoteInMultipleRow - 1) * noteStep)) / 2;
                for (int j = 0; j < multipleRow.Count; j++)
                {
                    Row row = multipleRow[j];
                    for (int i = 0; i < row.Count; i++)
                    {
                        Accord accord = row[i];

                        bool needShift = false;
                        for (int noteNum = 0; noteNum < accord.Count; noteNum++)
                        {
                            int note = accord[noteNum];
                            if (noteNum - 1 > 0 && Math.Abs((accord[noteNum - 1] - note)) == 1)
                            {
                                needShift = true;
                            }
                            if (noteNum + 1 < accord.Count && Math.Abs((accord[noteNum + 1] - note)) == 1)
                            {
                                needShift = true;
                            }
                        }

                        for (int noteNum = 0; noteNum < accord.Count; noteNum++)
                        {
                            int note = accord[noteNum];

                            int noteShiftVector = noteNum % 2 == 0 ? -1 : 1;

                            int x = xStart
                                + i * noteStep
                                + fistNoteShift
                                + (needShift ? noteShiftVector * noteWidth / 2 : 0);
                            int y = yStart //Поле
                                + noteShift //Сдвиг ноты относительно нотоносца
                                + j * shift //Номер ряда нотоносцев
                                + (((note - shiftFistNote) * rowHeight) / 2);//Номер ноты


                            //Нота
                            graphics.DrawArc(myPen, x, y, noteWidth, noteHeight, 0, 180);
                            graphics.FillPie(myBrush, x, y, noteWidth, noteHeight, 0, 180);
                            graphics.DrawArc(myPen, x, y, noteWidth, noteHeight, 180, 180);
                            graphics.FillPie(myBrush, x, y, noteWidth, noteHeight, 180, 180);

                            //Дополнительные нотоносцы с верху
                            for (int addNN = 0; addNN < ((shiftFistNote - note + 1) / 2); addNN++)
                            {
                                graphics.DrawLine(myPen,
                                    x - 10,
                                    yStart - addNN * rowHeight + j * shift,
                                    x + 25,
                                    yStart - addNN * rowHeight + j * shift
                                    );
                            }

                            //Дополнительные нотоносцы с низу
                            for (int addNN = 0; addNN < (((note + 1) - (shiftFistNote + 8)) / 2); addNN++)
                            {
                                graphics.DrawLine(myPen,
                                    x - 10,
                                    yStart + addNN * rowHeight + j * shift + 5 * rowHeight,
                                    x + 25,
                                    yStart + addNN * rowHeight + j * shift + 5 * rowHeight
                                    );
                            }

                            int xFlag = xStart
                                + i * noteStep
                                + fistNoteShift + 1;

                            int yFlag = yStart //Поле
                                + noteShift //Сдвиг ноты относительно нотоносца
                                + j * shift //Номер ряда нотоносцев
                                + (((note - shiftFistNote) * rowHeight) / 2);//Номер ноты

                            graphics.DrawLine(myPen,
                                xFlag + (needShift ? noteWidth / 2 : noteWidth),
                                yFlag + noteHeight / 2,
                                xFlag + (needShift ? noteWidth / 2 : noteWidth),
                                yFlag - 40);

                        }
                    }
                }
            }

            myPen.Dispose();
            myBrush.Dispose();

        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            generateMultipleRow(quNote);
            draw();
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
        }

        private void FormMain_Paint(object sender, PaintEventArgs e)
        {
            //draw();

        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            initGraphics();
        }
    }
}
