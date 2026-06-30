using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Cards.Modifier;
using Combat.Characters;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using GlobalCardManager = CardManager;

namespace Combat
{

    public class CardManager : MonoBehaviour
    {
        public GameObject CardPrefab; // 卡片预制件
        public GameObject CardDeck; // 手牌区

        public Text EnergySpotText; // 能量点

        private int energy = 0;

        public int EnergyPoint => energy; // 能量点属性

        public List<ICardData> NewCardData; // 新卡片数据列表
        public List<ICardData> HandCardData; // 手牌数据列表
        public List<ICardData> DiscardCardData; // 弃牌数据列表

        public List<ICardData> PendingCardData; // 待处理卡片数据列表

        public enum CardStackType
        {
            Hand, // 手牌
            Discard, // 弃牌
            DrawPile, // 抽牌堆
            Pending,
            All,
        }

        public List<ICardData> GetCardStack(CardStackType type)
        {
            return type switch
            {
                CardStackType.Hand => HandCardData,
                CardStackType.Discard => DiscardCardData,
                CardStackType.DrawPile => NewCardData,
                CardStackType.Pending => PendingCardData,
                CardStackType.All => NewCardData.Concat(HandCardData).Concat(DiscardCardData).Concat(PendingCardData).ToList(),
                _ => new List<ICardData>()
            };
        }

        private GlobalCardManager globalCardManager = GlobalCardManager.Instance; // 全局卡片管理器

        private List<GameObject> cards = new List<GameObject>(); // 卡片列表

        private CombatSystem combatSystem;

        [SerializeField] private RectTransform uiRectTransform;

        public enum CardSource
        {
            GlobalCardManager,
            RandomCardData,
            LocalCardLib
        }

        [SerializeField] private CardSource cardSource = CardSource.GlobalCardManager;

        public void AddEnergy(int energy)
        {
            this.setEnergy(this.EnergyPoint + energy); // 增加能量点
        }

        public void init(CombatSystem combatSystem, Character character)
        {
            this.NewCardData = (cardSource switch
            {
                CardSource.GlobalCardManager => globalCardManager.GetAllCards(), // 从全局卡片管理器获取随机卡片数据
                CardSource.RandomCardData => ViewCards.randomCardData(),
                CardSource.LocalCardLib => LocalCards.GetCards(),
                _ => globalCardManager.GetAllCards()
            }).Select(x => x.Clone()).ToList();

            this.NewCardData.ForEach(x => x.Modify(character));

            setEnergy(Setting.RoundEnergy); // 设置能量点
            ResetNewCards();
            HandCardData = new List<ICardData>(); // 初始化手牌数据列表
            DiscardCardData = new List<ICardData>(); // 初始化弃牌数据列表
            PendingCardData = new List<ICardData>(); // 初始化待处理卡片数据列表

            this.combatSystem = combatSystem; // 设置战斗系统
        }

        private void ResetNewCards()
        {
            foreach (var cardData in NewCardData)
            {
                cardData.Reset();
            }
        }
        // 抽卡
        public void drewCard(int num = -1)
        {
            if (num <= 0)
            {
                num = Setting.RoundGetCardNum; // 使用默认数量
            }
            for (int i = 0; i < num; i++)
            {
                if (NewCardData.Count == 0) // 如果没有新卡片了
                {
                    if (DiscardCardData.Count > 0) // 如果有弃牌
                    {
                        MoveDiscardToDrawPile();
                    }
                    else
                    {
                        break; // 退出循环
                    }
                }
                // 从新卡片数据列表中随机抽取一张卡片
                var index = Random.Range(0, NewCardData.Count);
                ICardData cardData = NewCardData[index];
                NewCardData.RemoveAt(index);

                CreateCard(cardData); // 创建卡片
                HandCardData.Add(cardData); // 添加到手牌数据列表
            }
            // 更新卡片位置
            updateCardPosition();
        }

