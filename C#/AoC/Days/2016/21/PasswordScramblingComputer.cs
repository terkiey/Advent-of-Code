using System.Runtime.InteropServices;

namespace AoC.Days;

internal class PasswordScramblingComputer
{
    char[] ScrambledPassword { get; set; } = [];

    public string ScrambleFromInstructions(char[] password, string[] instructions)
    {
        ScrambledPassword = [.. password];
        PasswordScrambleCommand[] commands = ParseInstructions(instructions);

        foreach (PasswordScrambleCommand command in commands)
        {
            ExecuteCommand(command);
        }
        return new(ScrambledPassword);
    }

    public string UnscrambleFromInstructions(char[] scrambledPassword, string[] instructions)
    {
        ScrambledPassword = scrambledPassword;
        PasswordScrambleCommand[] commands = ParseInstructions(instructions);

        for (int commandIndex = commands.Length - 1;  commandIndex >= 0; commandIndex--)
        {
            PasswordScrambleCommand invertedCommand = InvertCommand(commands[commandIndex]);
            ExecuteCommand(invertedCommand);
        }
        return new(ScrambledPassword);
    }

    private PasswordScrambleCommand InvertCommand(PasswordScrambleCommand command)
    {
        if (command.Verb == "swap")
        {
            return command;
        }

        if (command.Verb == "reverse")
        {
            return command;
        }

        if (command.Verb == "move")
        {
            string y = command.X;
            string x = command.Y;
            return new("move", x, y, string.Empty);
        }

        if (command.RotateType == "right")
        {
            return new("rotate", command.X, command.Y, "left");
        }

        if (command.RotateType == "left")
        {
            return new("rotate", command.X, command.Y, "right");
        }

        char letter = command.X[0];
        int rotationLandingSpot;
        for (rotationLandingSpot = 0; rotationLandingSpot < ScrambledPassword.Length; rotationLandingSpot++)
        {
            if (ScrambledPassword[rotationLandingSpot] == letter)
            {
                break;
            }
        }

        int rotatePowerFromIndex = -1;
        for (int originalIndexOfX = 0; originalIndexOfX < ScrambledPassword.Length; originalIndexOfX++)
        {
            rotatePowerFromIndex = 1 + originalIndexOfX;
            rotatePowerFromIndex += originalIndexOfX >= 4 ? 1 : 0;
            if ((originalIndexOfX + rotatePowerFromIndex) % ScrambledPassword.Length == rotationLandingSpot)
            {
                break;
            }
        }

        return new("rotate", rotatePowerFromIndex.ToString(), string.Empty, "left");
    }

    private PasswordScrambleCommand[] ParseInstructions(string[] instructions)
    {
        PasswordScrambleCommand[] commands = new PasswordScrambleCommand[instructions.Length];
        int commandIndex = 0;
        foreach (string instruction in instructions)
        {
            PasswordScrambleCommand command = ParseInstructionToCommand(instruction);
            commands[commandIndex++] = command;
        }
        return commands;
    }

    private PasswordScrambleCommand ParseInstructionToCommand(string instruction)
    {
        string[] splitInstruction = instruction.Split(' ');
        string verb = splitInstruction[0];
        string x;
        string y = string.Empty;
        string rotateType = string.Empty;
        if (verb == "swap" || verb == "reverse" || verb == "move")
        {
            x = splitInstruction[2];
            y = splitInstruction[^1];
        }
        else if (splitInstruction[1] == "based")
        {
            x = splitInstruction[^1];
        }
        else
        {
            x = splitInstruction[2];
            rotateType = splitInstruction[1];
        }
        return new(verb, x, y, rotateType);
    }

    private void ExecuteCommand(PasswordScrambleCommand command)
    {
        switch (command.Verb)
        {
            case "swap":
                ExecuteSwapCommand(command.X, command.Y);
                break;

            case "reverse":
                ExecuteReverseCommand(command.X, command.Y);
                break;

            case "move":
                ExecuteMoveCommand(command.X, command.Y);
                break;

            case "rotate":
                ExecuteRotateCommand(command.X, command.RotateType);
                break;

            default:
                throw new Exception("Unidientified command verb");
        }
    }

    private void ExecuteSwapCommand(string x, string y)
    {
        int xIndex;
        if (!int.TryParse(x, out xIndex))
        {
            for (xIndex = 0; xIndex < ScrambledPassword.Length; xIndex++)
            {
                if (ScrambledPassword[xIndex] == x[0])
                {
                    break;
                }
            }
        }

        int yIndex;
        if (!int.TryParse(y, out yIndex))
        {
            for (yIndex = 0; yIndex < ScrambledPassword.Length; yIndex++)
            {
                if (ScrambledPassword[yIndex] == y[0])
                {
                    break;
                }
            }
        }

        char temp = ScrambledPassword[xIndex];
        ScrambledPassword[xIndex] = ScrambledPassword[yIndex];
        ScrambledPassword[yIndex] = temp;
    }

    private void ExecuteReverseCommand(string x, string y)
    {
        int xInt = int.Parse(x);
        int yInt = int.Parse(y);

        ScrambledPassword.AsSpan(xInt, yInt - xInt + 1).Reverse();
    }

    private void ExecuteMoveCommand(string x, string y)
    {
        List<char> charList = [.. ScrambledPassword];
        int xInt = int.Parse(x);
        int yInt = int.Parse(y);
        char xChar = ScrambledPassword[xInt];
        charList.RemoveAt(xInt);
        charList.Insert(yInt, xChar);
        ScrambledPassword = [.. charList];
    }

    private void ExecuteRotateCommand(string x, string rotateType)
    {
        switch (rotateType)
        {
            case "left":
                Rotate(int.Parse(x) * -1);
                break;

            case "right":
                Rotate(int.Parse(x));
                break;

            default:
                RotateBased(x[0]);
                break;
        }
    }

    private void RotateBased(char x)
    {
        int xIndex;
        for (xIndex = 0; xIndex < ScrambledPassword.Length; xIndex++)
        {
            if (ScrambledPassword[xIndex] == x)
            {
                break;
            }
        }
        int rotationPower = 1 + xIndex;
        rotationPower += xIndex >= 4 ? 1 : 0;
        Rotate(rotationPower);
    }

    private void Rotate(int xInt)
    {
        char[] newPassword = new char[ScrambledPassword.Length];
        for (int charIndex = 0; charIndex < ScrambledPassword.Length; charIndex++)
        {
            int oldIndex = (charIndex - xInt + (ScrambledPassword.Length * 2))  % ScrambledPassword.Length;
            newPassword[charIndex] = ScrambledPassword[oldIndex];
        }
        ScrambledPassword = newPassword;
    }
}
