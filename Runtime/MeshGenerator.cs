using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshGenerator
{
    public Vector3[] vertices;
    public int[] triangles;
    private Vector2[] UVs;
    private int nbrVertices;
    private int chunkSize;
    private int height;
    private int width;
    private int currentTriangle;
    private int gridSize;

    public MeshGenerator(float[,] meshArray, int _gridSize)
    {
        height = meshArray.GetLength(0);
        width = meshArray.GetLength(1);
        nbrVertices = height * width;
        Debug.Log("ASDw: " + height + " asds " + width);

        chunkSize = height;
        currentTriangle = 0;
        gridSize = _gridSize;
        triangles = new int[(height - 1) * (width - 1) * 6];
        vertices = new Vector3[height * width];
        UVs = new Vector2[width * height];


        SetVerticesAndUVsCentered(meshArray);
        SetTriangles();
    }

    public MeshGenerator(float[,] meshArray, int _gridSize, Vector2 uvOffset, Vector2 uvScale)
    {
        height = meshArray.GetLength(0);
        width = meshArray.GetLength(1);
        nbrVertices = height * width;

        currentTriangle = 0;
        gridSize = _gridSize;

        vertices = new Vector3[nbrVertices];
        triangles = new int[(height - 1) * (width - 1) * 6];
        UVs = new Vector2[nbrVertices];

        SetVerticesAndUVsCenteredWithOffset(meshArray, uvOffset, uvScale);
        SetTriangles();
    }


    public Mesh GetMesh()
    {
        var mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = UVs;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds(); 
        return mesh;
    }


    private void SetVerticesAndUVsCenteredWithOffset(float[,] meshArray, Vector2 uvOffset, Vector2 uvScale)
    {
        float offsetZ = (height - 1) / 2f;
        float offsetX = (width - 1) / 2f;
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                vertices[width * j + i] = new Vector3((offsetX - i) * gridSize, meshArray[j, i], (offsetZ - j) * gridSize);
                UVs[width * j + i] = new Vector2(i / (float)width, j / (float)height) * uvScale + uvOffset;
            }
        }
    }
    public void SetVerticesAndUVsCentered(float[,] splitArray)
    {
        float offsetZ = (height - 1) / 2f;
        float offsetX = (width - 1) / 2f;
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                int index = width * j + i;
                vertices[index] = new Vector3((offsetX - i) * gridSize, splitArray[j, i], (offsetZ - j) * gridSize);
                UVs[index] = new Vector2(i / (float)(width - 1), j / (float)(height - 1));
            }
        }
    }




    public void AddTriangle(int a, int b, int c)
    {
        triangles[currentTriangle] = a;
        triangles[currentTriangle + 1] = b;
        triangles[currentTriangle + 2] = c;
        currentTriangle += 3;
    }

    private void SetTriangles()
    {
        for (int j = 0; j < height - 1; j++)
        {
            for (int i = 0; i < width - 1; i++)
            {
                int currVert = width * j + i;
                AddTriangle(currVert, currVert + width, currVert + width + 1);
                AddTriangle(currVert, currVert + width + 1, currVert + 1);
            }
        }
    }



}
