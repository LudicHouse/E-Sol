using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OpenAccessoryPanel : MonoBehaviour {
    public GameObject panel;
    public PanelMover panelMover;
    public SpriteRenderer accessoryRenderer;

    private Collider2D col;
    private TouchManager tMan;

	// Use this for initialization
	void Start () {
        col = GetComponent<Collider2D>();
        tMan = Object.FindObjectOfType<TouchManager>();

        tMan.onTap += onTap;
	}
	
	// Update is called once per frame
	void Update () {
        /*foreach (Touch t in Input.touches)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(t.position);

            if (col == Physics2D.OverlapPoint(worldPos) & t.phase == TouchPhase.Began & Object.FindObjectOfType<GameController>().isPanelOpen() == false)
            {
                foreach (Image option in panel.GetComponentsInChildren<Image>(true))
                {
                    if (option.tag == "AccessoryOption")
                    {
                        if (option.sprite == accessoryRenderer.sprite)
                        {
                            option.GetComponent<Toggle>().isOn = true;
                        }
                        else
                        {
                            option.GetComponent<Toggle>().isOn = false;
                        }
                    }
                }

                panel.SetActive(true);
            }
        }*/
	}

    /// <summary>
    /// Called when the player begins a tap touch event.
    /// </summary>
    /// <param name="screenPos">The position the player tapped the screen.</param>
    private void onTap(Vector2 screenPos)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        
        if (col == Physics2D.OverlapPoint(worldPos) & Object.FindObjectOfType<GameController>().isPanelOpen() == false)
        {
            Debug.Log("Open!");
            foreach (Image option in panel.GetComponentsInChildren<Image>(true))
            {
                if (option.tag == "AccessoryOption")
                {
                    /*if (option.sprite == accessoryRenderer.sprite)
                    {
                        option.GetComponent<Toggle>().isOn = true;
                    }
                    else
                    {
                        option.GetComponent<Toggle>().isOn = false;
                    }*/

                    if (option.name == Object.FindObjectOfType<GameController>().getAccessory())
                    {
                        option.GetComponent<Toggle>().isOn = true;
                    }
                    else
                    {
                        option.GetComponent<Toggle>().isOn = false;
                    }
                }
            }

            panel.GetComponent<CloseIfTouchElsewhere>().ignoreNextTap = true;
            panelMover.openPanel();

            Object.FindObjectOfType<TutorialManager>().setAccDone();
        }
    }
}
