using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ProtoScene");
    }
}
