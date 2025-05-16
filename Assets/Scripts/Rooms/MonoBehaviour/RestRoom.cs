using Combat;
using UnityEngine;
using UnityEngine.UIElements;

public class RestRoom : MonoBehaviour
{
    private CardManager cardManager;
    public ObjectEventSO loadMapEvent;

    private void OnMouseDown()
    {
        cardManager = CardManager.Instance;
        cardManager.AddHealth(100);
        //���ص�ͼ
        loadMapEvent.RaiseEvent(null, this);
    }
}
