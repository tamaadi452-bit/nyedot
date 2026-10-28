using UnityEngine;

public static class SizeSpeed
{
    // Tiap kelipatan 'step' point, speed berkurang 'reducePerStep' (0.05 = 5%)
    // minMultiplier = batas paling lambat (0.4 = minimal 40% dari speed awal)
    public static float Multiplier(float points, int step = 50,
                                   float reducePerStep = 0.05f, float minMultiplier = 0.4f)
    {
        int steps = Mathf.FloorToInt(points / step);
        float m = 1f - steps * reducePerStep;
        return Mathf.Max(m, minMultiplier);
    }
}