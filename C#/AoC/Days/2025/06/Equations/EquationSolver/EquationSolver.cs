using System.ComponentModel.Design;
using System.Globalization;

namespace AoC.Days;

internal class EquationSolver : IEquationSolver
{
    public EquationSolver() { }

    public List<ulong> SolveEquations(List<Equation> equations)
    {
        List<ulong> solutions = [];
        foreach(Equation equation in equations)
        {
            List<ushort> equationNumbers = equation.numbers;
            char equationOperation = equation.operation;
            ulong solution = SolveEquation(equationNumbers, equationOperation);
            solutions.Add(solution);
        }

        return solutions;
    }

    private ulong SolveEquation(List<ushort> equationNumbers, char equationOperation)
    {
        ulong solution = equationNumbers[0];

        for (int numIndex = 1; numIndex < equationNumbers.Count; numIndex++)
        {
            if (equationOperation == '+')
            {
                solution += equationNumbers[numIndex];
                continue;
            }

            solution *= equationNumbers[numIndex];
        }

        return solution;
    }
}
