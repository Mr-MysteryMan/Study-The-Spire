using Combat.Characters;
using Combat.VFX;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Combat
{
    public class CardDragController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public GameObject arrowPrefab;
        private GameObject currentArrow;

        private CombatSystem combatSystem;

        private Card currentCard;

        private (int width, int height) cardSize;
        private bool canMove;           // 能否移动
        private bool canExecute;        // 能否执行效果
        private Character target;

        private RectTransform uiRectTransform; // UI的RectTransform

        private void Awake()
        {
            currentCard = GetComponent<Card>();
            var rt = currentCard.GetComponent<RectTransform>();
            cardSize = (width: (int)rt.rect.width, height: (int)rt.rect.height);
        }

        private void OnDisable()
        {
            canMove = false;
            canExecute = false;
        }

        public void Init(CombatSystem combatSystem, RectTransform uiRectTransform)
        {
            this.combatSystem = combatSystem;
            this.uiRectTransform = uiRectTransform;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // 卡牌不可用
            if (combatSystem.CardManager.EnergyPoint <= 0 || currentCard.CardCost > combatSystem.CardManager.EnergyPoint)
            {
                Debug.Log("能量不足，无法使用卡牌！");
                return;
            }

            switch (currentCard.CardData.CardEffectTarget)
            {
                case CardEffectTarget.EnemyOne:
                case CardEffectTarget.AdventurerOne:
                case CardEffectTarget.CharacterOne:
                    currentArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
                    currentArrow.GetComponent<DragArrow>().Init(uiRectTransform);
                    canMove = false;
                    break;
                case CardEffectTarget.EnemyAll:
                case CardEffectTarget.AdventurerAll:
                case CardEffectTarget.CharacterAll:
                case CardEffectTarget.AdventurerSelf:
                    canMove = true;
                    break;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            // 卡牌不可用
            if (combatSystem.CardManager.EnergyPoint <= 0 || currentCard.CardCost > combatSystem.CardManager.EnergyPoint)
            {
                Debug.Log("能量不足，无法使用卡牌！");
                return;
            }

            do
            {
                // 跟随鼠标移动
                if (canMove)
                {
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(currentCard.GetComponent<RectTransform>().transform.parent.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out Vector2 localPoint);
                    currentCard.GetComponent<RectTransform>().anchoredPosition = localPoint;

                    // 到达屏幕上方区域即可执行
                    canExecute = Input.mousePosition.y > Screen.height * 0.6f;
                    if (!canExecute && target != null)
                    {
                        target.GetComponentInChildren<VfxManager>().SetIndicator(false);
                    }
                    target = canExecute ? combatSystem.PlayerCharacter : null;

                    // Debug.Log(Input.mousePosition.y + " " + Screen.height * 0.6f);
                }
                // 攻击牌指针的情况
                else
                {
                    if (eventData.pointerEnter == null || eventData.pointerEnter != target)
                    {
                        if (target != null)
                        {
                            target.GetComponentInChildren<VfxManager>().SetIndicator(false);
                        }
                        if (eventData.pointerEnter == null)
                        {
                            canExecute = false;
                            target = null;
                            break;
                        }
                    }
                    // Debug.Log(eventData.pointerEnter.name + " " + eventData.pointerEnter.tag);
                    // 指向敌人
                    if (eventData.pointerEnter.CompareTag("Character"))
                    {
                        target = eventData.pointerEnter.GetComponentInParent<Character>();
                        if (target is Adventurer adventurer)
                        {
                            canExecute = false; // 不能攻击自己
                        }
                        else if (target is Enemy enemy)
                        {
                            canExecute = true; // 可以攻击敌人
                        }
                        break;
                    }
                    canExecute = false;
                    target = null;
                }
            }
            while (false);
            if (canExecute && target != null)
            {
                target.GetComponentInChildren<VfxManager>().SetIndicator(true);
            }
            else if (target != null)
            {
                target.GetComponentInChildren<VfxManager>().SetIndicator(false);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // 卡牌不可用
            if (combatSystem.CardManager.EnergyPoint <= 0 || currentCard.CardCost > combatSystem.CardManager.EnergyPoint)
            {
                Debug.Log("能量不足，无法使用卡牌！");
                return;
            }

            if (currentArrow != null)
            {
                Destroy(currentArrow);
            }
            if (canExecute)
            {
                Debug.Log("使用卡牌");
                if (canMove)
                {
                    combatSystem.CardManager.UseHandCard(currentCard, combatSystem.PlayerCharacter, combatSystem.PlayerCharacter);
                }
                else
                {
                    combatSystem.CardManager.UseHandCard(currentCard, combatSystem.PlayerCharacter, target);
                }
            }
            else
            {
                // 还原卡牌位置
                if (canMove)
                {
                    combatSystem.CardManager.updateCardPosition();
                }
            }
            canExecute = canMove = false;
            if (target != null)
            {
                target.GetComponentInChildren<VfxManager>().SetIndicator(false);
            }
        }


    }
}