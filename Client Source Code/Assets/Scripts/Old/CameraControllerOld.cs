using UnityEngine;
using System.Collections;

public class CameraControllerOld : MonoBehaviour {
    public Transform flower;
    public float flowerHeightMargin;
    public float zoomSpeed;

    private float lowerLimit;
    private float upperLimit;
    private float lowerZoom;

    private int scrollFingerId = -1;
    private Vector2 scrollFingerPos;
    private Queue scrollVelHistory = new Queue();

    private int zoomFingerId = -1;
    private float lastZoomDist;

    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        lowerLimit = getLowerBound();
        upperLimit = flower.position.y + flowerHeightMargin;
        lowerZoom = Camera.main.orthographicSize;
        rb = GetComponent<Rigidbody2D>();
	}

    void Update()
    {
        updateTouches();

        string output = "";
        foreach (Touch t in Input.touches)
        {
            output += " " + t.fingerId;
        }
        //Debug.Log(output);
    }
	
	void FixedUpdate () {
        upperLimit = flower.position.y + flowerHeightMargin;

        //Debug.Log(scrollFingerId + " " + zoomFingerId);
        if (canZoom() == true)
        {
            //Debug.Log("Can zoom");
            handleZoom();
        }
        else if (canScroll() == true)
        {
            //Debug.Log("Can scroll");
            handleScroll();
        }
        else
        {
            //Debug.Log("Do nothing");
        }
	}

    void LateUpdate()
    {
        //Debug.Log(lowerZoom + " " + ((upperLimit - lowerLimit) / 2));
        if (lowerZoom <= (upperLimit - lowerLimit) / 2)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, lowerZoom, (upperLimit - lowerLimit) / 2);
        }
        else
        {
            Camera.main.orthographicSize = lowerZoom;
        }

        if (getUpperPosLimit() >= getLowerPosLimit())
        {
            Vector3 newPos = transform.position;
            newPos.y = Mathf.Clamp(transform.position.y, getLowerPosLimit(), getUpperPosLimit());
            transform.position = newPos;
        }
        else
        {
            Vector3 newPos = transform.position;
            newPos.y = getLowerPosLimit();
            transform.position = newPos;
        }
    }

    private float getLowerBound()
    {
        return transform.position.y - Camera.main.orthographicSize;
    }

    private float getUpperBound()
    {
        return transform.position.y + (Camera.main.orthographicSize * Screen.width / Screen.height);
    }

    private float getLowerPosLimit()
    {
        return lowerLimit + Camera.main.orthographicSize;
    }

    private float getUpperPosLimit()
    {
        return upperLimit - Camera.main.orthographicSize;
    }

    private void updateTouches()
    {
        if (scrollFingerId != -1)
        {
            if (touchExists(scrollFingerId) == false)
            {
                scrollFingerId = -1;
            }
            else if (getTouch(scrollFingerId).phase == TouchPhase.Ended | getTouch(scrollFingerId).phase == TouchPhase.Canceled)
            {
                scrollFingerId = -1;
            }

            if (scrollFingerId == -1)
            {
                float total = 0;
                foreach (float val in scrollVelHistory)
                {
                    total += val;
                }

                rb.velocity = new Vector2(0, total / 5);
                scrollVelHistory.Clear();
            }

            if (scrollFingerId == -1 & zoomFingerId != -1)
            {
                Debug.Log("Switching scroll finger to " + zoomFingerId + " at phase " + getTouch(zoomFingerId).phase);
                scrollFingerId = zoomFingerId;
                zoomFingerId = -1;
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(getTouch(scrollFingerId).position);
                scrollFingerPos = worldPos;

                Debug.Log("Fingers are now " + scrollFingerId + " " + zoomFingerId);
            }
        }

        if (zoomFingerId != -1)
        {
            if (touchExists(zoomFingerId) == false)
            {
                zoomFingerId = -1;
            }
            else if (getTouch(zoomFingerId).phase == TouchPhase.Ended | getTouch(zoomFingerId).phase == TouchPhase.Canceled)
            {
                zoomFingerId = -1;
            }

            if (zoomFingerId == -1 & scrollFingerId != -1)
            {
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(getTouch(scrollFingerId).position);
                scrollFingerPos = worldPos;
            }
        }

        if (scrollFingerId == -1)
        {
            foreach (Touch t in Input.touches)
            {
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(t.position);
                
                if (Physics2D.OverlapPoint(worldPos) == null & t.phase == TouchPhase.Began & Object.FindObjectOfType<GameController>().isPanelOpen() == false)
                {
                    scrollFingerId = t.fingerId;
                    scrollFingerPos = worldPos;
                    break;
                }
            }
        }

        if (scrollFingerId != -1 & zoomFingerId == -1)
        {
            foreach (Touch t in Input.touches)
            {
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(t.position);

                if (Physics2D.OverlapPoint(worldPos) == null & t.phase == TouchPhase.Began & t.fingerId != scrollFingerId & Object.FindObjectOfType<GameController>().isPanelOpen() == false)
                {
                    scrollVelHistory.Clear();
                    zoomFingerId = t.fingerId;
                    lastZoomDist = getFingerDist();
                    break;
                }
            }
        }
    }

    private bool canScroll()
    {
        if (scrollFingerId != -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void handleScroll()
    {
        //Debug.Log("Scrolling with finger " + scrollFingerId + " " + Input.touchCount + " " + Input.touches[0].fingerId);
        Touch t = getTouch(scrollFingerId);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(t.position);
        float diff = worldPos.y - scrollFingerPos.y;
        //Debug.Log(diff + " " + Time.deltaTime + " " + (-diff / Time.deltaTime));
        scrollVelHistory.Enqueue(-diff / Time.deltaTime);
        while (scrollVelHistory.Count > 5)
        {
            scrollVelHistory.Dequeue();
        }

        if (getUpperPosLimit() > transform.position.y)
        {
            rb.velocity = new Vector2(0, -diff / Time.deltaTime);
        }
    }

    private bool canZoom()
    {
        if (scrollFingerId != -1 & zoomFingerId != -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void handleZoom()
    {
        rb.velocity = Vector2.zero;

        float dist = getFingerDist();
        float diff = dist - lastZoomDist;
        Camera.main.orthographicSize -= diff * zoomSpeed;

        lastZoomDist = dist;
    }

    private float getFingerDist()
    {
        Vector2 pos1 = getTouch(scrollFingerId).position;
        Vector2 pos2 = getTouch(zoomFingerId).position;

        return Vector2.Distance(pos1, pos2);
    }

    private bool touchExists(int id)
    {
        foreach(Touch t in Input.touches)
        {
            if (t.fingerId == id)
            {
                return true;
            }
        }

        return false;
    }

    private Touch getTouch(int id)
    {
        foreach(Touch t in Input.touches)
        {
            if (t.fingerId == id)
            {
                return t;
            }
        }

        return new Touch();
    }

    private void onDragStart(Vector2 startPos)
    {

    }
}
