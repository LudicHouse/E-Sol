using UnityEngine;
using System.Collections;

public class TouchTest : MonoBehaviour {
    Vector2 dragTotal;
    Vector2 screenDragTotal;
    float pinchTotal;
    float screenPinchTotal;
    TouchManager tMan;

	// Use this for initialization
	void Start () {
        tMan = Object.FindObjectOfType<TouchManager>();
        tMan.onDragStart += onDragStart;
        tMan.onDrag += onDrag;
        tMan.onDragStop += onDragStop;
        tMan.onPinchStart += onPinchStart;
        tMan.onPinch += onPinch;
        tMan.onPinchStop += onPinchStop;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void onDragStart(Vector2 startPos)
    {
        dragTotal = Vector2.zero;
        screenDragTotal = Vector2.zero;
    }

    private void onDrag(Vector2 delta)
    {
        dragTotal += delta;
        screenDragTotal += (Vector2)Camera.main.ScreenToWorldPoint(delta) - (Vector2)Camera.main.ScreenToWorldPoint(Vector2.zero);
    }

    private void onDragStop()
    {
        Debug.Log("Drag total: " + dragTotal + " (Magnitude " + dragTotal.magnitude + ")");
        Debug.Log("Screen drag total: " + screenDragTotal + " (Magnitude " + screenDragTotal.magnitude + ")");
    }

    private void onPinchStart()
    {
        pinchTotal = 0;
        screenPinchTotal = 0;
    }

    private void onPinch(float delta)
    {
        pinchTotal += delta;
        screenPinchTotal += Camera.main.ScreenToWorldPoint(new Vector2(delta, 0)).x - Camera.main.ScreenToWorldPoint(Vector2.zero).x;
    }

    private void onPinchStop()
    {
        Debug.Log("Pinch total: " + pinchTotal);
        Debug.Log("Screen pinch total: " + screenPinchTotal);
    }
}
