using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string firstLevelName = "Nivel1";

    // Botón "Jugar"
    public void Play()
    {
        if (GameManager.Instance != null) GameManager.Instance.ResetGame();
        SceneManager.LoadScene(firstLevelName);
    }

    // Botón "Salir"
    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
