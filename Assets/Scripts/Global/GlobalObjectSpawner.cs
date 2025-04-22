using UnityEngine;

public class GlobalObjectSpawner : MonoBehaviour
{
    public GameObject cardManagerPrefab;

    private void Awake()
    {
        if (CardManager.Instance == null)
        {
            GameObject go = Instantiate(cardManagerPrefab);
            DontDestroyOnLoad(go);
        }
    }
}
