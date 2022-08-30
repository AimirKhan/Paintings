using UnityEngine;

[System.Serializable]
public struct DataPalette
{
    public Color Color  { get; set; }
    public int NumbMaterial { get; set; }
    public Material Material { get; set; }

    public DataPalette(Color color, int numbMat, Material material)
    {
        Color = color;
        NumbMaterial = numbMat;
        Material = material;
    }
}