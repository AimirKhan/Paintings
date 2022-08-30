using UnityEngine;

public struct DataShading
{
    public Color Color { get; set; }
    public int NumbMaterial { get; set; }

    public DataShading(Color color, int numbMat)
    {
        Color = color;
        NumbMaterial = numbMat;
    }
}
