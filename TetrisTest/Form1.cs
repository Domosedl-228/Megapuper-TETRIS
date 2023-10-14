using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Drawing.Text;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using TetrisTest;

namespace TetrisTest
{
    public partial class TetrisForm : Form
    {
        public TetrisForm()
        {
            InitializeComponent();

            map = new Bitmap(800, 800);
            graphics = Graphics.FromImage(map);
            colors[0] = GameArea.BackColor;
            labelScore.Text = "0";

            gameStatus.Text = "Game Begin";

            Random rnd = new Random();



            DrawFigure((byte)rnd.Next(0,8));


            

            timer.Tick += new EventHandler(testing);
            timer.Interval = 500;
            timer.Start();

        }

        System.Windows.Forms.Timer timer = new();


        Figure currentFigure = new Figure();


        bool isFlipped = false;


        private void testing(Object myObject, EventArgs myEventArgs)
        {

            if (!CheckFloor())
            {
                nextVector = 4;
                test();
                nextVector = 0;
            }
            else
            {
                if (CheckGameOver()) { timer.Stop(); gameStatus.Text = "Game Over!"; }
                CheckFull();
                Random rnd = new Random();
                DrawFigure(Convert.ToByte(rnd.Next(0, 8)));
            }
        }

        private bool CheckGameOver() 
        {
            int ii = 0;
            int j = 0;
            for (int i = 0; i < 20; i++) 
            {
                if (i <= 9) ii = i;
                else { ii = i - 10; j = 1;}

                if (pixels[ii, j] != 0) return true;

            }



            return false;
        }

