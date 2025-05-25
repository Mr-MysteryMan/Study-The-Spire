using System.Collections;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private ParticleSystem ps;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public IEnumerator WaitForEnd()
    {
        while (ps != null && ps.isPlaying)
        {
            yield return null;
        }
    }
}
