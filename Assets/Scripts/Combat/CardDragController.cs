using System.Collections.Generic;
using System.Linq;
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

        private (float width, float height) cardSize;
        private bool canMove;           // 能否移动
        private bool canExecute;        // 能否执行效果
        private Character target;

        private RectTransform uiRectTransform; // UI的RectTransform

        private void Awake()
        {
            currentCard = GetComponent<Card>();
            var rt = currentCard.GetComponent<RectTransform>();
            cardSize = (rt.rect.width, rt.rect.height);
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
            if (combatSystem.CardManager.IsPlayable(currentCard.CardData) is Result result && !result.Success)
            {
                Debug.Log($"卡牌不可用: {result.Error}");
                return;
            }

            canExecute = false;
            target = null;
            if (currentCard.CardData.CardEffectTarget.IsMoveToSelectTarget())
            {
                canMove = true;
                return;
            }

            if (currentCard.CardData.CardEffectTarget.IsDragToSelectTarget())
            {
                currentArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
                currentArrow.GetComponent<DragArrow>().Init(uiRectTransform);
                canMove = false;
                return;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            // 卡牌不可用
            if (combatSystem.CardManager.IsPlayable(currentCard.CardData) is Result result && !result.Success)
            {
                Debug.Log($"卡牌不可用: {result.Error}");
                return;
            }

            Character newTarget = null;
            bool newCanExecute = false;
            if (canMove)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(currentCard.GetComponent<RectTransform>().transform.parent.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out Vector2 localPoint);
                currentCard.GetComponent<RectTransform>().anchoredPosition = localPoint;

                // 到达屏幕上方区域即可执行
                newCanExecute = Input.mousePosition.y > Screen.height * 0.6f;
            }
            else
            {
                if (eventData.pointerEnter == null)
                {
                    newCanExecute = false;
                }
                else if (eventData.pointerEnter.CompareTag("Character"))
                {
                    newTarget = eventData.pointerEnter.GetComponentInParent<Character>();
                    if (newTarget == null)
                    {
                        newCanExecute = false;
                    }
                    else if (newTarget is Adventurer && currentCard.CardData.CardEffectTarget.IsAdventurerTarget())
                    {
                        newCanExecute = true;
                    }
                    else if (newTarget is Enemy && currentCard.CardData.CardEffectTarget.IsEnemyTarget())
                    {
                        newCanExecute = true;
                    }
                    else if (currentCard.CardData.CardEffectTarget.IsCharacterTarget())
                    {
                        newCanExecute = true;
                    }
                    else
                    {
                        newCanExecute = false;
                    }
                }
            }

            UpdateTarget(newCanExecute, newTarget);
        }

        private void UpdateTarget(bool newCanExecute, Character newTarget)
        {
            if (canExecute == newCanExecute && target == newTarget)
            {
                return; // 没有变化
            }

            if (newCanExecute != canExecute)
            {
                UpdateIndicator(newCanExecute, newTarget);
            }

            canExecute = newCanExecute;

            if (newTarget != target)
            {
                if (target != null)
                {
                    target.GetComponentInChildren<VfxManager>().SetIndicator(false);
                }

                if (newTarget != null && newCanExecute)
                {
                    newTarget.GetComponentInChildren<VfxManager>().SetIndicator(true);
                }
            }

            target = newTarget;
        }

        private void UpdateIndicator(bool newCanExecute, Character newTarget)
        {
            if (newCanExecute)
            {
                // 将新目标的指示器设置为true
                GetTargets(newTarget).ForEach(c =>
                {
                    c.GetComponentInChildren<VfxManager>().SetIndicator(true);
                });
            }
            else
            {
                // 将之前目标的指示器设置为false
                GetTargets(target).ForEach(c =>
                {
                    c.GetComponentInChildren<VfxManager>().SetIndicator(false);
                });
            }
        }

        private List<Character> GetTargets(Character target)
        {
            CardEffectTarget targetType = currentCard.CardData.CardEffectTarget;
            if (targetType.IsMultiTarget())
            {
                if (targetType.IsAdventurerTarget())
                {
                    return new List<Character> { combatSystem.PlayerCharacter };
                }
                else if (targetType.IsEnemyTarget())
                {
                    return combatSystem.MonsterCharacter.Select(c => c as Character).ToList();
                }
                else if (targetType.IsCharacterTarget())
                {
                    return combatSystem.AllCharacters;
                }
            }
            else if (canMove)
            {
                return new List<Character> { combatSystem.PlayerCharacter };
            }
            else
            {
                if (target != null)
                {
                    return new List<Character> { target };
                }
                else
                {
                    return new List<Character>();
                }
            }
            return new List<Character>();
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            // 卡牌不可用
            if (combatSystem.CardManager.IsPlayable(currentCard.CardData) is Result result && !result.Success)
            {
                Debug.Log($"卡牌不可用: {result.Error}");
                return;
            }

            if (currentArrow != null)
            {
                Destroy(currentArrow);
            }
            if (canExecute)
            {
                Debug.Log("使用卡牌");
                UpdateTarget(false, null);
                StartCoroutine(
                    combatSystem.CardManager.UseHandCard(currentCard, combatSystem.PlayerCharacter, GetTargets(target))
                );
                return;
            }
            else
            {
                // 还原卡牌位置
                if (canMove)
                {
                    combatSystem.CardManager.updateCardPosition();
                }
            }
            canMove = false;
            UpdateTarget(false, null); // 重置目标和可执行状态
        }
    }
}