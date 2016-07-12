using UnityEngine;
using System.Collections;

public class PanelMover : MonoBehaviour {
    public float closedPosY;
    public float openPosY;
    public float moveSpeed;
    public GameObject panel;

    private bool isOpen = false;

	// Use this for initialization
	void Start () {
        closePanel();
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 targetPos = transform.position;
        targetPos.y = closedPosY;

        if (isOpen == true)
        {
            targetPos.y = openPosY;
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (isOpen == true)
        {
            panel.SetActive(true);
        }
        else
        {
            if (transform.position.y == closedPosY)
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
