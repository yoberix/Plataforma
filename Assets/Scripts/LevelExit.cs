using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class LevelExit : MonoBehaviour
{
    [Tooltip("Nombre exacto de la escena a cargar (debe estar en Build Profiles > Scene List)")]
    public string nextSceneName = "Nivel2";

    [Header("Desbloqueo")]
    [Tooltip("Cuántos maná orbs hay que recoger para abrir el portal")]
    public int orbsRequired = 1;

    [Header("Efectos visuales (opcional)")]
    public GameObject lockedVisual;     // Se ve mientras el portal está bloqueado
    public GameObject unlockedVisual;   // Se activa cuando el portal se abre (la magia azul)
    public GameObject activateEffect;   // Efecto al entrar al portal (por ejemplo PickupBurst)
    public float delay = 0.8f;          // Tiempo antes de cambiar de nivel

    [Header("Victoria (solo en el último nivel)")]
    public GameObject victoryLabel;     // Texto "¡GANASTE!" del Canvas (opcional)

    private int orbsCollected;
    private bool unlocked;
    private bool activated;

    void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void Start()
    {
        unlocked = orbsRequired <= 0;
        ApplyVisuals();

        if (victoryLabel != null) victoryLabel.SetActive(false);
    }

    // Lo llama el Collectible (o el jefe) cada vez que se consigue una "llave"
    public void AddOrb()
    {
        orbsCollected++;
        Debug.Log("Maná recogido: " + orbsCollected + "/" + orbsRequired);

        if (!unlocked && orbsCollected >= orbsRequired)
        {
            unlocked = true;
            ApplyVisuals();
            SoundManager.PlayPortalOpen();
        }
    }

    void ApplyVisuals()
    {
        if (lockedVisual != null) lockedVisual.SetActive(!unlocked);
        if (unlockedVisual != null) unlockedVisual.SetActive(unlocked);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!unlocked || activated || !other.CompareTag("Player")) return;
        activated = true;

        // Detiene al jugador mientras se activa la magia
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null) controller.enabled = false;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        if (activateEffect != null)
            Instantiate(activateEffect, transform.position, Quaternion.identity);

        if (victoryLabel != null) victoryLabel.SetActive(true);

        SoundManager.PlayLevelComplete();
        Invoke(nameof(LoadNext), delay);
    }

    void LoadNext()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}