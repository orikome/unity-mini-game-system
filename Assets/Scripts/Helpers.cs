using UnityEngine;

public static class Helpers
{
    // Makes animations start fast and slow down at the end for a more natural feel.
    public static float EaseOutCubic(float t)
    {
        return 1 - Mathf.Pow(1 - t, 3);
    }
}
