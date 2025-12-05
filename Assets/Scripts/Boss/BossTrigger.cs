using TMPro.Examples;
using UnityEngine;
using System.Collections;

public class SnakeBossTrigger : MonoBehaviour
{
    public Boss boss;
    public CameraController cameraController;
    public GameObject cam;
    public GameObject arena;
    public CanvasGroup fadeScreen; // black overlay

    private bool triggered;

    private void Start()
    {
        arena.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && collision.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(StartBossSequence());
        }
    }

    IEnumerator StartBossSequence()
    {
        // 1. Close arena doors
        arena.SetActive(true);

        yield return new WaitForSeconds(0.6f);

        // 2. Zoom camera out
        cameraController.ZoomOutToBoss();

        yield return new WaitForSeconds(1.2f);

        // 3. Animate giant eye blink
        boss.anim.SetTrigger("Started");

        yield return new WaitForSeconds(0.8f);

        // 4. Fade to black
        yield return StartCoroutine(Fade(1f));

        // 5. Spawn boss
        boss.anim.SetTrigger("FadedIn");

        // 6. Fade back in
        yield return StartCoroutine(Fade(0f));

        // 7. Start fight
        boss.StartFight();
    }

    IEnumerator Fade(float target)
    {
        float t = 0f;
        float start = fadeScreen.alpha;

        while (t < 1f)
        {
            t += Time.deltaTime * 2.5f;
            fadeScreen.alpha = Mathf.Lerp(start, target, t);
            yield return null;
        }
    }
}
