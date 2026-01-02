using System.Text.RegularExpressions;

namespace AoC.Days;

internal class PipeGraphManager
{
    public List<HashSet<int>> ProgramsInGroupX { get; } = [];

    public void CreateGraph(string[] input)
    {
        int groupNum = 0;
        List<string> processList = input.ToList();
        
        while(processList.Count > 0)
        {
            int groupStarter = int.Parse(processList[0].Split(" <-> ")[0]);
            ProgramsInGroupX.Add([groupStarter]);
            HashSet<int> programGroup = ProgramsInGroupX[groupNum++];
            bool assignmentMade = true;
            while (assignmentMade)
            {
                assignmentMade = false;
                foreach (string programPipes in processList.ToList())
                {
                    string[] assignSplit = programPipes.Split(" <-> ");
                    int program = int.Parse(assignSplit[0]);

                    if (programGroup.Contains(program))
                    {
                        processList.Remove(programPipes);
                        string[] connectedTo = assignSplit[1].Split(", ");
                        foreach (string connectedProgram in connectedTo)
                        {
                            if (programGroup.Add(int.Parse(connectedProgram)))
                            {
                                assignmentMade = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
