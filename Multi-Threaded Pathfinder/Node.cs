namespace Multi_Threaded_Pathfinder
{
    public class Node
    {
        public int[] Position { get; set; }
        public Node Parent { get; set; }
        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost => GCost + HCost;

        public Node(int[] position, Node parent, int gCost, int hCost)
        {
            Position = position;
            Parent = parent;
            GCost = gCost;
            HCost = hCost;
        }
    }
}
