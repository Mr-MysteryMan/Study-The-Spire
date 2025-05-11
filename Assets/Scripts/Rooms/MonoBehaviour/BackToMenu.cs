using UnityEngine;

public class BackToMenu : MonoBehaviour
{
    public ObjectEventSO loadMenuEvent;
    private void OnMouseDown()
    {
        //·µ»ØµØÍ¼
        loadMenuEvent.RaiseEvent(null, this);
    }
}
