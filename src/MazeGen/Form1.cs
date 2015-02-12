using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace MazeGen
{
    public sealed partial class Form1 : Form
    {
        private readonly Dude _dude = new Dude();
        private Maze _maze;

        private readonly Pen _pen = new Pen(Brushes.Black);
        private readonly Pen _dudePen = new Pen(Brushes.Red);
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        protected override void OnResize(EventArgs e)
        {
            Invalidate();
            base.OnResize(e);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            var sw = new Stopwatch();
            sw.Start();
            _dude.Maze = _maze = Maze.Generate(200, 200);
            sw.Stop();

            MessageBox.Show(this, string.Format("Took {0} to generate that maze!", sw.Elapsed), @"Generation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            _dude.Reset();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);

            if (_maze == null) return;

            var cellWidth = ClientSize.Width/_maze.Width;
            var cellHeight = ClientSize.Height/_maze.Height;

            if (cellWidth < 1) cellWidth = 1;
            if (cellHeight < 1) cellHeight = 1;

            e.Graphics.DrawRectangle(_pen, 0, 0, cellWidth*_maze.Width, cellHeight*_maze.Height);

            var dx = _dude.Cell%_maze.Width;
            var dy = _dude.Cell/_maze.Width;

            foreach (var edge in _maze.Edges)
            {
                var cell1 = Math.Min(edge.Item1, edge.Item2);
                var cell2 = Math.Max(edge.Item1, edge.Item2);

                var x = cell1%_maze.Width;
                var y = cell1/_maze.Width;
                if (cell1 + 1 == cell2) //horizontal
                {
                    e.Graphics.DrawLine(_pen, (x + 1)*cellWidth, y*cellHeight,
                        (x + 1)*cellWidth, (y + 1)*cellHeight);
                }
                else //vertical
                {
                    e.Graphics.DrawLine(_pen, x*cellWidth, (y + 1)*cellHeight,
                        (x + 1)*cellWidth, (y + 1)*cellHeight);
                }
            }

            e.Graphics.DrawPie(_dudePen, dx * cellWidth, dy * cellHeight, cellWidth, cellHeight, (int)_dude.Direction * 90, 360);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _dude.Move();
            Invalidate();
        }
    }
}