using UnityEngine;
using UnityEditor;
using System.Collections;

namespace NGS.SuperLevelOptimizer
{
    public class EditorHelper
    {
        public static void DrawWireCube(Vector3 center, Vector3 size, Color color = default(Color))
        {
            var half = size / 2;

            Handles.color = color;

            Handles.DrawLine(center + new Vector3(-half.x, -half.y, half.z), center + new Vector3(half.x, -half.y, half.z));
            Handles.DrawLine(center + new Vector3(-half.x, -half.y, half.z), center + new Vector3(-half.x, half.y, half.z));
            Handles.DrawLine(center + new Vector3(half.x, half.y, half.z), center + new Vector3(half.x, -half.y, half.z));
            Handles.DrawLine(center + new Vector3(half.x, half.y, half.z), center + new Vector3(-half.x, half.y, half.z));

            Handles.DrawLine(center + new Vector3(-half.x, -half.y, -half.z), center + new Vector3(half.x, -half.y, -half.z));
            Handles.DrawLine(center + new Vector3(-half.x, -half.y, -half.z), center + new Vector3(-half.x, half.y, -half.z));
            Handles.DrawLine(center + new Vector3(half.x, half.y, -half.z), center + new Vector3(half.x, -half.y, -half.z));
            Handles.DrawLine(center + new Vector3(half.x, half.y, -half.z), center + new Vector3(-half.x, half.y, -half.z));

            Handles.DrawLine(center + new Vector3(-half.x, -half.y, -half.z), center + new Vector3(-half.x, -half.y, half.z));
            Handles.DrawLine(center + new Vector3(half.x, -half.y, -half.z), center + new Vector3(half.x, -half.y, half.z));
            Handles.DrawLine(center + new Vector3(-half.x, half.y, -half.z), center + new Vector3(-half.x, half.y, half.z));
            Handles.DrawLine(center + new Vector3(half.x, half.y, -half.z), center + new Vector3(half.x, half.y, half.z));
        }
    }
}