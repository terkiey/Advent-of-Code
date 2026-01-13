using static System.Net.Mime.MediaTypeNames;

namespace AoC.Days;

internal class FractalTilerMachine
{
    private Dictionary<bool[,], bool[,]> Maps { get; set; } = [];
    private bool[,][,] Image = new bool[1, 1][,]
    {
        {
            new bool[3,3]
            {
                { false, true, false },
                { false, false, true },
                { true, true, true },
            }
        }
    };
    

    public FractalTilerMachine(string[] enhanceInstructions)
    {
        Maps = ParseInstructions(enhanceInstructions);
    }

    public void Enhance(int enhanceCount)
    {
        for (int enhanceIndex = 0; enhanceIndex < enhanceCount; enhanceIndex++)
        {
            Enhance();
        }
    }

    public int GetOnPixelCount()
    {
        int count = 0;
        foreach (var tile in Image)
        {
            foreach(var pixel in tile)
            {
                if (pixel)
                {
                    count++;
                }
            }
        }
        return count;
    }

    private void Enhance()
    {
        // Iterate through the tiles in image, and replace with their match from instruction.
        int tilesPerRow = Image.GetLength(0);
        int tileCount = tilesPerRow * tilesPerRow;
        for (int tileIndex = 0; tileIndex < tileCount; tileIndex++)
        {
            int tileRowIndex = tileIndex / tilesPerRow;
            int tileColIndex = tileIndex % tilesPerRow;

            bool[,] tile = Image[tileRowIndex, tileColIndex];
            tile = EnhanceTile(tile);
            Image[tileRowIndex, tileColIndex] = tile;
        }

        // After enhancing every tile, we need to split the grid back into the required tiles for the next enhancement, so if total sidelength divisible by 2, into 2x2 tiles, else into 3x3 tiles.
        tilesPerRow = Image.GetLength(0);
        tileCount = tilesPerRow * tilesPerRow;
        int tileSize = Image[0, 0].GetLength(0);
        int size = tilesPerRow * tileSize;

        // To do this, just combine into a mega grid, and then split into the size we want.#
        // Combine
        bool[,] megaGrid = new bool[size, size];
        for (int tileIndex = 0; tileIndex < tileCount; tileIndex++)
        {
            int tileRowIndex = tileIndex / tilesPerRow;
            int tileColIndex = tileIndex % tilesPerRow;

            bool[,] tile = Image[tileRowIndex, tileColIndex];
            for (int rowIndex = 0; rowIndex < tileSize; rowIndex++)
            {
                int megaGridRowIndex = (tileSize * tileRowIndex) + rowIndex;
                
                for (int colIndex = 0; colIndex < tileSize; colIndex++)
                {
                    int megaGridColIndex = (tileSize * tileColIndex) + colIndex;
                    megaGrid[megaGridRowIndex, megaGridColIndex] = tile[rowIndex, colIndex];
                }
            }
        }

        // Split
        int newTileSize = size % 2 == 0 ? 2 : 3;
        int newImageDimension = size / newTileSize;
        bool[,][,] newImage = new bool[newImageDimension, newImageDimension][,];
        int newTileCount = newImageDimension * newImageDimension;
        for (int newTileIndex = 0; newTileIndex < newTileCount; newTileIndex++)
        {
            int newTileRowIndex = newTileIndex / newImageDimension;
            int newTileColIndex = newTileIndex % newImageDimension;
            newImage[newTileRowIndex, newTileColIndex] = new bool[newTileSize, newTileSize];
            for (int rowIndex = 0; rowIndex < newTileSize; rowIndex++)
            {
                int megaGridRowIndex = (newTileSize * newTileRowIndex) + rowIndex;
                for (int colIndex = 0; colIndex < newTileSize; colIndex++)
                {
                    int megaGridColIndex = (newTileSize * newTileColIndex) + colIndex;
                    newImage[newTileRowIndex, newTileColIndex][rowIndex, colIndex] = megaGrid[megaGridRowIndex, megaGridColIndex];
                }
            }
        }
        Image = newImage;
    }

    private bool[,] EnhanceTile(bool[,] tile)
    {
        int tileWidth = tile.GetLength(0);
        if (tileWidth != 2 && tileWidth != 3)
        {
            throw new ArgumentException("Invalid tile size passed");
        }

        foreach(var kvp in Maps)
        {
            if (TileMatch(tile, kvp.Key))
            {
                return kvp.Value;
            }
        }

        throw new Exception("Tile did not match any enhancement instruction.");
    }

