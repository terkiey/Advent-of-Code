namespace AoC.Days;

internal interface IDevicePather
{
    long CountPaths(string startDevice, string outDevice);
    long CountPathsSafe(string startDevice, string outDevice);
    public IDevicePather Clone();
}