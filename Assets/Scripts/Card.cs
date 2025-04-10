using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // 确保引用了TextMeshPro命名空间

public class Card : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public Image background; // 卡片背景
    public Text contentRB; // 右下角文本
    public Text contentLT; // 左上角文本

    public bool isDiscarded = false; // 是否是弃牌

    private int originalSiblingIndex; // 原始的层级索引

    public void SetCardText(string content)
    {
        // 设置卡片文本
        this.contentRB.text = content;
        this.contentLT.text = content;
    }

    public void setDiscard()
    {
        // 设置卡片为弃牌状态
        this.isDiscarded = true;
        // 设置背景透明度为0.5
        Color color = this.background.color;
        color.a = 0.7f;
        this.background.color = color;
    }

    // 鼠标放上时升起
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 卡片升为最高层
        originalSiblingIndex = this.transform.GetSiblingIndex(); // 获取原始的层级索引
        this.transform.SetAsLastSibling();
        // 卡片放大为1.1倍
        this.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }

        public void OnPointerExit(PointerEventData eventData)
    { 
        // 卡片放回原来的层级
        this.transform.SetSiblingIndex(originalSiblingIndex); // 恢复原始的层级索引
        // 卡片大小还原
        this.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
