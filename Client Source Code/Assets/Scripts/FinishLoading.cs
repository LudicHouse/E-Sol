using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FinishLoading : MonoBehaviour {
    public Camera mainCamera;
    public GameObject canvas;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Flags the scene as done initializing, unhides the plant and UI, and destroys the loading indicator.
    /// </summary>
    public void ready()
    {
        mainCamera.enabled = true;
        canvas.SetActive(true);

        Object.Destroy(this.gameObject);
    }
}
