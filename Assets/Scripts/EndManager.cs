using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_message;

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void text(string text)
    {
        m_message.text = text;
    }
}
