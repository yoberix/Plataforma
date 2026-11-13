using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;                         // El Player
    public Vector3 offset = new Vector3(0f, 1.5f, -10f);
    public float smoothTime = 0.15f;

    [Header("Límites del nivel (opcional)")]
    public bool useLimits = false;
    public float minX = -10f;
    public float maxX = 50f;
    public float minY = -5f;
    public float maxY = 10f;

    private Vector3 velocity;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = target.position + offset;

        if (useLimits)
        {
            desired.x = Mathf.Clamp(desired.x, minX, maxX);
            desired.y = Mathf.Clamp(desired.y, minY, maxY);
        }

        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
    }
}
