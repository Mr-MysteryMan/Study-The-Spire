using System.Threading.Tasks;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private AssetReference currentScene;

    public AssetReference map;
    public AssetReference menu;

    private Vector2Int currentRoomVector;
    private Room currentRoom;
    public ObjectEventSO updateRoomEvent;
    public ObjectEventSO afterRoomLoadedEvent;

    public Camera mainCamera;

    private void Awake()
    {
        currentRoomVector = Vector2Int.one * -1;
        LoadMenu();
        //   LoadMap();
    }
    public async void OnLoadRoomEvent(object data)
    {
        if (data is Room)
        {
            currentRoom = (Room)data;
            RoomDataSO currentData = currentRoom.roomData;
            currentRoomVector = new Vector2Int(currentRoom.col, currentRoom.row);
            // 设置当前场景
            currentScene = currentData.sceneToLoad;
            if (currentData.roomType.IsCombatRoom())
            {
                Camera.main.gameObject.SetActive(false);
            }
        }
        await UnloadSceneTask();
        await LoadSceneTask();
        afterRoomLoadedEvent.RaiseEvent(currentRoomVector, this);
    }

    private async Awaitable LoadSceneTask()
    {
        var s = currentScene.LoadSceneAsync(LoadSceneMode.Additive);
        await s.Task;

        if (s.Status == AsyncOperationStatus.Succeeded)
        {
            SceneManager.SetActiveScene(s.Result.Scene);
        }
    }

    private async Awaitable UnloadSceneTask()
    {
        await Awaitable.WaitForSecondsAsync(0.45f);
        await Awaitable.FromAsyncOperation(SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()));
    }

    public async void LoadMap()
    {
        if (Camera.main == null || Camera.main != mainCamera)
        {
            if (Camera.main)
            {
                Camera.main.gameObject.SetActive(false);
            }
            mainCamera.gameObject.SetActive(true);
        }
        if (currentScene != null)
        {
            await UnloadSceneTask();
        }

        //        await UnloadSceneTask();

        if (currentRoomVector != Vector2.one * -1)
        {
            updateRoomEvent.RaiseEvent(currentRoomVector, this);
        }
        currentScene = map;
        await LoadSceneTask();
    }

    public async void LoadMenu()
    {
        if (Camera.main == null || Camera.main != mainCamera)
        {
            if (Camera.main)
            {
                Camera.main.gameObject.SetActive(false);
            }
            mainCamera.gameObject.SetActive(true);
        }
        if (currentScene != null)
            await UnloadSceneTask();

        currentScene = menu;
        await LoadSceneTask();
    }

}