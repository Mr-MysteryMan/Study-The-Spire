using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public MapConfigSO mapConfig;

    public Room roomPrefab;

    private float screenHeight;

    private float screenWidth;

    private float colWidth;

    private float rowWidth;

    private Vector3 generatePoint;


    public void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;
        colWidth = screenWidth / (mapConfig.roomBlueprint.Count + 1);
    }

    private void Start()
    {
        CreatMap();
        Debug.Log("�������õ�ͼ");

    }

    public void CreatMap()
    {
        for (int col = 0; col < mapConfig.roomBlueprint.Count; col++)
        {
            var blueprint = mapConfig.roomBlueprint[col];

            var amount = Random.Range(blueprint.min, blueprint.max + 1);

            rowWidth = screenHeight / (amount + 1);

            //ѭ�����ɵ�ǰ�еķ���
            for (int i = 0; i < amount; i++)
            {
                generatePoint = new Vector3(-(screenWidth / 2) + colWidth * (col + 1), screenHeight / 2 - rowWidth * (i + 1), 0);

                var room = Instantiate(roomPrefab, generatePoint,Quaternion.identity,transform);

            }
        }
    }


}
