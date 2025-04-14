using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Card : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public Image background; // 卡片背景
    public Text contentRB; // 右下角文本
    public Text contentLT; // 左上角文本

    public bool isDiscarded = false; // 是否是弃牌


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
        // 设置背景透明度为0.7
        Color color = this.background.color;
        color.a = 0.7f;
        this.background.color = color;
    }

    // 鼠标放上时升起, 离开时落下
    private float animationDuration = 0.2f; // 动画时间
    private int originalSiblingIndex; // 原始的层级索引
    
    private Coroutine animationCoroutine; // 用于存储协程

    private Vector3 originalScale = new Vector3(1f,1f,1f); // 原始缩放比例
    private Vector3 targetScale = new Vector3(1.1f,1.1f,1.1f); // 放大比例
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 如果已经有动画在播放，先停止它
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        // 记录原始的层级索引
        originalSiblingIndex = transform.GetSiblingIndex();
        // 将当前卡片的层级提升到最上面
        transform.SetAsLastSibling();
        // 开始放大动画
        animationCoroutine = StartCoroutine(AnimateTransform(originalScale * 1.1f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 如果已经有动画在播放，先停止它
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        // 恢复原始的层级索引
        transform.SetSiblingIndex(originalSiblingIndex);
        // 开始还原大小和层级动画
        animationCoroutine = StartCoroutine(AnimateTransform(originalScale));
    }

    private IEnumerator AnimateTransform(Vector3 targetScale)
    {
        float timer = 0f;
        Vector3 startingScale = transform.localScale;

        while (timer < animationDuration)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startingScale, targetScale, timer / animationDuration);
            yield return null;
        }

        // 确保最终大小和层级正确
        transform.localScale = targetScale;

        // 清除协程引用
        animationCoroutine = null;
    }
}
