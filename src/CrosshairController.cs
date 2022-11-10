using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace Static_Crosshair_Generator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string Localization = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Static Crosshair Generator\Configs";
        public string configLocation = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Static Crosshair Generator\Configs\config.ini";

        public int Distance = 10;
        public int Percent = 30;
        public int Thickness = 10;
        public int Length = 41;

        public int DotPercent = 50;

        public int X = 0;
        public int Y = 0;

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        const int GWL_EXSTYLE = -20;
        const int WS_EX_LAYERED = 0x80000;
        const int WS_EX_TRANSPARENT = 0x20;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var style = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, style | WS_EX_LAYERED | WS_EX_TRANSPARENT);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(Localization))
            {
                Directory.CreateDirectory(Localization);
            }

            if (!File.Exists(configLocation))
            {
                File.WriteAllText(configLocation, "Distance: 10\nPercent: 30\nThickness: 10\nLength: 41\nR: 127\nG: 255\nB: 0\nDot: 0\nDotSize: 12\nRD: 127\nGD: 255\nBD: 0\nCross: 1\nX: 0\nY: 0");
            }
            ReadConfig();

            this.BackColor = Color.Aquamarine;
            this.TransparencyKey = Color.Aquamarine;

            //Setting length
            Up.Size = new Size(Up.Size.Width, Length);
            Down.Size = new Size(Down.Size.Width, Length);
            Left.Size = new Size(Length, Left.Size.Height);
            Right.Size = new Size(Length, Right.Size.Height);

            //Setting thickness
            Up.Size = new Size(Thickness, Up.Size.Height);
            Down.Size = new Size(Thickness, Down.Size.Height);
            Left.Size = new Size(Left.Size.Width, Thickness);
            Right.Size = new Size(Right.Size.Width, Thickness);

            //Setting size
            Up.Size = new Size(Up.Size.Width * Percent / 100, Up.Size.Height * Percent / 100);
            Down.Size = new Size(Down.Size.Width * Percent / 100, Down.Size.Height * Percent / 100);
            Left.Size = new Size(Left.Size.Width * Percent / 100, Left.Size.Height * Percent / 100);
            Right.Size = new Size(Right.Size.Width * Percent / 100, Right.Size.Height * Percent / 100);

            Dot.Size = new Size(Dot.Size.Width * DotPercent * 4 / 100, Dot.Size.Height * DotPercent  * 4 / 100);

            //Setting distance
            Up.Location = new Point((this.ClientSize.Width - Up.Width) / 2 + X, (this.ClientSize.Height - Up.Height) / 2 - Distance + Y);
            Down.Location = new Point((this.ClientSize.Width - Down.Width) / 2 + X, (this.ClientSize.Height - Down.Height) / 2 + Distance + Y);
            Left.Location = new Point((this.ClientSize.Width - Left.Width) / 2 - Distance + X, (this.ClientSize.Height - Left.Height) / 2 + Y);
            Right.Location = new Point((this.ClientSize.Width - Right.Width) / 2 + Distance + X, (this.ClientSize.Height - Right.Height) / 2 + Y);

            Dot.Location = new Point((this.ClientSize.Width - Dot.Width) / 2 + X, (this.ClientSize.Height - Dot.Height) / 2 + Y);
        }

        public void ReadConfig()
        {
            string readConfig = File.ReadAllText(configLocation);
            int numberOfLines = File.ReadAllLines(configLocation).Count();
            string[] configArray = File.ReadAllLines(configLocation);


            for (int i = 0; i < numberOfLines; i++)
            {
                if (configArray[i].Contains("Distance"))
                {
                    string strValue = Regex.Match(configArray[i], @"\d+").Value;
                    Distance = int.Parse(strValue);
                }
                if (configArray[i].Contains("Percent"))
                {
                    string strValue = Regex.Match(configArray[i], @"\d+").Value;
                    Percent = int.Parse(strValue);
                }
                if (configArray[i].Contains("Thickness"))
                {
                    string strValue = Regex.Match(configArray[i], @"\d+").Value;
                    Thickness = int.Parse(strValue);
                }
                if (configArray[i].Contains("Length"))
                {
                    string strValue = Regex.Match(configArray[i], @"\d+").Value;
                    Length = int.Parse(strValue);
                }
                if (configArray[i].Contains("R:"))
                {
                    string r = Regex.Match(configArray[i], @"\d+").Value;
                    int red = int.Parse(r);
                    Up.BackColor = Color.FromArgb(red, Up.BackColor.G, Up.BackColor.B);
                    Down.BackColor = Color.FromArgb(red, Down.BackColor.G, Down.BackColor.B);
                    Left.BackColor = Color.FromArgb(red, Left.BackColor.G, Left.BackColor.B);
                    Right.BackColor = Color.FromArgb(red, Right.BackColor.G, Right.BackColor.B);
                }
                if (configArray[i].Contains("G:"))
                {
                    string g = Regex.Match(configArray[i], @"\d+").Value;
                    int green = int.Parse(g);
                    Up.BackColor = Color.FromArgb(Up.BackColor.R, green, Up.BackColor.R);
                    Down.BackColor = Color.FromArgb(Down.BackColor.R, green, Down.BackColor.R);
                    Left.BackColor = Color.FromArgb(Left.BackColor.R, green, Left.BackColor.R);
                    Right.BackColor = Color.FromArgb(Right.BackColor.R, green, Right.BackColor.R);
                }
                if (configArray[i].Contains("B:"))
                {
                    string b = Regex.Match(configArray[i], @"\d+").Value;
                    int blue = int.Parse(b);
                    Up.BackColor = Color.FromArgb(Up.BackColor.R, Up.BackColor.G, blue);
                    Down.BackColor = Color.FromArgb(Down.BackColor.R, Down.BackColor.G, blue);
                    Left.BackColor = Color.FromArgb(Left.BackColor.R, Left.BackColor.G, blue);
                    Right.BackColor = Color.FromArgb(Right.BackColor.R, Right.BackColor.G, blue);
                }
                if (configArray[i].Contains("Dot:"))
                {
                    string strValue = Regex.Match(configArray[i], @"\d+").Value;
                    int value = int.Parse(strValue);

                    switch(value)
                    {
                        case 1:
                            Dot.Show();
                            break;
                        case 0:
                            Dot.Hide();
                            break;
                        default:
                            Dot.Hide();
                            break;
                    }
                }
                if (configArray[i].Contains("Cross:"))
                {
                    string strValue = Regex.Match(configArray[i], @"\d+").Value;
                    int value = int.Parse(strValue);

                    switch (value)
                    {
                        case 1:
                            Up.Show();
                            Down.Show();
                            Left.Show();
                            Right.Show();
                            break;
                        case 0:
                            Up.Hide();
                            Down.Hide();
                            Left.Hide();
                            Right.Hide();
                            break;
                        default:
                            Up.Hide();
                            Down.Hide();
                            Left.Hide();
                            Right.Hide();
                            break;
                    }
                }
                if (configArray[i].Contains("DotSize:"))
                {
                    string strValue = Regex.Match(configArray[i], @"\d+").Value;
                    int value = int.Parse(strValue);

                    DotPercent = value;
                }
                if (configArray[i].Contains("RD:"))
                {
                    string r = Regex.Match(configArray[i], @"\d+").Value;
                    int red = int.Parse(r);
                    Dot.BackColor = Color.FromArgb(red, Dot.BackColor.G, Dot.BackColor.B);
                }
                if (configArray[i].Contains("GD:"))
                {
                    string g = Regex.Match(configArray[i], @"\d+").Value;
                    int green = int.Parse(g);
                    Dot.BackColor = Color.FromArgb(Dot.BackColor.R, green, Dot.BackColor.R);
                }
                if (configArray[i].Contains("BD:"))
                {
                    string b = Regex.Match(configArray[i], @"\d+").Value;
                    int blue = int.Parse(b);
                    Dot.BackColor = Color.FromArgb(Dot.BackColor.R, Dot.BackColor.G, blue);
                }
                if (configArray[i].Contains("X:"))
                {
                    string b = Regex.Match(configArray[i], @"-?\d+").Value;
                    X = int.Parse(b);
                }
                if (configArray[i].Contains("Y:"))
                {
                    string b = Regex.Match(configArray[i], @"-?\d+").Value;
                    Y = int.Parse(b);
                }
            }
        }
    }
}