        private CardStackType FindCard(ICardData cardData)
        {
            if (HandCardData.Contains(cardData))
            {
                return CardStackType.Hand; // 在手牌中
            }
            if (DiscardCardData.Contains(cardData))
            {
                return CardStackType.Discard; // 在弃牌中
            }
            if (NewCardData.Contains(cardData))
            {
                return CardStackType.DrawPile; // 在抽牌堆中
            }
            if (PendingCardData.Contains(cardData))
            {
                return CardStackType.Pending; // 在待处理卡片中
            }
            throw new System.Exception($"Card {cardData.CardName} not found in any card stack.");
        }

        private void RemoveFromHand(ICardData cardData, bool destroy = true)
        {
            if (FindCard(cardData) == CardStackType.Hand)
            {
                var card = cards.Find(c => c.GetComponent<Card>().CardData == cardData)?.GetComponent<Card>();
                if (card != null)
                {
                    cards.Remove(card.cardObj); // 从卡片列表中移除
                    if (destroy)
                    {
                        Destroy(card.cardObj);
                    }
                    HandCardData.Remove(cardData); // 从手牌数据列表中移除
                }
            }
        }

        private void AddToHand(ICardData cardData)
        {
            if (FindCard(cardData) == CardStackType.Hand)
            {
                return;
            }
            CreateCard(cardData);
            updateCardPosition();
        }

        /// <summary>
        /// 获取指定类型的卡片列表
        /// </summary>
        private List<ICardData> GetCardList(CardStackType type)
        {
            return type switch
            {
                CardStackType.Hand => HandCardData,
                CardStackType.Discard => DiscardCardData,
                CardStackType.DrawPile => NewCardData,
                CardStackType.Pending => PendingCardData,
                _ => throw new System.Exception($"不合法的卡片堆类型: {type}")
            };
        }

        public void MoveTo(ICardData cardData, CardStackType type, bool destroy = true)
        {
            if (cardData == null)
            {
                Debug.LogWarning("尝试移动空卡片数据");
                return;
            }

            CardStackType currentType = FindCard(cardData);
            if (currentType == type)
            {
                return; // 如果卡片已经在目标堆中，则不进行任何操作
            }

            if (currentType == CardStackType.Hand)
            {
                RemoveFromHand(cardData, destroy); // 从手牌中移除
            }
            else
            {
                List<ICardData> currentList = GetCardList(currentType);
                currentList.Remove(cardData); // 从当前堆中移除卡片数据
            }

            if (type == CardStackType.Hand)
            {
                AddToHand(cardData); // 添加到手牌
            }
            else
            {
                List<ICardData> targetList = GetCardList(type);
                targetList.Add(cardData); // 添加到目标堆中
                if (type == CardStackType.DrawPile)
                {
                    cardData.Reset(); // 如果是抽牌堆，重置卡片数据
                }
            }
        }

        public void MoveDiscardToDrawPile()
        {
            if (DiscardCardData.Count > 0)
            {
                NewCardData.AddRange(DiscardCardData); // 将弃牌添加到新卡片数据列表
                DiscardCardData.Clear(); // 清空弃牌数据列表
                ResetNewCards(); // 重置新卡片数据
            }
        }

        public void discardCard(Card card)
        {
            if (HandCardData.Exists(x => x == card.CardData))
            {
                card.CardData.Discard();
                DiscardCardData.Add(card.CardData); // 将卡片添加到弃牌数据列表
                HandCardData.Remove(card.CardData); // 从手牌数据列表中移除已弃掉的卡片
            }
            // 销毁卡片对象
            DestroyImmediate(card.cardObj); // 销毁卡片对象
        }

        public void DiscardCards(List<ICardData> cardDatas)
        {
            foreach (var cardData in cardDatas)
            {
                MoveTo(cardData, CardStackType.Discard); // 将卡片移动到弃牌堆
            }
            // 更新卡片位置
            updateCardPosition();
        }

        public void discardAll()
        {
            // 将所有手牌添加到弃牌数据列表
            foreach (var cardData in HandCardData)
            {
                cardData.Discard(); // 弃掉卡片
            }
            DiscardCardData.AddRange(HandCardData);
            HandCardData.Clear(); // 清空手牌数据列表
                                  // 清空卡片列表
            foreach (var card in cards)
            {
                Destroy(card); // 销毁卡片对象
            }
            cards.Clear();
            updateCardPosition(); // 更新卡片位置
        }

