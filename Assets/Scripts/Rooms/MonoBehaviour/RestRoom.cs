using Combat;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class RestRoom : MonoBehaviour
{
    private CardManager cardManager;
    public ObjectEventSO loadMapEvent;
    public GameObject questionPrefab;

    private void HandleCorrectAnswer()
    {
        cardManager.CurAdvHealth = cardManager.CurAdvMaxHealth;
        loadMapEvent.RaiseEvent(null, this);
    }

    private void HandleWrongAnswer()
    {
        cardManager.CurAdvHealth += cardManager.CurAdvMaxHealth / 4;
        loadMapEvent.RaiseEvent(null, this);
    }

    private void OnMouseDown()
    {
        var question = Instantiate(questionPrefab);
        QuestionManager questionManager = question.GetComponent<QuestionManager>();
        questionManager.init(
            () => HandleCorrectAnswer(),  
            () => HandleWrongAnswer()
        );
        cardManager = CardManager.Instance;
     
    }
}
