using System.Collections;
using Combat.Events;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Combat.VFX
{
    public class VfxManager : MonoBehaviour
    {
        [SerializeField] private Character character;
        private CombatSystem combatSystem;
        private EventManager eventManager;

        [SerializeField] private GameObject HPDamageTextPrefab;
        [SerializeField] private GameObject AmmorDamageTextPrefab;

        [SerializeField] private GameObject HealTextPrefab;
        [SerializeField] private GameObject BasicTextPrefab;

        [SerializeField] private RectTransform textRectTransform;

        [SerializeField] private GameObject indicator;

        public void Init(Character character)
        {
            this.character = character;
            this.combatSystem = character.combatSystem;
            this.eventManager = this.combatSystem.EventManager;

            eventManager.Subscribe<DamageDealtEvent>(OnDamageDealt);
            eventManager.Subscribe<HealDealtEvent>(OnHealDealt);
            eventManager.Subscribe<AddAmmorEvent>(OnAddAmmor);
        }

        private void OnDestroy() {
            if (eventManager != null)
            {
                eventManager.Unsubscribe<DamageDealtEvent>(OnDamageDealt);
                eventManager.Unsubscribe<HealDealtEvent>(OnHealDealt);
                eventManager.Unsubscribe<AddAmmorEvent>(OnAddAmmor);
            }
        }

        private void OnDamageDealt(DamageDealtEvent e)
        {
            if (e.Target == character)
            {
                PlayTakeDamage(e.AmmorDamage, e.HPDamage);
            }
        }

        private void OnHealDealt(HealDealtEvent e)
        {
            if (e.Target == character)
            {
                PlayHeal(e.Heal);
            }
        }

        private void OnAddAmmor(AddAmmorEvent e)
        {
            if (e.Target == character)
            {
                PlayAddAmmor(e.Ammor);
            }
        }

        private void Start()
        {
            if (this.character == null)
            {
                Debug.LogError("VfxManager: Character component not found!");
                return;
            }
            Init(this.character);
        }

        private enum DamageType
        {
            HP,
            Ammor,
            Heal,
            Basic,
        }

        private Vector2 oriPos;
        public IEnumerator PlayAttackForward()
        {
            oriPos = new Vector2(transform.position.x, transform.position.y);
            var targetPos = transform.position;
            Vector2 offset = new Vector2(40f, 0);
            offset *= transform.localScale;
            targetPos += new Vector3(offset.x, offset.y, 0);
            yield return transform.DOMoveX(targetPos.x, 0.2f).WaitForCompletion(); // 移动到目标位置
        }

        public IEnumerator PlayAttackBack()
        {
            yield return transform.DOMoveX(oriPos.x, 0.2f).WaitForCompletion(); // 移动回原位置
        }

        private void PlayDamageText(DamageType type, int damage)
        {
            DisplayText(damage.ToString(), type);
        }

        private void DisplayText(string text, DamageType type = DamageType.Basic)
        {
            TMP_Text txt = Instantiate(
            original: type switch
            {
                DamageType.HP => HPDamageTextPrefab,
                DamageType.Ammor => AmmorDamageTextPrefab,
                DamageType.Heal => HealTextPrefab,
                _ => BasicTextPrefab,
            },
            parent: textRectTransform)
            .GetComponent<TMP_Text>();

            txt.text = text;

            var pos = textRectTransform.position;
            pos.x += Random.Range(-7f, 7f); // 在X轴上随机偏移
            pos.y += Random.Range(-3f, 3f); // 在Y轴上随机偏移
            txt.transform.position = pos; // 设置文本位置

            var seq = DOTween.Sequence();
            seq.Append(txt.transform.DOMoveY(pos.y + 1f, 0.6f))
            .Join(txt.DOFade(0, 1.4f))
            .OnComplete(() => Destroy(txt.gameObject)); // 动画完成后销毁文本对象
            seq.SetAutoKill(true); // 设置自动销毁
            seq.Play(); // 播放动画
        }

        private void PlayHeal(int heal)
        {
            PlayDamageText(DamageType.Heal, heal);
        }

        private void PlayAddAmmor(int ammor)
        {
            PlayDamageText(DamageType.Ammor, ammor);
        }

        private void PlayTakeDamage(int ammor, int hp)
        {
            transform.DOShakePosition(
                duration: 0.5f,
                strength: 4.0f,
                vibrato: 20,
                randomness: 90,   // 0～180°随机
                fadeOut: true
            );
            if (ammor > 0)
            {
                PlayDamageText(DamageType.Ammor, ammor);
            }
            if (hp > 0)
            {
                PlayDamageText(DamageType.HP, hp);
            }
        }

        public void SetIndicator(bool isActive)
        {
            if (indicator != null)
            {
                indicator.SetActive(isActive);
            }
        }
    }
}