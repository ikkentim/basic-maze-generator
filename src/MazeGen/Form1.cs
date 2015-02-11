using System;
using System.Drawing;
using System.Windows.Forms;

namespace MazeGen
{
    public partial class Form1 : Form
    {
        private readonly Dude _dude = new Dude();
        private Maze _maze;

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
            _dude.Maze = _maze = Maze.Generate(50, 50);
            _dude.Reset();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);

            if (_maze == null) return;

            var cellWidth = ClientSize.Width/_maze.Width;
            var cellHeight = ClientSize.Height/_maze.Height;

            var p = new Pen(Brushes.Black);
            e.Graphics.DrawRectangle(p, 0, 0, cellWidth*_maze.Width, cellHeight*_maze.Height);

            var dx = _dude.Cell%_maze.Width;
            var dy = _dude.Cell/_maze.Width;

            e.Graphics.DrawPie(p, dx*cellWidth, dy*cellHeight, cellWidth, cellHeight, (int) _dude.Direction*90, 360);

            foreach (var edge in _maze.Edges)
            {
                var cell1 = Math.Min(edge.Item1, edge.Item2);
                var cell2 = Math.Max(edge.Item1, edge.Item2);

                var x = cell1%_maze.Width;
                var y = cell1/_maze.Width;
                if (cell1 + 1 == cell2) //h
                {
                    e.Graphics.DrawLine(p, (x + 1)*cellWidth, y*cellHeight,
                        (x + 1)*cellWidth, (y + 1)*cellHeight);
                }
                else //v
                {
                    e.Graphics.DrawLine(p, x*cellWidth, (y + 1)*cellHeight,
                        (x + 1)*cellWidth, (y + 1)*cellHeight);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _dude.Move();
            Invalidate();
        }
    }
}