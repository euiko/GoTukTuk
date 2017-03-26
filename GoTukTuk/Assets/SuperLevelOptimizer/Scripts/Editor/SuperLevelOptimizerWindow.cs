using UnityEngine;
using UnityEditor;
using NGS.SuperLevelOptimizer;
using System.Collections;

namespace NGS.SuperLevelOptimizer
{
    public class CoefficientTableEditorWindow : EditorWindow
    {
        public CoefficientTable table;

        public void OnGUI()
        {
            if (table == null)
            {
                Debug.Log("No table set");
                Close();
            }

            table.floatValue = EditorGUILayout.FloatField("Float value : ", table.floatValue);

            EditorGUILayout.Space();

            Vector4 vectorValue = table.vectorValue;

            vectorValue.x = EditorGUILayout.FloatField("Vector value x : ", vectorValue.x);
            vectorValue.y = EditorGUILayout.FloatField("Vector value y : ", vectorValue.y);
            vectorValue.z = EditorGUILayout.FloatField("Vector value z : ", vectorValue.z);
            vectorValue.w = EditorGUILayout.FloatField("Vector value w : ", vectorValue.w);

            table.vectorValue = vectorValue;

            EditorGUILayout.Space();

            Color colorValue = table.colorValue;

            colorValue.r = EditorGUILayout.FloatField("Color value r : ", colorValue.r);
            colorValue.g = EditorGUILayout.FloatField("Color value g : ", colorValue.g);
            colorValue.b = EditorGUILayout.FloatField("Color value b : ", colorValue.b);
            colorValue.a = EditorGUILayout.FloatField("Color value a : ", colorValue.a);

            table.colorValue = colorValue;

            EditorGUILayout.Space();

            if (GUILayout.Button("Reset Values"))
            {
                table.floatValue = 0.1f;

                table.vectorValue = Vector4.one * 0.1f;

                table.colorValue = new Color(0.1f, 0.1f, 0.1f, 0.1f);
            }
        }
    }

    public class ManagerWindow : EditorWindow
    {
        public SuperLevelOptimizer optimizer;

        public void OnGUI()
        {
            if (optimizer == null)
                Close();

            if (GUILayout.Button("Add Selection"))
                AddSelection();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Remove Selection"))
                RemoveSelection();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Remove All"))
                RemoveAll();
        }

        private void AddSelection()
        {
            int count = 0;

            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                foreach (var renderer in Selection.gameObjects[i].GetComponentsInChildren<Renderer>())
                {
                    if (GameObjectUtility.AreStaticEditorFlagsSet(renderer.gameObject, StaticEditorFlags.BatchingStatic))
                        if (renderer.GetComponent<MeshFilter>() != null)
                            if (renderer.GetComponent<MeshFilter>().sharedMesh != null)
                            {
                                optimizer.AddObjectForCombine(renderer);
                                count++;
                            }
                }
            }

            Debug.Log("Added " + count + " objects");
        }

        private void RemoveSelection()
        {
            int count = optimizer.objectsForCombine.Count;

            for (int i = 0; i < Selection.gameObjects.Length; i++)
                optimizer.DeleteObjectsForCombine(Selection.gameObjects[i].GetComponentsInChildren<Renderer>());

            count = count - optimizer.objectsForCombine.Count;

            Debug.Log("Removed " + count + " objects");
        }

        private void RemoveAll()
        {
            optimizer.ClearObjectsForCombine();

            Debug.Log("Removed all objects");
        }
    }
}
