using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TexturePostProcessor : AssetPostprocessor
{

    // Sets correct texture properties 
    void OnPreprocessTexture()
    {

        if (Path.GetDirectoryName(assetPath).Contains(Path.Combine("Assets", "Heightmaps")))
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            textureImporter.textureType = TextureImporterType.SingleChannel;
            textureImporter.alphaSource = TextureImporterAlphaSource.FromGrayScale;
            //textureImporter.textureFormat = TextureImporterFormat.Alpha8;


            // TODO: Support for combination images
            //if (Path.GetDirectoryName(assetPath).Contains(Path.Combine("Assets", "Heightmaps", "Grayscale")))
            //{
            //
            //}
            //else if (Path.GetDirectoryName(assetPath).Contains(Path.Combine("Assets", "Heightmaps", "Combination")))
            //{
            //    textureImporter.textureCompression = TextureImporterCompression.Compressed;
            //}
            Debug.Log("Overriding import: Texture imported with readable settings.");
            textureImporter.isReadable = true;
        }
    }
}