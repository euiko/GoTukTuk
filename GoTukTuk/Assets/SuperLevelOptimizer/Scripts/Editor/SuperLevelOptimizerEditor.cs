using UnityEngine;
using UnityEditor;
using NGS.SuperLevelOptimizer;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

[CustomEditor(typeof(SuperLevelOptimizer))]
public class SuperLevelOptimizerEditor : Editor
{
    private SuperLevelOptimizer slo
    {
        get
        {
            return target as SuperLevelOptimizer;
        }
    }

    [MenuItem("Tools/NGSTools/SuperLevelOptimizer/Create Optimizer")]
    private static void CreateOptimizer()
    {
        new GameObject("SuperLevelOptimizer", typeof(SuperLevelOptimizer));
    }

    [MenuItem("Tools/NGSTools/SuperLevelOptimizer/Create Prefab")]
    private static void CreatePrefab()
    {
        if (!Directory.Exists("Assets/SLO/Prefabs/"))
            Directory.CreateDirectory("Assets/SLO/Prefabs/");

        GameObject go = Selection.activeGameObject;

        if (go == null)
            return;

        PrefabUtility.CreatePrefab("Assets/SLO/Prefabs/" + "Combined(" + go.GetInstanceID() + ").prefab", go);
    }

    public override void OnInspectorGUI()
    {
        slo.saveColliders = EditorGUILayout.Toggle("Save Colliders : ", slo.saveColliders);

        if (GUILayout.Button("Open Coefficient Table"))
        {
            var window = EditorWindow.GetWindow<CoefficientTableEditorWindow>();
            window.table = slo.coefficientTable;
        }

        EditorGUILayout.Space();

        slo.bakeType = (BakeType)EditorGUILayout.EnumPopup("Bake Type : ", slo.bakeType);

        if (slo.bakeType == BakeType.Zonal)
        {
            Vector3 zoneCount = slo.zoneCount;

            zoneCount.x = EditorGUILayout.IntField("Count X : ", (int)slo.zoneCount.x);
            zoneCount.y = EditorGUILayout.IntField("Count Y : ", (int)slo.zoneCount.y);
            zoneCount.z = EditorGUILayout.IntField("Count Z : ", (int)slo.zoneCount.z);

            slo.zoneCount = zoneCount;
        }

        slo.searchState = (SearchState)EditorGUILayout.EnumPopup("Search State : ", slo.searchState);

        if (slo.searchState == SearchState.User)
        {
            if (GUILayout.Button("Open Manager Window"))
            {
                var window = EditorWindow.GetWindow<ManagerWindow>();
                window.optimizer = slo;
            }

            #region RenderersDraw

            SerializedProperty s_prop = serializedObject.FindProperty("_objectsForCombine");

            serializedObject.Update();

            if (EditorGUILayout.PropertyField(s_prop))
            {
                for (int i = 0; i < s_prop.arraySize; i++)
                    EditorGUILayout.PropertyField(s_prop.GetArrayElementAtIndex(i));
            }

            serializedObject.ApplyModifiedProperties();

            #endregion

            EditorGUILayout.Space();
        }

        slo.combineState = (CombineState)EditorGUILayout.EnumPopup("Combine State : ", slo.combineState);

        if (slo.combineState == CombineState.CombineToPrefab)
            slo.folderPath = EditorGUILayout.TextField("Folder Path : ", slo.folderPath);

        if (GUILayout.Button("Create Atlases"))
            CreateAtlases();

        if (GUILayout.Button("Combine Meshes"))
            CombineMeshes();

        if (GUILayout.Button("Destroy Sources"))
            DestroySources();
    }

    private void CreateAtlases()
    {
        TexturePacker texturePacker = new TexturePacker();

        Renderer[] renderers = GetRenderers();

        texturePacker.CreateAtlases(renderers, AtlasSize._8192, 0, "Assets/SLO/", slo.coefficientTable);
    }

