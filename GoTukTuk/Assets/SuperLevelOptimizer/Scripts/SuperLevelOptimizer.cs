using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace NGS.SuperLevelOptimizer
{
    public enum BakeType { FullScene, Zonal }
    public enum SearchState { Automatical, User }
    public enum CombineState { CombineToScene, CombineToPrefab }

    public class SuperLevelOptimizer : MonoBehaviour
    {
        public bool saveColliders = true;

        public BakeType bakeType = BakeType.FullScene;

        [SerializeField]
        private Vector3 _zoneCount = Vector3.one;
        public Vector3 zoneCount
        {
            get
            {
                return _zoneCount;
            }

            set
            {
                _zoneCount.x = Mathf.Max(1, value.x);
                _zoneCount.y = Mathf.Max(1, value.y);
                _zoneCount.z = Mathf.Max(1, value.z);
            }
        }

        public SearchState searchState = SearchState.Automatical;

        [SerializeField]
        private string _folderPath = "Assets/SLO/Prefabs/";
        public string folderPath
        {
            get
            {
                return _folderPath;
            }

            set
            {
                if (!value.StartsWith("Assets/"))
                    value = "Assets/" + value;

                if (!value.EndsWith("/"))
                    value = value + "/";

                _folderPath = value;
            }
        }

        public CombineState combineState = CombineState.CombineToScene;

        [SerializeField]
        private CoefficientTable _coefficientTable = new CoefficientTable();
        public CoefficientTable coefficientTable
        {
            get
            {
                return _coefficientTable;
            }
        }

        [SerializeField]
        private List<Renderer> _objectsForCombine = new List<Renderer>();
        public List<Renderer> objectsForCombine
        {
            get
            {
                return _objectsForCombine;
            }
        }

        [SerializeField]
        private List<Renderer> _tempObjects = new List<Renderer>();
        public List<Renderer> tempObjects
        {
            get
            {
                return _tempObjects;
            }
        }

        public void AddObjectsForCombine(IEnumerable<Renderer> renderers)
        {
            foreach (var renderer in renderers)
                AddObjectForCombine(renderer);
        }

        public void AddObjectForCombine(Renderer renderer)
        {
            if (!_objectsForCombine.Contains(renderer))
                _objectsForCombine.Add(renderer);
        }

        public void DeleteObjectsForCombine(IEnumerable<Renderer> renderers)
        {
            foreach (var renderer in renderers)
                DeleteObjectForCombine(renderer);
        }

        public void DeleteObjectForCombine(Renderer renderer)
        {
            if (_objectsForCombine.Contains(renderer))
                _objectsForCombine.Remove(renderer);
        }

        public void ClearObjectsForCombine()
        {
            _objectsForCombine.Clear();
        }


        public void AddTempObjects(IEnumerable<Renderer> temp)
        {
            _tempObjects.AddRange(temp);
        }

        public void ClearTemp()
        {
            _tempObjects.Clear();
        }
    }

    [Serializable]
    public class CoefficientTable
    {
        [SerializeField]
        private float _floatValue = 0.1f;
        public float floatValue
        {
            get
            {
                return _floatValue;
            }

            set
            {
                _floatValue = Mathf.Max(value, 0);
            }
        }

        [SerializeField]
        private Vector4 _vectorValue = new Vector4(0.1f, 0.1f, 0.1f, 0.1f);
        public Vector4 vectorValue
        {
            get
            {
                return _vectorValue;
            }

            set
            {
                _vectorValue.x = Mathf.Max(0, value.x);
                _vectorValue.y = Mathf.Max(0, value.y);
                _vectorValue.z = Mathf.Max(0, value.z);
                _vectorValue.w = Mathf.Max(0, value.w);
            }
        }

        [SerializeField]
        private Color _colorValue = new Color(0.1f, 0.1f, 0.1f, 0.1f);
        public Color colorValue
        {
            get
            {
                return _colorValue;
            }

            set
            {
                _colorValue.r = Mathf.Max(0, value.r);
                _colorValue.g = Mathf.Max(0, value.g);
                _colorValue.b = Mathf.Max(0, value.b);
                _colorValue.a = Mathf.Max(0, value.a);
            }
        }


        public CoefficientTable() { }

        public CoefficientTable(float floatValue, Vector4 vectorValue, Color colorValue)
        {
            this.floatValue = floatValue;

            this.vectorValue = vectorValue;

            this.colorValue = colorValue;
        }
    }
}
