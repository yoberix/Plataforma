using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrulla")]
    public float speed = 1.5f;
    public float patrolDistance = 3f;

    [Header("Pisotón")]
    public float stompMargin = 0.25f;
    public float playerBounce = 9f;

    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;
    private float startX;
    private int direction = 1;
    private bool dead;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        startX = transform.position.x;
    }

    void FixedUpdate()
    {
        if (dead) return;

        float offset = transform.position.x - startX;
        if (offset > patrolDistance) direction = -1;
        else if (offset < -patrolDistance) direction = 1;

        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

        // Cambia el signo de flipX si tu sprite mira hacia el lado contrario
        if (sr != null) sr.flipX = direction < 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (dead || !collision.gameObject.CompareTag("Player")) return;

        float playerBottom = collision.collider.bounds.min.y;
        float enemyTop = col.bounds.max.y;

        if (playerBottom > enemyTop - stompMargin)
        {
            // El jugador cayó encima: el enemigo muere y el jugador rebota
            Die();
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, playerBounce);
        }
        else
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
            if (health != null) health.TakeDamage();
        }
    }

    void Die()
    {
        dead = true;
        rb.linearVelocity = Vector2.zero;
        col.enabled = false;
        rb.simulated = false;

        SoundManager.PlayEnemyDie();
        Destroy(gameObject, 0.15f);
    }
}