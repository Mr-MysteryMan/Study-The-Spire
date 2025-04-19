using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header(header:"地图配置表")]
    public MapConfigSO mapConfig;

    [Header(header: "预制体")]
    public Room roomPrefab;
    public LineRenderer linePrefab;

    private float screenHeight;

    private float screenWidth;

    private float colWidth;

    private float rowWidth;

    private Vector3 generatePoint;

    public List<RoomDataSO> roomDataList = new();

    private Dictionary<RoomType, RoomDataSO> roomDataDict = new();


    public void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;
        colWidth = screenWidth / (mapConfig.roomBlueprint.Count + 1);
    
        foreach (var roomData in roomDataList)
        {
            roomDataDict.Add(roomData.roomType, roomData);
        }
    
    
    }

    private void Start()
    {
        CreatMap();
        Debug.Log("正在设置地图");

    }

    public void CreatMap()
    {
        List<Room> preColRooms = new();

        for (int col = 0; col < mapConfig.roomBlueprint.Count; col++)
        {
            var blueprint = mapConfig.roomBlueprint[col];

            var amount = UnityEngine.Random.Range(blueprint.min, blueprint.max + 1);

            List<Room> curColRooms = new();

            rowWidth = screenHeight / (amount + 1);

            //循环生成当前列的房间
            for (int i = 0; i < amount; i++)
            {
                generatePoint = new Vector3(-(screenWidth / 2) + colWidth * (col + 1), screenHeight / 2 - rowWidth * (i + 1), 0);

                var room = Instantiate(roomPrefab, generatePoint,Quaternion.identity,transform);
                RoomType newType = GetRandomRoomType(mapConfig.roomBlueprint[col].roomType);
                room.SetupRoom(col, i, GetRoomData(newType));

                curColRooms.Add(room);
            }

            //判断连线
            if (preColRooms.Count > 0)
            {
                // 创建两个列表的房间连线
                CreateConnection(preColRooms, curColRooms);
            }

            preColRooms = curColRooms;

        }
    }

    private void CreateConnection(List<Room> column1, List<Room> column2)
    {
        HashSet<Room> connectedColumn2Rooms = new HashSet<Room>();

        foreach (var room in column1)
        {
            var targetRoom = ConnectToRandomRoom(room, column2, false);
            connectedColumn2Rooms.Add(targetRoom);
        }

        // 检查确保 Column2 中所有房间都有连接的房间
        foreach (var room in column2)
        {
            if (!connectedColumn2Rooms.Contains(room))
            {
                ConnectToRandomRoom(room, column1, true);
            }
        }
    }

    /// <summary>
    /// 将 room 与 column2 上一个随机的房间进行相连
    /// </summary>
    /// <param name="room">需要连线的房间</param>
    /// <param name="column2">需要被连接的房间列表</param>
    /// <param name="check">如果是后面的房间向前连接则为true，如果是前面的房间向后连接则为false</param>
    /// <returns></returns>
    private Room ConnectToRandomRoom(Room room, List<Room> column2, bool check)
    {

        Room targetRoom;

        targetRoom = column2[UnityEngine.Random.Range(0, column2.Count)];
        var line = Instantiate(linePrefab, transform);
        line.SetPosition(0, room.transform.position);
        line.SetPosition(1, targetRoom.transform.position);

        //if (check)
        //{
        //    // 说明是后面的房间向前连接
        //    targetRoom.linkTo.Add(new Vector2Int(room.column, room.line));
        //}
        //else
        //{
        //    // 说明是前面的房间向后连接
        //    room.linkTo.Add(new Vector2Int(targetRoom.column, targetRoom.line));
        //}

        //// 创建房间之间的连线
        //var line = Instantiate(linePrefab, transform);
        //// 要确保一下连线的方向是正确的
        //if (check)
        //{
        //    // 说明是后面的房间向前连接
        //    line.SetPosition(0, targetRoom.transform.position);
        //    line.SetPosition(1, room.transform.position);
        //}
        //else
        //{
        //    // 说明是前面的房间向后连接
        //    line.SetPosition(0, room.transform.position);
        //    line.SetPosition(1, targetRoom.transform.position);
        //}
        //lines.Add(line);

        return targetRoom;
    }

    private RoomDataSO GetRoomData(RoomType roomType)
    {
        return roomDataDict[roomType];
    }

    private RoomType GetRandomRoomType(RoomType roomTypes)
    {
        string[] options = roomTypes.ToString().Split(',');

        string roomType = options[UnityEngine.Random.Range(0, options.Length)];
    
        return (RoomType)Enum.Parse(typeof(RoomType), roomType);
    }


}
