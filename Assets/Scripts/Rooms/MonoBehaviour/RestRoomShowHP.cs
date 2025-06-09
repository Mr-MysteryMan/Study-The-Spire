using Combat;
using Combat.Events;
using Combat.EventVariable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RestRoomShowHP : MonoBehaviour
{
    private CardManager cardManager;
    [SerializeField] private TMP_Text curHpText;
    [SerializeField] private TMP_Text maxHpText;
    private SpriteRenderer sprite;
    public Sprite[] healthSprites;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // ��Character����
    public void OnEnable()
    {
        cardManager = CardManager.Instance;
        //Debug.Log("TEXT:");
        //Debug.Log(maxHpText.text);
        //Debug.Log(curHpText.text);
        //maxHpText.text = cardManager.MaxHealth.ToString();
        //curHpText.text = cardManager.Health.ToString();
        float healthPercentage = (float)cardManager.Health / cardManager.MaxHealth;

        int totalSprites = healthSprites.Length;
        int spriteIndex = Mathf.FloorToInt(healthPercentage * (totalSprites - 1));
        spriteIndex = Mathf.Clamp(spriteIndex, 0, totalSprites - 1); // 确保索引在范围内

        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = healthSprites[spriteIndex];

    }
}
