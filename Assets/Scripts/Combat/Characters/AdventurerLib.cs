using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Combat.Characters
{
    [CreateAssetMenu(fileName = "AdventurerLib", menuName = "ScriptableObjects/Combat/AdventurerLib")]
    public class AdventurerLib : ScriptableObject
    {
        [System.Serializable]
        public class AdventurerEntry
        {
            public CharacterType Type; // 冒险者类型
            public GameObject Prefab; // 冒险者预制体
        }

        [SerializeField] private List<AdventurerEntry> adventurers; // 冒险者预制体列表

        public GameObject GetAdventurer(CharacterType type)
        {
            return adventurers.Find(adventurer => adventurer.Type == type)?.Prefab;
        }

        public List<GameObject> GetAllAdventurers()
        {
            return adventurers.Select(adv => adv.Prefab).ToList();
        }
    }
}