        public IEnumerator UseHandCard(Card card, Character source, List<Character> targets)
        {
            Assert.IsTrue(this.HandCardData.Contains(card.CardData));
            if (this.EnergyPoint < card.CardCost) yield break;
            this.setEnergy(this.EnergyPoint - card.CardCost);
            MoveTo(card.CardData, CardStackType.Pending, destroy: false);
            yield return card.Effect.Work(source, targets);
            MoveTo(card.CardData, CardStackType.Discard);
            updateCardPosition();
        }

        public void reportUse(Card card)
        {
            discardCard(card); // 弃掉使用的卡片
            updateCardPosition(); // 更新卡片位置
        }

        private void CreateCard(ICardData cardData)
        {
            GameObject CardObj = Instantiate(CardPrefab, CardDeck.transform);
            Card card = CardObj.GetComponent<Card>();
            CardDragController dragController = CardObj.GetComponent<CardDragController>();
            dragController.Init(combatSystem, uiRectTransform); // 初始化拖拽控制器
            card.updateCardStatus(cardData); // 更新卡片状态
            card.addManager(this); // 添加卡片管理器
            cards.Add(CardObj); // 添加卡片到列表
        }

        public void UpdateCardStatus(ICardData cardData)
        {
            if (cards.Find(card => card.GetComponent<Card>().CardData == cardData) is GameObject cardObj)
            {
                Card card = cardObj.GetComponent<Card>();
                card.updateCardStatus(cardData); // 更新卡片状态
            }
        }

        public enum ModifySubType
        {
            Add,
            Mul
        }

        public void ModifyCard(ICardData cardData, float factor, ModifyType type, ModifySubType subType)
        {
            if (subType == ModifySubType.Add)
            {
                cardData.ModifyAdd(factor, type); // 添加修改
            }
            else if (subType == ModifySubType.Mul)
            {
                cardData.ModifyMul(factor, type); // 乘法修改
            }
            UpdateCardStatus(cardData); // 更新卡片状态
        }

        public void updateCardPosition()
        {
            // 移除cards中的null
            cards.RemoveAll(card => card == null);
            // 将所有卡片在手牌区排成一排
            // 每张牌偏移手牌区中心距离为: (手牌区宽度 - 卡片宽度) / 2 / ((卡片数量 - 1) / 2)
            if (cards.Count <= 1)
            {
                if (cards.Count <= 0) return; // 如果没有卡片, 则不更新位置
                cards[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0); // 如果只有一张牌, 则放在中间
                return;
            }
            float cardWidth = CardPrefab.GetComponent<RectTransform>().rect.width;
            float cardDeckWidth = CardDeck.GetComponent<RectTransform>().rect.width;
            float offset = (cardDeckWidth - cardWidth) / 2 / ((cards.Count - 1f) / 2);
            for (int i = 0; i < cards.Count; i++)
            {
                float x = (i - (cards.Count - 1f) / 2) * offset;
                cards[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 0);
            }
        }

        public void setEnergy(int energy)
        {
            this.energy = energy;
            if (EnergySpotText != null)
            {
                EnergySpotText.text = this.energy.ToString(); // 更新能量点文本
            }
        }

        public Result IsPlayable(ICardData card)
        {
            // 检查卡牌是否在手牌中
            if (!HandCardData.Contains(card))
            {
                return Result.Fail("卡牌不在手牌中");
            }

            if (card is ICardPlayable playableCard)
            {
                return playableCard.IsPlayable(combatSystem); // 如果卡牌实现了ICardPlayable接口，调用其IsPlayable方法
            }

            // 检查能量点是否足够
            if (EnergyPoint < card.Cost)
            {
                return Result.Fail($"能量点不足: {EnergyPoint}/{card.Cost}");
            }
            // 检查卡牌效果目标是否有效
            if (!card.CardEffectTarget.IsValidTarget())
            {
                return Result.Fail("卡牌不可用");
            }
            return Result.Ok(); // 卡牌可用
        }
    }
}