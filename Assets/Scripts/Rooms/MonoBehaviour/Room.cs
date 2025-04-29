using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //��¼�����ڵ�ͼ�ϵ�λ��
    public int col;

    public int row;

    private SpriteRenderer spriteRenderer;

    public RoomDataSO roomData;

    public RoomState roomState;
    public List<Vector2Int> linkTo = new List<Vector2Int>();

    [Header("�㲥")]
    public ObjectEventSO loadRoomEvent;
    
    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (roomState == RoomState.Attainable)
        {
            loadRoomEvent.RaiseEvent(this, this);
        }

        
    }

//�ⲿ��������ʱ���е��ã���䷿���λ��/����

    public void SetupRoom(int col, int row, RoomDataSO roomData)
    {
        this.col = col;
        this.row = row;
        this.roomData = roomData;
        spriteRenderer.sprite = roomData.roomIcon;
        spriteRenderer.color = roomState switch
        {
            RoomState.Locked => Color.grey,
            RoomState.Visited => new Color(0.6f, 0.8f, 0.6f, 1f),
            RoomState.Attainable => Color.white,
            RoomState.Passed => Color.black,
            _ => throw new System.NotImplementedException(),
        };
    }
}