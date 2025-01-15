namespace Core.Extentions
{
    public static class FloatExtensions
    {
        public static float Random(float min, float max)
        {
            System.Random random = new System.Random();
            return (float)(random.NextDouble() * (max - min) + min);
        }
    }
}