namespace AoC;

internal interface IRollRater
{
    int AccessibleRollCount { get; }
    void RateRolls();
    bool PeelLayer();

}
