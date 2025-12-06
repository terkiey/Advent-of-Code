namespace AoC.Days;

internal class EquationBuilder : IEquationBuilder
{
    public EquationBuilder() { }

    public List<Equation> BuildEquations(string[] inputRows)
    {
        int rowCount = inputRows.Count();
        int colCount = 0;
        string[][] inputGrid  = new string[inputRows.Length][];
        for (int rowIndex = 0; rowIndex < inputRows.Length; rowIndex++)
        {
            string row = inputRows[rowIndex];
            string[] rowItems = ParseRow(row);
            colCount = rowItems.Count();
            inputGrid[rowIndex] = rowItems;
        }

        List<Equation> equations = [];
        for (int colIndex = 0; colIndex < colCount; colIndex++)
        {
            List<ushort> equationNumbers = [];
            char equationChar = 'A';
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++ )
            {
                if (rowIndex == rowCount - 1)
                {
                    equationChar = inputGrid[rowIndex][colIndex][0];
                    continue;
                }

                ushort number = ushort.Parse(inputGrid[rowIndex][colIndex]);
                equationNumbers.Add(ushort.Parse(inputGrid[rowIndex][colIndex]));
            }

            Equation equation = new(equationNumbers, equationChar);
            equations.Add(equation);
        }

        return equations;
    }

    public List<Equation> BuildCephalopodEquations(string[] inputRows)
    {
        // operator is always left-aligned, make a list of indices for operators
        int rowCount = inputRows.Count();
        string lastRow = inputRows[rowCount - 1];

        List<int> operatorIndices = [];
        for (int charIndex = 0; charIndex < lastRow.Length; charIndex++)
        {
            if (lastRow[charIndex] != ' ')
            {
                operatorIndices.Add(charIndex);
            }
        }

        List<List<int>> operatorIndexPairs = [];
        // For each operator, find the next operator to separate equations.
        for (int operatorNum = 0;  operatorNum < operatorIndices.Count; operatorNum++)
        {
            List<int> operatorIndexPair = [];
            operatorIndexPair.Add(operatorIndices[operatorNum]);
            operatorIndexPair.Add(GetNextOperatorIndex(operatorNum, operatorIndices));
            operatorIndexPairs.Add(operatorIndexPair);
        }

        List<Equation> equations = [];
        foreach (List<int> operatorIndexPair in operatorIndexPairs)
        {
            Equation equation = BuildCephalopodEquation(operatorIndexPair, inputRows);
            equations.Add(equation);
        }

        return equations;
    }

    private string[] ParseRow(string row)
    {
        return row.Split(" ").Where(rowItem => rowItem != "").ToArray();
    }

    private int GetNextOperatorIndex(int operatorNum, List<int> operatorIndices)
    {
        int nextOperatorIndex;
        try
        {
            nextOperatorIndex = operatorIndices[operatorNum + 1];
        }
        catch
        {
            nextOperatorIndex = -1;
        }

        return nextOperatorIndex;
    }

    private Equation BuildCephalopodEquation(List<int> operatorIndexPair, string[] inputRows)
    {
        int maxIndex = inputRows[0].Length - 1;

        int ParseEndIndex = operatorIndexPair[0];
        int ParseStartIndex = operatorIndexPair[1] == -1 ? maxIndex : operatorIndexPair[1] - 2;

        char operation = ' ';

        List<string> numberRowRawStrings = [];
        for (int rowIndex = 0; rowIndex < inputRows.Count(); rowIndex++)
        {
            string row = inputRows[rowIndex];
            if (rowIndex == inputRows.Count() - 1)
            {
                operation = row[ParseEndIndex];
                continue;
            }

            string numberRowRawString = "";
            for (int charIndex = ParseStartIndex; charIndex >= ParseEndIndex; charIndex--)
            {
                numberRowRawString += row[charIndex];
            }
            numberRowRawStrings.Add(numberRowRawString);
        }

        List<ushort> cephalopodNumbers = [];
        for (int colIndex = 0; colIndex < numberRowRawStrings[0].Length; colIndex++)
        {
            string cephalopodNumberRawString = "";
            foreach (string rowRawString in numberRowRawStrings)
            {
                cephalopodNumberRawString += rowRawString[colIndex];
            }

            ushort cephalopodNumber = ushort.Parse(cephalopodNumberRawString.Replace(" ", ""));
            cephalopodNumbers.Add(cephalopodNumber);
        }

        return new(cephalopodNumbers, operation);
    }
}
