using UnityEditor;
using UnityEngine;

namespace EnemyStateMachine.Editor
{
    internal static class NodePosPrefs
    {
        static string Key(string guid) => $"EnemyGraph_NodePos_{guid}";

        public static void Save(string guid, Rect rect)
        {
            var v = new Vector4(rect.x, rect.y, rect.width, rect.height);
            EditorPrefs.SetString(Key(guid), JsonUtility.ToJson(v));
        }

        public static bool Load(string guid, out Rect rect)
        {
            if (EditorPrefs.HasKey(Key(guid)))
            {
                var json = EditorPrefs.GetString(Key(guid));
                var v = JsonUtility.FromJson<Vector4>(json);
                rect = new Rect(v.x, v.y, v.z, v.w);
                return true;
            }
            rect = default;
            return false;
        }
    }
}