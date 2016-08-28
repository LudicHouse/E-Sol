using UnityEngine;
using System.Collections;

public class PanelMover : MonoBehaviour {
    public float closedPosY;
    public float openPosY;
    public float moveSpeed;
    public GameObject panel;

    private RectTransform rt;

    private bool isOpen = false;


	// Use this for initialization
	void Start () {
        rt = GetComponent<RectTransform>();
        closePanel();
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 targetPos = rt.anchoredPosition;
        targetPos.y = closedPosY;

        if (isOpen == true)
        {
            targetPos.y = openPosY;
        }

        rt.anchoredPosition = Vector2.MoveTowards(rt.anchoredPosition, targetPos, moveSpeed * Time.deltaTime);

        if (isOpen == true)
        {
            panel.SetActive(true);
        }
        else
        {
            if (rt.anchoredPosition.y == closedPosY)
            {
                panel.SetActive(false);
            }
            else
            {
                panel.SetActive(true);
            }
        }
	}

    /// <summary>
    /// Causes the panel to slide up from the bottom of the screen.
    /// </summary>
    public void openPanel()
    {
        isOpen = true;
    }

    /// <summary>
    /// Causes the panel to slide down off the bottom of the screen.
    /// </summary>
    public void closePanel()
    {
        isOpen = false;
    }
}
