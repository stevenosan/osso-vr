namespace osso_vr_tests.Models;

public static class RandomGenerator
{
    private static Random _random = new Random();
    public static bool NextBool()
    {
        return _random.Next(2) == 0;
    }

    public static int NextInt(int floor = 0, int ceiling = int.MaxValue)
    {
        return _random.Next(floor, ceiling);
    }
}
