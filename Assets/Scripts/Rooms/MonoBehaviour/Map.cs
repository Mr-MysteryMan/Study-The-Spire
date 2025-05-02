using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public MapConfigSO mapConfig;
    public MapLayoutSO mapLayout;
    public Room roomPrefab;
    public LineRenderer linePrefab;

    private float screenHeight;

    private float screenWidth;

    private float colWidth;

    private float rowWidth;

    private Vector3 generatePoint;

    public List<RoomDataSO> roomDataList = new();
    private List<Room> rooms = new List<Room>();
    private List<LineRenderer> lines = new List<LineRenderer>();

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

    private void OnEnable()
    {
        Debug.Log("OnEnable" + mapLayout);
       // CreateMap();
        if (mapLayout.mapRoomDataList.Count > 0)
        {
          //  CreateMap();
            LoadMap();
        }
        else
        {
            CreateMap();
        }
    }

    public void CreateMap()
    {
        List<Room> preColRooms = new();

        for (int col = 0; col < mapConfig.roomBlueprint.Count; col++)
        {
            var blueprint = mapConfig.roomBlueprint[col];

            var amount = UnityEngine.Random.Range(blueprint.min, blueprint.max + 1);

            List<Room> curColRooms = new();

            rowWidth = screenHeight / (amount + 1);

            //ѭ�����ɵ�ǰ�еķ���
            for (int i = 0; i < amount; i++)
            {
                generatePoint = new Vector3(-(screenWidth / 2) + colWidth * (col + 1), screenHeight / 2 - rowWidth * (i + 1), 0);

                var room = Instantiate(roomPrefab, generatePoint,Quaternion.identity,transform);
                RoomType newType = GetRandomRoomType(mapConfig.roomBlueprint[col].roomType);
                if (col == 0)
                {
                    room.roomState = RoomState.Attainable;
                }
                else
                {
                    room.roomState = RoomState.Locked;
                }
                room.SetupRoom(col, i, GetRoomData(newType));
                
                rooms.Add(room);
                curColRooms.Add(room);
            }

            //�ж�����
            if (preColRooms.Count > 0)
            {
                // ���������б�ķ�������
                CreateConnection(preColRooms, curColRooms);
            }

            preColRooms = curColRooms;

        }
        
        SaveMap();
    }

    private void CreateConnection(List<Room> column1, List<Room> column2)
    {
        HashSet<Room> connectedColumn2Rooms = new HashSet<Room>();

        foreach (var room in column1)
        {
            var targetRoom = ConnectToRandomRoom(room, column2, false);
            connectedColumn2Rooms.Add(targetRoom);
        }

        // ���ȷ�� Column2 �����з��䶼�����ӵķ���
        foreach (var room in column2)
        {
            if (!connectedColumn2Rooms.Contains(room))
            {
                ConnectToRandomRoom(room, column1, true);
            }
        }
    }

    /// <summary>
    /// �� room �� column2 ��һ������ķ����������
    /// </summary>
    /// <param name="room">��Ҫ���ߵķ���</param>
    /// <param name="column2">��Ҫ�����ӵķ����б�</param>
    /// <param name="check">����Ǻ���ķ�����ǰ������Ϊtrue�������ǰ��ķ������������Ϊfalse</param>
    /// <returns></returns>
    private Room ConnectToRandomRoom(Room room, List<Room> column2, bool check)
    {

        Room targetRoom;

        targetRoom = column2[UnityEngine.Random.Range(0, column2.Count)];
        if (check)
        {
            targetRoom.linkTo.Add(new Vector2Int(room.col, room.row));
            Debug.Log("123");
        }
        else
        {
            Debug.Log("123");
            room.linkTo.Add(new Vector2Int(targetRoom.col, targetRoom.row));
        }

        var line = Instantiate(linePrefab, transform);
        line.SetPosition(0, room.transform.position);
        line.SetPosition(1, targetRoom.transform.position);
        lines.Add(line);
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
    
    private void SaveMap()
    {
      
        mapLayout.mapRoomDataList = new List<MapRoomData>();
        Debug.Log("222 " + mapLayout.mapRoomDataList.Count);
        // 添加已经生成的房间
        for (int i = 0; i < rooms.Count; i++)
        {
            var mapRoom = new MapRoomData()
            {
                posX = rooms[i].transform.position.x,
                posY = rooms[i].transform.position.y,
                column = rooms[i].col,
                row = rooms[i].row,
                roomData = rooms[i].roomData,
                roomState = rooms[i].roomState,
                linkTo = rooms[i].linkTo,
            };

            mapLayout.mapRoomDataList.Add(mapRoom);
        }

        mapLayout.linePositionList = new List<LinePosition>();
        // 添加连线
        for (int i = 0; i < lines.Count; i++)
        {
            var line = new LinePosition()
            {
                startPos = new SerializeVector3(lines[i].GetPosition(0)),
                endPos = new SerializeVector3(lines[i].GetPosition(1))
            };

            mapLayout.linePositionList.Add(line);
        }
    }
    
    private void LoadMap()
    {
        // 读取房间数据生成房间
        for (int i = 0; i < mapLayout.mapRoomDataList.Count; i++)
        {
            MapRoomData roomData = mapLayout.mapRoomDataList[i];
            var newPos = new Vector3(roomData.posX, roomData.posY, 0);
            var newRoom = Instantiate(roomPrefab, newPos, Quaternion.identity, transform);
            newRoom.roomState = roomData.roomState;
            newRoom.SetupRoom(roomData.column, roomData.row, roomData.roomData);
            newRoom.linkTo = roomData.linkTo;

            rooms.Add(newRoom);
        }

        // 读取连线数据生成连线
        for (int i = 0; i < mapLayout.linePositionList.Count; i++)
        {
            var line = Instantiate(linePrefab, transform);
            line.SetPosition(0, mapLayout.linePositionList[i].startPos.GetVector3());
            line.SetPosition(1, mapLayout.linePositionList[i].endPos.GetVector3());

            lines.Add(line);
        }
    }


}