    // Return true if the two tiles match, given you can rotate or flip to make it match.
    private bool TileMatch(bool[,] tileOne, bool[,] tileTwo)
    {
        if (SequenceEqual2D<bool>(tileOne, tileTwo))
        {
            return true;
        }

        for (int rotateIndex = 0; rotateIndex < 3; rotateIndex++)
        {
            tileOne = RotateTileClockwise(tileOne);
            if (SequenceEqual2D(tileOne, tileTwo))
            {
                return true;
            }
        }

        tileOne = FlipTile(tileOne);
        if (SequenceEqual2D<bool>(tileOne, tileTwo))
        {
            return true;
        }

        for (int rotateIndex = 0; rotateIndex < 3; rotateIndex++)
        {
            tileOne = RotateTileClockwise(tileOne);
            if (SequenceEqual2D(tileOne, tileTwo))
            {
                return true;
            }
        }

        return false;
    }

    private bool[,] RotateTileClockwise(bool[,] tile)
    {
        int tileDimension = tile.GetLength(0);
        bool[,] newTile = new bool[tileDimension, tileDimension];
        for (int rowIndex = 0; rowIndex < tileDimension; rowIndex++)
        {
            for (int columnIndex = 0; columnIndex < tileDimension; columnIndex++)
            {
                newTile[columnIndex, tileDimension - 1 - rowIndex] = tile[rowIndex, columnIndex];
            }
        }
        return newTile;
    }

    private bool[,] FlipTile(bool[,] tile)
    {
        int tileDimension = tile.GetLength(0);
        bool[,] newTile = new bool[tileDimension, tileDimension];
        for (int rowIndex = 0; rowIndex < tileDimension; rowIndex++)
        {
            for (int columnIndex = 0; columnIndex < tileDimension; columnIndex++)
            {
                newTile[tileDimension - 1 - rowIndex, columnIndex] = tile[rowIndex, columnIndex];
            }
        }
        return newTile;
    }

    public static bool SequenceEqual2D<T>(T[,] arrayOne, T[,] arrayTwo)
    {
        if (arrayOne == arrayTwo) return true;
        if (arrayOne is null || arrayTwo is null) return false;

        if (arrayOne.GetLength(0) != arrayTwo.GetLength(0) ||
            arrayOne.GetLength(1) != arrayTwo.GetLength(1))
            {
                return false;
            }

        int rows = arrayOne.GetLength(0);
        int cols = arrayOne.GetLength(1);

        var comparer = EqualityComparer<T>.Default;

        for (int rowIndex = 0; rowIndex < rows; rowIndex++)
        {
            for (int colIndex = 0; colIndex < cols; colIndex++)
            {
                if (!comparer.Equals(arrayOne[rowIndex, colIndex], arrayTwo[rowIndex, colIndex]))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private Dictionary<bool[,], bool[,]> ParseInstructions(string[] enhanceInstructions)
    {
        Dictionary<bool[,], bool[,]> instructionList = [];
        foreach (string instruction in enhanceInstructions)
        {
            string[] splitInstruction = instruction.Split(" => ");
            bool[,] instructionLhsEncoded = EncodeInstruction(splitInstruction[0]);
            bool[,] instructionRhsEncoded = EncodeInstruction(splitInstruction[1]);
            instructionList[instructionLhsEncoded] = instructionRhsEncoded;
        }
        return instructionList;
    }

    private bool[,] EncodeInstruction(string instruction)
    {
        // Deduce width of instruction by the number of / contained.
        int width;
        switch (instruction.Split('/').Length)
        {
            case 2:
                width = 2;
                break;

            case 3: 
                width = 3;
                break;

            case 4:
                width = 4;
                break;

            default:
                throw new Exception("No other instruction sizes.");
        }

        bool[,] encoded = new bool[width, width];
        int index = 0;
        foreach (char character in instruction)
        {
            int row = index / width;
            int col = index % width;
            switch (character)
            {
                case '.':
                    encoded[row, col] = false;
                    index++;
                    break;

                case '#':
                    encoded[row, col] = true;
                    index++;
                    break;

                default:
                    break;
            }
        }
        return encoded;
    }
}
