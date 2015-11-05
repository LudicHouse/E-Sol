using UnityEngine;
using System.Collections;

public class TouchAndDrag : MonoBehaviour {
    public float dragForce;

	private int touchCount = 0;
	private int touchingId = -1;
    private Vector2 fingerStartPos;
    private Vector2 objectStartPos;

	private Collider2D col;
    private Rigidbody2D rb;
    //private SpringJoint2D spring;

	// Use this for initialization
	void Start () {
		col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        //spring = GetComponent<SpringJoint2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate()
	{
        if (Input.touchCount > touchCount & touchingId == -1)
        {
            Debug.Log("New touch!");
            checkNewTouches();
        }

        if (touchingId != -1)
        {
            Touch t = Input.GetTouch(touchingId);

            if (t.phase == TouchPhase.Ended | t.phase == TouchPhase.Canceled)
            {
                Debug.Log("Touch ended.");
                //spring.enabled = true;
                touchingId = -1;
            }
            else
            {
                Vector2 fingerNewPos = Camera.main.ScreenToWorldPoint(t.position); //Note: Revisit this when working on camera scrolling
                Vector2 fingerDiff = fingerNewPos - fingerStartPos;

                Vector2 objectTargetPos = objectStartPos + fingerDiff;
                Vector2 objectDiff = objectTargetPos - (Vector2)transform.position;

                rb.AddForce(objectDiff * dragForce * Time.deltaTime);
            }
        }

        touchCount = Input.touchCount;
	}

    /// <summary>
    /// Checks if the user is touching the object and begins the dragging process.
    /// </summary>
	private void checkNewTouches()
	{
		foreach (Touch t in Input.touches)
        {
			Vector2 worldPos = Camera.main.ScreenToWorldPoint(t.position);
			if (col == Physics2D.OverlapPoint(worldPos))
			{
                Debug.Log("Touching object!");
				touchingId = t.fingerId;
                fingerStartPos = worldPos;
                objectStartPos = transform.position;
                //spring.enabled = false;
				return;
			}
		}
	}
}
