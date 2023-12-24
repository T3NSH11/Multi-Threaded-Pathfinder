namespace Multi_Threaded_Pathfinder
{
    public class Grid
    {
        private char[,] grid;
        private int rows;
        private int cols;

        public Grid(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            this.grid = new char[rows, cols];
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    grid[i, j] = '.';
                }
            }
        }

        public void DrawGrid()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(grid[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public void DrawPath(int[] start, int[] end, List<int[]> path)
        {
            grid[start[0], start[1]] = 'S';
            grid[end[0], end[1]] = 'E';

            foreach (var step in path)
            {
                int row = step[0];
                int col = step[1];
                grid[row, col] = '*';
            }
        }
    }

}
