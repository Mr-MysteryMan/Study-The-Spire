using System.Collections.Generic;
using CombatCM = Combat.CardManager;

namespace Cards
{
    /// <summary>
    /// 用于标记卡牌扩展信息的接口
    /// 会在GameObject形式的卡牌中显示标签对应的描述
    /// </summary>
    public interface ITag
    {
        string Name { get; }
        string Description { get; }

        bool Displayable { get; } // 是否在卡牌上显示
    }

    public interface ITagged
    {
        public List<ITag> Tags { get; }
    }

    /// <summary>
    /// 标记卡牌使用后的卡牌去向的标签
    /// 主要修改CardManager中UseHandCard的逻辑
    /// 例如：使用卡牌后将其放入弃牌堆（默认）
    /// 放入抽牌堆，回到手牌等
    /// </summary>
    public abstract class CardUseTag : ITag
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract bool Displayable { get; }

        // 卡牌使用后的去向
        public abstract void AfterUse(CombatCM cardManager);

        // 回合结束弃牌时
        public abstract void OnTurnEndDiscard(CombatCM cardManager);
    }

    public class BackToHandTag : CardUseTag
    {
        public override string Name => "回手";
        public override string Description => "使用后将卡牌放回手牌。";
        public override bool Displayable => true;

        private ICardData cardData;

        public BackToHandTag(ICardData cardData)
        {
            this.cardData = cardData;
        }

        public override void AfterUse(CombatCM cardManager)
        {
            // 将卡牌放回手牌
            cardManager.MoveTo(cardData, CombatCM.CardStackType.Hand);
        }
        public override void OnTurnEndDiscard(CombatCM cardManager)
        {
            // 回合结束时不进行任何操作 
        }
    }

/*
    public class BackToHandAndUpgardeTag : CardUseTag
    {
        public override string Name => "";
    }
*/
}