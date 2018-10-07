using System;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;

namespace WallpaperEngine
{
    class Program
    {
        static string path = Environment.CurrentDirectory;
        static string wallpaperName = "/WallPaper.png";

        static WebClient client = new WebClient();

        static string URL;
        static int delay;

        static float lastUpdatedTime = 0;

        //Used for setting the background
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, String pvParam, UInt32 fWinIni);
        static UInt32 SPI_SETDESKWALLPAPER = 20;
        static UInt32 SPIF_UPDATEINFILE = 0x1;

        //Used for hiding the window
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        static void Main(string[] args)
        {
            ShowWindow(GetConsoleWindow(), SW_HIDE);
            try
            {
                while (true)
                {
                    StreamReader sr = new StreamReader(path + "/URL.txt");
                    URL = sr.ReadLine();
                    delay = int.Parse(sr.ReadLine());
                    sr.Close();

                    SetNewBackground();
                    Thread.Sleep(delay);
                }
            }
            catch (Exception ex){Console.WriteLine(ex + "\n");}
        }

        static void SetNewBackground()
        {
            try
            {
                string downloadedString = client.DownloadString(URL);
                MatchCollection ImageList = Regex.Matches(downloadedString, "{\"source\": {\"url\": \\s*(.+?)\\s*}", RegexOptions.Singleline); //Gets the temperature
                string pictureURL = ImageList[0].ToString().Split('\"')[5];

                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        client.DownloadFile(pictureURL, path + wallpaperName);

                        //Image img = Image.FromFile(path + wallpaperName);
                        //if(img.Width < 1920 || img.Height < 1080) continue;
                        //img.Dispose();

                        Console.WriteLine("Set the new background to " + pictureURL);
                        SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path + wallpaperName, SPIF_UPDATEINFILE);

                        break;
                    }
                    catch (Exception ex){ Console.WriteLine(ex + "\n");}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "\n");
            }
        }
    }
} 