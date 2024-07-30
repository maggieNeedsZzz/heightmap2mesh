using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMesh : MonoBehaviour
{

    [SerializeField] private Texture2D heightmap;
    [SerializeField] private Texture2D texture;
    private Texture2D altTexture;
    private Texture2D alphaTexture;
    private Texture2D[,] textures;
    private int width { get; set; }
    private int height { get; set; }

    private int chunkSize = 128;
    private int gridSize = 30;


    private List<GameObject> chunkss;
    private GameObject[,] terrainObjs;
    //private int chunkExtra;
    private int chunkRows;
    private int chunks;

    private IHeightData heightData;
    private ITextureData texData;

    bool showTexture;

    MeshGenerator meshGen;

    private bool canToggleTexture;

    public bool IsAltTextureAvailable()
    {
        return altTexture == null;
    }

    public void SetVariableFields(Texture2D _heightmap, Texture2D _texture, int _chunkSize)
    {
        heightmap = _heightmap;
        texture = _texture;
        //altTexture = _texture;
        chunkSize = _chunkSize;
        canToggleTexture = false;
        showTexture = true;

    }


    public void RefreshTexture()
    {
        //TODO TEST WHAT HAPPENS IF I DONT INSERT TEXTURE AND PRESS THE BUTTON
        if (texture.format == TextureFormat.Alpha8)
            texture = new HeightmapTexture(texture).texture;
        altTexture = texture;
        canToggleTexture = true;
        for (int c = 0; c < chunkss.Count; c++)
            chunkss[c].GetComponent<MeshRenderer>().sharedMaterial.mainTexture = texture;
    }

    public void ToggleTexture(bool enableTexture)
    {
        if (canToggleTexture) { 
            if (enableTexture)
                texture = altTexture;
            else
                texture = alphaTexture;
            //texture.Apply();
            RefreshTexture();
        }
    }

    private void SetHeightAndTexture()
    {


        Debug.Log("Heightmap format: " + heightmap.format);
        if (heightmap.format == TextureFormat.RGBA32)
        {
            Debug.Log("RGABA??");
            heightData = new HeightData(heightmap);
            CombinationTexture tempTex = new CombinationTexture(heightmap);

            alphaTexture = tempTex.aTexture;
            altTexture = tempTex.rgbTexture;
            texture = altTexture;

            canToggleTexture = true;
            //TODO
        }
        else if (heightmap.format == TextureFormat.Alpha8)
        {
            heightData = new HeightData(heightmap);
            alphaTexture = new HeightmapTexture(heightmap).texture;

            if(texture != null)
            {
                if (texture.format == TextureFormat.Alpha8)
                {
                    altTexture = new HeightmapTexture(texture).texture;
                    texture = altTexture;
                }
                else
                {
                    altTexture = texture;
                }
                canToggleTexture = true;
            }
            else
            {
                texture = alphaTexture;
            }
        }
        else
        {
            Debug.LogError("Image format not recognized. Heightmap texture must be in Alpha8 format OR.");
        }
    }

    public void CreateTerrain()
    {
        chunkss = new List<GameObject>();
        SetHeightAndTexture();

        height = heightmap.height;
        width = heightmap.width;


        //ASSUMES height == width
        if (chunkSize > 256)
        {
            Debug.LogWarning("Unity Doesn't Support Chunks bigger than 256. Max of 65535 vertices per mesh");
            chunkSize = 256;
        }
        chunkRows = height / chunkSize;
        chunks = (int)Mathf.Pow(chunkRows, 2.0f);

        SplitIntoMeshChunks();
        //SplitIntoChunksWithoutGap();

    }
    public void SplitIntoMeshChunks()
    {

        float[,] initialArray = heightData.elevation;

        float[,][,] chunks = SplitArray(initialArray, chunkSize);
        int totalChunksPerColumn = chunks.GetLength(0); // Note: height is the first dimension
        int totalChunksPerRow = chunks.GetLength(1); // Note: width is the second dimension





        for (int row = 0; row < totalChunksPerColumn; row++)
        {
            for (int col = 0; col < totalChunksPerRow; col++)
            {
                float[,] chunk = chunks[row, col];

                // Calculate UV offset and scale for the chunk
                Vector2 uvOffset = new Vector2((float)col / totalChunksPerRow, (float)row / totalChunksPerColumn);
                Vector2 uvScale = new Vector2(1f / totalChunksPerRow, 1f / totalChunksPerColumn);

                MeshGenerator meshGenerator = new MeshGenerator(chunk, gridSize, uvOffset, uvScale);
                Mesh mesh = meshGenerator.GetMesh();

                GameObject meshObject = GenerateMeshObject($"Chunk_{row}_{col}");
                chunkss.Add(meshObject);
                meshObject.GetComponent<MeshFilter>().mesh = mesh;
                MeshRenderer meshRenderer = meshObject.GetComponent<MeshRenderer>();
                meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
                meshRenderer.sharedMaterial.mainTexture = texture;

                // Correctly position the chunk
                meshObject.transform.position = new Vector3(-col * (chunkSize - 0.5f ) * gridSize, 0, -row * (chunkSize - 0.5f) * gridSize);
            }
        }
    }

    public float[,][,] SplitArray(float[,] originalArray, int chunkSize)
    {
        int totalHeight = originalArray.GetLength(0);
        int totalWidth = originalArray.GetLength(1);
        int totalChunksPerRow = Mathf.CeilToInt(totalWidth / (float)chunkSize);
        int totalChunksPerColumn = Mathf.CeilToInt(totalHeight / (float)chunkSize);


        int row = 0;
        int col = 0;
        float[,][,] chunks = new float[totalChunksPerRow, totalChunksPerColumn][,];

        for (int y = 0; y < totalHeight; y += chunkSize)
        {
            for (int x = 0; x < totalWidth; x += chunkSize)
            {
                int currentChunkSizeY = Mathf.Min(chunkSize + 1, totalHeight - y);
                int currentChunkSizeX = Mathf.Min(chunkSize + 1, totalWidth - x);

                if (currentChunkSizeY == 1 || currentChunkSizeX == 1) Debug.Log("RhoRHo"); 

                float[,] chunk = new float[currentChunkSizeY, currentChunkSizeX];
                for (int j = 0; j < currentChunkSizeY; j++)
                {
                    for (int i = 0; i < currentChunkSizeX; i++)
                    {
                        chunk[j, i] = originalArray[y + j, x + i];
                    }
                }
                chunks[row, col]  = chunk;
                col++;
            }
            row++;
            col = 0;
        }

        return chunks;
    }






    public void SetMesh(GameObject meshObj, Mesh mesh, Texture2D texture)
    {

        meshObj.GetComponent<MeshFilter>().mesh = mesh;
        meshObj.GetComponent<MeshFilter>().mesh.RecalculateBounds();

        MeshRenderer renderer = meshObj.GetComponent<MeshRenderer>();
        renderer.sharedMaterial = new Material(Shader.Find("Standard"));
        renderer.sharedMaterial.mainTexture = texture;
        renderer.sharedMaterial.SetFloat("_Glossiness", 0.1f);
    }

    //void OnDrawGizmos()
    //{
    //    if (terrainObjs != null)
    //    {
    //        Vector3[] vertices = terrainObjs[0, 0].GetComponent<MeshFilter>().mesh.vertices;
    //        int[] triangles = terrainObjs[0, 0].GetComponent<MeshFilter>().mesh.triangles;
    //        if (vertices == null || triangles == null) return;

    //        for (int i = 0; i < triangles.Length; i += 3)
    //        {
    //            Vector3 p1 = vertices[triangles[i]];
    //            Vector3 p2 = vertices[triangles[i + 1]];
    //            Vector3 p3 = vertices[triangles[i + 2]];

    //            Gizmos.color = Color.red;
    //            Gizmos.DrawLine(p1, p2);
    //            Gizmos.DrawLine(p2, p3);
    //            Gizmos.DrawLine(p3, p1);
    //        }

    //    }
    //}



    private GameObject GenerateMeshObject(string objectName)
    {
        GameObject newMesh = new GameObject(objectName);
        newMesh.transform.position = new Vector3(0, 0, 0);
        newMesh.AddComponent<MeshFilter>();
        newMesh.AddComponent<MeshRenderer>();
        newMesh.transform.parent = this.transform;
        //terrainObjs[currentTerrainIdx++] = newMesh;
        return newMesh;
    }
}
