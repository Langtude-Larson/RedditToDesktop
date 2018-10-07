using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Net;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Drawing;

namespace RedditDesktop
{
    public partial class Form1 : Form
    {
        static string path = Environment.CurrentDirectory;
        static string wallpaperName = "WallPaper.png";

        static WebClient client = new WebClient();
        static string URL = "https://www.reddit.com/r/";

        static float lastUpdatedTime = 0;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, String pvParam, UInt32 fWinIni);
        static UInt32 SPI_SETDESKWALLPAPER = 20;
        static UInt32 SPIF_UPDATEINFILE = 0x1;

        string[] redditModes = {"hot" , "new" , "controversial" , "top" , "rising" };

        Process[] appsOpen;

        static bool isRunning = false;

        public Form1()
        {
            InitializeComponent();

            StreamReader sr = new StreamReader(path + "/URL.txt");
            string loadedURL = sr.ReadLine();
            string loadedDelay = sr.ReadLine();

            richTextBox1.Text = loadedURL.Split('/')[4];
            comboBox1.Text = loadedURL.Split('/')[5];
            numericUpDown1.Value = int.Parse(loadedDelay);

            comboBox1.Items.AddRange(redditModes);
            sr.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter(path + "/URL.txt");
            sw.WriteLine(URL + richTextBox1.Text + "/" + comboBox1.Text + "/.json");
            sw.WriteLine(numericUpDown1.Value);
            sw.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            appsOpen = Process.GetProcessesByName("RTDProcess");
            if (appsOpen.Length > 0)
            {
                label5.Text = "App is currently running";
                button2.Text = "Close";
            }
            else
            {
                label5.Text = "App is closed";
                button2.Text = "Start";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(appsOpen.Length > 0)
            {
                foreach (Process process in appsOpen)
                {
                    process.Kill();
                }
            }
            else
            {
                Process.Start(path + "/RTDProcess.exe");
            }
        }
    }
}