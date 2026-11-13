using TMPro;
using UnityEngine;

public class HudUI : MonoBehaviour
{
    public TMP_Text livesText;
    public TMP_Text coinsText;

    void Update()
    {
        if (GameManager.Instance == null) return;

        livesText.text = "Vidas: " + GameManager.Instance.lives;
        coinsText.text = "Mana Orb: " + GameManager.Instance.coins;
    }
}
