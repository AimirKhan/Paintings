using UnityEngine;

public struct SaveArray
{
    public int[] NumbsMaterial;
    public bool IsStickerReceived;
    public Color[] Colors;

    public SaveArray(int[] numbsMaterial, bool isStickerReceived, Color[] colors)
    {
        IsStickerReceived = isStickerReceived;
        NumbsMaterial = new int[numbsMaterial.Length];
        Colors = new Color[colors.Length];

        for (int i = 0; i < colors.Length; i++)
        {
            Colors[i] = colors[i];

            NumbsMaterial[i] = numbsMaterial[i];
        }
    }
}
