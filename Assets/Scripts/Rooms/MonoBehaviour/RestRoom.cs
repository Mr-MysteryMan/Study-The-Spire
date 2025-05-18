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
        Debug.Log($"回答正确！");
        cardManager.AddHealth(100);
        //返回地图
        loadMapEvent.RaiseEvent(null, this);

    }

    private void HandleWrongAnswer()
    {
        Debug.Log($"回答错误！");
        cardManager.AddHealth(-50);
        //返回地图
        loadMapEvent.RaiseEvent(null, this);
    }

    private void OnMouseDown()
    {
        var question = Instantiate(questionPrefab);
        QuestionManager questionManager = question.GetComponent<QuestionManager>();
        questionManager.init(
            () => HandleCorrectAnswer(),  // 正确答案回调
            () => HandleWrongAnswer()    // 错误答案回调
        );
        cardManager = CardManager.Instance;
     
    }
}
