using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CameraController : MonoBehaviour
{
    public GameObject camObj;
    public Camera cam;
    private void Start()
    {
        camObj.SetActive(false);
    }

    public void ZoomOutToBoss()
    {
        cam.orthographicSize = 15f;
        camObj.SetActive(true);
        StartCoroutine(ZoomRoutine());
    }

    IEnumerator ZoomRoutine()
    {
        float targetSize = 50f;
        float duration = 1.5f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, t / duration);
            yield return null;
        }
        cam.orthographicSize = targetSize;
    }
}
