using Google.OrTools.ConstraintSolver;

namespace AoC.Days;

internal class TuringMachineInsideTheCpu
{
    private Dictionary<char, Action<TuringMachineInsideTheCpu>> StateRules = [];
    private HashSet<int> Ones = [];
    private int Pointer = 0;
    private char State;
    private int StepMax;

    private const int stateRuleDefinitionSize = 10;

    public void ParseBlueprint(string[] blueprint)
    {
        State = blueprint[0][^2];
        StepMax = int.Parse(blueprint[1].Split(' ')[^2]);

        int linePointer = 0;
        // Only continue to read if theres another staterule to read.
        while (blueprint.Length - linePointer > 9)
        {
            // move line pointer to the start of the next state rule.
            while (blueprint[linePointer].Length < 3 || blueprint[linePointer][..2] != "In")
            {
                linePointer++;
            }

            // Read the entire state rule, and write an equivalent function to the StateRules dictionary.
            char stateChar = blueprint[linePointer][^2];
            linePointer += 2;
            int[] write = new int[2];
            int[] addToPointer = new int[2];
            char[] goToState = new char[2];

            write[0] = int.Parse(blueprint[linePointer++][^3..^1]);
            string direction = blueprint[linePointer++].Split(' ')[^1];
            addToPointer[0] = direction == "right." ? 1 : -1;
            goToState[0] = blueprint[linePointer][^2];
            linePointer += 2;
            write[1] = int.Parse(blueprint[linePointer++][^3..^1]);
            direction = blueprint[linePointer++].Split(' ')[^1];
            addToPointer[1] = direction == "right." ? 1 : -1;
            goToState[1] = blueprint[linePointer][^2];

            StateRules[stateChar] = turing => turing.StateFunc(write, addToPointer, goToState);
        }
    }

    public void Run()
    {
        int stepCount = 0;
        while (stepCount++ < StepMax)
        {
            StateRules[State](this);
        }
    }

    public int CalculateCheckSum()
    {
        return Ones.Count;
    }

    private void StateFunc(int[] write, int[] addToPointer, char[] goToState)
    {
        int currentValue = Ones.Contains(Pointer) ? 1 : 0;
        if (write[currentValue] == 0)
        {
            Ones.Remove(Pointer);
        }
        else
        {
            Ones.Add(Pointer);
        }

        Pointer += addToPointer[currentValue];
        State = goToState[currentValue];
    }
}
