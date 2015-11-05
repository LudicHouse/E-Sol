using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CloseIfTouchElsewhere : MonoBehaviour {
    public bool ignoreNextTap = false;

    private RectTransform rect;
    private TouchManager tMan;
    private RectTransform canvTransform;

	// Use this for initialization
	void Start () {
        rect = GetComponent<RectTransform>();
        tMan = Object.FindObjectOfType<TouchManager>();
        canvTransform = Object.FindObjectOfType<Canvas>().GetComponent<RectTransform>();

        tMan.onTap += onTap;
        tMan.onDragStart += onDragStart;
        Debug.Log("Init");
	}
	
	// Update is called once per frame
	void Update () {
        //Vector2 canvasPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //canvasPos.x *= Object.FindObjectOfType<Canvas>().GetComponent<RectTransform>().rect.width;
        //canvasPos.x -= Object.FindObjectOfType<Canvas>().GetComponent<RectTransform>().rect.width / 2;
        //canvasPos.y *= Object.FindObjectOfType<Canvas>().GetComponent<RectTransform>().rect.height;
        //canvasPos.y -= Object.FindObjectOfType<Canvas>().GetComponent<RectTransform>().rect.height / 2;

        //Debug.Log(Input.mousePosition + " " + rect.rect + " " + canvasPos + " " + rect.rect.Contains(canvasPos));
        

        /*foreach (Touch t in Input.touches)
        {
            Vector2 canvasPos = Camera.main.ScreenToViewportPoint(t.position);
            canvasPos.x *= Object.FindObjectOfType<Canvas>().GetComponent<RectTransform>().rect.width;
            canvasPos.x -= Object.FindObjectOfType<Canvas>().GetComponent<RectTransform>().rect.width / 2;
            canvasPos.y *= Object.FindObjectOfType<Canvas>().GetComponent<RectTransform>().rect.height;

            /*if  (t.phase == TouchPhase.Began)
            {
                Debug.Log(canvasPos + " x" + rect.rect.xMin + "-" + rect.rect.xMax + " y" + rect.rect.yMin + "-" + rect.rect.yMax);
            }

            if (rect.rect.Contains(canvasPos) == false & t.phase == TouchPhase.Began)
            {
                //Debug.Log("Closed!");
                gameObject.SetActive(false);
            }
        }*/
	}

    /// <summary>
    /// Called when the player begins a tap touch event.
    /// </summary>
    /// <param name="screenPos">The position the player tapped the screen.</param>
    private void onTap(Vector2 screenPos)
    {
        if (ignoreNextTap == false)
        {
            Vector2 canvasPos = Camera.main.ScreenToViewportPoint(screenPos);
            canvasPos.x *= canvTransform.rect.width;
            canvasPos.x -= canvTransform.rect.width / 2;
            canvasPos.y *= canvTransform.rect.height;

            if (rect.rect.Contains(canvasPos) == false)
            {
                gameObject.SetActive(false);
            }
        }

        ignoreNextTap = false;
    }

    /// <summary>
    /// Called when the player begins a drag touch event.
    /// </summary>
    /// <param name="startPos">The starting position of the drag.</param>
    private void onDragStart(Vector2 startPos)
    {
        if (ignoreNextTap == false)
        {
            Vector2 canvasPos = Camera.main.ScreenToViewportPoint(startPos);
            canvasPos.x *= canvTransform.rect.width;
            canvasPos.x -= canvTransform.rect.width / 2;
            canvasPos.y *= canvTransform.rect.height;

            if (rect.rect.Contains(canvasPos) == false)
            {
                gameObject.SetActive(false);
            }
        }

        ignoreNextTap = false;
    }
}
