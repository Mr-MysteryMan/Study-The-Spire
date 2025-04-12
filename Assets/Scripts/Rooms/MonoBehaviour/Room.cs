using UnityEngine;

public class Room : MonoBehaviour
{
    //��¼�����ڵ�ͼ�ϵ�λ��
    public int col;

    public int row;

    private SpriteRenderer spriteRenderer;

    public RoomDataSO roomData;

    public RoomState roomState;

    private void Start()
    {
        SetupRoom(1, 1, roomData);
    }
    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        Debug.Log("����˷��䣺" + roomData.roomType);
    }

//�ⲿ��������ʱ���е��ã���䷿���λ��/����

    public void SetupRoom(int col, int row, RoomDataSO roomData)
    {
        this.col = col;
        this.row = row;
        this.roomData = roomData;
        Debug.Log("�������÷��䣺(" + col + ", "+ row +") "+roomData.roomType);
        spriteRenderer.sprite = roomData.roomIcon;
    }
}
