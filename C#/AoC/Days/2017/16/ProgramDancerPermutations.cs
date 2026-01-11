using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace AoC.Days;

public partial class ProgramDancerPermutations
{
    // int[] are Indexed from 0 to 15. (just add 'a' to get the char)
    // Imagine there is 16 boxes, that contain 'a', 'b', 'c', ... 'p'
    // Notice that exchange and spin just move the boxes around
    // Wheras partner looks in the boxes and moves the actual letters.
    // We could just figure out what all the partner moves are overall, and then apply that to the final boxes and it would work out the same.
    public int[] IndexState { get; private set; } = new int[16]; // Represents positional permutation from spins and exchanges to get the output.
    public int[] ProgramState { get; private set; } = new int[16]; // Represents program permutation from partners to get the output

    public DanceMoveStruct[] DanceMoves { get; private set; } = []; // Instructions list.
    private const int ProgramCount = 16;

    public ProgramDancerPermutations()
    {
        InitialiseArrays();
    }

    private void InitialiseArrays()
    {
        for (int i = 0; i < ProgramCount; i++)
        {
            ProgramState[i] = i;
            IndexState[i] = i;
        }
    }

    public void Dance(string danceInstructions)
    {
        InitialiseArrays();
        if (DanceMoves.Length == 0)
        {
            ParseInstructions(danceInstructions);
        }
        PerformDanceMoves();
    }

    public string GetProgramOrder()
    {
        // Grab the permutations.
        int[] indexPermutation = [.. IndexState];
        int[] programPermutation = [.. ProgramState];
        // Assume indices are in their original positions, compose indice permutation with the program permutation to get the overall permutation.
        // I.e. find which box we are looking at, then separately check what has been replaced into that box as a value.
        StringBuilder output = new();
        for (int index = 0; index < ProgramCount; index++) 
        {
            int boxIndex = indexPermutation[index];
            int programValue = programPermutation[boxIndex];
            char programChar = (char)(programValue + 'a');
            output.Append(programChar);
        }
        return output.ToString();
    }

    // We can quickly calculate permutation for 2^n dances by just combining our 2-perm with another 2-perm to get 4-perm for each type index and program perms...
    // Figure out which 2^n numbers are needed to make danceCount, then combine the required permutations for each 2^n included for index and program separately, combine at the end.
    // Given danceCount, can check with 2^x is needed by checking if that bit is 1, i.e. if ((danceCount >> x) & 1) == 1 then include 2^x.
    public void DanceMultipleTimes(string danceInstructions, int danceCount)
    {
        if (danceCount < 1) { return; }

        // Parse instructions if needed.
        if (DanceMoves.Length == 0) { ParseInstructions(danceInstructions); }

        // Initialise output to the identity permutation (to start off with) and compute the 1-dance permutation for indices and programs.
        // Identities (we will build on these).
        InitialiseArrays();
        int[] finalIndexState = [.. IndexState];
        int[] finalProgramState = [.. ProgramState];
        // 1-dance permutations
        PerformDanceMoves();
        int[] oneDanceIndicePermutation = [.. IndexState];
        int[] oneDanceProgramPermutation = [.. ProgramState];

        // Do exponential permutating by putting permutations together and adding together as the binary decomposition of danceCount dictates.
        int[] currentIndexPerm = [.. oneDanceIndicePermutation];
        int[] currentProgramPerm = [.. oneDanceProgramPermutation];

        int bit = 0;
        while ((1 << bit) <= danceCount)
        {
            if (((danceCount >> bit) & 1) == 1)
            {
                finalIndexState = ComposePermutations(finalIndexState, currentIndexPerm);
                finalProgramState = ComposePermutations(finalProgramState, currentProgramPerm);
            }

            // Square the permutation for the next bit
            currentIndexPerm = ComposePermutations(currentIndexPerm, currentIndexPerm);
            currentProgramPerm = ComposePermutations(currentProgramPerm, currentProgramPerm);

            bit++;
        }

        // Record the final permutations for indices and programs.
        IndexState = finalIndexState;
        ProgramState = finalProgramState;
    }

    private int[] ComposePermutations(int[] perm1, int[] perm2)
    {
        int[] composedPerm = new int[16];
        for (int index = 0; index < ProgramCount; index++)
        {
            composedPerm[index] = perm2[perm1[index]];
        }
        return composedPerm;
    }

    private void PerformDanceMoves()
    {
        for (int moveIndex = 0; moveIndex < DanceMoves.Length; moveIndex++)
        {
            ref var danceMove = ref DanceMoves[moveIndex];
            switch (danceMove.Type)
            {
                case MoveType.Spin:
                    DanceSpin(danceMove.Arg1);
                    break;
                case MoveType.Exchange:
                    DanceExchange(danceMove.Arg1, danceMove.Arg2);
                    break;
                case MoveType.Partner:
                    DancePartner(danceMove.Char1, danceMove.Char2);
                    break;
            }
        }
    }

    private void ParseInstructions(string danceInstructions)
    {
        int spinCommands = 0;
        int exchangeCommands = 0;
        int partnerCommands = 0;
        string[] splitInstructions = danceInstructions.Split(',');
        List<DanceMoveStruct> danceMoves = [];
        foreach (string instruction in splitInstructions)
        {
            
            switch (instruction[0])
            {
                case 's':
                    spinCommands++;
                    int spinSize = int.Parse(instruction[1..]);
                    danceMoves.Add(new(spinSize));
                    break;

                case 'x':
                    exchangeCommands++;
                    int slash = instruction.IndexOf('/');
                    int firstNum = int.Parse(instruction[1..slash]);
                    int secondNum = int.Parse(instruction[(slash+1)..]);
                    danceMoves.Add(new(firstNum, secondNum));
                    break;

                case 'p':
                    partnerCommands++;
                    char charOne = instruction[1];
                    char charTwo = instruction[3];
                    danceMoves.Add(new(charOne, charTwo));
                    break;
            }
        }
        DanceMoves = [.. danceMoves];

        //StringBuilder report = new();
        //report.AppendLine($"Spins: {spinCommands}");
        //report.AppendLine($"Exchanges: {exchangeCommands}");
        //report.AppendLine($"Partners: {partnerCommands}");
        //Console.WriteLine(report.ToString());
    }

    public void DanceSpin(int spinSize)
    {
        int[] newIndexState = new int[ProgramCount];
        for (int index = 0; index < ProgramCount; index++)
        {
            int postSpinIndex = index + spinSize;
            if (postSpinIndex >= ProgramCount)
            {
                postSpinIndex -= ProgramCount;
            }
            newIndexState[postSpinIndex] = IndexState[index];
        }
        IndexState = newIndexState;
    }

    public void DanceExchange(int indexOne, int indexTwo)
    {
        (IndexState[indexOne], IndexState[indexTwo]) = (IndexState[indexTwo], IndexState[indexOne]);
    }

    public void DancePartner(char charOne, char charTwo)
    {
        if (charOne == charTwo)
        {
            return;
        }

        int value1 = charOne - 'a';
        int value2 = charTwo - 'a';

        int index1 = -1;
        int index2 = -1;

        for (int i = 0; i < ProgramCount; i++)
        {
            if (ProgramState[i] == value1)
            {
                index1 = i;
            }
            else if (ProgramState[i] == value2)
            {
                index2 = i;
            }
        }

        (ProgramState[index1], ProgramState[index2]) = (ProgramState[index2], ProgramState[index1]);
    }
}
