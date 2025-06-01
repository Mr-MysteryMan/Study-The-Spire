using Combat;
using UnityEngine;
using UnityEngine.UIElements;

public class RestRoom : MonoBehaviour
{
    private CardManager cardManager;
    public ObjectEventSO loadMapEvent;
    public GameObject questionPrefab;

    private void HandleCorrectAnswer()
    {
        if (cardManager.health > Setting.PlayerHp - 50)
        {
            cardManager.health = Setting.PlayerHp;
        }
        else
        {
            cardManager.health = 50;
        }



        loadMapEvent.RaiseEvent(null, this);
    }

    private void HandleWrongAnswer()
    {
        if (cardManager.health > 50)
        {
            cardManager.health -= 50;
        }
        else
        {
            cardManager.health = 1;
        }

        loadMapEvent.RaiseEvent(null, this);
    }

    private void OnMouseDown()
    {
        var question = Instantiate(questionPrefab);
        QuestionManager questionManager = question.GetComponent<QuestionManager>();
        questionManager.init(
            () => HandleCorrectAnswer(),  // ��ȷ�𰸻ص�
            () => HandleWrongAnswer()    // ����𰸻ص�
        );
        cardManager = CardManager.Instance;
     
    }
}
