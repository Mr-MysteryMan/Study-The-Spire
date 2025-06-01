using System.Collections.Generic;
using System.Linq;
using Combat.Characters;
using UnityEngine;
using UnityEngine.Assertions;
using static Combat.Characters.EnemyLib;
using GlobalCardManager = CardManager;

namespace Combat
{
    public class CharacterManager : MonoBehaviour
    {
        [SerializeField] private EnemyLib enemyLibSO; // 怪物库，包含所有怪物的预制体
        [SerializeField] private AdventurerLib adventurerLibSO; // 冒险者库，包含所有冒险者的预制体

        [SerializeField] private float enemyMargin = 120f;

        [SerializeField] private CombatSystem combatSystem;

        [SerializeField] private Transform uiTransform;

        // 系统角色，用于一些没有直接来源的命令，暂时没用
        private Character systemCharacter;

        // 角色列表，所有参与战斗的角色
        private List<Character> playerCharacters;

        private List<Enemy> monsterCharacters;

        public Character SystemCharacter => systemCharacter; // 系统角色
        public Character PlayerCharacter => playerCharacters.First(); // 玩家角色
        public List<Enemy> MonsterCharacters => monsterCharacters; // 怪物角色

        private List<Character> DeathPendingCharacters = new List<Character>(); // 死亡待处理角色列表
        public int DeathPending = 0;


        public List<Character> AllCharacters => new List<Character> { PlayerCharacter }.Concat(monsterCharacters).ToList(); // 所有角色列表

        public void Init(Character systemCharacter, List<CharacterType> characters, int Hp, int MaxHp, EnemyType enemyType)
        {
            this.systemCharacter = systemCharacter;
            playerCharacters = new List<Character>();
            monsterCharacters = new List<Enemy>();
            CreateCharacters(characters, Hp, MaxHp, enemyType);
        }


        private void CreateCharacters(List<CharacterType> characters, int Hp, int MaxHp, EnemyType enemyType)
        {
            // 指定角色位置
            Vector3 centerPlayerPosition = new Vector3(-240, 20, 0); // 玩家位置
            Vector3 centerMonsterPos = new Vector3(240, 20, 0); // 敌人位置

            // 创建玩家角色
            var playerCharacterInfos = GetCharacterInfos(characters, Hp, MaxHp);
            foreach (var info in playerCharacterInfos)
            {
                var playerPosition = GetPosition(CharacterSide.Player, playerCharacters.Count, playerCharacterInfos.Count, centerPlayerPosition);
                var playerPrefab = GetAdventurerPrefab(info.Type);
                var player = CreateCharacter(playerPosition, playerPrefab);
                player.SetInitHP(info.Hp, info.MaxHp); // 设置初始生命值
                player.InitCharacter();
                playerCharacters.Add(player); // 添加玩家角色
            }

            // 创建怪物角色
            var monsterPrefabs = enemyLibSO.GetEnemy(enemyType).ToList();
            for (int i = 0; i < monsterPrefabs.Count; i++)
            {
                var monsterPosition = GetPosition(CharacterSide.Enemy, i, monsterPrefabs.Count, centerMonsterPos);
                var monster = CreateCharacter(monsterPosition, monsterPrefabs[i]);
                monster.InitCharacter();
                monsterCharacters.Add(monster as Enemy); // 添加怪物角色
            }
        }

        private class CharacterInfo
        {
            public CharacterType Type;
            public int Hp, MaxHp;

            public CharacterInfo(CharacterType type, int hp, int maxHp)
            {
                Type = type;
                Hp = hp;
                MaxHp = maxHp;
            }
        }

        private List<CharacterInfo> GetCharacterInfos(List<CharacterType> characters, int hp, int maxHp)
        {
            int n = characters.Count;
            var characterInfos = new List<CharacterInfo>();

            int _maxHp = maxHp / n;
            int rem = maxHp % n;
            List<int> maxHps = new List<int>(new int[n]);
            for (int i = 0; i < n; i++)
            {
                maxHps[i] = _maxHp + (rem > 0 ? 1 : 0);
                rem--;
            }

            int hpSum = 0;
            for (int i = 0; i < n; i++)
            {
                int curHp = maxHps[i];
                hpSum += curHp;
                if (hpSum > hp)
                {
                    curHp -= hpSum - hp; // 调整当前生命值，确保总和不超过玩家的生命值
                    characterInfos.Add(new CharacterInfo(characters[i], curHp, maxHps[i]));
                    break;
                }
                characterInfos.Add(new CharacterInfo(characters[i], curHp, maxHps[i]));
            }

            return characterInfos;
        }

        /* 处理角色的部分函数 */
        private enum CharacterSide
        {
            Player,
            Enemy
        }

        private Vector3 GetPosition(CharacterSide side, int index, int total, Vector3 center)
        {
            float offset = side == CharacterSide.Player ? -1 : 1; // 玩家在左边，敌人在右边
            var startPos = center - new Vector3(enemyMargin * (total - 1) / 2, 0, 0) * offset;
            return startPos + new Vector3(enemyMargin * index, 0, 0) * offset;
        }

        private Character CreateCharacter(Vector3 position, GameObject prefab)
        {
            var Obj = Instantiate(prefab, uiTransform);
            Obj.transform.localPosition = position;
            var character = Obj.GetComponent<Character>();
            character.combatSystem = this.combatSystem; // 设置战斗系统为自身
            RegisterBasicRules(character);
            return character;
        }

        private GameObject GetAdventurerPrefab(CharacterType type)
        {
            var prefab = adventurerLibSO.GetAdventurer(type);
            return prefab;
        }

        private void RegisterBasicRules(Character character)
        {
            this.combatSystem.RegisterProcessorForCharacter(character);
        }

        public void LockDead()
        {
            DeathPending++;
        }

        public void KillCharacter(Character character)
        {
            if (DeathPending > 0)
            {
                character.SetDead();
                DeathPendingCharacters.Add(character);
                return;
            }
            character.SetDead();
            KillOneCharacter(character); // 销毁角色
        }

        private void KillOneCharacter(Character character)
        {
            if (character is Adventurer adventurer) // 如果是玩家角色
            {
                playerCharacters.Remove(adventurer);
            }
            else if (character is Enemy enemy) // 如果是怪物角色
            {
                monsterCharacters.Remove(enemy); // 从怪物列表中移除
            }
            Destroy(character.gameObject); // 销毁角色对象
        }

        public void ReleaseDead()
        {
            DeathPending--;
            if (DeathPending == 0)
            {
                ProcessDeathPendingCharacters();
            }
        }

        private void ProcessDeathPendingCharacters()
        {
            foreach (var character in DeathPendingCharacters)
            {
                KillOneCharacter(character);
            }
            DeathPendingCharacters.Clear();
        }

        public bool IsLose() => playerCharacters.Count == 0 || playerCharacters.All(c => c.IsDead);
        public bool IsWin() => monsterCharacters.Count == 0 || monsterCharacters.All(c => c.IsDead);
    }
}