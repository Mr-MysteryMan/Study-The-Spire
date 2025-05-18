using Cards.CardEffect;
using UnityEngine;

public enum CardEffectTarget
{
    None,
    AdventurerSelf,
    AdventurerOne,
    AdventurerAll,
    EnemyOne,
    EnemyAll,

    CharacterOne,
    CharacterAll,

    NotPlayable,
}

public enum CardCategory
{
    None,
    Attack,
    Skill,
    Status,
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

    void Modify(float factor); // 修改方法
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

    public abstract void Modify(float factor); // 修改方法
}

public abstract class BasicCardData : CardData
{
    public BasicCardData(string name, int cardCost, CardCategory cardCategory, CardEffectTarget cardEffectTarget, Sprite sprite, IEffect effect)
    {
        this.cardCost = cardCost; // 设置卡牌费用
        this.cardName = name; // 设置卡牌名称
        this.cardCategory = cardCategory; // 设置卡牌分类
        this.cardEffectTarget = cardEffectTarget; // 设置卡牌效果目标
        this.sprite = sprite; // 设置卡牌图片
    }

    private int cardCost; // 卡牌费用
    private string cardName; // 卡牌名称
    private IEffect effect; // 卡牌效果

    private CardCategory cardCategory; // 卡牌分类

    private CardEffectTarget cardEffectTarget; // 卡牌效果目标

    private Sprite sprite; // 卡牌图片
    public override CardCategory CardCategory => cardCategory; // 卡牌分类属性
    public override CardEffectTarget CardEffectTarget => cardEffectTarget; // 卡牌效果目标属性
    public override Sprite Sprite => sprite; // 卡牌图片属性
    public override int Cost => cardCost; // 卡牌费用属性
    public override string CardName => cardName; // 卡牌名称属性
    public override IEffect Effect => effect; // 卡牌效果属性
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