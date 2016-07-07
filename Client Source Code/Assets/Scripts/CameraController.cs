using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public Transform flower;
    public float flowerHeightMargin;
    public float zoomSpeed;
    public float scrollAverageSize;
    public float maxZoomDist;
    public float deadZoneMargin;

    public GameObject topButton;
    public GameObject bottomButton;
    public float minButtonShowDist;

    private float lowerLimit;
    private float upperLimit;
    private float lowerZoom;

    private bool scrolling = false;
    private Queue scrollVelHistory = new Queue();

    private Rigidbody2D rb;
    private TouchManager tMan;
    private Camera thisCam;

	// Use this for initialization
	void Start () {
        thisCam = GetComponent<Camera>();

        lowerLimit = getLowerBound();
        upperLimit = flower.position.y + flowerHeightMargin;
        lowerZoom = thisCam.orthographicSize;
        rb = GetComponent<Rigidbody2D>();

        tMan = Object.FindObjectOfType<TouchManager>();
        tMan.onDragStart += onDragStart;
        tMan.onDrag += onDrag;
        tMan.onDragStop += onDragStop;
        tMan.onPinch += onPinch;
	}

    void FixedUpdate()
    {
        upperLimit = flower.position.y + flowerHeightMargin;
    }

    void LateUpdate()
    {
        if (lowerZoom <= (upperLimit - lowerLimit) / 2)
        {
            thisCam.orthographicSize = Mathf.Clamp(thisCam.orthographicSize, lowerZoom, Mathf.Min((upperLimit - lowerLimit) / 2, maxZoomDist));
        }
        else
        {
            thisCam.orthographicSize = lowerZoom;
        }

        if (getUpperPosLimit() >= getLowerPosLimit())
        {
            Vector3 newPos = transform.position;
            newPos.y = Mathf.Clamp(transform.position.y, getLowerPosLimit(), getUpperPosLimit()); //TODO: Something here is causing the camera to stick to the limits when the velocity is still nonzero
            transform.position = newPos;
        }
        else
        {
            Vector3 newPos = transform.position;
            newPos.y = getLowerPosLimit();
            transform.position = newPos;
        }

        if (transform.position.y <= getUpperPosLimit() - minButtonShowDist)
        {
            topButton.SetActive(true);
        }
        else
        {
            topButton.SetActive(false);
        }

        if (transform.position.y >= getLowerPosLimit() + minButtonShowDist)
        {
            bottomButton.SetActive(true);
        }
        else
        {
            bottomButton.SetActive(false);
        }
    }

    /// <summary>
    /// Get the lower bound of the camera.
    /// </summary>
    /// <returns>The world-space Y value of the camera's lower bound.</returns>
    private float getLowerBound()
    {
        return transform.position.y - thisCam.orthographicSize;
    }

    /// <summary>
    /// Get the upper bound of the camera.
    /// </summary>
    /// <returns>The world-space Y value of the camera's upper bound.</returns>
    private float getUpperBound()
    {
        return transform.position.y + (thisCam.orthographicSize * Screen.width / Screen.height);
    }

    /// <summary>
    /// Get the lower movement limit of the camera.
    /// </summary>
    /// <returns>The world-space Y value that the camera must not move below.</returns>
    private float getLowerPosLimit()
    {
        return lowerLimit + thisCam.orthographicSize;
    }

    /// <summary>
    /// Get the upper movement limit of the camera.
    /// </summary>
    /// <returns>The world-space Y value that the camera must not move above.</returns>
    private float getUpperPosLimit()
    {
        return upperLimit - thisCam.orthographicSize;
    }

    /// <summary>
    /// Called when the player begins a drag touch event.
    /// </summary>
    /// <param name="startPos">The starting position of the drag.</param>
    private void onDragStart(Vector2 startPos)
    {
        if (Object.FindObjectOfType<GameController>().isPanelOpen() == false)
        {
            Debug.Log(startPos.y + " " + (Screen.height - startPos.y) + " " + (Screen.height * deadZoneMargin));
            if (startPos.y > Screen.height * deadZoneMargin & Screen.height - startPos.y > Screen.height * deadZoneMargin)
            {
                //Debug.Log("Drag detected.");
                Vector2 worldPos = thisCam.ScreenToWorldPoint(startPos);
                Collider2D col = Physics2D.OverlapPoint(worldPos);

                if (col == null)
                {
                    //Debug.Log("Beginning scroll.");
                    scrollVelHistory.Clear();
                    scrolling = true;
                }
                else if (col.tag != "DontScrollCamera")
                {
                    //Debug.Log("Beginning scroll.");
                    scrollVelHistory.Clear();
                    scrolling = true;
                }
                else
                {
                    //Debug.Log("Not scrolling.");
                    scrolling = false;
                }
            }
        }
    }

    /// <summary>
    /// Called as part of a drag touch event.
    /// </summary>
    /// <param name="delta">The amount the finger has moved since the last event.</param>
    private void onDrag(Vector2 delta)
    {
        if (scrolling == true)
        {
            //Debug.Log("Scrolling " + delta + " " + Input.GetTouch(0).deltaPosition);
            float diff = thisCam.ScreenToWorldPoint(delta).y - thisCam.ScreenToWorldPoint(Vector2.zero).y;
            scrollVelHistory.Enqueue(-diff / Time.deltaTime);
            while (scrollVelHistory.Count > scrollAverageSize)
            {
                scrollVelHistory.Dequeue();
            }

            if (getUpperPosLimit() > transform.position.y)
            {
                Vector3 newPos = transform.position;
                newPos.y -= diff;
                transform.position = newPos;
                //rb.velocity = new Vector2(0, 10);
            }
        }
    }

    /// <summary>
    /// Called when the player finished a drag touch event.
    /// </summary>
    private void onDragStop()
    {
        //Debug.Log("Drag ended.");
        float total = 0;
        foreach (float val in scrollVelHistory)
        {
            total += val;
        }

        rb.velocity = new Vector2(0, total / scrollAverageSize);
        scrollVelHistory.Clear();

        scrolling = false;
    }

    /// <summary>
    /// Called as part of a pinch touch event.
    /// </summary>
    /// <param name="delta">The amount the pinch distance has changed since the last event.</param>
    private void onPinch(float delta)
    {
        float diff = thisCam.ScreenToWorldPoint(new Vector2(delta, 0)).x - thisCam.ScreenToWorldPoint(Vector2.zero).x;
        //Debug.Log(delta + " " + diff);
        thisCam.orthographicSize -= diff * zoomSpeed;
    }

    /// <summary>
    /// Move the camera straight to the top of the plant.
    /// </summary>
    public void jumpToTop()
    {
        Vector3 newPos = transform.position;
        newPos.y = getUpperPosLimit();
        transform.position = newPos;
    }

    /// <summary>
    /// Move the camera straight to the bottom of the plant.
    /// </summary>
    public void jumpToBottom()
    {
        Vector3 newPos = transform.position;
        newPos.y = getLowerPosLimit();
        transform.position = newPos;
    }
}
