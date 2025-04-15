using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfigSO", menuName = "Map/MapConfigSO")]

public class MapConfigSO : ScriptableObject
{
    public List<RoomBlueprint> roomBlueprint;
}

[System.Serializable]
public class RoomBlueprint
{
    public int max, min;

    public RoomType roomType;

}