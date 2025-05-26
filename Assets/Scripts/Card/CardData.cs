using Cards.CardEffect;
using Combat;
using UnityEngine;

/// <summary>
/// 卡牌效果目标枚举
/// </summary>
public enum CardEffectTarget
{
    /// <summary>
    /// 无效目标
    /// </summary>
    None,

    /// <summary>
    /// 己方自身
    /// </summary>
    AdventurerSelf,

    /// <summary>
    /// 己方单体目标
    /// </summary>
    AdventurerOne,

    /// <summary>
    /// 己方无特定目标，全体或随机
    /// </summary>
    AdventurerAll,

    /// <summary>
    /// 敌方单体目标
    /// </summary>
    EnemyOne,

    /// <summary>
    /// 敌方无特定目标，全体或随机
    /// </summary>
    EnemyAll,

    /// <summary>
    /// 单体目标
    /// </summary>

    CharacterOne,

    /// <summary>
    /// 无特定目标，全体或随机
    /// </summary>
    CharacterAll,

    /// <summary>
    /// 不可释放
    /// </summary>
    NotPlayable,
}

public static class CardEffectTargetExtensions
{
    public static bool IsValidTarget(this CardEffectTarget target)
    {
        return target != CardEffectTarget.None && target != CardEffectTarget.NotPlayable;
    }

    public static bool IsSingleTarget(this CardEffectTarget target)
    {
        return target == CardEffectTarget.AdventurerOne || target == CardEffectTarget.EnemyOne || target == CardEffectTarget.CharacterOne;
    }

    public static bool IsMultiTarget(this CardEffectTarget target)
    {
        return target == CardEffectTarget.AdventurerAll || target == CardEffectTarget.EnemyAll || target == CardEffectTarget.CharacterAll;
    }

    public static bool IsEnemyTarget(this CardEffectTarget target)
    {
        return target == CardEffectTarget.EnemyOne || target == CardEffectTarget.EnemyAll;
    }

    public static bool IsAdventurerTarget(this CardEffectTarget target)
    {
        return target == CardEffectTarget.AdventurerOne || target == CardEffectTarget.AdventurerAll || target == CardEffectTarget.AdventurerSelf;
    }

    public static bool IsCharacterTarget(this CardEffectTarget target)
    {
        return target == CardEffectTarget.CharacterOne || target == CardEffectTarget.CharacterAll;
    }

    public static bool IsDragToSelectTarget(this CardEffectTarget target)
    {
        return target == CardEffectTarget.AdventurerOne || target == CardEffectTarget.EnemyOne || target == CardEffectTarget.CharacterOne;
    }

    public static bool IsMoveToSelectTarget(this CardEffectTarget target)
    {
        return target == CardEffectTarget.AdventurerAll || target == CardEffectTarget.EnemyAll || target == CardEffectTarget.CharacterAll ||
            target == CardEffectTarget.AdventurerSelf;
    }
}

public enum CardCategory
{
    None,
    Attack,
    Skill,
    Status,
}

public enum ModifyType
{
    All,
    Attack,
    Heal,
    Denfense,
}
public interface ICardData
{
    CardCategory CardCategory { get; } // 卡牌分类
    CardEffectTarget CardEffectTarget { get; } // 卡牌类型
    int CardId { get; } // 卡牌ID
    int Cost { get; } // 卡牌费用

    bool IsDiscarded { get; } // 是否是弃牌

    Sprite Sprite { get; } // 卡牌图片
    IEffect Effect { get; } // 卡牌效果

    string Desc { get; } // 卡牌描述

    string CardName { get; } // 卡牌名称

    void Discard(); // 弃牌方法
    void Reset(); // 重置方法

    object Clone(); // 克隆方法

    void Modify(float factor, ModifyType type); // 修改方法
    void Modify(Character character);
}

public abstract class CardData : ICardData
{
    private bool isDiscarded; // 是否是弃牌

    private int cardId; // 卡牌ID

    public CardData()
    {
        // 生成唯一的卡牌ID 使用系统时间戳
        cardId = (int)((System.DateTime.Now.Ticks + Random.Range(99, 99999)) % int.MaxValue);
    }

    public bool IsDiscarded => isDiscarded;

    public int CardId => cardId;

    public abstract CardCategory CardCategory { get; }
    public abstract CardEffectTarget CardEffectTarget { get; }
    public abstract int Cost { get; }
    public abstract Sprite Sprite { get; }
    public abstract IEffect Effect { get; }
    public abstract string Desc { get; }
    public abstract string CardName { get; }

    public void Discard()
    {
        isDiscarded = true; // 标记为弃牌
    }

    public void Reset()
    {
        isDiscarded = false; // 重置为非弃牌
    }

    public abstract void Modify(float factor, ModifyType modifyType); // 修改方法
    public abstract void Modify(Character character); // 修改方法
    public abstract object Clone();
}

public class BasicCardData : CardData
{
    public BasicCardData(string name, string desc, int cardCost, CardCategory cardCategory, CardEffectTarget cardEffectTarget, Sprite sprite, IEffect effect)
    {
        this.cardCost = cardCost; // 设置卡牌费用
        this.cardName = name; // 设置卡牌名称
        this.cardCategory = cardCategory; // 设置卡牌分类
        this.cardEffectTarget = cardEffectTarget; // 设置卡牌效果目标
        this.sprite = sprite; // 设置卡牌图片
        this.desc = desc;
    }

    private int cardCost; // 卡牌费用
    private string cardName; // 卡牌名称
    private IEffect effect; // 卡牌效果

    private CardCategory cardCategory; // 卡牌分类

    private CardEffectTarget cardEffectTarget; // 卡牌效果目标

    private Sprite sprite; // 卡牌图片

    private string desc; // 卡牌描述
    public override CardCategory CardCategory => cardCategory; // 卡牌分类属性
    public override CardEffectTarget CardEffectTarget => cardEffectTarget; // 卡牌效果目标属性
    public override Sprite Sprite => sprite; // 卡牌图片属性
    public override int Cost => cardCost; // 卡牌费用属性
    public override string CardName => cardName; // 卡牌名称属性
    public override IEffect Effect => effect; // 卡牌效果属性
    public override string Desc => desc; // 卡牌描述属性

    public override void Modify(float factor, ModifyType modifyType)
    { }

    public override void Modify(Character character)
    {}
    
    public override object Clone()
    {
        return new BasicCardData(cardName, desc, cardCost, cardCategory, cardEffectTarget, sprite, effect);
    }
}

public static class CardDataExtensions
{
    public static string GetDebugInfo(this ICardData cardData)
    {
        return $"[卡牌] ID: {cardData.CardId}, " +
               $"名称: {cardData.CardName}, " +
               $"描述: {cardData.Desc}, " +
               $"费用: {cardData.Cost}, " +
               $"分类: {cardData.CardCategory}, " +
               $"效果目标: {cardData.CardEffectTarget}, " +
               $"效果: {cardData.Effect.GetType().Name}";
    }

    public static string GetShortInfo(this ICardData cardData)
    {
        return $"[卡牌] ID: {cardData.CardId}, " +
               $"名称: {cardData.CardName}, " +
               $"描述: {cardData.Desc}";
    }
}