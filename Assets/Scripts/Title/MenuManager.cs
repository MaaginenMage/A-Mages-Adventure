using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject settingsPanel;

    private void Start()
    {
        settingsPanel.SetActive(false);
    }
    public void StartGame(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void OpenSettings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackToMain()
    {
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
