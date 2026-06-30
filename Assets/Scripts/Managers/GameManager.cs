using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MapLayoutSO mapLayout;

    public void UpdateMapLayoutData(object value)
    {
        Debug.Log("Updating data with value: " + value);
        var roomVector = (Vector2Int)value;
        if (mapLayout.mapRoomDataList.Count == 0)
            return;

        // 从布局数据中找到当前房间
        var currentRoom = mapLayout.mapRoomDataList.Find(r => r.column == roomVector.x && r.row == roomVector.y);

        // 设置当前房间以及同列房间的状态
        currentRoom.roomState = RoomState.Visited;
        var sameColumnRooms = mapLayout.mapRoomDataList.FindAll(r => r.column == currentRoom.column);

        foreach (var room in sameColumnRooms)
        {
            if (room.column < currentRoom.column)
            {
                room.roomState = RoomState.Passed;
            }
            else if (room.column > currentRoom.column)
            {
                room.roomState = RoomState.Locked;
            }
            else if (room.row != currentRoom.row)
            {
                room.roomState = RoomState.Passed;
            }
            // if (room.row < roomVector.y)
            // {
            //     room.roomState = RoomState.Passed;
            // }
            // else if (room.row > roomVector.y)
            // {
            //     room.roomState = RoomState.Locked;
            // }
        }

        // 设置连线房间为可达
        foreach (var link in currentRoom.linkTo)
        {
            var linkedRoom = mapLayout.mapRoomDataList.Find(r => r.column == link.x && r.row == link.y);
            linkedRoom.roomState = RoomState.Attainable;
        }
    }

    public void OnNewGameEvent()
    {
        mapLayout.mapRoomDataList.Clear();
        mapLayout.linePositionList.Clear();
        CardManager.Instance.ResetPlayerData();
    }
}