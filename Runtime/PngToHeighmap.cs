using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;




class ImageScaler 
{
    //Value will be between 0 and 1
    static public float Scale(float value)
    {
        //Downsizing by x30
        //float max = 291; // max is 8729;
        //float min = -14;  // min is -415;
        float max = 8768; // 8729;
        float min = -512; //-415;

        //Note: No need to normalize, result will be [0,1], if not then normalizedData = (value - min) / (max - min);
        float lambda = 0.11f;
        float scaledVal = InverseCumulativeExponential(value, lambda);

        float rescaledData = scaledVal * (max - min) + min;
        return rescaledData;
    }

    static public float InverseCumulativeExponential(float value, float lambda)
    {
        //Using  Mathf.Exp((-1 * value) / 0.11f) scaling, the inverse is:
        return -lambda * Mathf.Log(-value + 1);

    }
}

interface IHeightData
{
    public float[,] elevation { get; set; }
    public float[,] GetHeightData(Texture2D heightmap);

}





//Uses a 1 channel heighmap texture and translates into a jagged array: float[][] elevation
//Then scales the height of the data, given the fact that each point in the image has a 30km distance, and given the fact it was scaled previously.
class HeightData : IHeightData
{
    public float[,] elevation { get; set; }


    public HeightData(Texture2D heightmap)
    {
        
        elevation = GetHeightData(heightmap);

        // size = heightmap.height;
        // Color[] colors = heightmap.GetPixels();
        // elevation = new float[heightmap.height][];
        // for (int i = 0; i < heightmap.height; i++)
        // {
        //     elevation[i] = new float[heightmap.width];
        //     elevation[i] = colors[(i * heightmap.width)..(((i + 1) * heightmap.width))].Select(color => color.r).ToArray();
        // }

        Debug.Log("Image Processed");
        Debug.Log("Scaling...");
        int width = elevation.GetLength(1);
        int height = elevation.GetLength(0);

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                //float value = elevation[i, j];
                elevation[i,j] = ImageScaler.Scale(elevation[i,j]);
            }
        }




    }
    public HeightData(float[,] _elevation)
    {
        elevation = _elevation;
    }





    public float[,] GetHeightData(Texture2D heightmap)
    {
        int width = heightmap.width;
        int height = heightmap.height;
        Color[] colors = heightmap.GetPixels();

        float[,] elevation = new float[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {

                // With y-inversion: textures have origin on bottom-left
                //elevation[i, j] = colors[(height - 1 - i) * width + j].a;

                // Without y-inversion
                elevation[i, j] = colors[i * width + j].a;
            }
        }

        return elevation;
    }



}


public class RGBAHeightData : IHeightData
{
    public float[,] elevation { get; set; }

    public RGBAHeightData(Texture2D heightmap)
    {


        elevation = GetHeightData(heightmap);

        Debug.Log("Image Processed");
        Debug.Log("Scaling...");

        int width = elevation.GetLength(1);
        int height = elevation.GetLength(0);
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                //float value = elevation[i, j];
                elevation[i, j] = ImageScaler.Scale(elevation[i, j]);
            }
        }

    }


    public float[,] GetHeightData(Texture2D heightmap)
    {
        int width = heightmap.width;
        int height = heightmap.height;
        Color[] colors = heightmap.GetPixels();

        float[,] elevation = new float[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                elevation[i, j] = colors[i * width + j].a;
            }
        }

        return elevation;
    }
}


