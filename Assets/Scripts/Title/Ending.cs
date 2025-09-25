using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    public Image Image1;
    public Image Image2;
    private float p;

    private void Start()
    {
        Image1.GetComponent<Image>();
        Scene currentScene = SceneManager.GetActiveScene();
        StartCoroutine(End());
    }

    private void Update()
    {
        //for (int i = 0; i < 100; i++)
        //{
        //    StartCoroutine(Lighten());
        //}
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
    }

    //IEnumerator Lighten()
    //{
    //    p += 0.01f;
    //    Image1.color = new Color(p, p, p);
    //    yield return new WaitForSeconds(1f);
    //}

    IEnumerator End()
    {
        yield return new WaitForSeconds(180f);
        SceneManager.LoadScene(0);
    }
}
