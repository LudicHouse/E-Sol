using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FixedHeightTracker : MonoBehaviour {
    public float region;

	// Use this for initialization
	void Start () {
        Vector2 newPos = transform.position;
        newPos.y = Camera.main.WorldToScreenPoint(new Vector2(0, region)).y;
        transform.position = newPos;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector2 newPos = transform.position;
        newPos.y = Camera.main.WorldToScreenPoint(new Vector2(0, region)).y;
        transform.position = newPos;
	}
}
