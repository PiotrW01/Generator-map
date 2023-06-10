using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NoiseMapGenerator))]
public class EditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NoiseMapGenerator script = (NoiseMapGenerator)target;

        if (GUILayout.Button("Regenerate map"))
        {
            script.GenerateMap();
        }
    }
}