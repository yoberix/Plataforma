using UnityEngine;

// Obstáculo que hace daño al jugador al tocarlo (pinchos, sierra, fuego...).
// Pon un Collider2D con "Is Trigger" marcado en el mismo objeto.
[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour
{
    void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Hit(other);
    }

    // Si el jugador se queda encima, vuelve a recibir daño cuando termina su invulnerabilidad
    void OnTriggerStay2D(Collider2D other)
    {
        Hit(other);
    }

    void Hit(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null) health.TakeDamage();
    }
}
