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
    private Character character;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // ��Character����
    public void OnEnable()
    {
        cardManager = CardManager.Instance;
        Debug.Log("TEXT:");
        Debug.Log(maxHpText.text);
        Debug.Log(curHpText.text);
        maxHpText.text = cardManager.MaxHealth.ToString();
        curHpText.text = cardManager.Health.ToString();
    }
}
