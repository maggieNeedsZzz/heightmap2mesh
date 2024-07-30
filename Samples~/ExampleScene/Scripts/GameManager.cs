using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
public class TexturePostProcessor : AssetPostprocessor
{

    // Use this if you need to set compression format
    void OnPreprocessTexture()
    {

        //if (Path.GetDirectoryName(assetPath).Contains(Path.Combine("Assets", "Imported")) ||
        if(Path.GetDirectoryName(assetPath).Contains(Path.Combine("Assets", "Heightmaps")))
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            if (Path.GetDirectoryName(assetPath).Contains(Path.Combine("Assets", "Heightmaps", "Grayscale")))
            {
                textureImporter.textureType = TextureImporterType.SingleChannel;
                textureImporter.alphaSource = TextureImporterAlphaSource.FromGrayScale;
                //textureImporter.textureFormat = TextureImporterFormat.Alpha8;

            }
            else if (Path.GetDirectoryName(assetPath).Contains(Path.Combine("Assets", "Heightmaps", "Combination")))
            {
                textureImporter.textureCompression = TextureImporterCompression.Compressed;
            }
            Debug.Log("Overriding import: Texture imported with readable settings.");
            textureImporter.isReadable = true;
        }
    }
}
public class GameManager : MonoBehaviour
{
    void Start()
    {


    }
}
