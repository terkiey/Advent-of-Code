using Google.OrTools.ConstraintSolver;
using Google.OrTools.LinearSolver;
using Constraint = Google.OrTools.LinearSolver.Constraint;
using Solver = Google.OrTools.LinearSolver.Solver;

namespace AoC.Days;

/* This solver will just use linear programming to solve the problem, wish I realised this earlier...
 */
internal class JoltageSequenceSolverLP : IJoltageSequenceSolver
{
    public JoltageSequenceSolverLP() { }

    public int FewestButtonPresses(MachineManual machineManual)
    {
        Solver solver = Solver.CreateSolver("SCIP");

        int buttonCount = machineManual.Buttons.Count();
        int joltageCount = machineManual.Joltages.Count();

        int maxJoltage = machineManual.Joltages.Max();

        Variable[] variables = new Variable[buttonCount];
        

        for (int variableIndex = 0; variableIndex < buttonCount; variableIndex++)
        {
            variables[variableIndex] = solver.MakeIntVar(0, maxJoltage, $"variable{variableIndex}");
        }

        for (int joltageIndex = 0; joltageIndex < joltageCount; joltageIndex++)
        {
            int[] coefficients = new int[variables.Count()];
            for (int buttonIndex = 0; buttonIndex < buttonCount; buttonIndex++)
            {
                int[] button = machineManual.Buttons[buttonIndex];
                if (button.Contains(joltageIndex))
                {
                    coefficients[buttonIndex] = 1;
                }
            }

            int constraintJoltage = machineManual.Joltages[joltageIndex];

            LinearExpr expression = new();
            for (int index = 0; index < coefficients.Length; index++)
            {
                expression += coefficients[index] * variables[index];
            }

            solver.Add(expression == constraintJoltage);
        }

        Objective objective = solver.Objective();
        for (int variableIndex = 0; variableIndex < variables.Length; variableIndex++)
        {
            objective.SetCoefficient(variables[variableIndex], 1);
        }
        objective.SetMinimization();

        solver.Solve();
        return (int)solver.Objective().Value();
    }
}
