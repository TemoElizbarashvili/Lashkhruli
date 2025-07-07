public static class MarketManager
{
    public static readonly int[] Prices = { 100, 200, 300, 400, 500 };
    public static readonly int[] DamageAdditions = { 2, 8, 10, 15, 20 };
    public static readonly int[] HealthAdditions = { 10, 20, 30, 40, 50 };
    public static readonly float[] SpeedNJumpMultipliers = { 1.0f, 1.1f, 1.2f, 1.3f, 1.4f };
    public static int CurrentHealthIndex = 0;
    public static int CurrentDamageIndex = 0;
    public static int CurrentSpeedNJumpIndex = 0;
}
