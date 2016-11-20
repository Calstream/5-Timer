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
using System.Windows.Threading;
using System.IO;

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
            reset_labels();
            //randomize_labels();
            using (StreamReader sr = new StreamReader("results.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    listbox1.Items.Add(line);
                }
            }
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            //dispatcherTimer.Interval = TimeSpan.FromMilliseconds(10);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (!StartButton.IsEnabled)
            {
                
                ++t;
                textbox1.Text = string.Format("{0},{1}", t / 10, t % 10);
            }
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
                        while ((Math.Abs(hs[j] - hs[i]) < acwh) && (Math.Abs(ws[j] - ws[i]) < acwh))
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
                Canvas.SetTop(((Label)c), hs[k]);
                Canvas.SetLeft(((Label)c), ws[k]);
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

        private void first_label_Click(object sender, MouseButtonEventArgs e)
        {
            if (!StartButton.IsEnabled)
            {
                Label lbl = sender as Label;
                lbl.Background = Brushes.GreenYellow;
                int num = 0;
                Int32.TryParse(lbl.Name.ToString().Remove(0, 5), out num);
                Labels[num] = true;
            }
        }

        private void label_Click(object sender, MouseButtonEventArgs e)
        {
            if (!StartButton.IsEnabled)
            {
                Label lbl = sender as Label;
                int num = 0;
                Int32.TryParse(lbl.Name.ToString().Remove(0, 5), out num);
                if (Labels[num - 1])
                {
                    lbl.Background = Brushes.GreenYellow;
                    Labels[num] = true;
                }
            }
        }

        private void last_label_Click(object sender, MouseButtonEventArgs e)
        {
            if (!StartButton.IsEnabled)
            {
                Label lbl = sender as Label;
                int num = 0;
                Int32.TryParse(lbl.Name.ToString().Remove(0, 5), out num);
                if (Labels[num - 1])
                {
                    lbl.Background = Brushes.GreenYellow;
                    reset_labels();
                    StartButton.IsEnabled = true;
                    listbox1.Items.Add(textbox1.Text);
                    textbox1.Text = "0,0";
                    if (t > 100)
                        MessageBox.Show("Вы проиграли");
                    else MessageBox.Show("Вы выиграли");
                    t = 0;

                }
            }
        }

        private void save()
        {
            using (StreamWriter sr = new StreamWriter("results.txt"))
            {
                foreach (var item in listbox1.Items)
                {
                    sr.WriteLine(item.ToString());
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            save();
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var c in canvas1.Children)
            {
                Label l = c as Label;
                l.Width = l.ActualHeight;
            }
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            save();
        }

        
    }
}
