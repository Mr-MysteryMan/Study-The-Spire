using UnityEngine;
using UnityEngine.UI;

namespace Combat.Characters.EnemyEffect.UIController
{
    // 敌人血条控制器类，继承自MonoBehaviour
    // 负责敌人血条的UI控制
    [RequireComponent(typeof(Image))]
    public class EnemyIndentController : MonoBehaviour
    {
        [SerializeField] private Image image; // 精灵渲染器组件，用于渲染敌人意图
        public Sprite Indent {get => image.sprite; set => image.sprite = value;} // 敌人意图精灵

        public void Awake()
        {
            image = GetComponent<Image>(); // 获取精灵渲染器组件
        }
    }
}
