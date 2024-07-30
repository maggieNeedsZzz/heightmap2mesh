using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
//using System;


public class TerrainSpawner : EditorWindow
{
    Texture2D heightmap = null;
    Texture2D texture = null;

    float gridWidth;
    float gridHeight;
    int ChunkDivision = 128;//4;
    int VertexDistance = 30;//4;

    // UI vars
    //bool toggleTex = false;
    //int imageChannels = 0;

    [MenuItem("Tools/TerrainSpawner")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TerrainSpawner));
    }

    public static readonly GUIContent selectHeightmap = EditorGUIUtility.TrTextContent("Select Heightmap", "Select heightmap image to create terrain.");
    public static readonly GUIContent setVertexDistance = EditorGUIUtility.TrTextContent("Vertex Distance", "Set distance between mesh verteces. Should mash the resolution of the heightmap in m/pixel.");
    public static readonly GUIContent setChunkDivision = EditorGUIUtility.TrTextContent("Chunk Division", "Set the number of times the terrain will be devided. It must be so that each chunk is at most 256x256 pixels.");
    public static readonly GUIContent selectTexture= EditorGUIUtility.TrTextContent("Select Texture", "Select a texture for the terrain.");
    public static readonly GUIContent[] imageType =
    {
            EditorGUIUtility.TrTextContent("Combination", "Texture in RGB channel, Elevation in A channel"),
            EditorGUIUtility.TrTextContent("Grayscale", "One channel heightmap image"),
    };





    private void OnGUI()
    {

        // Heightmap
        EditorGUILayout.BeginHorizontal();
        heightmap = EditorGUILayout.ObjectField(selectHeightmap, heightmap, typeof(Texture2D), false, GUILayout.Height(100)) as Texture2D;

        EditorGUILayout.Space();

        texture = EditorGUILayout.ObjectField(selectTexture, texture, typeof(Texture2D), false, GUILayout.Height(100)) as Texture2D;
        EditorGUILayout.EndHorizontal();

        // Import & Format Options
        //EditorGUILayout.BeginHorizontal();
        //{
        //    if (GUILayout.Button("Import from System", GUILayout.Height(30)))
        //    {
        //        string path = EditorUtility.OpenFilePanel("Import terrain Heightmap", "", "png");
        //        FileInfo oFileInfo = new FileInfo(path);

        //        if (oFileInfo != null || oFileInfo.Length == 0)
        //        {
        //            if (!File.Exists(Path.Combine("Assets", "Imported", oFileInfo.Name)))
        //            {
        //                File.Copy(oFileInfo.FullName, "Assets/Imported/" + oFileInfo.Name);
        //                heightmap = LoadPNG(path);
        //            }
        //        }
        //        //Texture banner = (Texture)AssetDatabase.LoadAssetAtPath("Assets/(insert file path)/file.png", typeof(Texture));
        //    }



        //    EditorGUILayout.Space(10);
        //    EditorGUI.BeginChangeCheck();
        //    imageChannels = GUILayout.Toolbar((int)imageChannels, imageType, "LargeButton", GUI.ToolbarButtonSize.Fixed);
        //    if (EditorGUI.EndChangeCheck())
        //    {
        //        Debug.Log(imageType.GetValue(imageChannels));
        //    }
        //}
        //EditorGUILayout.EndHorizontal();



        // Texture
        //GUILayout.BeginVertical();
        //EditorGUILayout.Space(10);
        //toggleTex = GUILayout.Toggle(toggleTex, "Use Texture?");
        //GUILayout.EndVertical();

        //if (toggleTex)
        //{
        //    GUILayout.BeginHorizontal();
        //    texture = EditorGUILayout.ObjectField(selectTexture, texture, typeof(Texture2D), false, GUILayout.Height(100)) as Texture2D;
        //    GUILayout.EndHorizontal();
        //}


        GUILayout.BeginVertical();
        EditorGUILayout.Space(10);
        GUILayout.EndVertical();

        GUILayout.BeginHorizontal();
        VertexDistance = EditorGUILayout.IntField(setVertexDistance, VertexDistance);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        ChunkDivision = EditorGUILayout.IntField(setChunkDivision, ChunkDivision);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();
        EditorGUILayout.Space(10);
        GUILayout.EndVertical();

        // Spawn terrain mesh object with options
        if (GUILayout.Button("Spawn", GUILayout.Height(40)))
        {
            SpawnTerrain();
        }
    }
    public void SpawnTerrain()
    {
        GameObject newMesh = new GameObject("Terrain Mesh");
        newMesh.transform.position = new Vector3(0, 0, 0);
        newMesh.AddComponent<TerrainMesh>();
        newMesh.GetComponent<TerrainMesh>().SetVariableFields(heightmap, texture , ChunkDivision);
        newMesh.GetComponent<TerrainMesh>().CreateTerrain();
    }






    public static Texture2D LoadPNG(string filePath)
    {
        Texture2D tex = null;
        byte[] fileData;
        // string path = EditorUtility.OpenFilePanel("Import terrain Heightmap", "", "png");

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(1, 1,TextureFormat.Alpha8,false);
            tex.LoadImage(fileData); //WILL LOAD IN ARGB MODE 32 BIT!!!!!
        }
        return tex;
    }

    public void SaveToFile()
    {
        string path = EditorUtility.SaveFilePanel("Import terrain Heightmap", "", "MyTerrain","png");
        if (path.Length != 0)
        {
            var fileContent = File.ReadAllBytes(path);
            string now = Time.time.ToString();
            File.WriteAllBytes(Application.dataPath + "\\Temp\\" + now + ".png", fileContent);

            /* Loads png Into Texture
             * 
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileContent);
                cube.GetComponent<Renderer>().material.mainTexture = tex;


            //encoding to png
            var png = tex.EncodeToPNG();
            */
        }
    }


}
