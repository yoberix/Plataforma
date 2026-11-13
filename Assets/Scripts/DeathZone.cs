using UnityEngine;

// Zona invisible debajo del nivel. Si el jugador cae en ella, pierde una vida y reaparece.
[RequireComponent(typeof(Collider2D))]
public class DeathZone : MonoBehaviour
{
    void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null) health.Fall();
    }
}
