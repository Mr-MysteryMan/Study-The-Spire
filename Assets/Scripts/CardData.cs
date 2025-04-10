using UnityEngine;

public class CardData
{
    public string content; // 卡片名称

    public bool isDiscarded; // 是否是弃牌

    public CardData(string content, bool isDiscarded)
    {
        this.content = content;
        this.isDiscarded = isDiscarded;
    }
}
