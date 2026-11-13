using UnityEngine;

// Reproduce los efectos de sonido del juego. Sobrevive al cambiar de escena.
// Se usa como prefab: pon el mismo prefab en cada escena (el duplicado se destruye solo).
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Jugador")]
    public AudioClip jump;
    public AudioClip collect;
    public AudioClip hurt;
    public AudioClip death;

    [Header("Enemigos y jefe")]
    public AudioClip enemyDie;
    public AudioClip bossHit;
    public AudioClip bossDie;

    [Header("Portal")]
    public AudioClip portalOpen;
    public AudioClip levelComplete;

    [Range(0f, 1f)] public float volume = 0.8f;

    private AudioSource source;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
    }

    void PlayClip(AudioClip clip)
    {
        if (clip == null || source == null) return;
        source.PlayOneShot(clip, volume);
    }

    // Atajos seguros: si no hay SoundManager en la escena, no pasa nada
    public static void PlayJump()          { if (Instance != null) Instance.PlayClip(Instance.jump); }
    public static void PlayCollect()       { if (Instance != null) Instance.PlayClip(Instance.collect); }
    public static void PlayHurt()          { if (Instance != null) Instance.PlayClip(Instance.hurt); }
    public static void PlayDeath()         { if (Instance != null) Instance.PlayClip(Instance.death); }
    public static void PlayEnemyDie()      { if (Instance != null) Instance.PlayClip(Instance.enemyDie); }
    public static void PlayBossHit()       { if (Instance != null) Instance.PlayClip(Instance.bossHit); }
    public static void PlayBossDie()       { if (Instance != null) Instance.PlayClip(Instance.bossDie); }
    public static void PlayPortalOpen()    { if (Instance != null) Instance.PlayClip(Instance.portalOpen); }
    public static void PlayLevelComplete() { if (Instance != null) Instance.PlayClip(Instance.levelComplete); }
}
