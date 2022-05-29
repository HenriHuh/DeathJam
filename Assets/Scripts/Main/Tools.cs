using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools
{
    public static readonly Vector2Int[] sides = new Vector2Int[] { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };

    public static Color ColorRainbowLerp(float t, float S = 1f, float V = 1f)
    {
        return Color.HSVToRGB(t, S, V);
    }
}
