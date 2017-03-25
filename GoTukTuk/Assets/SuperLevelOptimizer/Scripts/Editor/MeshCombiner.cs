using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace NGS.SuperLevelOptimizer
{
    public class MeshCombiner
    {
        private const string saveFolder = "Assets/SLO/CombinedMeshes/";

        public Transform CombineMeshes(Renderer[] renderers, bool saveColliders)
        {
            if (renderers.Length == 0)
            {
                Debug.Log("No objects found, mark objects as static");
                return null;
            }

            if (!Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);

            MeshFilter[] filters = renderers.Select(r => r.GetComponent<MeshFilter>()).ToArray();

            MeshesGroup[] groups = GetMeshesGroups(filters);

            Transform root = new GameObject("Combined Meshes").transform;

            for (int i = 0; i < groups.Length; i++)
            {
                try
                {
                    EditorUtility.DisplayProgressBar("Combining meshes", "Ready : " + i + " of " + groups.Length, (float)i / groups.Length);

                    CreateObject(groups[i], root);
                }
                catch (System.Exception ex)
                {
                    Debug.Log("At runtime exception occurred.Do not worry, it did not lead to critical consequences.Please inform the developer(e-mail: andreorsk@yandex.ru)");
                    Debug.Log(ex.Message);
                    continue;
                }
            }

            EditorUtility.ClearProgressBar();

            return root;
        }

        public void SaveColliders(Renderer[] renderers)
        {
            Transform collidersRoot = new GameObject("Colliders").transform;

            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].GetComponent<Collider>() != null)
                {
                    var collider = Object.Instantiate(renderers[i].GetComponent<Collider>());

                    foreach (var comp in collider.GetComponents<Component>())
                        if (!(comp is Transform) && !(comp is Collider))
                            Object.DestroyImmediate(comp);

                    collider.transform.position = renderers[i].transform.position;
                    collider.transform.rotation = renderers[i].transform.rotation;
                    collider.transform.localScale = renderers[i].transform.lossyScale;

                    collider.transform.SetParent(collidersRoot);
                }
            }

            if (collidersRoot.childCount == 0)
                Object.DestroyImmediate(collidersRoot.gameObject);
        }

        private MeshesGroup[] GetMeshesGroups(MeshFilter[] filters)
        {
            List<MeshesGroup> groups = new List<MeshesGroup>();

            for (int i = 0; i < filters.Length; i++)
            { 
                bool added = false;

                for (int c = 0; c < groups.Count; c++)
                {
                    int result = groups[c].AddMeshFilter(filters[i]);

                    if (result == 0 || result == 2)
                    {
                        added = true;
                        break;
                    }
                }

                if (!added)
                    groups.Add(new MeshesGroup(filters[i]));
            }

            return groups.ToArray();
        }

        private void CreateObject(MeshesGroup group, Transform parent)
        {
            GameObject combined = new GameObject(group.meshFilters[0].name + "_combined");

            Renderer renderer = combined.AddComponent<MeshRenderer>();
            renderer.sharedMaterials = group.materials;

            MeshFilter filter = combined.AddComponent<MeshFilter>();
            filter.sharedMesh = CombineMeshesInGroup(group);

            combined.isStatic = true;

            combined.transform.SetParent(parent);
        }

        private Mesh CombineMeshesInGroup(MeshesGroup group)
        {
            int vertexCount = group.vertexCount;

            Mesh[] meshes = group.meshFilters.Select(f => f.sharedMesh).ToArray();

            Vector3[] vertices = new Vector3[vertexCount];
            Vector3[] normals = new Vector3[vertexCount];
            Vector4[] tangents = new Vector4[vertexCount];
            List<int[]> triangles = new List<int[]>();
            List<Vector2> uv = new List<Vector2>();
            List<Vector2> uv2 = new List<Vector2>();
            List<Vector2> uv3 = new List<Vector2>();
            List<Vector2> uv4 = new List<Vector2>();
            List<Color> colors = new List<Color>();

            int offset = 0;

            #region vertices

            for (int i = 0; i < meshes.Length; i++)
            {
                GetVertices(meshes[i].vertexCount, meshes[i].vertices, vertices, ref offset, group.matrix[i]);
            }

            #endregion

            #region normals

            offset = 0;

            for (int i = 0; i < meshes.Length; i++)
            {
                GetNormal(meshes[i].vertexCount, meshes[i].normals, normals, ref offset, group.matrix[i]);
            }

            #endregion

            #region tangents

            offset = 0;

            for (int i = 0; i < meshes.Length; i++)
            {
                GetTangents(meshes[i].vertexCount, meshes[i].tangents, tangents, ref offset, group.matrix[i]);
            }

            #endregion

            #region triangles

            for (int i = 0; i < meshes[0].subMeshCount; i++)
            {
                int curTrianglesCount = 0;

                for(int c = 0; c < meshes.Length; c++)
                    curTrianglesCount = curTrianglesCount + meshes[c].GetTriangles(i).Length;

                int[] curTriangles = new int[curTrianglesCount];

                int triangleOffset = 0;
                int vertexOffset = 0;

                for(int c = 0; c < meshes.Length; c++)
                {
                    int[] inputtriangles = meshes[c].GetTriangles(i);

                    for (int p = 0; p < inputtriangles.Length; p += 3)
                    {
                        Vector3 scale = group.meshFilters[c].transform.lossyScale;

                        if (scale.x < 0 || scale.y < 0 || scale.z < 0)
                        {
                            curTriangles[p + triangleOffset] = inputtriangles[p + 2] + vertexOffset;
                            curTriangles[p + 1 + triangleOffset] = inputtriangles[p + 1] + vertexOffset;
                            curTriangles[p + 2 + triangleOffset] = inputtriangles[p] + vertexOffset;
                        }
                        else
                        {
                            curTriangles[p + triangleOffset] = inputtriangles[p] + vertexOffset;
                            curTriangles[p + 1 + triangleOffset] = inputtriangles[p + 1] + vertexOffset;
                            curTriangles[p + 2 + triangleOffset] = inputtriangles[p + 2] + vertexOffset;
                        }
                    }

                    triangleOffset += inputtriangles.Length;
                    vertexOffset += meshes[c].vertexCount;
                }

                triangles.Add(curTriangles);
            }

            #endregion

            #region Other

            for(int i = 0; i < meshes.Length; i++)
            {
                uv.AddRange(meshes[i].uv);
                uv2.AddRange(meshes[i].uv2);
                uv3.AddRange(meshes[i].uv3);
                uv4.AddRange(meshes[i].uv4);
                colors.AddRange(meshes[i].colors);
            }

            #endregion

            Mesh mesh = new Mesh();

            mesh.name = "CombineMesh";
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.tangents = tangents;

            mesh.uv = uv.ToArray();

            if (uv2.Count == vertices.Length) mesh.uv2 = uv2.ToArray();
            if (uv3.Count == vertices.Length) mesh.uv3 = uv3.ToArray();
            if (uv4.Count == vertices.Length) mesh.uv4 = uv4.ToArray(); 

            if (colors.Count == vertexCount) mesh.colors = colors.ToArray();

            mesh.subMeshCount = meshes[0].subMeshCount;

            for (int i = 0; i < mesh.subMeshCount; i++)
                mesh.SetTriangles(triangles[i], i);

            Unwrapping.GenerateSecondaryUVSet(mesh);

            ;

            string fullPath = saveFolder + mesh.name + mesh.GetInstanceID() + ".asset";

            AssetDatabase.CreateAsset(mesh, fullPath);

            return (Mesh)AssetDatabase.LoadAssetAtPath(fullPath, typeof(Mesh));
        }

        private void GetVertices(int vertexcount, Vector3[] sources, Vector3[] main, ref int offset, Matrix4x4 transform)
        {
            for (int i = 0; i < sources.Length; i++)
                main[i + offset] = transform.MultiplyPoint(sources[i]);

            offset += vertexcount;
        }

        private void GetNormal(int vertexcount, Vector3[] sources, Vector3[] main, ref int offset, Matrix4x4 transform)
        {
            for (int i = 0; i < sources.Length; i++)
                main[i + offset] = transform.MultiplyVector(sources[i]).normalized;

            offset += vertexcount;
        }

        private void GetTangents(int vertexcount, Vector4[] sources, Vector4[] main, ref int offset, Matrix4x4 transform)
        {
            for (int i = 0; i < sources.Length; i++)
            {
                Vector4 p4 = sources[i];
                Vector3 p = new Vector3(p4.x, p4.y, p4.z);
                p = transform.MultiplyVector(p).normalized;
                main[i + offset] = new Vector4(p.x, p.y, p.z, p4.w);
            }

            offset += vertexcount;
        }
    }

    [SerializeField]
    public class MeshesGroup
    {
        public Material[] materials { get; private set; }

        private List<MeshFilter> _meshFilters = new List<MeshFilter>();
        public List<MeshFilter> meshFilters
        {
            get
            {
                return _meshFilters;
            }
        }

        private List<Matrix4x4> _matrix = new List<Matrix4x4>();
        public List<Matrix4x4> matrix
        {
            get
            {
                return _matrix;
            }
        }

        public int vertexCount { get; private set; }


        public MeshesGroup(MeshFilter filter)
        {
            materials = filter.GetComponent<Renderer>().sharedMaterials;

            _meshFilters.Add(filter);

            _matrix.Add(filter.transform.localToWorldMatrix);

            vertexCount = filter.sharedMesh.vertexCount;
        }

        public bool CanAdd(MeshFilter filter)
        {
            Material[] materials = filter.GetComponent<Renderer>().sharedMaterials;

            if (this.materials.Length != materials.Length)
                return false;

            if (_meshFilters[0].sharedMesh.subMeshCount != filter.sharedMesh.subMeshCount)
                return false;

            for(int i = 0; i < materials.Length; i++)
                if(this.materials[i] != materials[i])
                    return false;

            if (vertexCount + filter.sharedMesh.vertexCount >= 65536)
                return false;

            return true;
        }

        public int AddMeshFilter(MeshFilter filter)
        {
            //-1 can't add
            //0 already add
            //1 overflow
            //2 done

            if (!CanAdd(filter))
                return -1;

            if (_meshFilters.Contains(filter))
                return 0;

            _meshFilters.Add(filter);
            _matrix.Add(filter.transform.localToWorldMatrix);
            vertexCount += filter.sharedMesh.vertexCount;

            return 2;
        }
    }
}
