namespace AoC.Days;

internal class Y2015Day02 : Day
{
    protected override void RunLogic(string[] inputLines)
    {
        int wrappingPaperNeeded = 0;
        int ribbonNeeded = 0;
        foreach (string dimensions in inputLines)
        {
            string[] splitDimensions = dimensions.Split('x');
            int length = int.Parse(splitDimensions[0]);
            int width = int.Parse(splitDimensions[1]);
            int height = int.Parse(splitDimensions[2]);

            int side1 = length * width;
            int side2 = length * height;
            int side3 = width * height;

            wrappingPaperNeeded += (2 * (side1 + side2 + side3)) + new[] { side1, side2, side3 }.Min();

            int perimeter1 = 2 * (length + width);
            int perimeter2 = 2 * (length + height);
            int perimeter3 = 2 * (width + height);

            ribbonNeeded += (length * width * height) + new[] { perimeter1, perimeter2, perimeter3 }.Min();
        }
        AnswerOne = wrappingPaperNeeded.ToString();
        AnswerTwo = ribbonNeeded.ToString();
    }
}
