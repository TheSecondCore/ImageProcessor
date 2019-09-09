using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Bitmap Image_mem, Image_new, ImageR, ImageG, ImageB;
        public int X1, X2, Y1, Y2;

        public Form3 Gist;
        public int[] masR = new int[256];
        public int[] masG = new int[256];
        public int[] masB = new int[256];
        public Bitmap BMP_R, BMP_G, BMP_B;
        public Form1()
        {
            InitializeComponent();
            trackbarlabel.Text = Convert.ToString(trackBar1.Value);

            Gist = new Form3();
            BMP_R = new Bitmap(Gist.pictureBox1.Width, Gist.pictureBox1.Height);
            BMP_G = new Bitmap(Gist.pictureBox2.Width, Gist.pictureBox2.Height);
            BMP_B = new Bitmap(Gist.pictureBox3.Width, Gist.pictureBox3.Height);
        }

        private void rGBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 windowResult = new Form2();
            windowResult.Text = "BRG";
            toolStripProgressBar1.Maximum = Image_mem.Width;
            for (int i = 0; i < Image_mem.Width; i++)
            {
                toolStripProgressBar1.Value = i;
                for (int j = 0; j < Image_mem.Height; j++)
                {
                    Color color = Image_mem.GetPixel(i, j);
                    Image_mem.SetPixel(i, j, Color.FromArgb(255, color.B, color.R, color.G));
                    if (Image_mem.GetPixel(i, j) == Color.FromArgb(255, 0, 0, 0))
                    {
                        Image_mem.SetPixel(i, j, Color.FromArgb(255, 255, 255, 255));
                    }
                }
            }
            toolStripProgressBar1.Value = 0;
            windowResult.pictureBox1.Image = new Bitmap(Image_mem);
            windowResult.Show();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (radioButton1.Checked && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Graphics gr = Graphics.FromImage(Image_new);
                Rectangle part1 = new Rectangle();
                part1.X = X1 - 2;
                part1.Width = (X2 - X1) + 4;
                part1.Y = Y1 - 2;
                part1.Height = (Y2 - Y1) + 4;
                gr.DrawImage(Image_mem, part1, part1, GraphicsUnit.Pixel);
                X2 = e.X;
                Y2 = e.Y;
                Pen pen = new Pen(Color.Red);
                gr.DrawRectangle(pen, X1, Y1, (X2 - X1), (Y2 - Y1));
                pictureBox1.Image = new Bitmap(Image_new);
            }
        }

        private void арифмСреднееToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int W = (int)numericUpDown1.Value, k = 0;
            int[,] win = new int[W, W];
            int SumR, SumG, SumB;
            for (int i = 0; i < W; i++)
            {
                for (int j = 0; j < W; j++)
                {
                    win[i, j] = 1;
                }
            }
            k = W * W;
            toolStripProgressBar1.Maximum = X2 - X1;
            for (int i = X1 + W / 2; i < X2 - W / 2; i++)
            {
                toolStripProgressBar1.Value = i - X1;
                for (int j = Y1 + W / 2; j < Y2 - W / 2; j++)
                {
                    SumR = SumG = SumB = 0;
                    for (int i1 = -W / 2; i1 <= W / 2; i1++)
                    {
                        for (int j1 = -W / 2; j1 <= W / 2; j1++)
                        {
                            Color col = Image_mem.GetPixel(i + i1, j + j1);
                            SumR += win[i1 + W / 2, j1 + W / 2] * col.R;
                            SumG += win[i1 + W / 2, j1 + W / 2] * col.G;
                            SumB += win[i1 + W / 2, j1 + W / 2] * col.B;
                        }
                    }
                    SumR /= k;
                    SumG /= k;
                    SumB /= k;
                    Image_new.SetPixel(i, j, Color.FromArgb(255, SumR, SumG, SumB));
                }
            }
            pictureBox1.Image = new Bitmap(Image_new);
            toolStripProgressBar1.Value = 0;
        }

        private void пороговыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int W = (int)numericUpDown1.Value, k = 0;
            int porog = (int)numericUpDown2.Value;
            int[,] win = new int[W, W];
            int SumR, SumG, SumB;
            for (int i = 0; i < W; i++)
            {
                for (int j = 0; j < W; j++)
                {
                    win[i, j] = 1;
                }
            }
            win[W / 2, W / 2] = 0;
            k = W * W - 1;
            toolStripProgressBar1.Maximum = X2 - X1;
            for (int i = X1 + W / 2; i < X2 - W / 2; i++)
            {
                toolStripProgressBar1.Value = i - X1;
                for (int j = Y1 + W / 2; j < Y2 - W / 2; j++)
                {
                    SumR = SumG = SumB = 0;
                    for (int i1 = -W / 2; i1 <= W / 2; i1++)
                    {
                        for (int j1 = -W / 2; j1 <= W / 2; j1++)
                        {
                            Color col = Image_mem.GetPixel(i + i1, j + j1);
                            SumR += win[i1 + W / 2, j1 + W / 2] * col.R;
                            SumG += win[i1 + W / 2, j1 + W / 2] * col.G;
                            SumB += win[i1 + W / 2, j1 + W / 2] * col.B;
                        }
                    }
                    Color centercol = Image_mem.GetPixel(i, j);
                    SumR /= k;
                    SumG /= k;
                    SumB /= k;
                    if (Math.Abs(SumR - centercol.R) > porog || Math.Abs(SumG - centercol.G) > porog || Math.Abs(SumB - centercol.B) > porog)
                    {
                        Image_new.SetPixel(i, j, Color.FromArgb(255, SumR, SumG, SumB));
                    }
                }
            }
            pictureBox1.Image = new Bitmap(Image_new);
            toolStripProgressBar1.Value = 0;
        }

        private void медианныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int w = (int)numericUpDown1.Value, k = 0;
            int[,] win = new int[w, w];
            int[] winR = new int[w * w];
            int[] winG = new int[w * w];
            int[] winB = new int[w * w];

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    win[i, j] = 1;
                }
            }
            k = w * w;
            toolStripProgressBar1.Maximum = X2 - X1;

            for (int i = X1 + w / 2; i < X2 - w / 2; i++)
            {
                toolStripProgressBar1.Value = i - X1;
                for (int j = Y1 + w / 2; j < Y2 - w / 2; j++)
                {
                    int n = 0;
                    for (int i1 = -w / 2; i1 <= w / 2; i1++)
                    {
                        for (int j1 = -w / 2; j1 <= w / 2; j1++)
                        {
                            Color color = Image_mem.GetPixel(i + i1, j + j1);
                            winR[n] = win[i1 + w / 2, j1 + w / 2] * color.R;
                            winG[n] = win[i1 + w / 2, j1 + w / 2] * color.G;
                            winB[n] = win[i1 + w / 2, j1 + w / 2] * color.B;
                            n++;
                        }
                    }
                    Array.Sort(winR);
                    Array.Sort(winG);
                    Array.Sort(winB);
                    Image_new.SetPixel(i, j, Color.FromArgb(255, winR[k / 2 + 1], winG[k / 2 + 1], winB[k / 2 + 1]));
                    pictureBox1.Image = new Bitmap(Image_new);
                }
            }
            toolStripProgressBar1.Value = 0;
        }

        private void альфаУсеченноеСрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int w = (int)numericUpDown1.Value, k = 0;
            int Alpha = (int)numericUpDown3.Value;
            int[,] win = new int[w, w];
            int[] winR = new int[w * w];
            int[] winG = new int[w * w];
            int[] winB = new int[w * w];

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    win[i, j] = 1;
                }
            }
            k = w * w;
            toolStripProgressBar1.Maximum = X2 - X1;

            for (int i = X1 + w / 2; i < X2 - w / 2; i++)
            {
                toolStripProgressBar1.Value = i - X1;
                for (int j = Y1 + w / 2; j < Y2 - w / 2; j++)
                {
                    int n = 0;
                    for (int i1 = -w / 2; i1 <= w / 2; i1++)
                    {
                        for (int j1 = -w / 2; j1 <= w / 2; j1++)
                        {
                            Color color = Image_mem.GetPixel(i + i1, j + j1);
                            winR[n] = win[i1 + w / 2, j1 + w / 2] * color.R;
                            winG[n] = win[i1 + w / 2, j1 + w / 2] * color.G;
                            winB[n] = win[i1 + w / 2, j1 + w / 2] * color.B;
                            n++;
                        }
                    }
                    Array.Sort(winR);
                    Array.Sort(winG);
                    Array.Sort(winB);
                    Image_new.SetPixel(i, j, Color.FromArgb(255, (winR[Alpha] + winR[winR.Length - 1 - Alpha]) / 2, (winG[Alpha] + winG[winG.Length - 1 - Alpha]) / 2, (winB[Alpha] + winB[winB.Length - 1 - Alpha]) / 2));
                    pictureBox1.Image = new Bitmap(Image_new);
                }
            }
            toolStripProgressBar1.Value = 0;
        }

        private void gaussianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double sigma = double.Parse(textBox1.Text);
            int w = (int)sigma * 3 + (int)sigma * 3 + 1;
            double[,] win = new double[w, w];
            double SumR, SumG, SumB;
            double popr_koef = 0;
            for (int i = -w / 2; i <= w / 2; i++)
            {
                for (int j = -w / 2; j <= w / 2; j++)
                {
                    win[i + w / 2, j + w / 2] = Math.Exp(-(double)(i * i + j * j) / (2 * sigma * sigma)) / (Math.Sqrt(Math.PI * 2) * sigma);
                    popr_koef += win[i + w / 2, j + w / 2];
                }
            }
            toolStripProgressBar1.Maximum = X2 - X1;
            for (int i = X1 + w / 2; i < X2 - w / 2; i++)
            {
                toolStripProgressBar1.Value = i - X1;
                for (int j = Y1 + w / 2; j < Y2 - w / 2; j++)
                {
                    SumR = SumG = SumB = 0;
                    for (int i1 = -w / 2; i1 <= w / 2; i1++)
                    {
                        for (int j1 = -w / 2; j1 <= w / 2; j1++)
                        {
                            Color col = Image_mem.GetPixel(i + i1, j + j1);
                            SumR += win[i1 + w / 2, j1 + w / 2] * col.R;
                            SumG += win[i1 + w / 2, j1 + w / 2] * col.G;
                            SumB += win[i1 + w / 2, j1 + w / 2] * col.B;
                        }
                    }
                    SumR /= popr_koef;
                    SumG /= popr_koef;
                    SumB /= popr_koef;
                    Image_new.SetPixel(i, j, Color.FromArgb(255, (int)SumR, (int)SumG, (int)SumB));
                }
            }
            pictureBox1.Image = new Bitmap(Image_new);
            toolStripProgressBar1.Value = 0;
        }

        private void геометрическоеСреднееToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int W = (int)numericUpDown1.Value;
            double MultiplyR, MultiplyG, MultiplyB;
            toolStripProgressBar1.Maximum = X2 - X1;
            for (int i = X1 + W / 2; i < X2 - W / 2; i++)
            {
                toolStripProgressBar1.Value = i - X1;
                for (int j = Y1 + W / 2; j < Y2 - W / 2; j++)
                {
                    MultiplyR = MultiplyG = MultiplyB = 1;
                    for (int i1 = -W / 2; i1 <= W / 2; i1++)
                    {
                        for (int j1 = -W / 2; j1 <= W / 2; j1++)
                        {
                            Color col = Image_mem.GetPixel(i + i1, j + j1);
                            MultiplyR *= col.R;
                            MultiplyG *= col.G;
                            MultiplyB *= col.B;
                        }
                    }
                    MultiplyR = Math.Pow(MultiplyR, 1.0 / (W * W));
                    MultiplyG = Math.Pow(MultiplyG, 1.0 / (W * W));
                    MultiplyB = Math.Pow(MultiplyB, 1.0 / (W * W));
                    Image_new.SetPixel(i, j, Color.FromArgb(255, (int)MultiplyR, (int)MultiplyG, (int)MultiplyB));
                }
            }
            pictureBox1.Image = new Bitmap(Image_new);
            toolStripProgressBar1.Value = 0;
        }

        private void гармоническоеСреднееToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int W = (int)numericUpDown1.Value;
            double SumR, SumG, SumB;
            toolStripProgressBar1.Maximum = X2 - X1;
            for (int i = X1 + W / 2; i < X2 - W / 2; i++)
            {
                toolStripProgressBar1.Value = i - X1;
                for (int j = Y1 + W / 2; j < Y2 - W / 2; j++)
                {
                    SumR = SumG = SumB = 0;
                    for (int i1 = -W / 2; i1 <= W / 2; i1++)
                    {
                        for (int j1 = -W / 2; j1 <= W / 2; j1++)
                        {
                            Color col = Image_mem.GetPixel(i + i1, j + j1);
                            int colorR = col.R; int colorG = col.G; int colorB = col.B;
                            if (colorR == 0)
                            {
                                colorR = 1;
                            }
                            if (colorG == 0)
                            {
                                colorG = 1;
                            }
                            if (colorB == 0)
                            {
                                colorG = 1;
                            }
                            SumR += (1.0 / colorR);
                            SumG += (1.0 / colorG);
                            SumB += (1.0 / colorB);
                        }
                    }
                    SumR = (W * W) / SumR;
                    SumG = (W * W) / SumG;
                    SumB = (W * W) / SumB;
                    Image_new.SetPixel(i, j, Color.FromArgb(255, (int)SumR, (int)SumG, (int)SumB));
                }
            }
            pictureBox1.Image = new Bitmap(Image_new);
            toolStripProgressBar1.Value = 0;
        }

        private void контргармоническоеСрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int W = (int)numericUpDown1.Value;
            double q = double.Parse(textBox2.Text);
            double SumR, SumG, SumB;
            double SumRdenum, SumGdenum, SumBdenum;
            toolStripProgressBar1.Maximum = X2 - X1;
            for (int i = X1 + W / 2; i < X2 - W / 2; i++)
            {
                toolStripProgressBar1.Value = i - X1;
                for (int j = Y1 + W / 2; j < Y2 - W / 2; j++)
                {
                    SumR = SumG = SumB = 0;
                    SumRdenum = SumGdenum = SumBdenum = 0;
                    for (int i1 = -W / 2; i1 <= W / 2; i1++)
                    {
                        for (int j1 = -W / 2; j1 <= W / 2; j1++)
                        {
                            Color col = Image_mem.GetPixel(i + i1, j + j1);
                            int colorR = col.R; int colorG = col.G; int colorB = col.B;
                            if (colorR == 0)
                            {
                                colorR = 1;
                            }
                            if (colorG == 0)
                            {
                                colorG = 1;
                            }
                            if (colorB == 0)
                            {
                                colorB = 1;
                            }
                            SumR += Math.Pow(colorR, q + 1);
                            SumG += Math.Pow(colorG, q + 1);
                            SumB += Math.Pow(colorB, q + 1);
                            SumRdenum += Math.Pow(colorR, q);
                            SumGdenum += Math.Pow(colorG, q);
                            SumBdenum += Math.Pow(colorB, q);
                        }
                    }
                    SumR = SumR / SumRdenum;
                    SumG = SumG / SumGdenum;
                    SumB = SumB / SumBdenum;
                    Image_new.SetPixel(i, j, Color.FromArgb(255, (int)SumR, (int)SumG, (int)SumB));
                }
            }
            pictureBox1.Image = new Bitmap(Image_new);
            toolStripProgressBar1.Value = 0;
        }

        private void робертсаToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Brush MyBrush = new SolidBrush(Color.Black);
            Graphics gr = Graphics.FromImage(Image_new);
            gr.FillRectangle(MyBrush, 0, 0, Image_new.Width, Image_new.Height);
            int porog = int.Parse(numericUpDown2.Text);
            double R, G, B;
            double R1, B1, G1;
            double R2, B2, G2;
            double R3, B3, G3;
            double Robert_R, Robert_G, Robert_B;
            toolStripProgressBar1.Maximum = X2 - X1;

            for (int i = X1; i < X2 - 1; i++)
            {
                toolStripProgressBar1.Value = i - X1;
                for (int j = Y1; j < Y2 - 1; j++)
                {
                    Color col = Image_mem.GetPixel(i, j);
                    R = col.R;
                    G = col.G;
                    B = col.B;
                    col = Image_mem.GetPixel(i + 1, j + 1);
                    R1 = col.R;
                    G1 = col.G;
                    B1 = col.B;
                    col = Image_mem.GetPixel(i, j + 1);
                    R2 = col.R;
                    G2 = col.G;
                    B2 = col.B;
                    col = Image_mem.GetPixel(i + 1, j);
                    R3 = col.R;
                    G3 = col.G;
                    B3 = col.B;
                    Robert_R = Math.Sqrt((Math.Pow((R1 - R), 2) + Math.Pow((R2 - R3), 2)));
                    Robert_G = Math.Sqrt((Math.Pow((G1 - G), 2) + Math.Pow((G2 - G3), 2)));
                    Robert_B = Math.Sqrt((Math.Pow((B1 - B), 2) + Math.Pow((B2 - B3), 2)));

                    if (Robert_R > porog || Robert_G > porog || Robert_B > porog)
                    {
                        Image_new.SetPixel(i, j, Color.White);
                    }

                }
            }
            Form2 form_Result = new Form2();
            form_Result.pictureBox1.Image = new Bitmap(Image_new);
            form_Result.Show();
            toolStripProgressBar1.Value = 0;
        }

        private void собелаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Brush MyBrush = new SolidBrush(Color.Black);
            Graphics gr = Graphics.FromImage(Image_new);
            gr.FillRectangle(MyBrush, 0, 0, Image_new.Width, Image_new.Height);
            int porog = int.Parse(numericUpDown2.Text);
            double G_R, G_G, G_B, X_R, X_G, X_B, Y_R, Y_G, Y_B;
            int[] A_R = new int[8], A_G = new int[8], A_B = new int[8];
            toolStripProgressBar1.Maximum = X2 - X1;
            for (int i = X1 + 1; i < X2 - 1; i++)
            {
                toolStripProgressBar1.Value = i - X1;
                for (int j = Y1 + 1; j < Y2 - 1; j++)
                {
                    Color col = Image_mem.GetPixel(i + 1, j + 1);
                    A_R[0] = col.R;
                    A_G[0] = col.G;
                    A_B[0] = col.B;

                    col = Image_mem.GetPixel(i, j - 1);
                    A_R[1] = col.R;
                    A_G[1] = col.G;
                    A_B[1] = col.B;

                    col = Image_mem.GetPixel(i + 1, j - 1);
                    A_R[2] = col.R;
                    A_G[2] = col.G;
                    A_B[2] = col.B;

                    col = Image_mem.GetPixel(i + 1, j);
                    A_R[3] = col.R;
                    A_G[3] = col.G;
                    A_B[3] = col.B;

                    col = Image_mem.GetPixel(i + 1, j + 1);
                    A_R[4] = col.R;
                    A_G[4] = col.G;
                    A_B[4] = col.B;

                    col = Image_mem.GetPixel(i, j + 1);
                    A_R[5] = col.R;
                    A_G[5] = col.G;
                    A_B[5] = col.B;

                    col = Image_mem.GetPixel(i - 1, j + 1);
                    A_R[6] = col.R;
                    A_G[6] = col.G;
                    A_B[6] = col.B;

                    col = Image_mem.GetPixel(i - 1, j);
                    A_R[7] = col.R;
                    A_G[7] = col.G;
                    A_B[7] = col.B;

                    X_R = (A_R[2] + 2 * A_R[3] + A_R[4]) - (A_R[0] + 2 * A_R[7] + A_R[6]);
                    Y_R = (A_R[0] + 2 * A_R[1] + A_R[2]) - (A_R[6] + 2 * A_R[5] + A_R[4]);
                    G_R = Math.Sqrt(X_R * X_R + Y_R * Y_R);

                    X_G = (A_G[2] + 2 * A_G[3] + A_G[4]) - (A_G[0] + 2 * A_G[7] + A_G[6]);
                    Y_G = (A_G[0] + 2 * A_G[1] + A_G[2]) - (A_G[6] + 2 * A_G[5] + A_G[4]);
                    G_G = Math.Sqrt(X_G * X_G + Y_G * Y_G);

                    X_B = (A_B[2] + 2 * A_B[3] + A_B[4]) - (A_B[0] + 2 * A_B[7] + A_B[6]);
                    Y_B = (A_B[0] + 2 * A_B[1] + A_B[2]) - (A_B[6] + 2 * A_B[5] + A_B[4]);
                    G_B = Math.Sqrt(X_B * X_B + Y_B * Y_B);

                    if (G_R > porog || G_G > porog || G_B > porog)
                    {
                        Image_new.SetPixel(i, j, Color.White);
                    }
                }
            }
            Form2 form_Result = new Form2();
            form_Result.pictureBox1.Image = new Bitmap(Image_new);
            form_Result.Show();
            toolStripProgressBar1.Value = 0;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i<256; i++)
            {
                masR[i] = masG[i] = masB[i] = 0;
            }
            int R, G, B;
            Color color;
            toolStripProgressBar1.Maximum = X2 - X1;

            for (int i=X1; i<X2; i++)
            {
                toolStripProgressBar1.Value = i - X1;
                for (int j = Y1; j < Y2; j++)
                {
                    color = Image_mem.GetPixel(i, j);
                    R = color.R; G = color.G; B = color.B;
                    masR[R]++;
                    masG[G]++;
                    masB[B]++;
                }
            }
        }

        private void киршаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Brush MyBrush = new SolidBrush(Color.White);
            Graphics gr = Graphics.FromImage(Image_new);
            gr.FillRectangle(MyBrush, 0, 0, Image_new.Width, Image_new.Height);
            double porog = double.Parse(numericUpDown2.Text);

            double G_R, G_G, G_B, max = 0, S_R, S_G, S_B, T_R, T_G, T_B;

            Color[] colorArray = new Color[8];

            colorArray[0] = Color.FromArgb(255, 100, 56, 40);
            colorArray[1] = Color.FromArgb(255, 98, 20, 60);
            colorArray[2] = Color.FromArgb(255, 90, 65, 10);
            colorArray[3] = Color.FromArgb(255, 80, 70, 40);
            colorArray[4] = Color.FromArgb(255, 39, 85, 89);
            colorArray[5] = Color.FromArgb(255, 45, 68, 96);
            colorArray[6] = Color.FromArgb(255, 50, 68, 45);
            colorArray[7] = Color.FromArgb(255, 60, 40, 55);

            int[] A_R = new int[8];
            int[] A_G = new int[8];
            int[] A_B = new int[8];
            int i_max = -1;
            toolStripProgressBar1.Maximum = X2 - X1;

            for (int i = X1 + 1; i < X2 - 1; i++)
            {

                toolStripProgressBar1.Value = i - X1;
                for (int j = Y1 + 1; j < Y2 - 1; j++)
                {

                    Color col = Image_mem.GetPixel(i + 1, j + 1);
                    A_R[0] = col.R;
                    A_G[0] = col.G;
                    A_B[0] = col.B;

                    col = Image_mem.GetPixel(i, j - 1);
                    A_R[1] = col.R;
                    A_G[1] = col.G;
                    A_B[1] = col.B;

                    col = Image_mem.GetPixel(i + 1, j - 1);
                    A_R[2] = col.R;
                    A_G[2] = col.G;
                    A_B[2] = col.B;

                    col = Image_mem.GetPixel(i + 1, j);
                    A_R[3] = col.R;
                    A_G[3] = col.G;
                    A_B[3] = col.B;

                    col = Image_mem.GetPixel(i + 1, j + 1);
                    A_R[4] = col.R;
                    A_G[4] = col.G;
                    A_B[4] = col.B;

                    col = Image_mem.GetPixel(i, j + 1);
                    A_R[5] = col.R;
                    A_G[5] = col.G;
                    A_B[5] = col.B;

                    col = Image_mem.GetPixel(i - 1, j + 1);
                    A_R[6] = col.R;
                    A_G[6] = col.G;
                    A_B[6] = col.B;

                    col = Image_mem.GetPixel(i - 1, j);
                    A_R[7] = col.R;
                    A_G[7] = col.G;
                    A_B[7] = col.B;

                    max = 0;
                    i_max = 0;
                    for (int i1 = 0; i1 < 8; i1++)
                    {
                        S_R = A_R[i1] + A_R[(i1 + 1) % 8] + A_R[(i1 + 2) % 8];
                        T_R = A_R[(i1 + 3) % 8] + A_R[(i1 + 4) % 8] + A_R[(i1 + 5) % 8] + A_R[(i1 + 6) % 8] + A_R[(i1 + 7) % 8];
                        G_R = Math.Abs(5 * S_R - 3 * T_R);
                        if (max < G_R)
                            max = G_R;
                        i_max = i1;


                        S_G = A_G[i1] + A_G[(i1 + 1) % 8] + A_G[(i1 + 2) % 8];
                        T_G = A_G[(i1 + 3) % 8] + A_G[(i1 + 4) % 8] + A_G[(i1 + 5) % 8] + A_G[(i1 + 6) % 8] + A_G[(i1 + 7) % 8];
                        G_G = Math.Abs(5 * S_G - 3 * T_G);
                        if (max < G_G)
                            max = G_G;
                        i_max = i1;

                        S_B = A_B[i1] + A_B[(i1 + 1) % 8] + A_B[(i1 + 2) % 8];
                        T_B = A_B[(i1 + 3) % 8] + A_B[(i1 + 4) % 8] + A_B[(i1 + 5) % 8] + A_B[(i1 + 6) % 8] + A_B[(i1 + 7) % 8];
                        G_B = Math.Abs(5 * S_B - 3 * T_B);
                        if (max < G_B)
                            max = G_B;
                        i_max = i1;
                    }
                    if (max > porog)
                        Image_new.SetPixel(i, j, colorArray[i_max]);
                }
            }
            Form2 form_Results = new Form2();
            form_Results.pictureBox1.Image = new Bitmap(Image_new);
            form_Results.Show();
            toolStripProgressBar1.Value = 0;
        }

        private void уоллисаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Brush MyBrush = new SolidBrush(Color.Black);
            Graphics gr = Graphics.FromImage(Image_new);
            gr.FillRectangle(MyBrush, 0, 0, Image_new.Width, Image_new.Height);

            double porog = double.Parse(numericUpDown2.Text);
            double G_R, G_G, G_B;
            int A_R1, A_G1, A_B1, A_R3, A_G3, A_B3, A_R5, A_G5, A_B5, A_R7, A_G7, A_B7;
            double A_R0, A_G0, A_B0;

            toolStripProgressBar1.Maximum = X2 - X1;

            for (int i = X1; i < X2; i++)
            {

                toolStripProgressBar1.Value = i - X1;

                for (int j = Y1; j < Y2; j++)
                {
                    Color col = Image_mem.GetPixel(i, j);

                    A_R0 = col.R;
                    if (A_R0 == 0) A_R0 = 1;

                    A_G0 = col.G;
                    if (A_G0 == 0) A_G0 = 1;


                    A_B0 = col.B;
                    if (A_B0 == 0) A_B0 = 1;

                    col = Image_mem.GetPixel(i, j - 1);
                    A_R1 = col.R;
                    if (A_R1 == 0) A_R1 = 1;

                    A_G1 = col.G;
                    if (A_G1 == 0)
                        A_G1 = 1;

                    A_B1 = col.B;
                    if (A_B1 == 0)
                        A_B1 = 1;

                    col = Image_mem.GetPixel(i + 1, j);
                    A_R3 = col.R;
                    if (A_R3 == 0)
                        A_R3 = 1;

                    A_G3 = col.G;
                    if (A_G3 == 0)
                        A_G3 = 1;

                    A_B3 = col.B;
                    if (A_B3 == 0)
                        A_B3 = 1;


                    col = Image_mem.GetPixel(i, j + 1);
                    A_R5 = col.R;

                    if (A_R5 == 0)
                        A_R5 = 1;

                    A_G5 = col.G;
                    if (A_G5 == 0)
                        A_G5 = 1;

                    A_B5 = col.B;
                    if (A_B5 == 0)
                        A_B5 = 1;

                    col = Image_mem.GetPixel(i - 1, j);
                    A_R7 = col.R;

                    if (A_R7 == 0)
                        A_R7 = 1;

                    A_G7 = col.G;
                    if (A_G7 == 0)
                        A_G7 = 1;

                    A_B7 = col.B;
                    if (A_B7 == 0)
                        A_B7 = 1;

                    G_R = (Math.Log10(Math.Pow(A_R0, 4) / (A_R1 * A_R3 * A_R5 * A_R7)));
                    G_G = (Math.Log10(Math.Pow(A_G0, 4) / (A_G1 * A_G3 * A_G5 * A_G7)));
                    G_B = (Math.Log10(Math.Pow(A_B0, 4) / (A_B1 * A_B3 * A_B5 * A_B7)));

                    if (G_R > porog || G_G > porog || G_B > porog) Image_new.SetPixel(i, j, Color.White);
                }
            }

            Form2 form_Results = new Form2();
            form_Results.pictureBox1.Image = new Bitmap(Image_new);
            form_Results.Show();
            toolStripProgressBar1.Value = 0;
        }

        private void changeBrightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 windowResult = new Form2();
            windowResult.Text = "Brighness Changed";
            toolStripProgressBar1.Maximum = Image_mem.Width;
            int tolerance = Convert.ToInt32(trackBar1.Value);
            for (int i = 0; i < Image_mem.Width; i++)
            {
                toolStripProgressBar1.Value = i;
                for (int j = 0; j < Image_mem.Height; j++)
                {
                    Color color = Image_mem.GetPixel(i, j);
                    byte r = color.R;
                    byte g = color.G;
                    byte b = color.B;
                    if (r + tolerance > 255) { r = 255; } else { r += (byte)tolerance; }
                    if (g + tolerance > 255) { g = 255; } else { g += (byte)tolerance; }
                    if (b + tolerance > 255) { b = 255; } else { b += (byte)tolerance; }
                    Image_mem.SetPixel(i, j, Color.FromArgb(color.A, r, g, b));
                }
            }
            toolStripProgressBar1.Value = 0;
            windowResult.pictureBox1.Image = new Bitmap(Image_mem);
            windowResult.Show();
        }

        private void contrastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 windowResult = new Form2();
            windowResult.Text = "Contrast Changed";
            toolStripProgressBar1.Maximum = Image_mem.Width;
            int tolerance = Convert.ToInt32(trackBar1.Value);
            byte rmax = 0, rmin = 255, gmax = 0, gmin = 255, bmax = 0, bmin = 255, r, g, b;
            for (int i = 0; i < Image_mem.Width; i++)
            {
                toolStripProgressBar1.Value = i;
                for (int j = 0; j < Image_mem.Height; j++)
                {
                    Color color = Image_mem.GetPixel(i, j);
                    r = color.R;
                    g = color.G;
                    b = color.B;
                    if (r > rmax) { rmax = r; }
                    if (r < rmin) { rmin = r; }
                    if (g > gmax) { gmax = g; }
                    if (g < gmin) { gmin = g; }
                    if (b > bmax) { bmax = b; }
                    if (b < bmin) { bmin = b; }
                }
            }

            int scaleR = 255 / (rmax - rmin);
            int scaleG = 255 / (gmax - gmin);
            int scaleB = 255 / (bmax - bmin);

            for (int i = 0; i < Image_mem.Width; i++)
            {
                for (int j = 0; j < Image_mem.Height; j++)
                {
                    Color color = Image_mem.GetPixel(i, j);
                    r = color.R;
                    g = color.G;
                    b = color.B;
                    Image_mem.SetPixel(i, j, Color.FromArgb(color.A, (r - rmin) * scaleR, (g - gmin) * scaleG, (b - bmin) * scaleB));
                }
            }

            windowResult.Text = Convert.ToString(rmax) + "-rmax " + Convert.ToString(rmin) + "-rmin " + Convert.ToString(gmax) + "-gmax " + Convert.ToString(gmin) + "-gmin " + Convert.ToString(bmax) + "-bmax " + Convert.ToString(bmin) + "-bmin ";
            toolStripProgressBar1.Value = 0;
            windowResult.pictureBox1.Image = new Bitmap(Image_mem);
            windowResult.Show();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (radioButton1.Checked)
            {
                X1 = e.X; Y1 = e.Y; X2 = e.X; Y2 = e.Y;
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            trackbarlabel.Text = Convert.ToString(trackBar1.Value);
        }

        private void spectrumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 windowR = new Form2();
            Form2 windowG = new Form2();
            Form2 windowB = new Form2();
            windowR.Text = "RED";
            windowG.Text = "GREEN";
            windowB.Text = "BLUE";
            toolStripProgressBar1.Maximum = Image_mem.Width;
            for (int i = 0; i < Image_mem.Width; i++)
            {
                toolStripProgressBar1.Value = i;
                for (int j = 0; j < Image_mem.Height; j++)
                {
                    Color color = Image_mem.GetPixel(i, j);
                    ImageR.SetPixel(i, j, Color.FromArgb(255, color.R, color.R, color.R));
                    ImageG.SetPixel(i, j, Color.FromArgb(255, color.G, color.G, color.G));
                    ImageB.SetPixel(i, j, Color.FromArgb(255, color.B, color.B, color.B));
                }
            }
            toolStripProgressBar1.Value = 0;
            windowR.pictureBox1.Image = new Bitmap(ImageR);
            windowG.pictureBox1.Image = new Bitmap(ImageG);
            windowB.pictureBox1.Image = new Bitmap(ImageB);
            windowR.Show();
            windowG.Show();
            windowB.Show();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image_mem = new Bitmap(openFileDialog1.FileName);
                Image_new = new Bitmap(openFileDialog1.FileName);
                ImageR = new Bitmap(openFileDialog1.FileName);
                ImageG = new Bitmap(openFileDialog1.FileName);
                ImageB = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
