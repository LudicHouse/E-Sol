using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchManager : MonoBehaviour {
    public float dragDeadzone;

    private List<Tap> tapping = new List<Tap>();
    private List<Drag> dragging = new List<Drag>();

    private class Tap
    {
        public int fingerId;
        public Vector2 startPos;
    }

    private class Drag
    {
        public int fingerId;
        public Vector2 lastPos;
    }

    public delegate void onTapEvent(Vector2 screenPos);
    public event onTapEvent onTap;

    public delegate void onDragStartEvent(Vector2 screenPos);
    public event onDragStartEvent onDragStart;

    public delegate void onDragEvent(Vector2 delta);
    public event onDragEvent onDrag;

    public delegate void onDragStopEvent();
    public event onDragStopEvent onDragStop;

    public delegate void onPinchStartEvent();
    public event onPinchStartEvent onPinchStart;

    public delegate void onPinchEvent(float delta);
    public event onPinchEvent onPinch;

    public delegate void onPinchStopEvent();
    public event onPinchStopEvent onPinchStop;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        cleanTaps();
        cleanDrags();

        foreach (Touch t in Input.touches)
        {
            switch(t.phase)
            {
                case TouchPhase.Began:
                    Tap newTap = new Tap();
                    newTap.fingerId = t.fingerId;
                    newTap.startPos = t.position;
                    tapping.Add(newTap);
                    break;
                case TouchPhase.Moved:
                    if (isTapping(t.fingerId) == true)
                    {
                        if (Vector2.Distance(t.position, getTap(t.fingerId).startPos) > dragDeadzone)
                        {
                            Drag newDrag = new Drag();
                            newDrag.fingerId = t.fingerId;
                            newDrag.lastPos = getTap(t.fingerId).startPos;
                            dragging.Add(newDrag);

                            removeTap(t.fingerId);

                            if (dragging.Count == 2)
                            {
                                if (onPinchStart != null)
                                {
                                    onPinchStart();
                                }
                            }
                            else if (dragging.Count == 1)
                            {
                                if (onDragStart != null)
                                {
                                    onDragStart(newDrag.lastPos);
                                }
                            }
                        }
                    }
                    break;
                case TouchPhase.Ended:
                    if (isTapping(t.fingerId) == true)
                    {
                        if (Vector2.Distance(t.position, getTap(t.fingerId).startPos) <= dragDeadzone)
                        {
                            if (onTap != null)
                            {
                                onTap(t.position);
                            }
                        }
                    }

                    removeTap(t.fingerId);
                    removeDrag(t.fingerId);
                    break;
                case TouchPhase.Canceled:
                    removeTap(t.fingerId);
                    removeDrag(t.fingerId);
                    break;
            }
        }

        if (dragging.Count > 1) //Pinching
        {
            float lastDist = Vector2.Distance(dragging[0].lastPos, dragging[1].lastPos);
            //Debug.Log(lastDist);
            float currentDist = Vector2.Distance(getTouchById(dragging[0].fingerId).position, getTouchById(dragging[1].fingerId).position);
            float delta = currentDist - lastDist;

            if (onPinch != null)
            {
                onPinch(delta);
            }
        }
        else if (dragging.Count > 0) //Dragging
        {
            Vector2 delta = getTouchById(dragging[0].fingerId).position - dragging[0].lastPos;

            if (onDrag != null)
            {
                onDrag(delta);
            }
        }

        for (int loop = 0; loop < dragging.Count; loop++)
        {
            dragging[loop].lastPos = getTouchById(dragging[loop].fingerId).position;
        }

        string debug = "Tapping: ";
        foreach (Tap t in tapping)
        {
            debug += t.fingerId + ",";
        }
        debug += " -- Zooming: ";
        foreach (Drag d in dragging)
        {
            debug += d.fingerId + ",";
        }
        //Debug.Log(debug);
	}

    /// <summary>
    /// Check if the specified finger is tapping.
    /// </summary>
    /// <param name="fingerId">The finger ID to check.</param>
    /// <returns>True if the finger is tapping, false otherwise.</returns>
    private bool isTapping(int fingerId)
    {
        foreach (Tap t in tapping)
        {
            if (t.fingerId == fingerId)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Get the specified tap data.
    /// </summary>
    /// <param name="fingerId">The finger ID to check.</param>
    /// <returns>The tap data of the specified finger.</returns>
    private Tap getTap(int fingerId)
    {
        foreach (Tap t in tapping)
        {
            if (t.fingerId == fingerId)
            {
                return t;
            }
        }

        return new Tap();
    }

    /// <summary>
    /// Remove a finger from the tapping list.
    /// </summary>
    /// <param name="fingerId">The finger ID to remove.</param>
    private void removeTap(int fingerId)
    {
        if (isTapping(fingerId) == true)
        {
            Tap toRemove = getTap(fingerId);
            tapping.Remove(toRemove);
        }
    }

    /// <summary>
    /// Remove any fingers from the tapping list that are no longer touching the screen.
    /// </summary>
    private void cleanTaps() //Note: Include event calls?
    {
        List<Tap> toRemove = new List<Tap>();
        foreach (Tap t in tapping)
        {
            if (touchExists(t.fingerId) == false)
            {
                toRemove.Add(t);
            }
        }

        foreach (Tap t in toRemove)
        {
            tapping.Remove(t);
        }
    }

    /// <summary>
    /// Check if the specified finger is dragging.
    /// </summary>
    /// <param name="fingerId">The finger ID to check.</param>
    /// <returns>True if the finger is dragging, false otherwise.</returns>
    private bool isDragging(int fingerId)
    {
        foreach (Drag d in dragging)
        {
            if (d.fingerId == fingerId)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Get the specified drag data.
    /// </summary>
    /// <param name="fingerId">The finger ID to check.</param>
    /// <returns>The drag data of the specified finger.</returns>
    private Drag getDrag(int fingerId)
    {
        foreach (Drag d in dragging)
        {
            if (d.fingerId == fingerId)
            {
                return d;
            }
        }

        return new Drag();
    }

    /// <summary>
    /// Remove a finger from the dragging list.
    /// </summary>
    /// <param name="fingerId">The finger ID to remove.</param>
    private void removeDrag(int fingerId)
    {
        if (isDragging(fingerId) == true)
        {
            Drag toRemove = getDrag(fingerId);
            dragging.Remove(toRemove);

            if (dragging.Count == 1)
            {
                if (onPinchStop != null)
                {
                    onPinchStop();
                }
                if (onDragStart != null)
                {
                    onDragStart(dragging[0].lastPos);
                }
            }
            else if (dragging.Count == 0)
            {
                if (onDragStop != null)
                {
                    onDragStop();
                }
            }
        }
    }

    /// <summary>
    /// Remove any fingers from the dragging list that are no longer touching the screen.
    /// </summary>
    private void cleanDrags() //Note: Include event calls?
    {
        List<Drag> toRemove = new List<Drag>();
        foreach (Drag d in dragging)
        {
            if (touchExists(d.fingerId) == false)
            {
                toRemove.Add(d);
            }
        }

        foreach (Drag d in toRemove)
        {
            dragging.Remove(d);
        }
    }

    /// <summary>
    /// Check if the specified finger is touching the screen.
    /// </summary>
    /// <param name="fingerId">The finger ID to check.</param>
    /// <returns>True if the finger is touching the screen, false otherwise.</returns>
    private bool touchExists(int fingerId)
    {
        foreach (Touch t in Input.touches)
        {
            if (t.fingerId == fingerId)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Get the specified touch data.
    /// </summary>
    /// <param name="fingerId">The finger ID to check.</param>
    /// <returns>The touch data of the specified finger.</returns>
    private Touch getTouchById(int fingerId)
    {
        foreach (Touch t in Input.touches)
        {
            if (t.fingerId == fingerId)
            {
                return t;
            }
        }

        return new Touch();
    }

    /// <summary>
    /// Get the touch data of the currently dragging finger.
    /// </summary>
    /// <returns>The touch data of the finger.</returns>
    public Touch getDraggingTouch()
    {
        if (dragging.Count > 0)
        {
            return getTouchById(dragging[0].fingerId);
        }
        else
        {
            return new Touch();
        }
    }
}
