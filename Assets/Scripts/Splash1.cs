using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("Fade", 3f);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Fade();
            CancelInvoke();
        }
    }

    void Fade()
    {
        FadeManager.Instance.LoadScene("Splash2", 0.5f);
    }
}
