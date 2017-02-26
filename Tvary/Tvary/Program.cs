using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tvary
{
    abstract class Shape
    {
        public enum Types
        {
            Square,
            Rectangle,
            Triangle,
            Trapezoid,
            Circle
        }

        protected const char RENDER_CHAR = '#';
        protected double[] Sizes;

        public abstract double Width { get; }
        public abstract double Height { get; }

        public abstract void Render(int X, int Y, bool renderInfo = true);

        public static Shape GetShape(Types type)
        {
            switch (type)
            {
                case Types.Square:
                    return Square.Get();

                case Types.Rectangle:
                    return Rectangle.Get();

                case Types.Triangle:
                    return Triangle.Get();

                case Types.Trapezoid:
                    return Trapezoid.Get();

                case Types.Circle:
                    return Circle.Get();

                default:
                    Console.WriteLine("How");
                    return null;
            }
        }
    }

    class Square : Shape
    {
        public override double Width
        {
            get { return Sizes[0]; }
        }
        public override double Height
        {
            get { return Sizes[0]; }
        }

        private Square()
        { }
        public Square(double size)
        {
            Sizes = new double[] { size };
        }
        public override void Render(int X, int Y, bool renderInfo = true)
        {
            if (renderInfo)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write("Square");
                Console.SetCursorPosition(0, 1);
                Console.Write("A: " + Sizes[0].ToString("F2"));
            }

            Console.BackgroundColor = ConsoleColor.Gray;

            for (double i = 0; i < Height; i += 0.1f)
            {
                Console.CursorTop = Y + (int)i;

                for (double j = 0; j < Width; j += 0.1f)
                {
                    Console.CursorLeft = X + (int)j;

                    Console.Write(RENDER_CHAR);
                }
            }

            Console.BackgroundColor = ConsoleColor.Black;
        }

        public static Square Get()
        {
            int INDEX = (Console.WindowHeight - 5) / 2;
            ConsoleKeyInfo k;
            string sideA = "";

            do
            {
                Console.Clear();

                ConsoleHelper.CenteredText("SQUARE", INDEX);
                ConsoleHelper.CenteredText("Side A", INDEX + 2);
                ConsoleHelper.CenteredText(sideA, INDEX + 3);

                ConsoleHelper.Cursor(sideA.Length, INDEX + 3);

                k = Console.ReadKey(true);

                if (k.Key == ConsoleKey.Enter)
                {
                    double tmp;

                    if (double.TryParse(sideA, out tmp))
                    {
                        return new Square(tmp);
                    }
                }

                else if (k.Key == ConsoleKey.Backspace)
                {
                    if (sideA.Length > 0)
                        sideA = sideA.Remove(sideA.Length - 1, 1);
                }
                    

                else if (char.IsLetterOrDigit(k.KeyChar))
                    sideA += k.KeyChar;

            } while (k.Key != ConsoleKey.Escape);

            return null;
        }
    }
    class Rectangle : Shape
    {
        public override double Width
        {
            get { return Sizes[0]; }
        }
        public override double Height
        {
            get { return Sizes[1]; }
        }

        private Rectangle()
        { }
        public Rectangle(double Width, double Height)
        {
            Sizes = new double[] { Width, Height };
        }
        public override void Render(int X, int Y, bool renderInfo = true)
        {
            if (renderInfo)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write("Rectangle");
                Console.SetCursorPosition(0, 1);
                Console.Write("A: " + Sizes[0].ToString("F2"));
                Console.SetCursorPosition(0, 2);
                Console.Write("B: " + Sizes[1].ToString("F2"));
            }

            Console.BackgroundColor = ConsoleColor.Gray;

            for (double i = 0; i < Height; i += 0.1f)
            {
                Console.CursorTop = Y + (int)i;

                for (double j = 0; j < Width; j += 0.1f)
                {
                    Console.CursorLeft = X + (int)j;

                    Console.Write(RENDER_CHAR);
                }
            }

            Console.BackgroundColor = ConsoleColor.Black;
        }

        public static Rectangle Get()
        {
            int INDEX = (Console.WindowHeight - 9) / 2;
            ConsoleKeyInfo k;
            int selection = 0;

            string[] sizes = new string[2] { "", "" };

            do
            {
                Console.Clear();

                ConsoleHelper.CenteredText("RECTANGLE", INDEX);
                ConsoleHelper.CenteredText("Side A", INDEX + 2);
                ConsoleHelper.CenteredText(sizes[0], INDEX + 3);
                ConsoleHelper.CenteredText("Side B", INDEX + 4);
                ConsoleHelper.CenteredText(sizes[1], INDEX + 5);

                ConsoleHelper.Cursor(sizes[selection].Length, INDEX + 3 + selection * 2);

                k = Console.ReadKey();

                if (k.Key == ConsoleKey.Enter)
                {
                    double tmpA;
                    double tmpB;

                    if (double.TryParse(sizes[0], out tmpA) && double.TryParse(sizes[1], out tmpB))
                    {
                            return new Rectangle(tmpA, tmpB);
                    }
                }

                else if (k.Key == ConsoleKey.Backspace)
                {
                    if (sizes[selection].Length > 0)
                        sizes[selection] = sizes[selection].Remove(sizes[selection].Length - 1, 1);
                }
                 
                else if (k.Key == ConsoleKey.UpArrow && selection > 0)
                    selection--;

                else if (k.Key == ConsoleKey.DownArrow && selection < 1)
                    selection++;

                else if (char.IsLetterOrDigit(k.KeyChar))
                    sizes[selection] += k.KeyChar;

            } while (k.Key != ConsoleKey.Escape);

            return null;
        }
    }
    class Triangle : Shape
    {
        public override double Height
        {
            get { return _vc; }
        }
        public override double Width
        {
            get { return _width; }
        }

        public double A
        {
            get { return Sizes[0]; }
        }
        public double B
        {
            get { return Sizes[1]; }
        }
        public double C
        {
            get { return Sizes[2]; }
        }

        private double Alpha;
        private double Beta;
        private double Gamma;

        private double[] startX;
        private double[] endX;

        private double _width;
        private double _vc;

        private Triangle()
        { }
        public Triangle(double A, double B, double C)
        {
            A = Math.Abs(A);
            B = Math.Abs(B);
            C = Math.Abs(C);

            if (A == 0 || B == 0 || C == 0)
                throw new Exception("One or more sides are equal to ZERO");

            Sizes = new double[] { A, B, C };

            Alpha = Math.Acos((C * C + B * B - A * A) / (2 * B * C));
            Beta = Math.Acos((C * C + A * A - B * B) / (2 * A * C));
            //Gamma = (float)Math.Acos((float)(A * A + B * B - C * C) / (2 * A * B));
            Gamma = Math.PI - Alpha - Beta;

            _vc = Math.Round(A * Math.Sin(Beta));

            startX = new double[(int)Height];
            endX = new double[(int)Height];

            startX[(int)Height - 1] = 0;
            endX[(int)Height - 1] = C;

            /*double _alpha = Math.PI - Alpha;
            double _gamma = Math.PI - _alpha - Math.PI / 3;*/

            double _aX;
            // (int)Math.Sqrt(A * A - Vc * Vc)
            if (Alpha > (Math.PI / 3))
                _aX = -Math.Sqrt(A * A - Height * Height);

            else if (Beta > Math.PI / 3)
                _aX = A + Math.Sqrt(A * A - Height * Height);

            else
                _aX = Math.Sqrt(A * A - Height * Height);

            double tmp = C - _aX;

            for(int i = 0; i < Height - 1; i++)
            {
                double height = _vc - i;

                endX[i] = _aX + tmp - height / Math.Tan(Beta);
                startX[i] = height / Math.Tan(Alpha);
            }

            while ((int)startX[0] < 0)
            {
                for (int i = 0; i < startX.Length; i++)
                {
                    startX[i]++;
                    endX[i]++;
                }
            }

            _width = endX[(int)Height - 1] > endX[0] ? endX[(int)Height - 1] : endX[0];
        }
        public override void Render(int X, int Y, bool renderInfo = true)
        {
            if (renderInfo)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write("Triangle");
                Console.SetCursorPosition(0, 1);
                Console.Write("A: " + Sizes[0].ToString("F2"));
                Console.SetCursorPosition(0, 2);
                Console.Write("B: " + Sizes[1].ToString("F2"));
                Console.SetCursorPosition(0, 3);
                Console.Write("C: " + Sizes[2].ToString("F2"));
            }

            Console.BackgroundColor = ConsoleColor.Gray;

            for (int i = 0; i < startX.Length; i++)
            {
                Console.CursorTop = Y + i;
                
                for(double j = startX[i]; j < endX[i]; j += 0.1f)
                {
                    Console.CursorLeft = X + (int)j;
                    
                    Console.Write(RENDER_CHAR);
                }
            }

            Console.BackgroundColor = ConsoleColor.Black;
        }
        public static bool CanBeConstructed(double A, double B, double C)
        {
            A = Math.Abs(A);
            B = Math.Abs(B);
            C = Math.Abs(C);

            return A < C + B && B < C + A && C < A + B;
        }

        public static Triangle Get()
        {
            int INDEX = (Console.WindowHeight - 9) / 2;
            ConsoleKeyInfo k;
            int selection = 0;

            string[] sizes = new string[3] { "", "", "" };

            do
            {
                Console.Clear();

                ConsoleHelper.CenteredText("TRIANGLE", INDEX);
                ConsoleHelper.CenteredText("Side A", INDEX + 2);
                ConsoleHelper.CenteredText(sizes[0], INDEX + 3);
                ConsoleHelper.CenteredText("Side B", INDEX + 4);
                ConsoleHelper.CenteredText(sizes[1], INDEX + 5);
                ConsoleHelper.CenteredText("Side C", INDEX + 6);
                ConsoleHelper.CenteredText(sizes[2], INDEX + 7);

                ConsoleHelper.Cursor(sizes[selection].Length, INDEX + 3 + selection * 2);

                k = Console.ReadKey(true);
                while(Console.KeyAvailable) Console.ReadKey(true);

                if (k.Key == ConsoleKey.Enter)
                {
                    double tmpA;
                    double tmpB;
                    double tmpC;

                    if (double.TryParse(sizes[0], out tmpA) && double.TryParse(sizes[1], out tmpB) && double.TryParse(sizes[2], out tmpC))
                    {
                        if(Triangle.CanBeConstructed(tmpA, tmpB, tmpC))
                            return new Triangle(tmpA, tmpB, tmpC);
                    }
                }

                else if (k.Key == ConsoleKey.Backspace)
                {
                    if (sizes[selection].Length > 0)
                        sizes[selection] = sizes[selection].Remove(sizes[selection].Length - 1, 1);
                }
                    

                else if (k.Key == ConsoleKey.UpArrow && selection > 0)
                    selection--;

                else if (k.Key == ConsoleKey.DownArrow && selection < 2)
                    selection++;

                else if (char.IsLetterOrDigit(k.KeyChar))
                    sizes[selection] += k.KeyChar;

            } while (k.Key != ConsoleKey.Escape);

            return null;
        }
    }
    class Trapezoid : Shape
    {
        Shape[] Elements;

        public override double Height
        {
            get { return Sizes[2]; }
        }
        public override double Width
        {
            get { return Sizes[0]; }
        }

        private Trapezoid()
        { }
        public Trapezoid(double Height, double A, double C)
        {
            Elements = new Shape[3];

            if(C > A)
            {
                double temp = A;
                A = C;
                C = temp;
            }

            Sizes = new double[] { A, C, Height };

            double tringleWidth = (A - C) / 2;
            double thirdSize = Math.Sqrt(tringleWidth * tringleWidth + Height * Height);

            if (Triangle.CanBeConstructed(thirdSize, tringleWidth, Height))
            {
                Elements[0] = new Triangle(thirdSize, Height, tringleWidth);
                Elements[1] = new Rectangle(C, Height);
                Elements[2] = new Triangle(Height, thirdSize, tringleWidth);
            }
            else
                throw new Exception("Can't be constructed");
        }
        public override void Render(int X, int Y, bool renderInfo = true)
        {
            if (renderInfo)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write("Trapezoid");
                Console.SetCursorPosition(0, 1);
                Console.Write("A: " + Sizes[0].ToString("F2"));
                Console.SetCursorPosition(0, 2);
                Console.Write("C: " + Sizes[1].ToString("F2"));
                Console.SetCursorPosition(0, 3);
                Console.Write("Height: " + Sizes[2].ToString("F2"));
            }

            Console.BackgroundColor = ConsoleColor.Gray;

            Elements[2].Render(X, Y, false);
            Elements[1].Render(X + (int)Elements[0].Width, Y, false);
            Elements[0].Render(X + (int)Elements[0].Width + (int)Elements[1].Width, Y, false);

            Console.BackgroundColor = ConsoleColor.Black;
        }

        public static Trapezoid Get()
        {
            int INDEX = (Console.WindowHeight - 9) / 2;
            ConsoleKeyInfo k;
            int selection = 0;

            string[] sizes = new string[3] { "", "", "" };

            do
            {
                Console.Clear();

                ConsoleHelper.CenteredText("TRAPEZOID", INDEX);
                ConsoleHelper.CenteredText("Side A", INDEX + 2);
                ConsoleHelper.CenteredText(sizes[0], INDEX + 3);
                ConsoleHelper.CenteredText("Side C", INDEX + 4);
                ConsoleHelper.CenteredText(sizes[1], INDEX + 5);
                ConsoleHelper.CenteredText("Height", INDEX + 6);
                ConsoleHelper.CenteredText(sizes[2], INDEX + 7);

                ConsoleHelper.Cursor(sizes[selection].Length, INDEX + 3 + selection * 2);

                k = Console.ReadKey();

                if (k.Key == ConsoleKey.Enter)
                {
                    double tmpA;
                    double tmpC;
                    double tmpHeight;

                    if (double.TryParse(sizes[0], out tmpA) && double.TryParse(sizes[1], out tmpC) && double.TryParse(sizes[2], out tmpHeight))
                    {
                        return new Trapezoid(tmpHeight, tmpA, tmpC);
                    }
                }

                else if (k.Key == ConsoleKey.Backspace)
                {
                    if (sizes[selection].Length > 0)
                        sizes[selection] = sizes[selection].Remove(sizes[selection].Length - 1, 1);
                }
                   
                else if (k.Key == ConsoleKey.UpArrow && selection > 0)
                    selection--;

                else if (k.Key == ConsoleKey.DownArrow && selection < 2)
                    selection++;

                else if (char.IsLetterOrDigit(k.KeyChar))
                    sizes[selection] += k.KeyChar;

            } while (k.Key != ConsoleKey.Escape);

            return null;
        }
    }
    class Circle : Shape
    {
        public override double Height
        {
            get { return Radius * 2; }
        }
        public override double Width
        {
            get { return Radius * 2; }
        }
        public double Radius
        {
            get { return _radius; }
        }

        private double _radius;
        private double[] startX;
        private double[] endX;

        private Circle()
        { }
        public Circle(double radius)
        {
            _radius = radius;

            Sizes = new double[] { radius };
            startX = new double[(int)Height];
            endX = new double[(int)Height];

            double tmp = Math.PI / 2;

            for (double i = 0; i < tmp; i += 0.01)
            {
                double _startX = Math.Cos(i) * radius;
                double _startY = (Math.Sin(tmp) - Math.Sin(i)) * radius;

                double minus = Math.Round(radius -_startX);
                double plus = Math.Round(radius + _startX);

                double bottomLayer = Math.Round(Height - _startY);
                double topLayer = Math.Round(_startY) - 1;

                if (topLayer < 0)
                    break;

                startX[(int)topLayer] = /*startX[topLayer] <*/ minus /*? minus : startX[topLayer]*/;
                startX[(int)bottomLayer] = /*startX[bottomLayer] <*/ minus /*? minus : startX[bottomLayer]*/;

                endX[(int)topLayer] = /*endX[topLayer] <*/ plus/* ? plus : endX[topLayer]*/;
                endX[(int)bottomLayer] = /*endX[bottomLayer] >*/ plus /*? endX[bottomLayer] : plus*/;
            }

            while (startX[(int)radius] < 0)
            {
                for (int i = 0; i < Width; i++)
                {
                    startX[i]++;
                    endX[i]++;
                }
            }
        }
        public override void Render(int X, int Y, bool renderInfo = true)
        {
            if (renderInfo)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write("Circle");
                Console.SetCursorPosition(0, 1);
                Console.Write("Radius: " + Radius.ToString("F2"));
            }

            Console.BackgroundColor = ConsoleColor.Gray;

            for (int i = 0; i < startX.Length; i++)
            {
                Console.CursorTop = Y + i;

                for (double j = startX[i]; j < endX[i]; j += 0.1f)
                {
                    Console.CursorLeft = X + (int)j;

                    Console.Write(RENDER_CHAR);
                }
            }

            Console.BackgroundColor = ConsoleColor.Black;
        }

        public static Circle Get()
        {
            int INDEX = (Console.WindowHeight - 5) / 2;
            ConsoleKeyInfo k;
            string radius = "";

            do
            {
                Console.Clear();

                ConsoleHelper.CenteredText("CIRCLE", INDEX);
                ConsoleHelper.CenteredText("Radius", INDEX + 2);
                ConsoleHelper.CenteredText(radius, INDEX + 3);

                ConsoleHelper.Cursor(radius.Length, INDEX + 3);

                k = Console.ReadKey();

                if (k.Key == ConsoleKey.Enter)
                {
                    double tmp;

                    if (double.TryParse(radius, out tmp))
                    {
                        return new Circle(tmp);
                    }
                }

                else if (k.Key == ConsoleKey.Backspace)
                {
                    if (radius.Length > 0)
                        radius = radius.Remove(radius.Length - 1, 1);
                }
                    
                else if (char.IsLetterOrDigit(k.KeyChar))
                    radius += k.KeyChar;

            } while (k.Key != ConsoleKey.Escape);

            return null;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.SetIn(TextReader.Null);
            ConsoleHelper.SetConsoleFont(8, 8, "Terminal", false);

            bool setSize = true;
            int Width = 80;
            int Height = 40;

            do
            {
                try
                {
                    Console.SetWindowSize(Width, Height);
                    Console.SetBufferSize(Width, Height);

                    setSize = true;
                }
                catch
                {
                    Width--;
                    Height--;
                    setSize = false;
                }
                
            } while (!setSize);

            Shape shape;
            ConsoleKeyInfo k;
        
            int INDEX = (Console.WindowHeight - 8) / 2;
            int selection = 0;
            
            do
            {
                Console.Clear();

                ConsoleHelper.CenteredText("Select shape", INDEX);
                ConsoleHelper.CenteredText("Square", INDEX + 2);
                ConsoleHelper.CenteredText("Rectangle", INDEX + 3);
                ConsoleHelper.CenteredText("Triangle", INDEX + 4);
                ConsoleHelper.CenteredText("Trapezoid", INDEX + 5);
                ConsoleHelper.CenteredText("Circle", INDEX + 6);

                ConsoleHelper.Cursor("Rectangle".Length, INDEX + 2 + selection);

                k = Console.ReadKey(true);

                if (k.Key == ConsoleKey.UpArrow && selection > 0)
                    selection--;

                else if (k.Key == ConsoleKey.DownArrow && selection < 4)
                    selection++;

                else if(k.Key == ConsoleKey.Enter)
                {
                    while (Console.KeyAvailable)
                        Console.ReadKey(true);
                    
                    shape = Shape.GetShape((Shape.Types)selection);
                    if (shape != null)
                    {
                        Console.Clear();
                        int X = (int)((Console.WindowWidth - shape.Width) / 2);
                        int Y = (int)((Console.WindowHeight - shape.Height) / 2);
                        shape.Render(X, Y);

                        while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;
                    }
                }

            } while (k.Key != ConsoleKey.Escape);
        }
    }

    static class ConsoleHelper
    {
        public static void CenteredText(string txt, int Y)
        {
            Console.CursorLeft = (Console.WindowWidth - txt.Length) / 2;
            Console.CursorTop = Y;

            Console.Write(txt);
        }

        public static void Cursor(int Width, int Y)
        {
            const string ARROWL = "> ";
            const string ARROWR = " <";

            Console.CursorTop = Y;
            Console.CursorLeft = (Console.WindowWidth - Width) / 2 - 3;
            Console.Write(ARROWL);
            Console.CursorLeft = (Console.WindowWidth + Width) / 2 + 2;
            Console.Write(ARROWR);
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal unsafe struct CONSOLE_FONT_INFO_EX
        {
            internal uint cbSize;
            internal uint nFont;
            internal COORD dwFontSize;
            internal int FontFamily;
            internal int FontWeight;
            internal fixed char FaceName[LF_FACESIZE];
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct COORD
        {
            internal short X;
            internal short Y;

            internal COORD(short x, short y)
            {
                X = x;
                Y = y;
            }
        }
        private const int STD_OUTPUT_HANDLE = -11;
        private const int TMPF_TRUETYPE = 4;
        private const int LF_FACESIZE = 32;
        private static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetCurrentConsoleFontEx(
            IntPtr consoleOutput,
            bool maximumWindow,
            ref CONSOLE_FONT_INFO_EX consoleCurrentFontEx);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int dwType);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int SetConsoleFont(IntPtr hOut, uint dwFontNum);

        public unsafe static void SetConsoleFont(short X, short Y, string fontName = "Lucida Console", bool truetype = false, int fontWeight = 300)
        {
            IntPtr hnd = GetStdHandle(STD_OUTPUT_HANDLE);
            if (hnd != INVALID_HANDLE_VALUE)
            {
                CONSOLE_FONT_INFO_EX newInfo = new CONSOLE_FONT_INFO_EX();
                newInfo.cbSize = (uint)Marshal.SizeOf(newInfo);
                newInfo.FontFamily = truetype ? TMPF_TRUETYPE : 0;
                IntPtr ptr = new IntPtr(newInfo.FaceName);
                Marshal.Copy(fontName.ToCharArray(), 0, ptr, fontName.Length);
                newInfo.dwFontSize = new COORD(X, Y);
                newInfo.FontWeight = fontWeight;
                SetCurrentConsoleFontEx(hnd, false, ref newInfo);
            }
        }
    }
}
