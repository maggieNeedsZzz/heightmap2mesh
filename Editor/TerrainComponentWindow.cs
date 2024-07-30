using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainMesh))]
public class TerrainComponentWindow : Editor
{
    private string btnText = "Remove Texture";
    private bool textureEnabled = false;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TerrainMesh terrainMesh = (TerrainMesh)target;

        if (GUILayout.Button("Refresh", GUILayout.Height(20)))
        {
            Debug.Log("Refresh");
            terrainMesh.RefreshTexture();
        }
        if (GUILayout.Button(btnText, GUILayout.Height(20)))
        {
            if (textureEnabled)
            {
                btnText = "Remove Texture";
                textureEnabled = false;
                terrainMesh.ToggleTexture(textureEnabled);
            }
            else
            {
                btnText = "Draw Texture";
                textureEnabled = true;
                terrainMesh.ToggleTexture(textureEnabled);
            }
        }
    }
}
