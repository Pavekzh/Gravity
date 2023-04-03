using UnityEngine;

public struct ColorInterval
{
    public Color Color;
    public float Limit;

    public ColorInterval(Color color, float limit)
    {
        this.Color = color;
        this.Limit = limit;
    }
}