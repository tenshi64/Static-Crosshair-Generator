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
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Threading;

namespace Crosshair_Control_Panel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        ColorDialog colorDlg = new ColorDialog();

        public string Localization = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Static Crosshair Generator\Configs";
        public string configLocation = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Static Crosshair Generator\Configs\config.ini";
        public string AppPath = @".\Generator\Static Crosshair Generator.exe";

        private void Form1_Load(object sender, EventArgs e)
        {
            lblFocus.Focus();
            btnOnOff.Text = CheckProcess();
            if (!Directory.Exists(Localization))
            {
                Directory.CreateDirectory(Localization);
            }

            if (!File.Exists(configLocation))
            {
                File.WriteAllText(configLocation, "Distance: 10\nPercent: 30\nThickness: 10\nLength: 41\nR: 127\nG: 255\nB: 0\nDot: 0\nDotSize: 12\nRD: 127\nGD: 255\nBD: 0\nCross: 1\nX: 0\nY: 0");
            }
            ReadConfig();
            if(numX.Value == 0 && numY.Value == 0)
            {
                lblCenter.Text = "Crosshair is centered.";
            }
            else
            {
                lblCenter.Text = "Crosshair isn't centered.";
            }
        }

        private void btnOnOff_Click(object sender, EventArgs e)
        {
            lblFocus.Focus();

            switch(btnOnOff.Text)
            {
                case "Turn on":
                    Process.Start(AppPath);
                    btnOnOff.Text = "Turn off";
                    break;

                case "Turn off":
                    KillProcess kill = new KillProcess();
                    btnOnOff.Text = "Turn on";
                    break;
            }
        }

        public string CheckProcess()
        {
            if (Process.GetProcessesByName("Static Crosshair Generator").Length > 0)
            {
                return "Turn off";
            }
            return "Turn on";
        }

        public void RestartProcess()
        {
            if(Process.GetProcessesByName("Static Crosshair Generator").Length > 0)
            {
                Process process = Process.GetProcessesByName("Static Crosshair Generator")[0];
                process.Kill();
                Process.Start(AppPath);
            }
        }

        public void ChangeConfig(string option, float value)
        {
            string readConfig = File.ReadAllText(configLocation);
            int numberOfLines = File.ReadAllLines(configLocation).Count();
            string[] configArray = File.ReadAllLines(configLocation);

            for(int i = 0; i < numberOfLines; i++)
            {
                if(configArray[i].Contains(option))
                {
                    if(option.Contains(':'))
                    {
                        readConfig = readConfig.Replace(configArray[i], option + " " + value);
                        File.WriteAllText(configLocation, readConfig);
                    }
                    else
                    {
                        readConfig = readConfig.Replace(configArray[i], option + ": " + value);
                        File.WriteAllText(configLocation, readConfig);
                    }
                }
            }
            RestartProcess();
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
                    tbDistance.Value = int.Parse(strValue);
                    lblDistance.Text = strValue + "%";
                }
                if (configArray[i].Contains("Percent"))
                {
                    string strValue = Regex.Match(configArray[i], @"\d+").Value;
                    tbSize.Value = int.Parse(strValue);
                    lblSize.Text = strValue + "%";
                }
                if (configArray[i].Contains("Thickness"))
                {
                    string strValue = Regex.Match(configArray[i], @"\d+").Value;
                    tbThickness.Value = int.Parse(strValue);
                    lblThickness.Text = strValue + "%";
                }
                if (configArray[i].Contains("Length"))
                {
                    string strValue = Regex.Match(configArray[i], @"\d+").Value;
                    tbLength.Value = int.Parse(strValue);
                    lblLength.Text = strValue + "%";
                }
                if (configArray[i].Contains("R:"))
                {
                    string r = Regex.Match(configArray[i], @"\d+").Value;
                    int red = int.Parse(r);
                    panelColor.BackColor = Color.FromArgb(red, panelColor.BackColor.G, panelColor.BackColor.B);
                }
                if (configArray[i].Contains("G:"))
                {
                    string g = Regex.Match(configArray[i], @"\d+").Value;
                    int green = int.Parse(g);
                    panelColor.BackColor = Color.FromArgb(panelColor.BackColor.R, green, panelColor.BackColor.R);
                }
                if (configArray[i].Contains("B:"))
                {
                    string b = Regex.Match(configArray[i], @"\d+").Value;
                    int blue = int.Parse(b);
                    panelColor.BackColor = Color.FromArgb(panelColor.BackColor.R, panelColor.BackColor.G, blue);
                }
                if (configArray[i].Contains("DotSize"))
                {
                    string strValue = Regex.Match(configArray[i], @"\d+").Value;
                    tbDotSize.Value = int.Parse(strValue);
                    lblDotSizePercent.Text = strValue + "%";
                }
                if (configArray[i].Contains("Dot:"))
                {
                    string strValue = Regex.Match(configArray[i], @"\d+").Value;
                    int value = int.Parse(strValue);

                    switch (value)
                    {
                        case 1:
                            cbDot.Checked = true;
                            break;
                        case 0:
                            cbDot.Checked = false;
                            break;
                        default:
                            cbDot.Checked = false;
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
                            cbCross.Checked = true;
                            break;
                        case 0:
                            cbCross.Checked = false;
                            break;
                        default:
                            cbCross.Checked = false;
                            break;
                    }
                }
                if (configArray[i].Contains("RD:"))
                {
                    string r = Regex.Match(configArray[i], @"\d+").Value;
                    int red = int.Parse(r);
                    panelDotColor.BackColor = Color.FromArgb(red, panelColor.BackColor.G, panelColor.BackColor.B);
                }
                if (configArray[i].Contains("GD:"))
                {
                    string g = Regex.Match(configArray[i], @"\d+").Value;
                    int green = int.Parse(g);
                    panelDotColor.BackColor = Color.FromArgb(panelColor.BackColor.R, green, panelColor.BackColor.R);
                }
                if (configArray[i].Contains("BD:"))
                {
                    string b = Regex.Match(configArray[i], @"\d+").Value;
                    int blue = int.Parse(b);
                    panelDotColor.BackColor = Color.FromArgb(panelColor.BackColor.R, panelColor.BackColor.G, blue);
                }
                if (configArray[i].Contains("X:"))
                {
                    string b = Regex.Match(configArray[i], @"\d+").Value;
                    numX.Value = int.Parse(b);
                }
                if (configArray[i].Contains("Y:"))
                {
                    string b = Regex.Match(configArray[i], @"\d+").Value;
                    numY.Value = int.Parse(b);
                }
            }
        }

        private void tbDistance_Scroll(object sender, EventArgs e)
        {
            lblDistance.Text = tbDistance.Value + "%";
            ChangeConfig("Distance", tbDistance.Value);
        }

        private void tbLength_Scroll(object sender, EventArgs e)
        {
            lblLength.Text = tbLength.Value + "%";
            ChangeConfig("Percent", tbLength.Value);
        }

        private void tbThickness_Scroll(object sender, EventArgs e)
        {
            lblThickness.Text = tbThickness.Value + "%";
            ChangeConfig("Thickness", tbThickness.Value);
        }

        private void tbSize_Scroll(object sender, EventArgs e)
        {
            lblSize.Text = tbSize.Value + "%";
            ChangeConfig("Length", tbSize.Value);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            lblFocus.Focus();
            RestartProcess();
            File.Delete(configLocation);
            Application.Restart();
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            lblFocus.Focus();
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                Color pickedColor = colorDlg.Color;
                colorDlg.Color = pickedColor;
                panelColor.BackColor = pickedColor;
                ChangeConfig("R", pickedColor.R);
                ChangeConfig("G", pickedColor.G);
                ChangeConfig("B", pickedColor.B);
            }
        }

        private void cbDot_CheckedChanged(object sender, EventArgs e)
        {
            if(cbDot.Checked)
            {
                ChangeConfig("Dot:", 1);
            }
            else
            {
                ChangeConfig("Dot:", 0);
            }
        }

        private void btnDotColor_Click(object sender, EventArgs e)
        {
            lblFocus.Focus();
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                Color pickedColor = colorDlg.Color;
                colorDlg.Color = pickedColor;
                panelDotColor.BackColor = pickedColor;
                ChangeConfig("RD", pickedColor.R);
                ChangeConfig("GD", pickedColor.G);
                ChangeConfig("BD", pickedColor.B);
            }
        }

        private void tbDotSize_Scroll(object sender, EventArgs e)
        {
            lblDotSizePercent.Text = tbDotSize.Value + "%";
            ChangeConfig("DotSize", tbDotSize.Value);
        }

        private void cbCross_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCross.Checked)
            {
                ChangeConfig("Cross:", 1);
            }
            else
            {
                ChangeConfig("Cross:", 0);
            }
        }

        private void numX_ValueChanged(object sender, EventArgs e)
        {
            if (numX.Value == 0 && numY.Value == 0)
            {
                lblCenter.Text = "Crosshair is centered.";
            }
            else
            {
                lblCenter.Text = "Crosshair isn't centered.";
            }
        }

        private void numY_ValueChanged(object sender, EventArgs e)
        {
            if (numX.Value == 0 && numY.Value == 0)
            {
                lblCenter.Text = "Crosshair is centered.";
            }
            else
            {
                lblCenter.Text = "Crosshair isn't centered.";
            }
        }

        private void btnSetLocation_Click(object sender, EventArgs e)
        {
            lblFocus.Focus();
            ChangeConfig("Y:", (float)-numY.Value);
            ChangeConfig("X:", (float)numX.Value);
        }
    }

    class KillProcess
    {
        public KillProcess()
        {
            Process[] processes = Process.GetProcessesByName("Static Crosshair Generator");
            foreach (Process process in processes)
            {
                process.Kill();
            }
        }
    }
}
