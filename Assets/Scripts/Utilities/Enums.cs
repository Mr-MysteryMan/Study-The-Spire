
using System;

[Flags]
public enum RoomType //��������
{
    MinorEnemy = 1, //��ͨ����
    EliteEnemy = 2, //��Ӣ����
    Shop = 4, //�̵�
    Treasure = 8, //����
    RestRoom = 16, //����
    BossRoom = 32
}

public enum RoomState //����״̬
{
    Locked, //��ʱ�޷�����
    Visited, //�Ѿ�������
    Attainable //����ǰ����
}
