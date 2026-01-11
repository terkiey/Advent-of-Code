namespace AoC.Days;

internal class Y2017Day19 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        PacketTubeNavigator navigator = new();
        navigator.Navigate(inputLines);
        AnswerOne = navigator.LocationsOrder;
        AnswerTwo = navigator.StepsRequired;
    }
}
