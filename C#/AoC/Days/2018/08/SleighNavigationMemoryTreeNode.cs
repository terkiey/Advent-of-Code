namespace AoC.Days;

internal class SleighNavigationMemoryTreeNode
{
    public SleighNavigationMemoryTreeNode[] Children { get; } = [];
    public int[] Metadata { get; set; } = [];

    public SleighNavigationMemoryTreeNode(int childrenCount, int metadataCount)
    {
        Children = new SleighNavigationMemoryTreeNode[childrenCount];
        Metadata = new int[metadataCount];
    }
}
