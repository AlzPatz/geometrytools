namespace GeometryTools
{
    public static class Functions
    {
        public static float Clamp0To1(float fraction)
        {
            if (fraction < 0.0f)
                fraction = 0.0f;
            if (fraction > 1.0f)
                fraction = 1.0f;
            return fraction;
        }
    }
}