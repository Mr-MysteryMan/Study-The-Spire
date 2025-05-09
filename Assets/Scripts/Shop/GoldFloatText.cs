using UnityEngine;
using UnityEngine.UI;

public class GoldFloatText : MonoBehaviour {
    public Text text;
    public float floatSpeed = 40f;
    public float fadeDuration = 0.5f;

    private float timer = 0f;
    private CanvasGroup canvasGroup;

    void Start() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update() {
        timer += Time.deltaTime;
        transform.localPosition += Vector3.up * floatSpeed * Time.deltaTime;

        if (canvasGroup != null) {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
        }

        if (timer >= fadeDuration) {
            Destroy(gameObject);
        }
    }

    public void SetAmount(int amount) {
        text.text = $"-{amount}";
    }
}
