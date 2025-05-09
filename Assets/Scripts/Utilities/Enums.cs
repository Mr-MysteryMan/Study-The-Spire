
using System;
using System.Collections.Generic;

[Flags]
public enum RoomType //��������
{
    MinorEnemy = 1, //��ͨ����
    EliteEnemy = 2, //��Ӣ����
    Shop = 4, //�̵�
    Treasure = 8, //����
    RestRoom = 16, //����
    BossRoom = 32,
}

public static class RoomTypeExtension
{
    public static List<RoomType> combatTypes = new() { RoomType.MinorEnemy, RoomType.EliteEnemy, RoomType.BossRoom };
    public static bool IsCombatRoom(this RoomType roomType)
    {
        return combatTypes.Contains(roomType);
    }
}

public enum RoomState //����״̬
{
    Locked,
    Visited,
    Attainable,
    Passed,
}

public enum LineState
{
    Move,
    NoMove
}
