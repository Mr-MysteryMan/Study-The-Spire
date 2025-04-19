using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header(header:"��ͼ���ñ�")]
    public MapConfigSO mapConfig;

    [Header(header: "Ԥ����")]
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
        Debug.Log("�������õ�ͼ");

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

            //ѭ�����ɵ�ǰ�еķ���
            for (int i = 0; i < amount; i++)
            {
                generatePoint = new Vector3(-(screenWidth / 2) + colWidth * (col + 1), screenHeight / 2 - rowWidth * (i + 1), 0);

                var room = Instantiate(roomPrefab, generatePoint,Quaternion.identity,transform);
                RoomType newType = GetRandomRoomType(mapConfig.roomBlueprint[col].roomType);
                room.SetupRoom(col, i, GetRoomData(newType));

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
        var line = Instantiate(linePrefab, transform);
        line.SetPosition(0, room.transform.position);
        line.SetPosition(1, targetRoom.transform.position);

        //if (check)
        //{
        //    // ˵���Ǻ���ķ�����ǰ����
        //    targetRoom.linkTo.Add(new Vector2Int(room.column, room.line));
        //}
        //else
        //{
        //    // ˵����ǰ��ķ����������
        //    room.linkTo.Add(new Vector2Int(targetRoom.column, targetRoom.line));
        //}

        //// ��������֮�������
        //var line = Instantiate(linePrefab, transform);
        //// Ҫȷ��һ�����ߵķ�������ȷ��
        //if (check)
        //{
        //    // ˵���Ǻ���ķ�����ǰ����
        //    line.SetPosition(0, targetRoom.transform.position);
        //    line.SetPosition(1, room.transform.position);
        //}
        //else
        //{
        //    // ˵����ǰ��ķ����������
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
