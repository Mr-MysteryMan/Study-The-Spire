using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.Characters
{
    [CreateAssetMenu(fileName = "EnemyLib", menuName = "ScriptableObjects/Combat/EnemyLib")]
    public class EnemyLib : ScriptableObject
    {
        [Serializable]
        public class EnmeyGoup
        {
            public List<GameObject> enemies;
        }

        [Serializable]
        public class EnemyEntry
        {
            public EnemyType enemyType;
            public List<EnmeyGoup> enmeyGoups;
        }

        public List<EnemyEntry> enemyGroups;

        public enum EnemyType
        {
            Minor,
            Elite,
            Boss
        }

        public IEnumerable<GameObject> GetEnemy(EnemyType enemyType)
        {
            var enemyGroup = enemyGroups.Find(group => group.enemyType == enemyType);
            if (enemyGroup != null && enemyGroup.enmeyGoups.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, enemyGroup.enmeyGoups.Count);
                var selectedGroup = enemyGroup.enmeyGoups[randomIndex];
                return selectedGroup.enemies;
            }
            else
            {
                Debug.LogError($"No enemies found for type: {enemyType}");
                return null;
            }
        }
    }
}