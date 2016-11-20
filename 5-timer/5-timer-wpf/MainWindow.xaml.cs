using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _5_timer_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool[] Labels = new bool[6];
        private int t = 0;

        private void reset_labels()
        {
            for (int i = 0; i < 6; i++)
                Labels[i] = false;
        }

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void randomize_labels()
        {
            Random rand = new Random();
            int acwh = (int)label1.ActualWidth;
            int maxx = (int)canvas1.Width - acwh;
            int maxy = (int)canvas1.Height - acwh;
            int[] hs = new int[6];
            int[] ws = new int[6];
            for (int i = 0; i < 6; i++)
            {
                hs[i] = (int)canvas1.Height;
                ws[i] = (int)canvas1.Width;
            }

            for (int i = 0; i < 6; ++i)
            {
                hs[i] = rand.Next(0, maxy);
                ws[i] = rand.Next(0, maxx);

                for (int j = 0; j < 6; ++j)
                {
                    if (i != j)
                    {
                        while ((Math.Abs(hs[j] - hs[i]) < 20) && (Math.Abs(ws[j] - ws[i]) < acwh))
                        {
                            hs[i] = rand.Next(0, maxy);
                            ws[i] = rand.Next(0, maxx);
                        }
                    }
                }

            }

            int k = 0;
            foreach (var c in canvas1.Children)
            {
                Canvas.SetTop(((Label)c), ws[k]);
                Canvas.SetLeft(((Label)c), hs[k]);
                //((Label)c). = new Point(ws[k], hs[k]);
                ++k;
            }

        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            MenuItem start = sender as MenuItem;
            start.IsEnabled = false;
            randomize_labels();
            reset_labels();
            foreach (var c in canvas1.Children)
                ((Label)c).Background = Brushes.LightCoral;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var c in canvas1.Children)
            {
                Label l = c as Label;
                l.Width = l.ActualHeight;
            }
            
        }
    }
}
