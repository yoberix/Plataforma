using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float invulnerableTime = 1.2f;

    [Header("Game Over")]
    public GameObject gameOverLabel;          // El texto "¡MORISTE!" del Canvas
    public string firstSceneName = "Nivel1";  // Escena a la que se vuelve al morir
    public float gameOverDelay = 2.5f;        // Segundos que se ve el letrero

    private bool invulnerable;
    private bool dead;
    private SpriteRenderer sr;
    private Vector3 spawnPoint;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        spawnPoint = transform.position;   // El punto donde empieza el nivel

        if (gameOverLabel != null) gameOverLabel.SetActive(false);
    }

    public void TakeDamage()
    {
        if (invulnerable || dead) return;
        if (GameManager.Instance == null) return;

        bool stillAlive = GameManager.Instance.LoseLife();

        if (stillAlive)
        {
            SoundManager.PlayHurt();
            StartCoroutine(InvulnerabilityBlink());
        }
        else
        {
            Die();
        }
    }

    // Caída al vacío: pierde una vida y vuelve al punto de inicio
    public void Fall()
    {
        if (dead) return;
        if (GameManager.Instance == null) return;

        bool stillAlive = GameManager.Instance.LoseLife();

        if (stillAlive)
        {
            SoundManager.PlayHurt();
            transform.position = spawnPoint;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;

            StopAllCoroutines();
            sr.enabled = true;
            StartCoroutine(InvulnerabilityBlink());
        }
        else
        {
            Die();
        }
    }

    IEnumerator InvulnerabilityBlink()
    {
        invulnerable = true;
        float elapsed = 0f;
        while (elapsed < invulnerableTime)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }
        sr.enabled = true;
        invulnerable = false;
    }

    void Die()
    {
        dead = true;
        StopAllCoroutines();
        sr.enabled = true;

        PlayerController controller = GetComponent<PlayerController>();
        if (controller != null) controller.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        if (gameOverLabel != null) gameOverLabel.SetActive(true);

        SoundManager.PlayDeath();
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(gameOverDelay);

        if (GameManager.Instance != null) GameManager.Instance.ResetGame();
        SceneManager.LoadScene(firstSceneName);
    }
}