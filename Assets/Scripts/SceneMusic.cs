using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    public AudioClip music;

    void Start()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.musicSource.clip = music;
            AudioManager.instance.musicSource.Play();
        }
    }
}
