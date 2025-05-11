using UnityEngine;
using UnityEngine.AddressableAssets;

public class InitLoad : MonoBehaviour
{
    public AssetReference Scene;

    private void Awake() {
        Addressables.LoadSceneAsync(Scene);
    }
}
