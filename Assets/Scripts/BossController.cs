using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BossController : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 5;
    public float invulnerableTime = 1.2f;

    [Header("Movimiento")]
    public float speed = 2f;
    public float speedBonusPerHit = 0.6f;   // Cada golpe recibido lo hace más rápido
    public float patrolDistance = 5f;

    [Header("Pisotón")]
    public float stompMargin = 0.35f;
    public float playerBounce = 11f;

    [Header("Al ser derrotado")]
    public LevelExit portalToUnlock;        // El portal que se abre al vencerlo
    public GameObject deathEffect;          // Efecto de partículas (opcional)

    private int health;
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;
    private float startX;
    private int direction = 1;
    private bool invulnerable;
    private bool dead;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        health = maxHealth;
        startX = transform.position.x;
    }

    void FixedUpdate()
    {
        if (dead) return;

        float offset = transform.position.x - startX;
        if (offset > patrolDistance) direction = -1;
        else if (offset < -patrolDistance) direction = 1;

        float currentSpeed = speed + (maxHealth - health) * speedBonusPerHit;
        rb.linearVelocity = new Vector2(direction * currentSpeed, rb.linearVelocity.y);

        // Cambia el signo si tu sprite mira hacia el lado contrario
        if (sr != null) sr.flipX = direction < 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleContact(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        HandleContact(collision);
    }

    void HandleContact(Collision2D collision)
    {
        if (dead || !collision.gameObject.CompareTag("Player")) return;

        float playerBottom = collision.collider.bounds.min.y;
        float bossTop = col.bounds.max.y;

        if (playerBottom > bossTop - stompMargin)
        {
            // El jugador cayó encima
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, playerBounce);

            if (!invulnerable) Hit();
        }
        else
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null) playerHealth.TakeDamage();
        }
    }

    void Hit()
    {
        health--;
        Debug.Log("Vida del jefe: " + health + "/" + maxHealth);

        if (health <= 0)
        {
            Die();
        }
        else
        {
            SoundManager.PlayBossHit();
            StartCoroutine(BlinkRoutine());
        }
    }

    IEnumerator BlinkRoutine()
    {
        invulnerable = true;
        float elapsed = 0f;
        while (elapsed < invulnerableTime)
        {
            if (sr != null) sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }
        if (sr != null) sr.enabled = true;
        invulnerable = false;
    }

    void Die()
    {
        dead = true;
        StopAllCoroutines();
        if (sr != null) sr.enabled = true;

        rb.linearVelocity = Vector2.zero;
        col.enabled = false;
        rb.simulated = false;

        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        SoundManager.PlayBossDie();

        if (portalToUnlock != null)
            portalToUnlock.AddOrb();

        Destroy(gameObject, 0.6f);
    }
}