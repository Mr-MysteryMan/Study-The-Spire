using UnityEngine;

public class BackToMenu : MonoBehaviour
{
    public ObjectEventSO loadMenuEvent;
    private void OnMouseDown()
    {
        //���ص�ͼ
        loadMenuEvent.RaiseEvent(null, this);
    }
}
