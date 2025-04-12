using UnityEngine;

public class Room : MonoBehaviour
{
    //记录房间在地图上的位置
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
        Debug.Log("点击了房间：" + roomData.roomType);
    }

//外部创建房间时进行调用，填充房间的位置/属性

    public void SetupRoom(int col, int row, RoomDataSO roomData)
    {
        this.col = col;
        this.row = row;
        this.roomData = roomData;
        Debug.Log("正在设置房间：(" + col + ", "+ row +") "+roomData.roomType);
        spriteRenderer.sprite = roomData.roomIcon;
    }
}
