namespace AoC.Days;

internal class SleighNavigationMemoryTreeBuilder
{
    // State
    private SleighNavigationMemoryTreeNode RootNode { get; set; } = new(0, 0);
    
    // Building State
    private string[] LicenseFile { get; }
    private int NumberPointer = 0;

    public SleighNavigationMemoryTreeBuilder(string licenseFile)
    {
        LicenseFile = licenseFile.Split(' ');
    }

    public void BuildTree()
    {
        SleighNavigationMemoryTreeNode root = new(int.Parse(LicenseFile[NumberPointer++]), int.Parse(LicenseFile[NumberPointer++]));
        int childCount = root.Children.Length;
        int metadataCount = root.Metadata.Length;
        for (int childIndex = 0; childIndex < childCount; childIndex++)
        {
            SleighNavigationMemoryTreeNode child = ParseNode();
            root.Children[childIndex] = child;
        }

        for (int metadataIndex = 0; metadataIndex < metadataCount; metadataIndex++)
        {
            root.Metadata[metadataIndex] = int.Parse(LicenseFile[NumberPointer++]);
        }
        RootNode = root;
    }

    public int SumRootMetadata() => SumNodeMetadata(RootNode);
    public int GetRootValue() => GetNodeValue(RootNode);

    private int SumNodeMetadata(SleighNavigationMemoryTreeNode node)
    {
        int sum = 0;
        sum += node.Metadata.Sum();
        foreach (var child in node.Children)
        {
            sum += SumNodeMetadata(child);
        }
        return sum;
    }

    private int GetNodeValue(SleighNavigationMemoryTreeNode node)
    {
        int value = 0;
        if (node.Children.Length == 0)
        {
            value = node.Metadata.Sum();
        }
        else
        {
            foreach (var metadata in node.Metadata)
            {
                if (metadata > 0 && metadata <= node.Children.Length)
                {
                    value += GetNodeValue(node.Children[metadata - 1]);
                }
            }
        }
        return value;
    }

    private SleighNavigationMemoryTreeNode ParseNode()
    {
        SleighNavigationMemoryTreeNode node = new(int.Parse(LicenseFile[NumberPointer++]), int.Parse(LicenseFile[NumberPointer++]));
        int childCount = node.Children.Length;
        int metadataCount = node.Metadata.Length;
        for (int childIndex = 0; childIndex < childCount; childIndex++)
        {
            SleighNavigationMemoryTreeNode child = ParseNode();
            node.Children[childIndex] = child;
        }

        for (int metadataIndex = 0; metadataIndex < metadataCount; metadataIndex++)
        {
            node.Metadata[metadataIndex] = int.Parse(LicenseFile[NumberPointer++]);
        }
        return node;
    }
}
