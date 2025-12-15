namespace AoC.Days;

internal interface IEquationBuilder
{
    List<Equation> BuildEquations(string[] input);
    List<Equation> BuildCephalopodEquations(string[] input);
}
