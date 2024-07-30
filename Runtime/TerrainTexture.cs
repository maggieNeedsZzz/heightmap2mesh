using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ITextureData
{
    public Texture2D texture { get; set; }
}

public class HeightmapTexture : ITextureData
{
    public Texture2D texture { get; set; }
    private Color[] colorArray;
    private int width;
    private int height;

    public HeightmapTexture(Texture2D heightmap)
    {
        width = heightmap.width;
        height = heightmap.height;
        texture = new Texture2D(width, height);


        colorArray = heightmap.GetPixels();
        for (int i = 0; i < colorArray.Length; i++)
        {
            colorArray[i].r = colorArray[i].a;
            colorArray[i].g = colorArray[i].a;
            colorArray[i].b = colorArray[i].a;
            colorArray[i].a = 1;
        }


        for (int h = 0; h < height; h++)
            for (int w = 0; w < width; w++)
                texture.SetPixel(w, h, colorArray[h * width + w]);
        texture.Apply();
    }

}




public class CombinationTexture : ITextureData
{
    public Texture2D texture { get; set; }
    public Texture2D[] textures { get; set; }
    private Color[] colorArray;
    private int width;
    private int height;
    public Color[] textureArray;
    public Color[] rgb;
    public Color[] a;
    public Texture2D rgbTexture;
    public Texture2D aTexture;

    public CombinationTexture(Texture2D combinationImage)
    {
        width = combinationImage.width;
        height = combinationImage.height;
        texture = new Texture2D(width, height);
        aTexture = new Texture2D(width, height);



        Color[] aColors = combinationImage.GetPixels();
        for (int i = 0; i < aColors.Length; i++)
        {
            aColors[i].r = aColors[i].a;
            aColors[i].g = aColors[i].a;
            aColors[i].b = aColors[i].a;
            aColors[i].a = 1;
        }

        for (int h = 0; h < combinationImage.height; h++)
            for (int w = 0; w < combinationImage.width; w++)
                aTexture.SetPixel(w, h, aColors[h * combinationImage.width + w]);
        aTexture.Apply();
        



        Color[] colors = combinationImage.GetPixels();
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i].a = 1;
        }
        rgb = colors;
        a = aColors;
        //colors.Select(color => color.a = 1);

        rgbTexture = new Texture2D(combinationImage.width, combinationImage.height);

        for (int h = 0; h < combinationImage.height; h++)
            for (int w = 0; w < combinationImage.width; w++)
                rgbTexture.SetPixel(w, h, colors[h * combinationImage.width + w]);
        rgbTexture.Apply();

    }

}
