namespace Multi_Threaded_Pathfinder
{
    class Pathfinder
    {
        static void Main()
        {
            Console.Write("Enter the number of rows in the grid: ");
            int rows = int.Parse(Console.ReadLine());

            Console.Write("Enter the number of columns in the grid: ");
            int cols = int.Parse(Console.ReadLine());

            Grid grid = new Grid(rows, cols);

            Console.Write("Enter the start coordinates (row[Space]col): ");
            int[] start = Array.ConvertAll(Console.ReadLine().Split(), int.Parse);

            Console.Write("Enter the end coordinates (row[Space]col): ");
            int[] end = Array.ConvertAll(Console.ReadLine().Split(), int.Parse);

            List<int[]> path = FindPath(start, end, grid);

            grid.DrawPath(start, end, path);
        }

        static List<int[]> FindPath(int[] start, int[] end, Grid grid)
        {
            List<int[]> path = new List<int[]>();

            Thread pathfindingThread = new Thread(() =>
            {
                path = AStar(start, end, grid);
            });

            Thread drawingThread = new Thread(() =>
            {
                pathfindingThread.Join();
                grid.DrawPath(start, end, path);
                grid.DrawGrid();
            });

            pathfindingThread.Start();
            drawingThread.Start();

            drawingThread.Join();

            return path;
        }

        static List<int[]> AStar(int[] start, int[] end, Grid grid)
        {
            List<int[]> path = new List<int[]>();

            object lockObject = new object();

            Node startNode = new Node(start, null, 0, CalculateHCost(start, end));

            HashSet<Node> openSet = new HashSet<Node> { startNode };
            HashSet<Node> closedSet = new HashSet<Node>();

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.OrderBy(node => node.FCost).First();

                if (currentNode.Position.SequenceEqual(end))
                {
                    while (currentNode != null)
                    {
                        path.Insert(0, currentNode.Position);
                        currentNode = currentNode.Parent;
                    }
                    break;
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                Parallel.ForEach(GetNeighbors(currentNode, end, grid), neighbor =>
                {
                    if (closedSet.Contains(neighbor))
                        return;

                    int tentativeGCost = currentNode.GCost + 1;

                    lock (lockObject)
                    {
                        if (!openSet.Contains(neighbor) || tentativeGCost < neighbor.GCost)
                        {
                            neighbor.GCost = tentativeGCost;
                            neighbor.HCost = CalculateHCost(neighbor.Position, end);
                            neighbor.Parent = currentNode;

                            if (!openSet.Contains(neighbor))
                                openSet.Add(neighbor);
                        }
                    }
                });
            }

            return path;
        }

        static int CalculateHCost(int[] current, int[] end)
        {
            int dx = Math.Abs(current[0] - end[0]);
            int dy = Math.Abs(current[1] - end[1]);
            return (int)(Math.Sqrt(dx * dx + dy * dy) * 10);
        }

        static IEnumerable<Node> GetNeighbors(Node node, int[] end, Grid grid)
        {
            int[][] directions = { new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 0, -1 }, new int[] { -1, 0 } };

            foreach (var direction in directions)
            {
                int[] neighborPos = { node.Position[0] + direction[0], node.Position[1] + direction[1] };

                yield return new Node(neighborPos, node, 0, CalculateHCost(neighborPos, end));
            }
        }
    }
}
