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
using System.Windows.Shapes;
using System.Diagnostics;

namespace lianliankan
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class InfoWindow : Window
    {
        Boolean mousedown = false;
        int mx = 0;
        int my = 0;

        public InfoWindow()
        {
            InitializeComponent();
        }

        private void infowindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void button_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = System.Windows.Input.Cursors.Hand;
        }

        private void button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = System.Windows.Input.Cursors.Arrow;
        }

        private void button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void infowindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mousedown = true;
            mx = System.Windows.Forms.Control.MousePosition.X;
            my = System.Windows.Forms.Control.MousePosition.Y;
        }

        private void infowindow_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            mousedown = false;
        }

        private void infowindow_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (mousedown == true)
            {
                infowindow.Left = infowindow.Left + System.Windows.Forms.Control.MousePosition.X - mx;
                infowindow.Top = infowindow.Top + System.Windows.Forms.Control.MousePosition.Y - my;
                mx = System.Windows.Forms.Control.MousePosition.X;
                my = System.Windows.Forms.Control.MousePosition.Y;
            }
        }

        private void infowindow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mousedown = false;
        }
    }
}
