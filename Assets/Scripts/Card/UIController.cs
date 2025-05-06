using UnityEngine;

namespace Combat
{
    public class UIController : MonoBehaviour
    {
        void Awake()
        {
            this.GetComponent<Canvas>().worldCamera = Camera.main; // 设置UI的世界相机为主相机
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}