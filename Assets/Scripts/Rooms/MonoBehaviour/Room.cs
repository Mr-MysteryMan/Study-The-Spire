using UnityEngine;

public class Room : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public RoomDataSO roomData;

    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        Debug.Log("点击了房间：" + roomData.roomType);
    }
}
