using UnityEngine;
using DG.Tweening;

public class Punch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void PunchEffect()
    {
        // transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.5f, 10, 1);
        transform.DOShakePosition(
            duration: 0.5f,
            strength: 4.0f,
            vibrato: 20,
            randomness: 90,   // 0～180°随机
            fadeOut: true
        );
    }

    // Update is called once per frame
    void Update()
    {

    }
}
