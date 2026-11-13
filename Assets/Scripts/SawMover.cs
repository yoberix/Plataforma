using UnityEngine;

// Mueve un objeto de ida y vuelta desde su posición inicial.
// Úsalo junto con Hazard para la sierra.
public class SawMover : MonoBehaviour
{
    [Tooltip("Hasta dónde se mueve desde su posición inicial. Ejemplo: X 4, Y 0 = 4 unidades a la derecha y vuelve")]
    public Vector2 moveOffset = new Vector2(4f, 0f);

    [Tooltip("Velocidad en unidades por segundo")]
    public float speed = 2f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float distance = moveOffset.magnitude;
        if (distance <= 0.001f) return;

        float t = Mathf.PingPong(Time.time * speed / distance, 1f);
        transform.position = startPosition + (Vector3)(moveOffset * t);
    }

    void OnDrawGizmosSelected()
    {
        // Muestra el recorrido en la Scene cuando seleccionas la sierra
        Vector3 origin = Application.isPlaying ? startPosition : transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + (Vector3)moveOffset);
        Gizmos.DrawWireSphere(origin + (Vector3)moveOffset, 0.2f);
    }
}
