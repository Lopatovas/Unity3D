using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delay : MonoBehaviour {

    public GameObject countDown;
    public float delay;

	// Use this for initialization
	void Start () {
        StartCoroutine("StartDelay");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator StartDelay()
    {
        Time.timeScale = 0f;
        float pauseTime = Time.realtimeSinceStartup + delay;
        while(Time.realtimeSinceStartup < pauseTime)
        {
            yield return 0;
        }
        countDown.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
