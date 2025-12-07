using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseUI;
    private bool paused = false;

    private void Start()
    {
        pauseUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused) Resume();
            else PauseG();
        }
    }

    public void Resume()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    public void PauseG()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    public void QuitToMenu(string name)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(name);
    }

}