    private void CombineMeshes()
    {
        MeshCombiner combiner = new MeshCombiner();

        Renderer[] renderers = GetRenderers();

        if (slo.saveColliders && slo.combineState != CombineState.CombineToPrefab)
            combiner.SaveColliders(renderers);

        Transform root = null;

        if (slo.bakeType == BakeType.FullScene)
        {
            root = combiner.CombineMeshes(renderers, slo.saveColliders);
        }
        else
        {
            Vector3 min, max; GetMinMax(renderers, out min, out max);
            Bounds[] bounds = GetBounds(min, max, slo.zoneCount);

            if (renderers == null || renderers.Length == 0)
            {
                Debug.Log("No objects found, mark objects as static");
                return;
            }

            List<Renderer> renderersList = new List<Renderer>(renderers);
            List<Renderer> renderersForCombining = new List<Renderer>();

            root = new GameObject("Combined Meshes").transform;

            for (int i = 0; i < bounds.Length; i++)
            {
                renderersForCombining.Clear();

                int c = 0;
                while (c < renderersList.Count)
                {
                    if (bounds[i].Contains(renderersList[c].bounds.center))
                    {
                        renderersForCombining.Add(renderersList[c]);
                        renderersList.RemoveAt(c);
                    }
                    else
                        c++;
                }

                if (renderersForCombining.Count == 0)
                    continue;

                combiner.CombineMeshes(renderersForCombining.ToArray(), slo.saveColliders).SetParent(root);
            }
        }

        if (slo.combineState == CombineState.CombineToPrefab)
        {
            if (!Directory.Exists(slo.folderPath))
                Directory.CreateDirectory(slo.folderPath);

            PrefabUtility.CreatePrefab(slo.folderPath + "Combined(" + root.GetInstanceID() + ").prefab", root.gameObject);
            DestroyImmediate(root.gameObject);
        }

        slo.AddTempObjects(renderers);
    }

    private void DestroySources()
    {
        List<Renderer> objects = slo.tempObjects;

        for (int i = 0; i < objects.Count; i++)
            if (objects[i] != null) DestroyImmediate(objects[i].gameObject);

        slo.ClearTemp();
    }


    private void OnSceneGUI()
    {
        if (slo.bakeType != BakeType.Zonal)
            return;

        Renderer[] renderers = GetRenderers();

        if (renderers == null || renderers.Length == 0)
        {
            Debug.Log("No objects found, mark objects as static");
            return;
        }

        Vector3 min, max; GetMinMax(renderers, out min, out max);

        Bounds[] bounds = GetBounds(min, max, slo.zoneCount);

        for (int i = 0; i < bounds.Length; i++)
            EditorHelper.DrawWireCube(bounds[i].center, bounds[i].size, Color.blue);
    }

    private Bounds[] GetBounds(Vector3 min, Vector3 max, Vector3 zoneCount)
    {
        List<Bounds> bounds = new List<Bounds>();

        Vector3 size = new Vector3(
            (max.x - min.x) / zoneCount.x,
            (max.y - min.y) / zoneCount.y,
            (max.z - min.z) / zoneCount.z);

        for (int i = 0; i < zoneCount.x; i++)
            for (int c = 0; c < zoneCount.y; c++)
                for (int j = 0; j < zoneCount.z; j++)
                {
                    Vector3 center = new Vector3(
                        min.x + size.x / 2 + size.x * i,
                        min.y + size.y / 2 + size.y * c,
                        min.z + size.z / 2 + size.z * j);

                    bounds.Add(new Bounds(center, size));
                }

        return bounds.ToArray();
    }

    private void GetMinMax(Renderer[] renderers, out Vector3 min, out Vector3 max)
    {
        min = Vector3.one * Mathf.Infinity;
        max = Vector3.one * (-Mathf.Infinity);

        for (int i = 0; i < renderers.Length; i++)
        {
            Bounds bounds = renderers[i].bounds;

            min.x = Mathf.Min(min.x, bounds.min.x);
            min.y = Mathf.Min(min.y, bounds.min.y);
            min.z = Mathf.Min(min.z, bounds.min.z);

            max.x = Mathf.Max(max.x, bounds.max.x);
            max.y = Mathf.Max(max.y, bounds.max.y);
            max.z = Mathf.Max(max.z, bounds.max.z);
        }
    }

    private Renderer[] GetRenderers()
    {
        if (slo.searchState == SearchState.User)
            return slo.objectsForCombine.ToArray();

        return (from r in FindObjectsOfType<Renderer>()
                where GameObjectUtility.AreStaticEditorFlagsSet(r.gameObject, StaticEditorFlags.BatchingStatic)
                where r.GetComponent<MeshFilter>() != null
                where r.GetComponent<MeshFilter>().sharedMesh != null
                select r).ToArray();
    }
}
































































































































































































































































































































































































































































































































































































































































































































































































































































































































