using UnityEngine;

namespace Cards
{
    public class CardResources
    {
        public static Sprite AttackSprite => Resources.Load<Sprite>("CardUI/Attack");
        public static Sprite DefenseSprite => Resources.Load<Sprite>("CardUI/Defense");
        public static Sprite SkillSprite => Resources.Load<Sprite>("CardUI/Skill");
        public static Sprite NoneSprite => Resources.Load<Sprite>("CardUI/None");

        public static Sprite AttackCardSprite => Resources.Load<Sprite>("CardUI/AttackCardSprite");
        public static Sprite DefenseCardSprite => Resources.Load<Sprite>("CardUI/DefenseCardSprite");
        public static Sprite SkillCardSprite => Resources.Load<Sprite>("CardUI/SkillCardSprite");

        public static Sprite QuestionCardSprite => Resources.Load<Sprite>("CardUI/QuestionCardSprite");

        public static GameObject QuestionPrefab => Resources.Load<GameObject>("CardPrefabs/Question");
    }
}