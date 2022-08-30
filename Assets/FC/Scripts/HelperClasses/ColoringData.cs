using UnityEngine;

public class ColoringData //Class to implement cancellation
{
    public int ID;
    public int NumbMaterial;
    public Color Color;

    public ColoringData(int id, int numbMaterial, Color color)
    {
        ID = id;
        NumbMaterial = numbMaterial;
        Color = color;
    }
}
