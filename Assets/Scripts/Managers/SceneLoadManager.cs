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
    private Vector2Int currentRoomVector;
    private Room currentRoom;
    
    public ObjectEventSO afterRoomLoadedEvent;
    public async void OnLoadRoomEvent(object data)
    {
        if (data is Room)
        {
            currentRoom = (Room)data;
            RoomDataSO currentData = currentRoom.roomData;
            currentRoomVector = new Vector2Int(currentRoom.col, currentRoom.row);
            // 设置当前场景
            currentScene = currentData.sceneToLoad;

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
        await UnloadSceneTask();

        //if (currentRoomVector != Vector2.one * -1)
        //{
        //    updateRoomEvent.RaiseEvent(currentRoomVector, this);
        //}
        currentScene = map;
        await LoadSceneTask();
    }

}