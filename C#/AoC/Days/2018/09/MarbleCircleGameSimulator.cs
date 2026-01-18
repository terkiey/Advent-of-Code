namespace AoC.Days;

internal class MarbleCircleGameSimulator
{
    public long GetWinningScore(int playerCount, int highestMarble)
    {
        List<int> marbleCircle = [0];
        int player = 1;
        int marble = 0;
        int currentMarbleIndex = 0;
        long[] playerScores = new long[playerCount + 1];
        while (marble < highestMarble)
        {
            marble++;
            if (marble % 23 != 0)
            {
                currentMarbleIndex += 1;
                currentMarbleIndex %= marbleCircle.Count;
                marbleCircle.Insert(++currentMarbleIndex, marble);
            }
            else
            {
                playerScores[player] += marble;
                currentMarbleIndex -= 7;
                if (currentMarbleIndex < 0)
                {
                    currentMarbleIndex += marbleCircle.Count;
                }
                playerScores[player] += marbleCircle[currentMarbleIndex];
                marbleCircle.RemoveAt(currentMarbleIndex);
                currentMarbleIndex %= marbleCircle.Count;
            }

            player++;
            if (player > playerCount)
            {
                player -= playerCount;
            }
        }

        return playerScores.Max();
    }
}
