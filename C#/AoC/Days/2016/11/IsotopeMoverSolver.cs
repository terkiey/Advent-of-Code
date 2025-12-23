namespace AoC.Days;

internal class IsotopeMoverSolver
{
    public int MinimumMoves()
    {
        /* I realised the solution to this seems really simple, atleast given my input...
        * If I move two items up, and then bring one down with me (I always need to have one for the lift to move), this will have the net result of moving one item up.
        * If I repeat this, the last move is when theres two items left and I can bring them both and forget about the floor below.
        * So I need to repeat that up down motion x-2 times where x is the number of items on the floor, then, add one for the last single move up.
        * Special case when x = 1 or x = 2 where we just do it in one move.
        */

        /* the input is tiny so I just hardcoded it in */
        int[] itemCounts = [8, 2, 0, 0];
        int moves = 0;
        int currentFloor = 0;

        while (true)
        {
            if (itemCounts[3] == itemCounts.Sum())
            {
                return moves;
            }

            if (itemCounts[currentFloor] == 0)
            {
                currentFloor++;
                moves++;
            }
            else
            {
                moves += ((itemCounts[currentFloor] - 2) * 2) + 1;
                itemCounts[currentFloor + 1] += itemCounts[currentFloor];
                itemCounts[currentFloor] = 0;
                currentFloor++;
            }
        }
    }

    public int MinimumMovesPartTwo()
    {
        // Same as part one just change the item counts...
        int[] itemCounts = [12, 2, 0, 0];
        int moves = 0;
        int currentFloor = 0;

        while (true)
        {
            if (itemCounts[3] == itemCounts.Sum())
            {
                return moves;
            }

            if (itemCounts[currentFloor] == 0)
            {
                currentFloor++;
                moves++;
            }
            else
            {
                moves += ((itemCounts[currentFloor] - 2) * 2) + 1;
                itemCounts[currentFloor + 1] += itemCounts[currentFloor];
                itemCounts[currentFloor] = 0;
                currentFloor++;
            }
        }
    }
}
