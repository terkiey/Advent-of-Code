namespace AoC.Days;

internal class ElephantPartyElfNode
{
    public int ElfNumber { get; }
    public bool InGame { get; set; } = true;

    public ElephantPartyElfNode(int elfNumber)
    {
        ElfNumber = elfNumber;
    }
}
