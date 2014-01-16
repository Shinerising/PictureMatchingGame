using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace lianliankan
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
    }

    //Public Class
    public class mouse
    {
        public Boolean down = false;
        public System.Drawing.Point point = new System.Drawing.Point();
        public int X = new int();
        public int Y = new int();
        public void set(System.Drawing.Point Npoint)
        {
            point = Npoint;
            X = (int)Npoint.X;
            Y = (int)Npoint.Y;
        }
        public Boolean onbox = false;
        public Boolean once = false;
        public int onx = new int();
        public int ony = new int();
        public int ox = new int();
        public int oy = new int();
    }
    public class pictureitems
    {
        private BitmapImage bi = new BitmapImage();
        private string rootpath = "";
        public void sets()
        {
            rootpath= Environment.CurrentDirectory;
        }
        public BitmapImage button(int id, int sit, path path)
        {
            string picpath = "";
            picpath = path.pic + @"button" + Convert.ToString(id) + Convert.ToString(sit) + ".png";
            bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(picpath);
            bi.EndInit();
            return bi;
        }
        public BitmapImage Bi(int bx, path path)
        {
            String picpath = "";
            picpath = path.pic + @"box\" + Convert.ToString(bx) + ".png";
            bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(picpath);
            bi.EndInit();
            return bi;
        }
        public BitmapImage check(path path)
        {

            bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(path.pic + @"checkmark.png");
            bi.EndInit();
            return bi;
        }
        public BitmapImage main(int bx,path path)
        {
            string picpath = "";
            if (bx == 0)
            {
                picpath = path.pic + @"back.png";
            }
            else if (bx == 1)
            {
                picpath = path.pic + @"cover.png";
            }
            else if (bx == 2)
            {
                picpath = path.pic + @"didadida.png";
            }
            else if (bx == 3)
            {
                picpath = path.pic + @"exit.png";
            }
            else if (bx == 4)
            {
                picpath = path.pic + @"setting.png";
            }
            bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(picpath);
            bi.EndInit();
            return bi;
        }
        public BitmapImage number(int bx, path path)
        {
            string picpath = path.pic + @"number\" + Convert.ToString(bx) + ".png";
            bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(picpath);
            bi.EndInit();
            return bi;
        }
    }
    public class sounds
    {
        public MediaPlayer bgm = new MediaPlayer();
        public MediaPlayer click = new MediaPlayer();
        public MediaPlayer erase = new MediaPlayer();
        public MediaPlayer win = new MediaPlayer();
        private string rootpath = "";
        public void sets(path path)
        {
            rootpath = Environment.CurrentDirectory;
            bgm.Open(new Uri(path.sound + @"bgm.ogg"));
            click.Open(new Uri(path.sound + @"click.ogg"));
            erase.Open(new Uri(path.sound + @"erase.ogg"));
            win.Open(new Uri(path.sound + @"win.ogg"));
        }
    }
    public class path
    {
        public string root;
        public string pic;
        public string sound;
        public string theme;
        public string level;
        public string otheme;
        public string olevel;
        public void set()
        {
            theme = ConfigurationManager.AppSettings["theme"];
            level = ConfigurationManager.AppSettings["level"];
            otheme = theme;
            olevel = level;
            root = Environment.CurrentDirectory;
            pic = root + @"\resource\" + theme + @"\images\";
            sound = root + @"\resource\" + theme + @"\Sounds\";
        }
        public void change()
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings["theme"].Value = theme;
            cfa.AppSettings.Settings["level"].Value = level;
            cfa.Save();
            root = Environment.CurrentDirectory;
            pic = root + @"\resource\" + theme + @"\images\";
            sound = root + @"\resource\" + theme + @"\Sounds\";
        }
    }
    public class boxes
    {
        public Boolean[,] bshow = new Boolean[14, 9];
        public int[,] bsquare = new int[14, 9];
        public Boolean show(int sx, int sy)
        {
            if (sx == -1 || sx == 14 || sy == -1 || sy == 9) return false;
            return bshow[sx, sy];
        }
        public int square(int sx, int sy)
        {
            if (sx == -1 || sx == 14 || sy == -1 || sy == 9) return 0;
            return bsquare[sx, sy];
        }
        public void Show(int sx, int sy, Boolean set)
        {
            bshow[sx, sy] = set;
        }
        public void Square(int sx, int sy, int set)
        {
            bsquare[sx, sy] = set;
        }
    }
    public class line
    {
        public int me = 0;
        public int x = 0, y = 0;
        public int[] sx = { 0, 0, 0 }, sy = { 0, 0, 0 }, l = { 0, 0, 0 };
    }
    public class game
    {
        public Boolean run = false;
        public Boolean pause = false;
        public int time = 0;
        public int tim0 = 0;
        public int tim1 = 0;
        public int min = 0;
        public int sec = 0;
        public int score = 0;
        public int combo = 0;
        public int leftbox = 126;
        public int boxnum = 1;
        public void timeadd()
        {
            time++;
            sec = time % 60;
            min = (time - sec) / 60;
        }
        public void scoreadd()
        {
            score += 100 + combo * 50;
        }
    }
}
