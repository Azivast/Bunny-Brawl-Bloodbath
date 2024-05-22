using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProceduralGeneration
{
#if UNITY_EDITOR // => Ignore from here to next endif if not in editor
    [CustomEditor(typeof(LevelGenerator))]
    public class LevelGenerationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Generate"))
            {
                LevelGenerator generator = target as LevelGenerator;
                generator.Generate();
            }
                
            if (GUILayout.Button("Randomize New"))
            {
                LevelGenerator generator = target as LevelGenerator;
                generator.GenerateNew();
            }
        }
    }
#endif
}