using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour
{
    public enum CollectibleType { Coin, Heart }

    public CollectibleType type = CollectibleType.Coin;
    public int value = 1;

    [Header("Portal (opcional)")]
    [Tooltip("Si lo asignas, al recoger este objeto cuenta como llave para abrir ese portal")]
    public LevelExit portalToUnlock;

    [Header("Efectos (opcional)")]
    public GameObject pickupEffect;   // Prefab de partículas que se crea al recoger

    void Reset()
    {
        // Al añadir el script, el collider se vuelve trigger automáticamente
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (GameManager.Instance == null) return;

        if (type == CollectibleType.Coin)
            GameManager.Instance.AddCoin(value);
        else
            GameManager.Instance.AddLife();

        if (portalToUnlock != null)
            portalToUnlock.AddOrb();

        if (pickupEffect != null)
            Instantiate(pickupEffect, transform.position, Quaternion.identity);

        SoundManager.PlayCollect();
        Destroy(gameObject);
    }
}