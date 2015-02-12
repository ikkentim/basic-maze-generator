using System.Linq;

namespace MazeGen
{
    /// <summary>
    /// Represents a dude running around the maze, trying to solve it.
    /// </summary>
    public class Dude
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Dude" /> class.
        /// </summary>
        public Dude()
        {
            Reset();
        }

        /// <summary>
        /// Gets or sets the Maze for this Dude to run around in.
        /// </summary>
        public Maze Maze { get; set; }

        public Direction Direction { get; private set; }
        public int Cell { get; private set; }

        public int CellOnLeft
        {
            get
            {
                if (Maze == null) return -1;
                switch (Direction)
                {
                    case Direction.Left:
                        return Cell/Maze.Width == Maze.Height - 1 ? -1 : Cell + Maze.Width;
                    case Direction.Right:
                        return Cell/Maze.Width == 0 ? -1 : Cell - Maze.Width;
                    case Direction.Up:
                        return Cell%Maze.Width == 0 ? -1 : Cell - 1;
                    case Direction.Down:
                        return Cell%Maze.Width == Maze.Width - 1 ? -1 : Cell + 1;
                    default:
                        return 0;
                }
            }
        }

        public int CellInFront
        {
            get
            {
                if (Maze == null) return -1;
                switch (Direction)
                {
                    case Direction.Left:
                        return Cell%Maze.Width == 0 ? -1 : Cell - 1;
                    case Direction.Right:
                        return Cell%Maze.Width == Maze.Width - 1 ? -1 : Cell + 1;
                    case Direction.Up:
                        return Cell/Maze.Width == 0 ? -1 : Cell - Maze.Width;
                    case Direction.Down:
                        return Cell/Maze.Width == Maze.Height - 1 ? -1 : Cell + Maze.Width;
                    default:
                        return 0;
                }
            }
        }

        public bool IsDone
        {
            get { return Maze == null || Cell == (Maze.Width*Maze.Height - 1); }
        }

        public void Reset()
        {
            Cell = 0;
            Direction = Direction.Right;
        }

        private bool IsValidCell(int cell)
        {
            return Maze != null && (cell >= 0 && cell < Maze.Width*Maze.Height);
        }

        private bool IsWallBetweenMeAnd(int cell)
        {
            return !IsValidCell(cell) || Maze.Edges.Any(e =>
                (e.Item1 == Cell && e.Item2 == cell) ||
                (e.Item2 == Cell && e.Item1 == cell));
        }

        private void MoveForward()
        {
            Cell = CellInFront;
        }

        private void RotateLeft()
        {
            Direction = (Direction) ((((int) Direction - 1) + 4)%4);
        }

        private void RotateRight()
        {
            Direction = (Direction) (((int) Direction + 1)%4);
        }

        public void Move()
        {
            if (IsDone) return;

            if (IsWallBetweenMeAnd(CellOnLeft))
            {
                if (IsWallBetweenMeAnd(CellInFront))
                    RotateRight();
                else
                    MoveForward();
            }
            else
            {
                RotateLeft();

                if (!IsWallBetweenMeAnd(CellInFront))
                    MoveForward();
            }
        }
    }
}