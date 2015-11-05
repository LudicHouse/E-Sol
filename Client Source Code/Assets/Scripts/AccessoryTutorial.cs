using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AccessoryTutorial : MonoBehaviour {
    public GameObject accessoryPanel;
    public TutorialManager manager;

    private SpriteRenderer rend;

	// Use this for initialization
	void Start () {
        rend = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(getNumAccessories());
        if (getNumAccessories() > 0 & manager.showAccTutorial() == true)
        {
            rend.enabled = true;
        }
        else
        {
            rend.enabled = false;
        }
	}

    /// <summary>
    /// Gets the number of unlocked accessories for the active save.
    /// </summary>
    /// <returns>The number of available accessories.</returns>
    private int getNumAccessories()
    {
        int total = 0;

        foreach (Image i in accessoryPanel.GetComponentsInChildren<Image>(true))
        {
            if (i.tag == "AccessoryOption")
            {
                if (i.GetComponent<Toggle>().interactable == true)
                {
                    total++;
                }
            }
        }

        return total;
    }
}
