namespace AoC.Days;

internal interface IRollRater
{
    int AccessibleRollCount { get; }
    void RateRolls();
    bool PeelLayer();
}
