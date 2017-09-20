using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash2 : MonoBehaviour
{

    void Start()
    {
        Invoke("Fade", 3f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fade();
            CancelInvoke();
        }
    }

    void Fade()
    {
        FadeManager.Instance.LoadScene("StartScreen", 0.5f);
    }
}
