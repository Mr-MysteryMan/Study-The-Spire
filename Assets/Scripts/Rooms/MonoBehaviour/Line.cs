using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public float offsetSpeed = 0.1f;

    private void Update()
    {
        if (lineRenderer != null)
        {
            // ��ȡ��ǰ����ƫ��
            var offset = lineRenderer.material.mainTextureOffset;
            offset.x -= offsetSpeed * Time.deltaTime;
            lineRenderer.material.mainTextureOffset = offset;
        }
    }
}
