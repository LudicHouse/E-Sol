using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AccessoryController : MonoBehaviour {
    public GameObject accessoryPanel;
    public List<string> accessoryNames;
    public List<Sprite> accessorySprites;
    public Sprite defaultSprite;
    public Color defaultColor;

    public bool debug;

    private SpriteRenderer rend;

	// Use this for initialization
	void Awake () {
        rend = GetComponent<SpriteRenderer>();
        
        if (accessoryPanel == null)
        {
            accessoryPanel = Object.FindObjectOfType<GameController>().accessoryPanel;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Apply the specified accessory to the plant.
    /// </summary>
    /// <param name="accessoryName">The name of the accessory.</param>
    public void setAccessory(string accessoryName)
    {
        setAccessory(accessoryName, false);
    }

    public void setAccessory(string accessoryName, bool force)
    {
        if (debug == true)
        {
            Debug.Log("Setting " + accessoryName);
        }
        //Debug.Log("First accessory is " + accessoryNames[0]);
        Sprite toSet = null;
        if (accessoryNames.Contains(accessoryName) == true)
        {
            //Debug.Log("Contains " + accessoryName);
            int index = accessoryNames.IndexOf(accessoryName);
            toSet = accessorySprites[index];
        }

        setAccessory(toSet, force);
    }

    /// <summary>
    /// Apply the selected accessory sprite to the plant.
    /// </summary>
    /// <param name="newAccessory">The accessory to apply.</param>
    public void setAccessory(Sprite newAccessory)
    {
        setAccessory(newAccessory, false);
    }

    /// <summary>
    /// Apply the selected accessory sprite to the plant.
    /// </summary>
    /// <param name="newAccessory">The accessory to apply.</param>
    /// <param name="force">TODO</param>
    public void setAccessory(Sprite newAccessory, bool force)
    {
        if (debug == true)
        {
            if (newAccessory != null)
            {
                Debug.Log("Setting sprite " + newAccessory.name);
            }
            else
            {
                Debug.Log("Setting null sprite.");
            }
        }
        if (accessoryPanel.activeSelf == true | force == true)
        {
            if (debug == true)
            {
                Debug.Log("Can set");
            }
            if (newAccessory == rend.sprite | newAccessory == null)
            {
                if (debug == true)
                {
                    Debug.Log("Cleared accessory");
                }
                rend.sprite = defaultSprite;
                rend.color = defaultColor;
            }
            else
            {
                if (debug == true)
                {
                    Debug.Log("Sprite set");
                }
                rend.sprite = newAccessory;
                rend.color = Color.white;
            }
        }

        if (GetComponent<BodyMoodColor>() != null)
        {
            GetComponent<BodyMoodColor>().reset();
        }
    }

    /// <summary>
    /// Get the currently applied accessory.
    /// </summary>
    /// <returns>The name of the active accessory.</returns>
    public string getAccessory()
    {
        if (rend.sprite == defaultSprite)
        {
            return null;
        }
        else if (rend.sprite != null)
        {
            int index = accessorySprites.IndexOf(rend.sprite);
            return accessoryNames[index];
        }
        else
        {
            return null;
        }
    }
}