        private void CheckFull()
        {
            bool[] stage = new bool[10];



            for (int i = 19; i >= 0; i--)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (pixels[j, i] != 0) stage[j] = true;
                }
                if (CheckStage(stage)) { RemoveStage(i); break; }
                stage = new bool[10];
            }
        }

        private void RemoveStage(int stage)
        {
            var newPixels = new byte[10,20];
            for (int i = 0; i < 20; i++)
                for (int j = 0; j < 10; j++) 
                {
                    if (i - 1 > 0) newPixels[j, i] = pixels[j, i - 1];
                }

            for (int i = stage + 1; i < 20; i++) 
            {
                for (int j = 0; j < 10; j++) 
                {
                    newPixels[j, i] = pixels[j, i];
                }
            }



            pixels = newPixels;
            labelScore.Text = (int.Parse(labelScore.Text) + 100).ToString();
        }


        private bool CheckStage(bool[] bools)
        {
            foreach (bool bl in bools)
            {
                if (!bl) return false;
            }
            return true;
        }


        private void test()
        {
            if (isFall)
            {
                isFall = false;
                OneStep();
                DrawFrame();
                GameArea.Image = map;
                isFall = true;
            }

        }


        private bool isFall = true;

        private class Figure
        {

            public byte classs;


            List<Point> pixels = new List<Point>();

            public byte rotation;

            public List<Point> GetPoints() { return this.pixels; }

            public void SetPoints(List<Point> points) { this.pixels = points; }
        }


        private byte[,] pixels = new byte[10, 20];




        private Color[] colors = new Color[9] { Color.White, Color.Black, Color.Yellow, Color.Orange, Color.Red, Color.Green, Color.Blue, Color.Violet, Color.CadetBlue };


        Bitmap map = new Bitmap(10, 10);

        Graphics graphics;

        Size sizeStndr = new Size(20, 20);

        Figure prevPosition = new Figure();

        private byte nextVector = 0;


        private void TetrisForm_KeyUp(object sender, KeyEventArgs e)
        {

        }


        private void OneStep()
        {
            var prevPosition = this.prevPosition.GetPoints();
            int x = 0;
            int y = 0;




            for (int i = 0; i < prevPosition.Count; i++)
            {
                x = prevPosition[i].X;
                y = prevPosition[i].Y;
                pixels[x, y] = 0;
            }
            if (isFlipped) FlipFigure();
            else if (CheckMove()) ChangeFigurePositon();

            var curentPosition = this.currentFigure.GetPoints();
            for (int i = 0; i < curentPosition.Count; i++)
            {
                x = curentPosition[i].X;
                y = curentPosition[i].Y;
                pixels[x, y] = currentFigure.classs;
            }
            this.prevPosition = currentFigure;

        }


        private void ChangeFigurePositon()
        {
            var currentposition = currentFigure.GetPoints();
            for (int i = 0; i < currentFigure.GetPoints().Count; i++)
            {
                Point point = currentposition[i];

                if (nextVector == 1) point.X--;
                if (nextVector == 2) point.X++;
                if (nextVector == 3) point.Y--;
                if (nextVector == 4) point.Y++;

                currentposition[i] = point;
            }
            currentFigure.SetPoints(currentposition);
        }

        private bool CheckFloor()
        {
            var points = currentFigure.GetPoints();
            foreach (Point point in points)
            {
                if (point.Y == 19 || (CheckPixel(new Point(point.X, point.Y + 1)) && pixels[point.X, point.Y + 1] != 0)) return true;
            }

            return false;
        }


        private bool CheckPixel(Point pointw)
        {
            var points = currentFigure.GetPoints();
            foreach (Point point in points)
            {
                if (point == pointw) return false;
            }
            return true;
        }




        private bool CheckMove()
        {
            var points = currentFigure.GetPoints();
            foreach (Point point in points)
            {
                if (nextVector == 1 && point.X <= 0) return false;
                if (nextVector == 2 && point.X >= 9) return false;
                if (nextVector == 4 && point.Y >= 19) return false;
                if (CheckFloor()) return false;
            }

            return true;
        }




        private void DrawFigure(byte figure)
        {
            switch (figure)
            {
                case 0:
                    DrawCube();
                    break;
                case 1:
                    DrawStick();
                    break;
                case 2:
                    DrawL();
                    break;
                case 3:
                    DrawS();
                    break;
                case 4:
                    DrawT();

                    break;
                case 5:
                    DrawZ();

                    break;
                case 6:

                    DrawJ();
                    break;


            }
        }

        private void DrawPixel(Point point, Color color)
        {
            Rectangle rectangle = new Rectangle(point, sizeStndr);
            Size size = sizeStndr;
            size.Width--;
            size.Height--;
            rectangle = new Rectangle(point, size);
            graphics.DrawRectangle(new Pen(Color.Black), rectangle);
            for (int i = 0; size.Width > 0; i++)
            {
                size.Width--;
                size.Height--;
                rectangle = new Rectangle(point, size);
                graphics.DrawRectangle(new Pen(color), rectangle);
            }
        }

        public Point GetPoint(int x, int y)
        {
            return new Point(x * 20, y * 20);
        }


        private void DrawFrame()
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 20; j++)
                {


                    DrawPixel(GetPoint(i, j), colors[pixels[i, j]]);


                }

        }

        private void DrawCube()
        {
            List<Point> points = new List<Point>();

            points.Add(new Point(5, 0));
            points.Add(new Point(5, 1));
            points.Add(new Point(6, 0));
            points.Add(new Point(6, 1));
            currentFigure.SetPoints(points);
            currentFigure.classs = 2;
        }


        private void DrawStick()
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(3, 0));
            points.Add(new Point(4, 0));
            points.Add(new Point(5, 0));
            points.Add(new Point(6, 0));
            currentFigure.SetPoints(points);
            currentFigure.classs = 8;
        }

        private void DrawJ()
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(5, 0));
            points.Add(new Point(6, 0));
            points.Add(new Point(7, 0));
            points.Add(new Point(7, 1));
            currentFigure.SetPoints(points);
            currentFigure.classs = 7;
        }

        private void DrawL()
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(5, 0));
            points.Add(new Point(6, 0));
            points.Add(new Point(7, 0));
            points.Add(new Point(5, 1));
            currentFigure.SetPoints(points);
            currentFigure.classs = 3;
        }


        private void DrawS()
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(5, 0));
            points.Add(new Point(6, 0));
            points.Add(new Point(5, 1));
            points.Add(new Point(4, 1));
            currentFigure.SetPoints(points);
            currentFigure.classs = 4;
        }

        private void DrawZ()
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(5, 0));
            points.Add(new Point(4, 0));
            points.Add(new Point(5, 1));
            points.Add(new Point(6, 1));
            currentFigure.SetPoints(points);
            currentFigure.classs = 6;
        }

        private void DrawT()
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(5, 0));
            points.Add(new Point(6, 0));
            points.Add(new Point(7, 0));
            points.Add(new Point(6, 1));
            currentFigure.SetPoints(points);
            currentFigure.classs = 5;
        }

        private void FlipFigure()
        {
            isFlipped = false;
            currentFigure.rotation++;
            if (currentFigure.rotation > 3) currentFigure.rotation = 0;

            if (currentFigure.classs == 8)
            {
                var points = currentFigure.GetPoints();
                var point = points[0];
                switch (currentFigure.rotation % 2)
                {
                    case 0:
                        points[1] = new(point.X - 1, point.Y);
                        points[2] = new(point.X+1, point.Y);
                        points[3] = new(point.X + 2, point.Y);
                        break;
                    case 1:
                        points[1] = new(point.X, point.Y - 1);
                        points[2] = new(point.X, point.Y+1);
                        points[3] = new(point.X, point.Y + 2);
                        break;

                }



                currentFigure.SetPoints(points);
            }
            else switch (currentFigure.classs)
                {
                    case 3:
                        FlipL();
                        break;
                    case 4:
                        FlipS();
                        break;
                    case 5:
                        FlipT();
                        break;
                    case 6:
                        FlipZ();
                        break;
                    case 7:
                        FlipJ();
                        break;
                }
            if (CheckFlipped()) 
            {
                currentFigure.rotation--;
                currentFigure.rotation--;
                FlipFigure(); 
            }


        }

        private bool CheckFlipped() 
        {
            int x = 0, y = 0;
            foreach (Point point in currentFigure.GetPoints()) 
            {
                x = point.X; y = point.Y;
                if (x > 9 || x < 0) return true;
                if (y > 19 || y < 0) return true;
            }


            return false;
        }


        private void FlipJ()
        {
            var points = currentFigure.GetPoints();
            Point point = points[0];
            switch (currentFigure.rotation)
            {
                case 0:
                    points[1] = new(point.X - 1, point.Y);
                    points[2] = new(point.X + 1, point.Y);
                    points[3] = new(point.X + 1, point.Y + 1);
                    break;
                case 1:
                    points[1] = new(point.X, point.Y - 1);
                    points[2] = new(point.X, point.Y + 1);
                    points[3] = new(point.X - 1, point.Y + 1);
                    break;
                case 2:
                    points[1] = new(point.X - 1, point.Y);
                    points[2] = new(point.X + 1, point.Y);
                    points[3] = new(point.X - 1, point.Y - 1);
                    break;
                case 3:
                    points[1] = new(point.X, point.Y - 1);
                    points[2] = new(point.X + 1, point.Y - 1);
                    points[3] = new(point.X, point.Y + 1);
                    break;



            }


            currentFigure.SetPoints(points);
        }




        private void FlipZ()
        {
            var points = currentFigure.GetPoints();
            Point point = points[0];
            switch (currentFigure.rotation % 2)
            {
                case 0:
                    points[1] = new(point.X - 1, point.Y);
                    points[2] = new(point.X, point.Y + 1);
                    points[3] = new(point.X + 1, point.Y + 1);
                    break;
                case 1:
                    points[1] = new(point.X, point.Y - 1);
                    points[2] = new(point.X + 1, point.Y);
                    points[3] = new(point.X + 1, point.Y + 1);
                    break;

            }
            currentFigure.SetPoints(points);
        }



        private void FlipT()
        {
            var points = currentFigure.GetPoints();
            Point point = points[0];
            switch (currentFigure.rotation)
            {
                case 0:
                    points[1] = new Point(point.X - 1, point.Y);
                    points[2] = new(point.X + 1, point.Y);
                    points[3] = new(point.X, point.Y - 1);
                    break;
                case 1:
                    points[1] = new Point(point.X, point.Y - 1);
                    points[2] = new(point.X, point.Y + 1);
                    points[3] = new(point.X + 1, point.Y);
                    break;
                case 2:
                    points[1] = new Point(point.X - 1, point.Y);
                    points[2] = new(point.X + 1, point.Y);
                    points[3] = new(point.X, point.Y + 1);
                    break;
                case 3:
                    points[1] = new Point(point.X, point.Y + 1);
                    points[2] = new(point.X, point.Y - 1);
                    points[3] = new(point.X - 1, point.Y);
                    break;
            }
            currentFigure.SetPoints(points);

        }






        private void FlipS()
        {

            var points = currentFigure.GetPoints();
            Point point = points[0];

            switch (currentFigure.rotation)
            {
                case 0:
                    points[1] = new Point(point.X + 1, point.Y);
                    points[2] = new Point(point.X, point.Y + 1);
                    points[3] = new Point(point.X - 1, point.Y + 1);
                    currentFigure.SetPoints(points);
                    break;

                case 1:
                    points[1] = new Point(point.X, point.Y - 1);
                    points[2] = new Point(point.X + 1, point.Y);
                    points[3] = new Point(point.X + 1, point.Y + 1);
                    currentFigure.SetPoints(points);

                    break;
                case 2:
                    points[1] = new Point(point.X + 1, point.Y);
                    points[2] = new Point(point.X, point.Y + 1);
                    points[3] = new Point(point.X - 1, point.Y + 1);
                    currentFigure.SetPoints(points); break;
                case 3:
                    points[1] = new Point(point.X, point.Y - 1);
                    points[2] = new Point(point.X + 1, point.Y);
                    points[3] = new Point(point.X + 1, point.Y + 1);
                    currentFigure.SetPoints(points);

                    break;

            }
        }

        private void FlipL()
        {
            var points = currentFigure.GetPoints();
            Point point = points[0];
            var tmp = point;
            switch (currentFigure.rotation)
            {
                case 0:
                    for (int i = 1; i < 3; i++)
                    {
                        tmp.X++;
                        points[i] = tmp;
                    }
                    point.Y++;
                    points[3] = point;
                    currentFigure.SetPoints(points);


                    break;

                case 1:
                    for (int i = 1; i < 3; i++)
                    {
                        tmp.Y++;
                        points[i] = tmp;
                    }
                    tmp.X++;
                    points[3] = tmp;
                    currentFigure.SetPoints(points);
                    break;
                case 2:
                    for (int i = 1; i < 3; i++)
                    {
                        tmp.X++;
                        points[i] = tmp;
                    }
                    tmp.Y--;
                    points[3] = tmp;
                    currentFigure.SetPoints(points);
                    break;

                case 3:
                    for (int i = 1; i < 3; i++)
                    {
                        tmp.Y++;
                        points[i] = tmp;
                    }
                    point.X--;
                    points[3] = point;
                    currentFigure.SetPoints(points);
                    break;
            }




        }



        private void TetrisForm_Load(object sender, EventArgs e)
        {

        }

        private void TetrisForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    nextVector = 1;
                    break;
                case Keys.Right:
                    nextVector = 2;
                    break;
                case Keys.Up:
                    isFlipped = true;
                    break;
                case Keys.Down:
                    nextVector = 4;
                    break;
            }
        }


        




        private void TetrisForm_KeyUp_1(object sender, KeyEventArgs e)
        {
            test();
            nextVector = 0;
        }


        private void PrintOnLabel(Figure figure, Label label)
        {
            label.Text = string.Empty;

            var gf = figure.GetPoints();

            foreach (Point point in gf)
            {
                label.Text += " ";
                label.Text += point;
            }


        }





    }
}
