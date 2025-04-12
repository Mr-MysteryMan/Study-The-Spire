public enum RoomType //房间类型
{
    MinorEnemy, //普通敌人
    EliteEnemy, //精英敌人
    Shop, //商店
    Treasure, //宝箱
    RestRoom, //休整
    BossRoom
}

public enum RoomState //房间状态
{
    Locked, //暂时无法到达
    Visited, //已经经过的
    Attainable //可以前往的
}
