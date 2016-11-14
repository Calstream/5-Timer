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

namespace _5_timer
{
    public partial class Form1 : Form
    {
        private bool[] Labels = new bool[6];
        private int t = 0;
        private void reset_labels()
        {
            for (int i = 0; i < 6; i++)
                Labels[i] = false;
        }

        private void randomize_labels()
        {
            Random rand = new Random();
            int maxx = panel1.Width - 20;
            int maxy = panel1.Height - 20;
            int[] hs = new int[6];
            int[] ws = new int[6];
            for (int i = 0; i < 6; i++)
            {
                hs[i] = panel1.Height;
                ws[i] = panel1.Width;
            }

            for (int i = 0; i < 6; ++i)
            {
                hs[i] = rand.Next(0, maxy);
                ws[i] = rand.Next(0, maxx);

                for (int j = 0; j < 6; ++j)
                {
                    if (i != j)
                    {
                        while ((Math.Abs(hs[j] - hs[i]) < 20) && (Math.Abs(ws[j] - ws[i]) < 20))
                        {
                            hs[i] = rand.Next(0, maxy);
                            ws[i] = rand.Next(0, maxx);
                        }
                    }
                }

            }

            int k = 0;
            foreach (Control c in panel1.Controls)
            {
                ((Label)c).Location = new Point(ws[k], hs[k]);
                ++k;
            }

        }

        public Form1()
        {
            InitializeComponent();
            reset_labels();
            randomize_labels();
            using (StreamReader sr = new StreamReader("results.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    listBox1.Items.Add(line);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!startToolStripMenuItem.Enabled)
            {
                ++t;
                textBox1.Text = string.Format("{0},{1}", t / 10, t % 10);
            }
        }

        private void startMenu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem start = sender as ToolStripMenuItem;
            start.Enabled = false;
            randomize_labels();
            reset_labels();
            foreach (var l in panel1.Controls)
                ((Label)l).BackColor = Color.LightCoral;
        }

        private void first_label_Click(object sender, EventArgs e)
        {
            if (!startToolStripMenuItem.Enabled)
            {
                Label lbl = sender as Label;
                lbl.BackColor = Color.GreenYellow;
                int num = 0;
                Int32.TryParse(lbl.Name.ToString().Remove(0, 5), out num);
                Labels[num] = true;
            }
        }

        private void label_Click(object sender, EventArgs e)
        {
            if (!startToolStripMenuItem.Enabled)
            {
                Label lbl = sender as Label;
                int num = 0;
                Int32.TryParse(lbl.Name.ToString().Remove(0, 5), out num);
                if (Labels[num - 1])
                {
                    lbl.BackColor = Color.GreenYellow;
                    Labels[num] = true;
                }
            }
        }

        private void save_and_close()
        {
            using (StreamWriter sr = new StreamWriter("results.txt"))
            {
                foreach (var item in listBox1.Items)
                {
                   sr.WriteLine(item.ToString());
                }
            }
            Dispose();
        }

        private void Form1_FormClosing(object sender, EventArgs e)
        {
            save_and_close();
        }

        private void last_label_Click(object sender, EventArgs e)
        {
            if (!startToolStripMenuItem.Enabled)
            {
                Label lbl = sender as Label;
                int num = 0;
                Int32.TryParse(lbl.Name.ToString().Remove(0, 5), out num);
                if (Labels[num - 1])
                {
                    lbl.BackColor = Color.GreenYellow;
                    reset_labels();
                    startToolStripMenuItem.Enabled = true;
                    listBox1.Items.Add(textBox1.Text);
                    textBox1.Text = "0,0";
                    if (t > 100)
                        MessageBox.Show("Вы проиграли");
                    else MessageBox.Show("Вы выиграли");
                    t = 0;

                }
            }
        }

        private void exitMenu_Click(object sender, EventArgs e)
        {
            save_and_close();
        }
    }

        

    
}
