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
        Debug.Log($"�ش���ȷ��");
        cardManager.health += 100;
        //���ص�ͼ
        loadMapEvent.RaiseEvent(null, this);
    }

    private void HandleWrongAnswer()
    {
        Debug.Log($"�ش����");
        cardManager.health -= 50;
        //���ص�ͼ
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
