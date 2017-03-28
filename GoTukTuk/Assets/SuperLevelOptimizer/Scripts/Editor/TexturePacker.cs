using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace NGS.SuperLevelOptimizer
{
    public enum AtlasSize { _512 = 512, _1024 = 1024, _2048 = 2048, _4096 = 4096, _8192 = 8192 };

    public class TexturePacker
    {
        public void CreateAtlases(Renderer[] renderers, AtlasSize maxAtlasSize, int padding, string folderPath, CoefficientTable coefficientTable)
        {
            if (renderers == null || renderers.Length == 0)
            {
                Debug.Log("No objects found, mark objects as static");
                return;
            }

            MaterialsGroup[] materialsGroups = GetMaterialsGroups(renderers, maxAtlasSize, coefficientTable);

            materialsGroups = materialsGroups.Where(g => g.materials.Distinct().Count() > 1).ToArray();

            for (int i = 0; i < materialsGroups.Length; i++)
            {
                try
                {
                    EditorUtility.DisplayProgressBar("Creating atlases", "Ready : " + i + " of " + materialsGroups.Length, (float)i / materialsGroups.Length);

                    CreateMultimaterial(materialsGroups[i], maxAtlasSize, padding, folderPath);
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex.Message);
                }
                finally
                {
                    materialsGroups[i] = null;

                    System.GC.Collect();
                }
            }

            MaterialsWorker.ClearCache();

            EditorUtility.ClearProgressBar();
        }

        private MaterialsGroup[] GetMaterialsGroups(Renderer[] renderers, AtlasSize maxAtlasSize, CoefficientTable table)
        {
            List<MaterialsGroup> materialsGroups = new List<MaterialsGroup>();

            for (int i = 0; i < renderers.Length; i++)
            {
                Material[] materials = renderers[i].sharedMaterials;

                string[] textureNames = null;
                materials = materials.Where(m => MaterialsWorker.GetTextures(m, ref textureNames).Length > 0).ToArray();

                for (int c = 0; c < materials.Length; c++)
                {
                    bool added = false;

                    for (int p = 0; p < materialsGroups.Count; p++)
                    {
                        if (MaterialsWorker.IsMaterialsMatch(materials[c], materialsGroups[p].comparerMaterial, table))
                        {
                            if (materialsGroups[p].AddMaterial(materials[c], renderers[i], c) != 1)
                            {
                                added = true;
                                break;
                            }
                        }
                    }

                    if (!added)
                    {
                        MaterialsGroup group = new MaterialsGroup(materials[c], maxAtlasSize);

                        group.AddMaterial(materials[c], renderers[i], c);

                        materialsGroups.Add(group);
                    }
                }
            }

            return materialsGroups.ToArray();
        }

        private void CreateMultimaterial(MaterialsGroup materialsGroup, AtlasSize maxAtlasSize, int padding, string folderPath)
        {
            CreateCopyOfMeshes(materialsGroup.renderers.ToArray(), folderPath);

            List<Texture2D[]> textures = materialsGroup.textures;

            for (int i = 0; i < textures.Count; i++)
            {
                MaterialsWorker.AllowAccessToTextures(textures[i]);

                textures[i] = MaterialsWorker.ResizeTextures(textures[i]);

                textures[i] = MaterialsWorker.TileTextures(textures[i], materialsGroup.textureNames[i], materialsGroup.materials[i]);
            }

            Material multimaterial = MaterialsWorker.SaveAssetToFile(new Material(materialsGroup.comparerMaterial), folderPath + "Materials/", "mat");

            Rect[] rects = new Rect[0];

            for (int i = 0; i < textures[0].Length; i++)
            {
                Texture2D[] texturesForAtlas = new Texture2D[textures.Count];

                for (int c = 0; c < textures.Count; c++)
                    texturesForAtlas[c] = textures[c][i];

                Texture2D atlas = new Texture2D(0, 0);

                if (rects.Length == 0)
                    rects = atlas.PackTextures(texturesForAtlas, padding, (int)maxAtlasSize, false);
                else
                    atlas.PackTextures(texturesForAtlas, padding, (int)maxAtlasSize, false);

                atlas = MaterialsWorker.SaveAssetToFile(atlas, folderPath + "Atlases/", "atlas");

                MaterialsWorker.CopyTextureParametrs(textures[0][i], atlas);

                MaterialsWorker.ChangeTextureFormat(atlas, TextureImporterFormat.AutomaticCompressed);

                multimaterial.SetTexture(materialsGroup.textureNames[0][i], atlas);
            }

            for (int i = 0; i < materialsGroup.renderers.Count; i++)
            {
                if (!MaterialsWorker.SetUV(materialsGroup.renderers[i], rects[i], materialsGroup.subMeshIndexes[i]))
                    continue;

                Material[] materials = materialsGroup.renderers[i].sharedMaterials;

                materials[materialsGroup.subMeshIndexes[i]] = multimaterial;

                materialsGroup.renderers[i].sharedMaterials = materials;
            }
        }

        private void CreateCopyOfMeshes(Renderer[] renderers, string folderPath)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                MeshFilter filter = renderers[i].GetComponent<MeshFilter>();

                filter.sharedMesh = MaterialsWorker.SaveAssetToFile(Object.Instantiate(filter.sharedMesh), folderPath + "Meshes/", "mesh");
            }
        }
    }

    [System.Serializable]
    public class MaterialsGroup
    {
        public Material comparerMaterial { get; private set; }
        public float freeSpace { get; private set; }

        [SerializeField]
        private List<Material> _materials = new List<Material>();
        public List<Material> materials
        {
            get
            {
                return _materials;
            }
        }

        [SerializeField]
        private List<int> _subMeshIndexes = new List<int>();
        public List<int> subMeshIndexes
        {
            get
            {
                return _subMeshIndexes;
            }
        }

        [SerializeField]
        private List<Texture2D> _firstTextures = new List<Texture2D>();

        [SerializeField]
        private List<Texture2D[]> _textures = new List<Texture2D[]>();
        public List<Texture2D[]> textures
        {
            get
            {
                return _textures;
            }
        }

        [SerializeField]
        private List<string[]> _textureNames = new List<string[]>();
        public List<string[]> textureNames
        {
            get
            {
                return _textureNames;
            }
        }

        [SerializeField]
        private List<Renderer> _renderers = new List<Renderer>();
        public List<Renderer> renderers
        {
            get
            {
                return _renderers;
            }
        }


        public MaterialsGroup(Material comparerMaterial, AtlasSize maxAtlasSize)
        {
            this.comparerMaterial = comparerMaterial;

            freeSpace = (int)maxAtlasSize * (int)maxAtlasSize;
        }

        public int AddMaterial(Material material, Renderer renderer, int subMeshIndex)
        {
            if (_materials.Contains(material) && _renderers.Contains(renderer) && _subMeshIndexes.Contains(subMeshIndex))
                return 0;

            string[] names = new string[0];
            Texture2D[] textures = MaterialsWorker.GetTextures(material, ref names);

            Texture2D firstTexture = textures[0];

            if (!_firstTextures.Contains(firstTexture))
            {
                if ((freeSpace - (firstTexture.width * firstTexture.height)) < 0)
                    return 1;

                _firstTextures.Add(firstTexture);
                freeSpace -= (firstTexture.width * firstTexture.height);
            }

            _materials.Add(material);
            _textures.Add(textures);
            _textureNames.Add(names);
            _renderers.Add(renderer);
            _subMeshIndexes.Add(subMeshIndex);

            return 2;
        }
    }

    public static class MaterialsWorker
    {
        private static List<Texture2D> resizedOriginals = new List<Texture2D>();
        private static List<Vector2> size = new List<Vector2>();
        private static List<Texture2D> resizedTextures = new List<Texture2D>();

        private static List<Texture2D> tiledOriginals = new List<Texture2D>();
        private static List<Vector2> tiling = new List<Vector2>();
        private static List<Texture2D> tiledTextures = new List<Texture2D>();


        public static bool IsMaterialsMatch(Material mat1, Material mat2, CoefficientTable table)
        {
            if (mat1.shader != mat2.shader)
                return false;

            Shader shader = mat1.shader;
            int count = ShaderUtil.GetPropertyCount(shader);

            for (int i = 0; i < count; i++)
            {
                string propName = ShaderUtil.GetPropertyName(shader, i);
                ShaderUtil.ShaderPropertyType propType = ShaderUtil.GetPropertyType(shader, i);

                #region Float

                if (propType == ShaderUtil.ShaderPropertyType.Float)
                {
                    float float1 = mat1.GetFloat(propName);
                    float float2 = mat2.GetFloat(propName);

                    if (!IsEqual(float1, float2, table.floatValue))
                        return false;
                }

                #endregion

                #region Vector

                if (propType == ShaderUtil.ShaderPropertyType.Vector)
                {
                    Vector4 vector1 = mat1.GetVector(propName);
                    Vector4 vector2 = mat2.GetVector(propName);

                    if (!IsEqual(vector1.x, vector2.x, table.vectorValue.x)) return false;
                    if (!IsEqual(vector1.y, vector2.y, table.vectorValue.y)) return false;
                    if (!IsEqual(vector1.z, vector2.z, table.vectorValue.z)) return false;
                    if (!IsEqual(vector1.w, vector2.w, table.vectorValue.w)) return false;
                }

                #endregion

                #region Color

                if (propType == ShaderUtil.ShaderPropertyType.Color)
                {
                    Color color1 = mat1.GetColor(propName);
                    Color color2 = mat2.GetColor(propName);

                    if (!IsEqual(color1.r, color2.r, table.colorValue.r)) return false;
                    if (!IsEqual(color1.g, color2.g, table.colorValue.g)) return false;
                    if (!IsEqual(color1.b, color2.b, table.colorValue.b)) return false;
                    if (!IsEqual(color1.a, color2.a, table.colorValue.a)) return false;
                }

                #endregion

                #region Texture

                if (propType == ShaderUtil.ShaderPropertyType.TexEnv)
                {
                    bool isNull1 = mat1.GetTexture(propName) == null;
                    bool isNull2 = mat2.GetTexture(propName) == null;

                    if (isNull1 != isNull2) return false;
                }

                #endregion
            }

            return true;
        }

        public static bool IsEqual(this float arg1, float arg2, float range = 0.00001f)
        {
            return Mathf.Abs(arg1 - arg2) <= range;
        }

        public static Texture2D GetTexture(Material material)
        {
            string[] names = null;
            Texture2D[] textures = GetTextures(material, ref names);

            if (textures == null || textures.Length == 0)
                return null;

            return textures[0];
        }

        public static Texture2D[] GetTextures(Material material, ref string[] textureNames)
        {
            List<Texture2D> textures = new List<Texture2D>();
            List<string> names = new List<string>();

            Shader shader = material.shader;
            int count = ShaderUtil.GetPropertyCount(shader);

            for (int i = 0; i < count; i++)
            {
                if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
                    if (ShaderUtil.GetTexDim(shader, i) == UnityEngine.Rendering.TextureDimension.Tex2D)
                    {
                        Texture2D texture = material.GetTexture(ShaderUtil.GetPropertyName(shader, i)) as Texture2D;

                        if (texture != null)
                        {
                            names.Add(ShaderUtil.GetPropertyName(shader, i));
                            textures.Add(texture);
                        }
                    }
            }

            if (textureNames != null) textureNames = names.ToArray();

            return textures.ToArray();
        }

        public static void AllowAccessToTextures(Texture2D[] textures)
        {
            for (int i = 0; i < textures.Length; i++)
                AllowAccessToTexture(textures[i]);
        }

        public static void AllowAccessToTexture(Texture2D texture)
        {
            TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(texture));

            if (importer == null) return;

            if (importer.isReadable && importer.textureFormat == TextureImporterFormat.ARGB32) return;

            importer.isReadable = true;

            importer.textureFormat = TextureImporterFormat.ARGB32;

            importer.SaveAndReimport();
        }

        public static void ChangeTextureFormat(Texture2D texture, TextureImporterFormat format)
        {
            TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(texture));

            if (importer == null) return;

            importer.textureFormat = format;

            importer.SaveAndReimport();
        }

        public static void CopyTextureParametrs(Texture2D original, Texture2D copy)
        {
            TextureImporter originalImporter = (TextureImporter)AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(original));
            TextureImporter copyImporter = (TextureImporter)AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(copy));

            if (originalImporter == null || copyImporter == null)
                return;

            copyImporter.textureType = originalImporter.textureType;

            TextureImporterSettings settings = new TextureImporterSettings();

            originalImporter.ReadTextureSettings(settings);

            copyImporter.SetTextureSettings(settings);

            copyImporter.SaveAndReimport();
        }

        public static Texture2D[] ResizeTextures(Texture2D[] textures)
        {
            Texture2D[] newTextures = new Texture2D[textures.Length];

            int width = textures[0].width;
            int height = textures[0].height;

            for (int i = 0; i < textures.Length; i++)
                newTextures[i] = ResizeTexture(textures[i], width, height);

            return newTextures;
        }

        public static Texture2D ResizeTexture(Texture2D texture, int width, int height)
        {
            if (texture.width == width && texture.height == height)
                return texture;

            Texture2D result = GetResized(texture, width, height);

            if (result != null)
                return result;

            result = new Texture2D(width, height, TextureFormat.RGBA32, false);
            Color[] rpixels = result.GetPixels();

            float incX = (1.0f / width);
            float incY = (1.0f / height);

            for (int px = 0; px < rpixels.Length; px++)
                rpixels[px] = texture.GetPixelBilinear(incX * ((float)px % width), incY * ((float)Mathf.Floor(px / width)));

            result.SetPixels(rpixels);
            result.Apply(true, false);

            SetResized(texture, result, width, height);

            return result;
        }

        public static Texture2D[] TileTextures(Texture2D[] textures, string[] textureNames, Material material)
        {
            Texture2D[] newTextures = new Texture2D[textures.Length];

            Vector2 tiling = material.shader.name.Contains("Standard") ? material.GetTextureScale("_MainTex") : Vector2.one;

            for (int i = 0; i < textures.Length; i++)
            {
                if (!material.shader.name.Contains("Standard"))
                    tiling = material.GetTextureScale(textureNames[i]);

                else if (textureNames[i] == "_DetailAlbedoMap")
                    tiling = material.GetTextureScale(textureNames[i]);

                if (IsEqual(tiling.x, 1) && IsEqual(tiling.y, 1))
                {
                    newTextures[i] = textures[i];
                    continue;
                }

                newTextures[i] = TileTexture(textures[i], tiling);
                material.SetTextureScale(textureNames[i], Vector2.one);
            }

            return newTextures;
        }

        public static Texture2D TileTexture(Texture2D texture, Vector2 tiling)
        {
            if (Mathf.RoundToInt(tiling.x) == 0 || Mathf.RoundToInt(tiling.y) == 0)
                return texture;

            Texture2D tex = GetTiled(texture, tiling);

            if (tex != null)
                return tex;

            tex = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, true);

            int t_width = texture.width / Mathf.RoundToInt(tiling.x);
            int t_height = texture.height / Mathf.RoundToInt(tiling.y);

            int offset_x = 0, offset_y = 0;
            for (int i = 0; i < Mathf.RoundToInt(tiling.x); i++)
            {
                for (int c = 0; c < Mathf.RoundToInt(tiling.y); c++)
                {
                    int x = 0;
                    for (int p = 0; p < texture.width; p += Mathf.RoundToInt(tiling.x))
                    {
                        int y = 0;
                        for (int j = 0; j < texture.height; j += Mathf.RoundToInt(tiling.y))
                        {
                            tex.SetPixel(x + offset_x, y + offset_y, texture.GetPixel(p, j));
                            y++;
                        }
                        x++;
                    }
                    offset_y += t_height;
                }
                offset_x += t_width;
            }

            tex.Apply(true, false);

            SetTiled(texture, tex, tiling);

            return tex;
        }

        public static T SaveAssetToFile<T>(T asset, string path, string name) where T : Object
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fullPath = path + name + asset.GetHashCode();

            if (asset is Texture2D)
            {
                Texture2D texture = asset as Texture2D;

                byte[] bytes = texture.EncodeToPNG();

                fullPath += ".png";

                using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.ReadWrite))
                {
                    BinaryWriter writer = new BinaryWriter(stream);

                    writer.Write(bytes);

                    writer.Close();
                }

                AssetDatabase.ImportAsset(fullPath);

                AllowAccessToTexture(AssetDatabase.LoadAssetAtPath(fullPath, typeof(Texture2D)) as Texture2D);

                return (T)AssetDatabase.LoadAssetAtPath(fullPath, typeof(Texture2D));
            }
            else
            {
                fullPath += ".asset";

                AssetDatabase.CreateAsset(asset, fullPath);

                AssetDatabase.ImportAsset(fullPath);

                return (T)AssetDatabase.LoadAssetAtPath(fullPath, typeof(T));
            }
        }

        public static bool SetUV(Renderer renderer, Rect rect, int subMeshIndex)
        {
            MeshFilter filter = renderer.GetComponent<MeshFilter>();

            if (filter.sharedMesh.subMeshCount <= subMeshIndex)
                return false;

            int[] triangles = filter.sharedMesh.GetTriangles(subMeshIndex).Distinct().ToArray();
            Vector2[] uv = filter.sharedMesh.uv;

            for (int i = 0; i < triangles.Length; i++)
            {
                int idx = triangles[i];

                if (uv[idx].x > 1 || uv[idx].x < 0)
                    return false;

                if (uv[idx].y > 1 || uv[idx].y < 0)
                    return false;

                uv[idx] = new Vector2((float)(((float)uv[idx].x * (float)rect.width) + (float)rect.x), (float)(((float)uv[idx].y * (float)rect.height) + (float)rect.y));
            }

            filter.sharedMesh.uv = uv;

            return true;
        }

        public static void ClearCache()
        {
            resizedOriginals.Clear();

            size.Clear();

            resizedTextures.Clear();

            tiledOriginals.Clear();

            tiling.Clear();

            tiledTextures.Clear();
        }


        private static void SetResized(Texture2D original, Texture2D resized, int width, int height)
        {
            resizedOriginals.Add(original);

            resizedTextures.Add(resized);

            size.Add(new Vector2(width, height));
        }

        private static Texture2D GetResized(Texture2D original, int width, int height)
        {
            Texture2D resized = null;

            for (int i = 0; i < resizedOriginals.Count; i++)
            {
                if (resizedOriginals[i] == original)
                    if (IsEqual(width, size[i].x) && IsEqual(height, size[i].y))
                    {
                        resized = resizedTextures[i];
                        break;
                    }
            }

            return resized;
        }

        private static void SetTiled(Texture2D original, Texture2D tiled, Vector2 tile)
        {
            tiledOriginals.Add(original);

            tiledTextures.Add(tiled);

            tiling.Add(tile);
        }

        private static Texture2D GetTiled(Texture2D original, Vector2 tile)
        {
            Texture2D tiled = null;

            for (int i = 0; i < tiledOriginals.Count; i++)
            {
                if (tiledOriginals[i] == original)
                    if (tiling[i] == tile)
                    {
                        tiled = tiledTextures[i];
                        break;
                    }
            }

            return tiled;
        }
    }
}
