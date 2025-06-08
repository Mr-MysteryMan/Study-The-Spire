using UnityEngine;

public class PixelCursorManager : MonoBehaviour
{
    [Header("像素光标贴图")]
    public Texture2D pixelCursorTexture;

    [Header("像素光标热点")]
    public Vector2 hotSpot = Vector2.zero;

    private void Start()
    {
        pixelCursorTexture = Resources.Load<Texture2D>("Cursors/mouse");
        if (pixelCursorTexture == null)
        {
            Debug.LogError("光标贴图未加载！");
        }

        ApplyPixelCursor();
    }


    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            // 鼠标回到游戏窗口，应用像素光标
            ApplyPixelCursor();
        }
        else
        {
            // 鼠标离开游戏窗口，还原系统光标
            RestoreDefaultCursor();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            RestoreDefaultCursor();
        }
        else
        {
            ApplyPixelCursor();
        }
    }

    private void ApplyPixelCursor()
    {
        Cursor.SetCursor(pixelCursorTexture, hotSpot, CursorMode.Auto);
    }

    private void RestoreDefaultCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
