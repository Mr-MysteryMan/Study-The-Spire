
using System;

[Flags]
public enum RoomType //房间类型
{
    MinorEnemy = 1, //普通敌人
    EliteEnemy = 2, //精英敌人
    Shop = 4, //商店
    Treasure = 8, //宝箱
    RestRoom = 16, //休整
    BossRoom = 32
}

public enum RoomState //房间状态
{
    Locked, //暂时无法到达
    Visited, //已经经过的
    Attainable //可以前往的
}
