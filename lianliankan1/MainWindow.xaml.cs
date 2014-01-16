using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Threading;
using System.Configuration;

namespace lianliankan
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        mouse mouse = new mouse();
        boxes boxes = new boxes();
        pictureitems pics = new pictureitems();
        game game = new game();
        sounds sound = new sounds();
        line line = new line();
        path path = new path();
        const int maxnum = 16;
        private DispatcherTimer timer = new DispatcherTimer();
        int errortick = 0;
        InfoWindow infowindow = new InfoWindow();
        System.Windows.Input.Cursor curnormal;
        System.Windows.Input.Cursor curselect;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mouse = new mouse();
            game = new game();
            path.set();
            start(0);
            timer.Tick += new EventHandler(timer_Tick);
            App.Current.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

        }

        public void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            error();
        }

        public void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            error();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (game.run == true && game.pause == false)
            {
                game.timeadd();
            }
            infodraw(0);
            game.tim1++;
        }

        private void bgm_End(object sender, EventArgs e)
        {
            sound.bgm.Position = TimeSpan.FromSeconds(0);
            sound.bgm.Play();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int x = System.Windows.Forms.Control.MousePosition.X - (int)Left - 45;
            int y = System.Windows.Forms.Control.MousePosition.Y - (int)Top - 45;
            if ((x > 40 && x < 660) && (y > 120 && y < 540))
            {
                mouse.down = false;
            }
            else
            {
                mouse.down = true;
                mouse.set(System.Windows.Forms.Control.MousePosition);
            }
        }

        private void window_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            mouse.down = false;
        }


        private void window_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = curnormal;
        }

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if ((mouse.down == true) && (System.Windows.Forms.Control.MousePosition.X > Left && System.Windows.Forms.Control.MousePosition.X < (Left + 900) && System.Windows.Forms.Control.MousePosition.Y > Top && System.Windows.Forms.Control.MousePosition.Y < (Top + 600)))
            {
                Left = Left + System.Windows.Forms.Control.MousePosition.X - mouse.X;
                Top = Top + System.Windows.Forms.Control.MousePosition.Y - mouse.Y;
                mouse.set(System.Windows.Forms.Control.MousePosition);
            }
            else
            {
                mouse.down = false;
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouse.down = false;
        }

        private void canvas1_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            mousecheck();
            pointerdraw();
        }

        private void canvas1_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            mouse.onbox = false;
            pointerdraw();
        }


        private void canvas1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Boolean j = false;
            if (mouse.onbox == true && boxes.show(mouse.onx, mouse.ony) == true)
            {
                if (mouse.once == false)
                {
                    mouse.ox = mouse.onx;
                    mouse.oy = mouse.ony;
                    mouse.once = true;
                    pointerdraw();
                    sound.click.Stop();
                    sound.click.Play();
                }
                else
                {
                    if (mouse.ox == mouse.onx && mouse.oy == mouse.ony)
                    {
                        mouse.once = false;
                        sound.click.Stop();
                        sound.click.Play();
                    }
                    else if (boxjudge() == true)
                    {
                        j = true;
                        game.leftbox -= 2;
                        if (game.leftbox == 0) win();
                        squaredraw(1);
                        linedraw();
                        sound.erase.Stop();
                        sound.erase.Play();
                    }
                    else
                    {
                        sound.click.Stop();
                        sound.click.Play();
                    }
                    mouse.once = false;
                }
            }
            else if (mouse.onbox == false || boxes.show(mouse.onx, mouse.ony) == false)
            {
                game.tim0 = 0;
                game.combo = 0;
            }
            if (j == true && ((game.tim1 - game.tim0) < 3))
            {
                game.combo++;
                game.tim0 = game.tim1;
                game.scoreadd();
                infodraw(1);
            }
            else if (j == true)
            {
                game.tim0 = game.tim1;
                game.scoreadd();
                infodraw(1);
            }
            else if (mouse.once == false)
            {
                game.tim0 = 0;
                game.combo = 0;
            }
        }

        private void button1_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonchange(1, 1);
            Cursor = curselect;
        }

        private void button1_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonchange(1, 0);
            Cursor = curnormal;
        }

        private void button1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            buttonchange(1, 2);
        }

        private void button1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            buttonchange(1, 0);
            canvas1.Visibility = Visibility.Visible;
            image1.Visibility = Visibility.Visible;
            button1.Visibility = Visibility.Hidden;
            button2.Visibility = Visibility.Visible;
            game.pause = false;
            sound.bgm.Play();
        }

        private void button2_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonchange(2, 1);
            Cursor = curselect;
        }

        private void button2_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonchange(2, 0);
            Cursor = curnormal;
        }

        private void button2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            buttonchange(2, 2);
        }

        private void button2_MouseUp(object sender, MouseButtonEventArgs e)
        {
            buttonchange(2, 0);
            canvas1.Visibility = Visibility.Hidden;
            image1.Visibility = Visibility.Hidden;
            button1.Visibility = Visibility.Visible;
            button2.Visibility = Visibility.Hidden;
            game.pause = true;
            sound.bgm.Pause();
        }

        private void button3_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonchange(3, 1);
            Cursor = curselect;
        }

        private void button3_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonchange(3, 0);
            Cursor = curnormal;
        }

        private void button3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            buttonchange(3, 2);
        }

        private void button3_MouseUp(object sender, MouseButtonEventArgs e)
        {
            buttonchange(3, 0);
            if (game.leftbox == 0)
            {
                image4.Visibility = Visibility.Hidden;
                image5.Visibility = Visibility.Hidden;
                canvas1.Visibility = Visibility.Hidden;
                image1.Visibility = Visibility.Hidden;
                image3.Visibility = Visibility.Hidden;
                button1.Visibility = Visibility.Hidden;
                button2.Visibility = Visibility.Hidden;
                button3.Visibility = Visibility.Hidden;
                button7.Visibility = Visibility.Visible;
                button8.Visibility = Visibility.Visible;
                button9.Visibility = Visibility.Hidden;
                button10.Visibility = Visibility.Hidden;
                canvas01.Visibility = Visibility.Hidden;
                canvas02.Visibility = Visibility.Hidden;
                canvas03.Visibility = Visibility.Hidden;
                game.run = false;
            }
            else
            {
                game.pause = true;
                button1.Visibility = Visibility.Hidden;
                button2.Visibility = Visibility.Hidden;
                button3.Visibility = Visibility.Hidden;
                image1.Source = pics.main(3, path);
                canvas1.Visibility = Visibility.Hidden;
                button9.Visibility = Visibility.Visible;
                button10.Visibility = Visibility.Visible;
            }
        }

        private void button4_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonchange(4, 1);
            Cursor = curselect;
        }

        private void button4_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonchange(4, 0);
            Cursor = curnormal;
        }

        private void button4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            buttonchange(4, 2);
        }

        private void button4_MouseUp(object sender, MouseButtonEventArgs e)
        {
            buttonchange(4, 0);
            System.Windows.Application.Current.Shutdown();
        }

        private void button5_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonchange(5, 1);
            Cursor = curselect;
        }

        private void button5_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonchange(5, 0);
            Cursor = curnormal;
        }

        private void button5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            buttonchange(5, 2);
        }

        private void button5_MouseUp(object sender, MouseButtonEventArgs e)
        {
            buttonchange(5, 0);
            WindowState= WindowState.Minimized;
        }

        private void button6_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonchange(6, 1);
            Cursor = curselect;
        }

        private void button6_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonchange(6, 0);
            Cursor = curnormal;
        }

        private void button6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            buttonchange(6, 2);
        }

        private void button6_MouseUp(object sender, MouseButtonEventArgs e)
        {
            buttonchange(6, 0);
            try
            {
                infowindow.Close();
            }
            catch { };
            infowindow = new InfoWindow();
            infowindow.Show();
        }

        private void button7_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonchange(7, 1);
            Cursor = curselect;
        }

        private void button7_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonchange(7, 0);
            Cursor = curnormal;
        }

        private void button7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            buttonchange(7, 2);
        }

        private void button7_MouseUp(object sender, MouseButtonEventArgs e)
        {
            buttonchange(7, 0);
            try
            {
                image1.Source = pics.main(4, path);
                button7.Visibility = Visibility.Hidden;
                button8.Visibility = Visibility.Hidden;
                button9.Visibility = Visibility.Visible;
                button10.Visibility = Visibility.Visible;
                image1.Visibility = Visibility.Visible;
                canvas1.Visibility = Visibility.Visible;
                canvas2.Visibility = Visibility.Hidden;
                canvas3.Visibility = Visibility.Hidden;
                setting();
            }
            catch { error(); }
        }

        private void button8_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonchange(8, 1);
            Cursor = curselect;
        }

        private void button8_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonchange(8, 0);
            Cursor = curnormal;
        }

        private void button8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            buttonchange(8, 2);
        }

        private void button8_MouseUp(object sender, MouseButtonEventArgs e)
        {
            buttonchange(8, 0);
            gamestart();
            game.run = true;
            button2.Visibility = Visibility.Visible;
        }

        private void button9_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonchange(9, 1);
            Cursor = curselect;
        }

        private void button9_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonchange(9, 0);
            Cursor = curnormal;
        }

        private void button9_MouseDown(object sender, MouseButtonEventArgs e)
        {
            buttonchange(9, 2);
        }

        private void button9_MouseUp(object sender, MouseButtonEventArgs e)
        {
            buttonchange(9, 0);
            if (game.run == true)
            {
                sound.bgm.Stop();
                canvas1.Visibility = Visibility.Hidden;
                image1.Visibility = Visibility.Hidden;
                image3.Visibility = Visibility.Hidden;
                button1.Visibility = Visibility.Hidden;
                button2.Visibility = Visibility.Hidden;
                button3.Visibility = Visibility.Hidden;
                button7.Visibility = Visibility.Visible;
                button8.Visibility = Visibility.Visible;
                button9.Visibility = Visibility.Hidden;
                button10.Visibility = Visibility.Hidden;
                canvas01.Visibility = Visibility.Hidden;
                canvas02.Visibility = Visibility.Hidden;
                canvas03.Visibility = Visibility.Hidden;
                game.run = false;
            }
            else
            {

                button7.Visibility = Visibility.Visible;
                button8.Visibility = Visibility.Visible;
                button9.Visibility = Visibility.Hidden;
                button10.Visibility = Visibility.Hidden;
                image1.Visibility = Visibility.Hidden;
                canvas1.Visibility = Visibility.Hidden;
                canvas2.Visibility = Visibility.Hidden;
                canvas3.Visibility = Visibility.Hidden;
                path.olevel = path.level;
                if (path.otheme != path.theme)
                {
                     path.otheme = path.theme;
                     path.change();
                     start(1);
                }
                else
                    path.change();
            }
        }

        private void button10_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonchange(10, 1);
            Cursor = curselect;
        }

        private void button10_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonchange(10, 0);
            Cursor = curnormal;
        }

        private void button10_MouseDown(object sender, MouseButtonEventArgs e)
        {
            buttonchange(10, 2);
        }

        private void button10_MouseUp(object sender, MouseButtonEventArgs e)
        {
            buttonchange(10, 0);
            if (game.run == true)
            {
                game.pause = false;
                button1.Visibility = Visibility.Visible;
                button2.Visibility = Visibility.Visible;
                button3.Visibility = Visibility.Visible;
                image1.Source = pics.main(1, path);
                canvas1.Visibility = Visibility.Visible;
                button9.Visibility = Visibility.Hidden;
                button10.Visibility = Visibility.Hidden;
            }
            else
            {

                button7.Visibility = Visibility.Visible;
                button8.Visibility = Visibility.Visible;
                button9.Visibility = Visibility.Hidden;
                button10.Visibility = Visibility.Hidden;
                image1.Visibility = Visibility.Hidden;
                canvas1.Visibility = Visibility.Hidden;
                canvas2.Visibility = Visibility.Hidden;
                canvas3.Visibility = Visibility.Hidden;
                path.theme = path.otheme;
                path.level = path.olevel;
            }
        }

        private void image1_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (game.run == false)
            {
                Cursor = curselect;
                int x = System.Windows.Forms.Control.MousePosition.X - (int)Left - 65;
                int y = System.Windows.Forms.Control.MousePosition.Y - (int)Top - 145;
                if (x > 54 && x < 95 && y > 151 && y < 192) Cursor = curselect;
                else if (x > 240 && x < 283 && y > 151 && y < 192) Cursor = curselect;
                else if (x > 427 && x < 474 && y > 151 && y < 192) Cursor = curselect;
                else if (x > 105 && x < 145 && y > 267 && y < 307) Cursor = curselect;
                else if (x > 260 && x < 300 && y > 267 && y < 307) Cursor = curselect;
                else if (x > 412 && x < 452 && y > 267 && y < 307) Cursor = curselect;
                else Cursor = curnormal;
            }
        }

        private void image1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (game.run == false)
            {
                int x = System.Windows.Forms.Control.MousePosition.X - (int)Left - 65;
                int y = System.Windows.Forms.Control.MousePosition.Y - (int)Top - 145;
                if (x > 54 && x < 95 && y > 151 && y < 192) path.theme="theme01";
                if (x > 240 && x < 283 && y > 151 && y < 192) path.theme = "theme02";
                if (x > 427 && x < 474 && y > 151 && y < 192) path.theme = "theme03";
                if (x > 105 && x < 145 && y > 267 && y < 307) path.level = "easy";
                if (x > 260 && x < 300 && y > 267 && y < 307) path.level = "normal";
                if (x > 412 && x < 452 && y > 267 && y < 307) path.level = "hard";
                setting();
            }
        }

        //Subs

        //Draw Squares

        public void squaredraw(int s)
        {
            DoubleAnimation animation = new DoubleAnimation(0, 36, TimeSpan.FromMilliseconds(50));
            int i, j;
            System.Windows.Controls.Image box0 = new System.Windows.Controls.Image();
            canvas1.Children.Clear();
            for (i = 0; i < 14; i++)
            {
                for (j = 0; j < 9; j++)
                {
                    if (boxes.show(i, j) == true)
                    {
                        box0.Source = pics.Bi(boxes.square(i, j), path);
                        box0.Margin = new Thickness(42 + 40 * i, 42 + 40 * j, 0, 0);
                        box0.Width = 36;
                        box0.Height = 36;
                        if (s == 0)
                        {
                            box0.Width = 0;
                            box0.Height = 0;
                            animation.BeginTime = animation.BeginTime + TimeSpan.FromMilliseconds(10);
                            box0.BeginAnimation(System.Windows.Controls.Image.WidthProperty, animation);
                            box0.BeginAnimation(System.Windows.Controls.Image.HeightProperty, animation);
                        }
                        canvas1.Children.Add(box0);
                        box0 = new System.Windows.Controls.Image();
                    }
                }
            }
        }

        public void pointerdraw()
        {
            PathGeometry myOathGeometry = new PathGeometry();
            Path myPath = new Path();
            canvas2.Children.Clear();
            if (mouse.onbox == true)
            {
                myOathGeometry.AddGeometry(new RectangleGeometry(new Rect(40 + 40 * mouse.onx, 40 + 40 * mouse.ony, 40, 40)));
                myPath.Stroke = System.Windows.Media.Brushes.Black;
                myPath.StrokeThickness = 1;
                myPath.Data = myOathGeometry;
            }
            if (mouse.once == true)
            {
                myOathGeometry.AddGeometry(new RectangleGeometry(new Rect(40 + 40 * mouse.ox, 40 + 40 * mouse.oy, 40, 40)));
                myPath = new Path();
                myPath.Stroke = System.Windows.Media.Brushes.Red;
                if (mouse.ox == mouse.onx && mouse.oy == mouse.ony)
                {
                    myPath.Stroke = System.Windows.Media.Brushes.Pink;
                }
                myPath.StrokeThickness = 1;
                myPath.Data = myOathGeometry;
            }
            canvas2.Children.Add(myPath);
        }

        public void linedraw()
        {
            int i = 0, m = 0;
            int ix = 0, iy = 0;
            PathGeometry myOathGeometry = new PathGeometry();
            Path myPath = new Path();
            DoubleAnimation myanimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));
            myPath.Stroke = System.Windows.Media.Brushes.Black;
            myPath.StrokeThickness = 1;
            ix = 52 + 40 * line.x;
            iy = 52 + 40 * line.y;
            for (m = 0; m < line.me; m++)
            {
                for (i = 0; i < line.l[m] - 1; i++)
                {
                    myOathGeometry.AddGeometry(new RectangleGeometry(new Rect(ix, iy, 16, 16)));
                    ix += line.sx[m] * 20;
                    iy += line.sy[m] * 20;
                }
                myOathGeometry.AddGeometry(new RectangleGeometry(new Rect(ix, iy, 16, 16)));
            }
            myPath.Data = myOathGeometry;
            myPath.Stroke = System.Windows.Media.Brushes.Black;
            myPath.StrokeThickness = 1;
            myanimation = new DoubleAnimation(1,0,TimeSpan.FromSeconds(1));
            myPath.BeginAnimation(Path.OpacityProperty, myanimation);
            canvas3.Children.Add(myPath);
        }

        public void infodraw(int inp)
        {
            try{
            int minute = 0, second = 0;
            int n = 0, m = 0;
            System.Windows.Controls.Image number = new System.Windows.Controls.Image();
            canvas01.Children.Clear();
            minute = (int)((game.time % 3600) / 60);
            second = (game.time % 3600) - minute * 60;
            number.Source = pics.number((int)(minute / 10), path);
            number.Width = 18;
            number.Height = 24;
            number.Margin = new Thickness(26, 0, 0, 0);
            canvas01.Children.Add(number);
            number = new System.Windows.Controls.Image();
            number.Source = pics.number(minute % 10, path);
            number.Width = 18;
            number.Height = 24;
            number.Margin = new Thickness(44, 0, 0, 0);
            canvas01.Children.Add(number);
            number = new System.Windows.Controls.Image();
            number.Source = pics.number((int)(second / 10), path);
            number.Width = 18;
            number.Height = 24;
            number.Margin = new Thickness(78, 0, 0, 0);
            canvas01.Children.Add(number);
            number = new System.Windows.Controls.Image();
            number.Source = pics.number(second % 10, path);
            number.Width = 18;
            number.Height = 24;
            number.Margin = new Thickness(96, 0, 0, 0);
            canvas01.Children.Add(number);


            if (inp == 1)
            {

                canvas02.Children.Clear();

                DoubleAnimation animation1 = new DoubleAnimation(18, 22, TimeSpan.FromSeconds(0.1));
                animation1.AutoReverse = true;
                DoubleAnimation animation2 = new DoubleAnimation(24, 30, TimeSpan.FromSeconds(0.1));
                animation2.AutoReverse = true;

                number = new System.Windows.Controls.Image();
                m = game.score;
                n = (int)(m / 100000);
                number.Source = pics.number(n, path);
                number.Width = 18;
                number.Height = 24;
                number.VerticalAlignment = VerticalAlignment.Center;
                number.Margin = new Thickness(16, 0, 0, 0);
                number.BeginAnimation(System.Windows.Controls.Image.WidthProperty, animation1);
                number.BeginAnimation(System.Windows.Controls.Image.HeightProperty, animation2);
                canvas02.Children.Add(number);
                number = new System.Windows.Controls.Image();
                m = m - n * 100000;
                n = (int)(m / 10000);
                number.Source = pics.number(n, path);
                number.Width = 18;
                number.Height = 24;
                number.VerticalAlignment = VerticalAlignment.Center;
                number.Margin = new Thickness(34, 0, 0, 0);
                number.BeginAnimation(System.Windows.Controls.Image.WidthProperty, animation1);
                number.BeginAnimation(System.Windows.Controls.Image.HeightProperty, animation2);
                canvas02.Children.Add(number);
                number = new System.Windows.Controls.Image();
                m = m - n * 10000;
                n = (int)(m / 1000);
                number.Source = pics.number(n, path);
                number.Width = 18;
                number.Height = 24;
                number.VerticalAlignment = VerticalAlignment.Center;
                number.Margin = new Thickness(52, 0, 0, 0);
                number.BeginAnimation(System.Windows.Controls.Image.WidthProperty, animation1);
                number.BeginAnimation(System.Windows.Controls.Image.HeightProperty, animation2);
                canvas02.Children.Add(number);
                number = new System.Windows.Controls.Image();
                m = m - n * 1000;
                n = (int)(m / 100);
                number.Source = pics.number(n, path);
                number.Width = 18;
                number.Height = 24;
                number.VerticalAlignment = VerticalAlignment.Center;
                number.Margin = new Thickness(70, 0, 0, 0);
                number.BeginAnimation(System.Windows.Controls.Image.WidthProperty, animation1);
                number.BeginAnimation(System.Windows.Controls.Image.HeightProperty, animation2);
                canvas02.Children.Add(number);
                number = new System.Windows.Controls.Image();
                m = m - n * 100;
                n = (int)(m / 10);
                number.Source = pics.number(n, path);
                number.Width = 18;
                number.Height = 24;
                number.VerticalAlignment = VerticalAlignment.Center;
                number.Margin = new Thickness(88, 0, 0, 0);
                number.BeginAnimation(System.Windows.Controls.Image.WidthProperty, animation1);
                number.BeginAnimation(System.Windows.Controls.Image.HeightProperty, animation2);
                canvas02.Children.Add(number);
                number = new System.Windows.Controls.Image();
                number.Source = pics.number(game.score % 10, path);
                number.Width = 18;
                number.Height = 24;
                number.VerticalAlignment = VerticalAlignment.Center;
                number.Margin = new Thickness(106, 0, 0, 0);
                number.BeginAnimation(System.Windows.Controls.Image.WidthProperty, animation1);
                number.BeginAnimation(System.Windows.Controls.Image.HeightProperty, animation2);
                canvas02.Children.Add(number);

                canvas03.Children.Clear();

                number = new System.Windows.Controls.Image();
                m = game.combo;
                n = (int)(m / 100);
                number.Source = pics.number(n, path);
                number.Width = 18;
                number.Height = 24;
                number.Margin = new Thickness(43, 0, 0, 0);
                canvas03.Children.Add(number);
                number = new System.Windows.Controls.Image();
                m = m - n * 100;
                n = (int)(m / 10);
                number.Source = pics.number(n, path);
                number.Width = 18;
                number.Height = 24;
                number.Margin = new Thickness(61, 0, 0, 0);
                canvas03.Children.Add(number);
                number = new System.Windows.Controls.Image();
                number.Source = pics.number(m % 10, path);
                number.Width = 18;
                number.Height = 24;
                number.Margin = new Thickness(79, 0, 0, 0);
                canvas03.Children.Add(number);
            }
            }
            catch { error(); }
        }

        public void buttonchange(int b, int s)
        {
            try
            {
                if (b == 1) button1.Background = new ImageBrush(pics.button(b, s, path));
                else if (b == 2) button2.Background = new ImageBrush(pics.button(b, s, path));
                else if (b == 3) button3.Background = new ImageBrush(pics.button(b, s, path));
                else if (b == 4) button4.Background = new ImageBrush(pics.button(b, s, path));
                else if (b == 5) button5.Background = new ImageBrush(pics.button(b, s, path));
                else if (b == 6) button6.Background = new ImageBrush(pics.button(b, s, path));
                else if (b == 7) button7.Background = new ImageBrush(pics.button(b, s, path));
                else if (b == 8) button8.Background = new ImageBrush(pics.button(b, s, path));
                else if (b == 9) button9.Background = new ImageBrush(pics.button(b, s, path));
                else if (b == 10) button10.Background = new ImageBrush(pics.button(b, s, path));
            }
            catch { error(); }
        }


        //Mouse Position Check
        public void mousecheck()
        {
            if (game.run == true)
            {
                System.Drawing.Point position = new System.Drawing.Point();
                position.X = Convert.ToInt32(System.Windows.Forms.Control.MousePosition.X - Left - image1.Margin.Left);
                position.Y = Convert.ToInt32(System.Windows.Forms.Control.MousePosition.Y - Top - image1.Margin.Top);
                if (((position.X - 65) % 40 < 36) && ((position.Y - 65) % 40 < 36))
                {
                    mouse.onbox = true;
                    mouse.onx = (position.X - 65 - ((position.X - 65) % 40)) / 40;
                    mouse.ony = (position.Y - 65 - ((position.Y - 65) % 40)) / 40;
                    if (mouse.onx >= 14 || position.X < 65 || mouse.ony >= 9 || position.Y < 65)
                    {
                        mouse.onbox = false;
                    }
                }
                else
                {
                    mouse.onbox = false;
                }
                if (mouse.onbox == true)
                {
                    Cursor = curselect;
                }
                else
                {
                    Cursor = curnormal;
                }
            }
        }

        public void boxstart(int num)
        {
            int x, y, z = 0;
            int nx, ny, n;
            int[] bx = new int[16];
            Random rnd = new Random();
            for (y = 0; y < 16; y++)
            {
                bx[y] = y;
            }
            for (z = 0; z < 160; z++)
            {
                y = rnd.Next(16);
                x = bx[z % 16];
                bx[z % 16] = bx[y];
                bx[y] = x;
            }
            for (x = 0; x < 14; x += 2)
            {
                for (y = 0; y < 9; y++)
                {
                    boxes.Square(x, y, bx[z % num] + 1);
                    boxes.Show(x, y, true);
                    boxes.Square(x + 1, y, bx[z % num] + 1);
                    boxes.Show(x + 1, y, true);
                    z++;
                }
            }
            for (z = 0; z < 200; z++)
            {
                x = rnd.Next(14);
                y = rnd.Next(9);
                nx = rnd.Next(14);
                ny = rnd.Next(9);
                n = boxes.square(x, y);
                boxes.Square(x, y, boxes.square(nx, ny));
                boxes.Square(nx, ny, n);
            }
        }

        public Boolean boxjudge()
        {
            int x, y;
            int x0, y0;
            int x1 = mouse.ox, y1 = mouse.oy;
            int x2 = mouse.onx, y2 = mouse.ony;
            if (boxes.square(x1, y1) != boxes.square(x2, y2)) return false;
            boxes.Show(x1, y1, false);
            boxes.Show(x2, y2, false);
            int i1 = 1, i2 = 1, i = 1;
            Boolean j = true;
            if (y1 > y2) i = -1;
            for (x = -1; x < 15; x++)
            {
                if (x > x1) i1 = -1;
                if (x > x2) i2 = -1;
                j = true;
                if (y1 == y2) j = false;
                for (y0 = y1; y0 != y2; y0 += i)
                {
                    if (boxes.show(x, y0) == true) j = false;
                }
                for (x0 = x; x0 != x1; x0 += i1)
                {
                    if (boxes.show(x0, y1) == true) j = false;
                }
                for (x0 = x; x0 != x2; x0 += i2)
                {
                    if (boxes.show(x0, y2) == true) j = false;
                }
                if (j == true)
                {
                    line = new line();
                    line.me = 3;
                    line.x = x1;
                    line.y = y1;
                    if (x < x1)
                    { 
                        line.sx[0] = -1;
                        line.l[0] = Math.Abs(x - x1) * 2 + 1;
                    }
                    else if (x == x1) line.me--;
                    else if (x > x1)
                    {
                        line.sx[0] = 1;
                        line.l[0] = Math.Abs(x - x1) * 2 + 1;
                    }
                    if (y1 < y2)
                    {
                        line.sy[line.me-2] = 1;
                        line.l[line.me - 2] = Math.Abs(y1 - y2) * 2 + 1;
                    }
                    else if (y1 > y2)
                    {
                        line.sy[line.me - 2] = -1;
                        line.l[line.me - 2] = Math.Abs(y1 - y2) * 2 + 1;
                    }
                    if (x < x2)
                    {
                        line.sx[line.me - 1] = 1;
                        line.l[line.me - 1] = Math.Abs(x - x2) * 2 + 1;
                    }
                    else if (x == x2) line.me--;
                    else if (x > x2)
                    {
                        line.sx[line.me - 1] = -1;
                        line.l[line.me - 1] = Math.Abs(x - x2) * 2 + 1;
                    }
                    return j;
                }
            }
            i1 = 1;
            i2 = 1;
            i = 1;
            if (x1 > x2) i = -1;
            for (y = -1; y < 10; y++)
            {
                if (y > y1) i1 = -1;
                if (y > y2) i2 = -1;
                j = true;
                if (x1 == x2) j = false;
                for (x0 = x1; x0 != x2; x0 += i)
                {
                    if (boxes.show(x0, y) == true) j = false;
                }
                for (y0 = y; y0 != y1; y0 += i1)
                {
                    if (boxes.show(x1, y0) == true) j = false;
                }
                for (y0 = y; y0 != y2; y0 += i2)
                {
                    if (boxes.show(x2, y0) == true) j = false;
                }
                if (j == true)
                {
                    line = new line();
                    line.me = 3;
                    line.x = x1;
                    line.y = y1;
                    if (y < y1)
                    {
                        line.sy[0] = -1;
                        line.l[0] = Math.Abs(y - y1) * 2 + 1;
                    }
                    else if (y == y1) line.me--;
                    else if (y > y1)
                    {
                        line.sy[0] = 1;
                        line.l[0] = Math.Abs(y - y1) * 2 + 1;
                    }
                    if (x1 < x2)
                    {
                        line.sx[line.me - 2] = 1;
                        line.l[line.me - 2] = Math.Abs(x1 - x2) * 2 + 1;
                    }
                    else if (x1 > x2)
                    {
                        line.sx[line.me - 2] = -1;
                        line.l[line.me - 2] = Math.Abs(x1 - x2) * 2 + 1;
                    }
                    if (y < y2)
                    {
                        line.sy[line.me - 1] = 1;
                        line.l[line.me - 1] = Math.Abs(y - y2) * 2 + 1;
                    }
                    else if (y == y2) line.me--;
                    else if (y > y2)
                    {
                        line.sy[line.me - 1] = -1;
                        line.l[line.me - 1] = Math.Abs(y - y2) * 2 + 1;
                    }
                    return j; 
                }
            }
            if (j == false)
            {
                boxes.Show(x1, y1, true);
                boxes.Show(x2, y2, true);
            }
            return j;
        }

        public void win()
        {
            try
            {
                game.pause = true;
                game.run = false;
                sound.bgm.Stop();
                sound.win.Stop();
                sound.win.Play();
                canvas1.Visibility = Visibility.Hidden;
                button1.Visibility = Visibility.Hidden;
                button2.Visibility = Visibility.Hidden;
                DoubleAnimation animation1 = new DoubleAnimation(0, 200, TimeSpan.FromSeconds(1));
                DoubleAnimation animation2 = new DoubleAnimation(0, 200, TimeSpan.FromSeconds(1));
                DoubleAnimation animation3 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(2));
                animation3.AutoReverse = true;
                animation3.RepeatBehavior = RepeatBehavior.Forever;
                animation2.BeginTime = animation1.BeginTime + TimeSpan.FromSeconds(1);
                animation3.BeginTime = animation1.BeginTime + TimeSpan.FromSeconds(4);
                image4.BeginAnimation(System.Windows.Controls.Image.WidthProperty, animation1);
                image4.BeginAnimation(System.Windows.Controls.Image.HeightProperty, animation1);
                image5.BeginAnimation(System.Windows.Controls.Image.WidthProperty, animation2);
                image5.BeginAnimation(System.Windows.Controls.Image.HeightProperty, animation2);
                image4.BeginAnimation(System.Windows.Controls.Image.OpacityProperty, animation3);
                image5.BeginAnimation(System.Windows.Controls.Image.OpacityProperty, animation3);
                image4.Visibility = Visibility.Visible;
                image5.Visibility = Visibility.Visible;
            }
            catch { error(); }
        }

        public void setting()
        {
            try
            {
                System.Windows.Controls.Image img1 = new System.Windows.Controls.Image();
                System.Windows.Controls.Image img2 = new System.Windows.Controls.Image();
                canvas1.Children.Clear();
                img1.Source = pics.check(path);
                img2.Source = pics.check(path);
                img1.Width = 60;
                img2.Width = 60;
                img1.Height = 60;
                img2.Height = 60;
                if (path.theme == "theme01") img1.Margin = new Thickness(48, 134, 0, 0);
                else if (path.theme == "theme02") img1.Margin = new Thickness(237, 134, 0, 0);
                else if (path.theme == "theme03") img1.Margin = new Thickness(424, 134, 0, 0);
                if (path.level == "easy") img2.Margin = new Thickness(100, 250, 0, 0);
                else if (path.level == "normal") img2.Margin = new Thickness(252, 250, 0, 0);
                else img2.Margin = new Thickness(407, 250, 0, 0);
                canvas1.Children.Add(img1);
                canvas1.Children.Add(img2);
            }
            catch { error(); }
        }

        public void error()
        {
            ErrorWindow newwindow = new ErrorWindow();
            if (errortick == 0)
            {
                newwindow.Show();
            }
            errortick++;
            Close();
        }

        public void start(int s)
        {
            try
            {
                pics.sets();
                sound.sets(path);
                canvas1.Visibility = Visibility.Hidden;
                image1.Visibility = Visibility.Hidden;
                image3.Visibility = Visibility.Hidden;
                image2.Source = pics.main(2, path);
                grid1.Background = new ImageBrush(pics.main(0, path));
                button1.Background = new ImageBrush(pics.button(1, 0, path));
                button2.Background = new ImageBrush(pics.button(2, 0, path));
                button3.Background = new ImageBrush(pics.button(3, 0, path));
                button4.Background = new ImageBrush(pics.button(4, 0, path));
                button5.Background = new ImageBrush(pics.button(5, 0, path));
                button6.Background = new ImageBrush(pics.button(6, 0, path));
                button7.Background = new ImageBrush(pics.button(7, 0, path));
                button8.Background = new ImageBrush(pics.button(8, 0, path));
                button9.Background = new ImageBrush(pics.button(9, 0, path));
                button10.Background = new ImageBrush(pics.button(10, 0, path));
                button1.Visibility = Visibility.Hidden;
                button2.Visibility = Visibility.Hidden;
                button3.Visibility = Visibility.Hidden;
                button9.Visibility = Visibility.Hidden;
                button10.Visibility = Visibility.Hidden;
                timer.Interval = new TimeSpan(0, 0, 1);
                sound.bgm.MediaEnded += new EventHandler(bgm_End);
                curnormal = new System.Windows.Input.Cursor(path.pic + @"\cursor\NormalSelect.cur");
                curselect = new System.Windows.Input.Cursor(path.pic + @"\cursor\LinkSelect.cur");
                if (s == 1)
                {
                    DoubleAnimation animation = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(500));
                    window.BeginAnimation(Window.OpacityProperty, animation);
                }
            }
            catch { error(); }
        }

        public void gamestart()
        {
            game=new game();
            if (path.level == "easy") game.boxnum = 8;
            else if (path.level == "normal") game.boxnum = 12;
            else game.boxnum = 16;
            boxstart(game.boxnum);
            squaredraw(0);
            image1.Source = pics.main(1, path);
            canvas1.Visibility = Visibility.Visible;
            canvas2.Visibility = Visibility.Visible;
            canvas3.Visibility = Visibility.Visible;
            image1.Visibility = Visibility.Visible;
            image3.Visibility = Visibility.Visible;
            button1.Visibility = Visibility.Visible;
            button2.Visibility = Visibility.Visible;
            button3.Visibility = Visibility.Visible;
            canvas01.Visibility = Visibility.Visible;
            canvas02.Visibility = Visibility.Visible;
            canvas03.Visibility = Visibility.Visible;
            button7.Visibility = Visibility.Hidden;
            button8.Visibility = Visibility.Hidden;
            button1.Background = new ImageBrush(pics.button(1, 0, path));
            button2.Background = new ImageBrush(pics.button(2, 0, path));
            button3.Background = new ImageBrush(pics.button(3, 0, path));
            button4.Background = new ImageBrush(pics.button(4, 0, path));
            button5.Background = new ImageBrush(pics.button(5, 0, path));
            button6.Background = new ImageBrush(pics.button(6, 0, path));
            infodraw(1);
            sound.bgm.Stop();
            sound.bgm.Play();
            timer.Stop();
            timer.Start();
            game.run = true;
            game.pause = false;
        }
    }
}
