using UnityEngine;

// Guarda vidas y monedas. Sobrevive al cambiar de escena.
// Pon un objeto con este script en CADA nivel: si ya existe uno, el duplicado se destruye solo.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int startingLives = 3;
    public int coinsForExtraLife = 20;

    public int lives;
    public int coins;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        lives = startingLives;
    }

    public void AddCoin(int amount)
    {
        coins += amount;
        if (coins >= coinsForExtraLife)
        {
            coins -= coinsForExtraLife;
            lives++;
        }
    }

    public void AddLife()
    {
        lives++;
    }

    // Devuelve true si el jugador sigue con vida
    public bool LoseLife()
    {
        lives--;
        return lives > 0;
    }

    public void ResetGame()
    {
        lives = startingLives;
        coins = 0;
    }
}
