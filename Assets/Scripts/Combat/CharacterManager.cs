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
        private List<Adventurer> allPlayerCharacters;
        private List<Adventurer> AlivePlayerCharacters => allPlayerCharacters.Where(c => !c.IsDead).ToList(); // 活着的玩家角色列表
        private List<Enemy> monsterCharacters;

        public Character SystemCharacter => systemCharacter; // 系统角色
        public Character PlayerCharacter => AlivePlayerCharacters.First(); // 玩家角色
        public List<Enemy> MonsterCharacters => monsterCharacters; // 怪物角色

        private List<Character> DeathPendingCharacters = new List<Character>(); // 死亡待处理角色列表
        public int DeathPending = 0;

        public List<Adventurer> GetAllPlayerCharacters() => allPlayerCharacters;

        public List<Character> AllCharacters => new List<Character> { PlayerCharacter }.Concat(monsterCharacters).ToList(); // 所有角色列表

        public void Init(Character systemCharacter, List<CharacterInfo> characters, EnemyType enemyType)
        {
            this.systemCharacter = systemCharacter;
            this.allPlayerCharacters = new List<Adventurer>();
            monsterCharacters = new List<Enemy>();
            CreateCharacters(characters, enemyType);
        }


        private void CreateCharacters(List<CharacterInfo> characters, EnemyType enemyType)
        {
            // 指定角色位置
            Vector3 centerPlayerPosition = new Vector3(-240, 20, 0); // 玩家位置
            Vector3 centerMonsterPos = new Vector3(240, 20, 0); // 敌人位置

            // 创建玩家角色
            foreach (var info in characters.Where(c => !c.IsDead))
            {
                var playerPosition = GetPosition(CharacterSide.Player, allPlayerCharacters.Count, characters.Count, centerPlayerPosition);
                var playerPrefab = GetAdventurerPrefab(info.characterType);
                var player = CreateCharacter(playerPosition, playerPrefab) as Adventurer;
                player.SetInitHP(info.MaxHealth, info.Health); // 设置初始生命值
                player.SetPowers(info.AttackPower, info.DefensePower, info.HealPower);
                player.InitCharacter();
                player.InitOriInfo(info); // 初始化原始角色信息
                allPlayerCharacters.Add(player); // 添加玩家角色
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
                adventurer.SetDead(); // 设置角色为死亡状态
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

        public bool IsLose() => AlivePlayerCharacters.Count == 0;
        public bool IsWin() => monsterCharacters.Count == 0 || monsterCharacters.All(c => c.IsDead);

        public List<CharacterInfo> GetPlayerCharacterInfos()
        {
            return allPlayerCharacters.Select(c => c.ToInfo()).ToList();
        }
    }
}