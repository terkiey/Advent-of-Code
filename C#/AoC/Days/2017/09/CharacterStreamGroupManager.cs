using System.Collections.Generic;
using System.Reflection;

namespace AoC.Days;

internal class CharacterStreamGroupManager
{
    // Final Output
    private List<StreamGroup> Groups { get; } = [];
    private HashSet<StreamGroup> GroupSet { get; } = [];

    public int GarbageCharacters = 0;

    // Trackers (while building)
    private int NextId { get; set; } = 0;
    private int Pointer { get; set; } = 0;
    private int BracketCounter { get; set; } = 0;
    private Stack<List<StreamGroup>> GroupListsByNesting { get; } = [];

    public void ParseStream(string characterStream)
    {
        GarbageCharacters = 0;
        Pointer = 1;
        BracketCounter = 1;
        GroupListsByNesting.Push([]);
        GroupListsByNesting.Push([]);

        while (BracketCounter > 0)
        {
            char pointerChar = characterStream[Pointer];
            if (pointerChar == '{')
            {
                BracketCounter++;
                GroupListsByNesting.Push([]);
            }
            else if (pointerChar == '}')
            {
                BracketCounter--;
                StreamGroup streamGroup = RegisterNewGroup();
                List<StreamGroup> childGroups = GroupListsByNesting.Pop();
                List<StreamGroup> parentChildList = GroupListsByNesting.Peek();
                parentChildList.Add(streamGroup);
                foreach (StreamGroup group in childGroups)
                {
                    streamGroup.AddChild(group);
                }
            }
            else if (pointerChar == '<')
            {
                int garbagePointer = Pointer + 1;
                bool inGarbage = true;
                while (inGarbage)
                {
                    char garbageChar = characterStream[garbagePointer];
                    if (garbageChar == '>')
                    {
                        Pointer = garbagePointer;
                        break;
                    }
                    else if (garbageChar == '!')
                    {
                        garbagePointer++;
                    }
                    else
                    {
                        GarbageCharacters++;
                    }
                    garbagePointer++;
                }
            }
            Pointer++;
        }
    }

    public int TotalScore()
    {
        int score = 0;
        foreach (StreamGroup group in Groups)
        {
            score += group.Score;
        }
        return score;
    }

    private StreamGroup RegisterNewGroup()
    {
        StreamGroup group = new(NextId++);
        if (GroupSet.Add(group))
        {
            Groups.Add(group);
        }
        return group;
    }
}
