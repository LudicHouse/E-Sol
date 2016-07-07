using UnityEngine;
using System.Collections;

public class FollowDraggingFinger : MonoBehaviour {
    private TouchManager tMan;

	// Use this for initialization
	void Start () {
        tMan = Object.FindObjectOfType<TouchManager>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Camera.main.ScreenToWorldPoint(tMan.getDraggingTouch().position);
	}
}
