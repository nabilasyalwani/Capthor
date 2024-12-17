using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnExitButtonClicked()
    {

        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif

    }

}
