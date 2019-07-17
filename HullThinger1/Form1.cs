using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HullThinger1
{
    public partial class Form1 : Form
    {
        private Graphics g;
        private Graphics screen;
        private Graphics buff;
        private Bitmap b1;
        private Bitmap b2;
        private Timer t;
        private List<Point> points;
        private int W, H;
        private int pointCount = 50;
        private Random r;
        private int idxLeftMost = 0;
        private List<Point> hull;
        private int idx = 0;
        private int nxtIdx = 1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            W = pictureBox1.Width;
            H = pictureBox1.Height;
            hull = new List<Point>();
            r = new Random();

            //angleBetweenToVect(new Point(-6, 0), new Point(-15, -7));

            screen = pictureBox1.CreateGraphics();
            //b1 = new Bitmap(W, H);
            //g = Graphics.FromImage(b1);
            //b2 = new Bitmap(W, H);
            //buff = Graphics.FromImage(b2);

            pickPoints();

            t = new Timer();
            t.Interval = 100;
            t.Tick += T_Tick;
            t.Enabled = true;
        }

        private void T_Tick(object sender, EventArgs e)
        {

            Draw();
        }

        private void Draw()
        {
            g.FillRectangle(new SolidBrush(Color.FromArgb(30,30,30)), 0, 0, W*4,H*4);

            foreach(var p in points)
            {
                g.DrawEllipse(Pens.LightGray, p.X, p.Y, 3, 3);

            }
            foreach (var p in hull)
            {
                g.FillEllipse(Brushes.LimeGreen, p.X, p.Y, 10, 10);

            }

            var lowst = 90.0;
            var lowstIdx = 0;
            for(int a = 0; a < points.Count; a++) // (var p in points)
            {
                var cur = points[idx];
                var nxt = points[nxtIdx];
                var p = points[a];

                var aV = ptSub(nxt, cur);
                var bV = ptSub(p, cur);

                var cross = CrossProduct(aV, bV);

                if (cross < 0)
                {
                    nxtIdx = a;
                } else if (cross == 0)
                {
                    //nxtIdx = 1;
                }

                if (p.X != cur.X && p.Y != cur.Y)
                {
                    //buff.DrawLine(Pens.White, cur, p);
                    buff.DrawLine(new Pen(Color.FromArgb(20,r.Next(0,255),r.Next(0, 255),r.Next(0, 255)),3), cur, p);
                }

                //var thisAng = angleBetweenToVect(new Point(cur.X - 6, cur.Y), new Point(p.X - cur.X, p.Y - cur.Y));
                //if (thisAng < lowst)
                //{
                //    lowst = thisAng;
                //    lowstIdx = a;
                //}

                //buff.DrawString(angleBetweenToVect(new Point(cur.X-6,cur.Y), new Point(p.X-cur.X,p.Y-cur.Y)).ToString(), SystemFonts.DefaultFont, Brushes.Orange, p);
                //buff.DrawString(new Point(cur.X - 6, cur.Y).ToString(), SystemFonts.DefaultFont, Brushes.Orange, new Point(p.X,p.Y+10));
                //buff.DrawLine(Pens.Orange, cur, new Point(cur.X - 6, cur.Y));

            }
            if (idx == nxtIdx)
            {
                buff.DrawLine(new Pen(Color.Orange, 4), points[idx], hull[0]);
                t.Enabled = false;
            }
            else
            {
                buff.DrawLine(new Pen(Color.Orange, 4), points[idx], points[nxtIdx]);
            }
            idx = nxtIdx;
            hull.Add(points[nxtIdx]);
            nxtIdx = 1;
            if (idx < points.Count-1)
            {
                //idx++;
            }


            g.DrawImage(b2, 0, 0);
            screen.DrawImage(b1, 0, 0,W,H);
        }

        private float CrossProduct(Point v1,  Point v2)
        {
            return (v1.X* v2.Y) - (v1.Y* v2.X);
        }

        private Point ptSub(Point v1, Point v2)
        {
            var p = new Point(v1.X - v2.X, v1.Y - v2.Y);

            return p;
        }

    private double angleBetweenToVect(Point top, Point branch)
        {
            var d = 0.0;

            var dp = (top.X * branch.X) + (top.Y * branch.Y);

            var tpMag = Math.Sqrt((Math.Pow(top.X, 2) + Math.Pow(top.Y, 2)));
            var brMag = Math.Sqrt((Math.Pow(branch.X, 2) + Math.Pow(branch.Y, 2)));

            var bot = tpMag * brMag;

            var prod = dp / bot;

            d = prod;

            //d = Math.Acos(prod);

            return d;
        }

        

        private void pickPoints()
        {
            b1 = new Bitmap(W*4, H*4);
            g = Graphics.FromImage(b1);
            b2 = new Bitmap(W*4, H*4);
            buff = Graphics.FromImage(b2);
            points = new List<Point>();
            hull = new List<Point>();
            idx = 0;
            nxtIdx = 1;

            for (int a = 0; a < pointCount; a++)
            {
                points.Add(new Point(r.Next(30, W*4-30), r.Next(30, H*4-30)));
            }

            points = points.OrderBy(x => x.X).ToList();

            hull.Add(points[0]);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            pickPoints();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            b1.Save(Guid.NewGuid().ToString() + ".png", ImageFormat.Png);
        }

    }
}
