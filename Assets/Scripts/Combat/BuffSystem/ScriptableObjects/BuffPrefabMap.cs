using System;
using System.Collections.Generic;
using UnityEngine;
namespace Combat.Buffs
{
    [CreateAssetMenu(fileName = "BuffPrefabMap", menuName = "Combat/BuffSystem/BuffPrefabMap")]
    public class BuffPrefabMap : ScriptableObject
    {

        [SerializeField] private GameObject DefaultPrefab;
        [SerializeField] private GameObject DefaultBuffPrefab;
        [SerializeField] private GameObject DefaultDebuffPrefab;

        [Serializable]
        public class BuffPrefabMapEntry
        {
            public string BuffName;
            public GameObject Prefab;
        }

        [SerializeField] private BuffPrefabMapEntry[] BuffPrefabMapEntries;

        private Dictionary<string, GameObject> prefabDict;

        public GameObject GetBuffPrefab(IBuff buff)
        {
            if (prefabDict == null) {
                Init();
            }
            var buffName = buff.Name;
            var prefab = prefabDict.GetValueOrDefault(buffName, null);
            if (prefab != null)
            {
                return prefab;
            }
            else
            {
                return buff.BuffType switch
                {
                    BuffType.Buff => DefaultBuffPrefab,
                    BuffType.Debuff => DefaultDebuffPrefab,
                    _ => DefaultPrefab,
                };
            }
        }

        private void Init() {
            prefabDict = new Dictionary<string, GameObject>();
            foreach (var entry in BuffPrefabMapEntries)
            {
                if (!prefabDict.ContainsKey(entry.BuffName))
                {
                    prefabDict.Add(entry.BuffName, entry.Prefab);
                }
                else
                {
                    Debug.LogWarning($"Duplicate buff name found: {entry.BuffName}. Ignoring this entry.");
                }
            }
        }

    }
}