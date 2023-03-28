using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wykres
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Rysuj();
        }

        private void Rysuj()
        {
            this.Invalidate();
            Application.DoEvents();
            Graphics g = this.CreateGraphics();

            int szerokosc = (int)g.VisibleClipBounds.Width;
            int wysokosc = (int)g.VisibleClipBounds.Height;
            if (wysokosc < 47) return;
            Bitmap b = new Bitmap(szerokosc, wysokosc);

            int d = 40; // odsunięcie osi od krawędzi
            int m = 10; // margines
            int zeroX = d; // punkt 0 na X (położenie osi Y) w pikselach
            int zeroY = wysokosc - d; // punkt 0 na Y (położenie osi X) w pikselach
            int xMax = (int)numericUpDownX.Value; // maskymalna wartość argumentu
            int yMax = (int)numericUpDownY.Value;
            int jednostkaX = (szerokosc - d - zeroX) / xMax; // jednostka osi X w pikselach
            int jednostkaY = (zeroY - d) / yMax;

            for (int i = m; i <= szerokosc - m; i++) b.SetPixel(i, zeroY, Color.Black); // rysuje oś X
            for (int i = m; i <= wysokosc - m; i++) b.SetPixel(zeroX, i, Color.Black); // rysuje oś Y
            for (int i = 3; i < 20; i++)
            {
                b.SetPixel(szerokosc - m - i, zeroY + i * 2 / 5, Color.Black); // grot dolny osi X
                b.SetPixel(szerokosc - m - i, zeroY - i * 2 / 5, Color.Black); // grot górny osi X
                b.SetPixel(zeroX + i * 2 / 5, m + i, Color.Black); // grot prawy osi Y
                b.SetPixel(zeroX - i * 2 / 5, m + i, Color.Black); // grot lewy osi Y
            }
            for (int i = 1; i <= xMax; i++) // skala osi X
                for (int j = -5; j <= 5; j++)
                    b.SetPixel(zeroX + i * jednostkaX, zeroY + j, Color.Black);
            for (int i = 1; i <= yMax; i++) // skala osi Y
                for (int j = -5; j <= 5; j++)
                    b.SetPixel(zeroX + j, zeroY - i * jednostkaY, Color.Black);

            float dx = 1 / (jednostkaX * 10.0f);
            for (float x = 0; x <= xMax; x += dx)
            {
                int xx = zeroX + (int)(x * jednostkaX);
                int yy = zeroY - (int)(f(x) * jednostkaY);
                if (0 <= yy && yy <= wysokosc) b.SetPixel(xx, yy, Color.Blue);
            }

            g.DrawImage(b, 0, 0);

            for (int i = 1; i <= xMax; i++) g.DrawString(i.ToString(), Font, Brushes.Black, zeroX - 5 + i * jednostkaX, zeroY + 8);
            for (int i = 1; i <= yMax; i++) g.DrawString(i.ToString(), Font, Brushes.Black, zeroX - 20, zeroY - 6 - i * jednostkaY);
        }

        private float f(float x)
        {
            return (float)(x * Math.Sin(x) * Math.Sin(x));
        }

        private void numericUpDownX_ValueChanged(object sender, EventArgs e)
        {
            Rysuj();
        }

        private void numericUpDownY_ValueChanged(object sender, EventArgs e)
        {
            Rysuj();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Rysuj();
        }

        private int x1 = 0, y1 = 0;

        private void Form1_Move(object sender, EventArgs e)
        {
            int x = this.Location.X, y = this.Location.Y;
            if (x1 < 0 && x1 < x || Screen.PrimaryScreen.WorkingArea.Width < x1 + this.Size.Width && x < x1 || Screen.PrimaryScreen.WorkingArea.Height < y1 + this.Size.Height && y < y1) Rysuj();
            x1 = x; y1 = y;
        }
    }
}
