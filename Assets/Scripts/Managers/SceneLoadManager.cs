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

    public async void OnLoadRoomEvent(object data)
    {
        if (data is RoomDataSO)
        {
            var room = (RoomDataSO)data;
            Debug.Log(room.roomType);
            currentScene = room.sceneToLoad;
        }

        //卸载当前房间
        await UnloadSceneTask();

        //加载新房间
        await LoadSceneTask();
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
