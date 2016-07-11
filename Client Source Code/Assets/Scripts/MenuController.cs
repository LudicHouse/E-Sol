using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {
    public string targetAccessory;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Save the current game state.
    /// </summary>
    public void save()
    {
        Debug.Log("save");
        Object.FindObjectOfType<GameController>().save();
    }

    /// <summary>
    /// Load the stored game.
    /// </summary>
    public void load()
    {
        Object.FindObjectOfType<GameController>().load();
    }

    /// <summary>
    /// Applies an animal to the selected branch.
    /// </summary>
    /// <param name="newAnimal">The animal sprite to apply.</param>
    public void setAnimal(string newAnimal)
    {
        Object.FindObjectOfType<GameController>().setAnimal(newAnimal);
    }

    /// <summary>
    /// Apply the specified accessory.
    /// </summary>
    /// <param name="accessoryName">The name of the accessory to apply.</param>
    public void setAccessory(string accessoryName)
    {
        setAccessory(accessoryName, false);
    }

    /// <summary>
    /// Apply the specified accessory.
    /// </summary>
    /// <param name="accessoryName">The name of the accessory to apply.</param>
    /// <param name="force">Set to true to apply even if the accessory panel is closed.</param>
    public void setAccessory(string accessoryName, bool force)
    {
        targetAccessory = accessoryName;

        Debug.Log(Object.FindObjectsOfType<AccessoryController>().Length + " accessory controllers.");
        int count = 0;
        foreach (AccessoryController ac in Object.FindObjectsOfType<AccessoryController>())
        {
            count++;
            ac.setAccessory(accessoryName, force);
        }
        Debug.Log("Tried to set " + count);
    }

    /// <summary>
    /// Starts a new game.
    /// </summary>
    public void newGame()
    {
        Object.FindObjectOfType<GameController>().newGame();
    }

    /// <summary>
    /// Changes the local country.
    /// </summary>
    public void changeCountry() //Deprecated?
    {
        Debug.Log("changecountry");
        save();
        Object.FindObjectOfType<GameController>().changeCountry();
    }

    /// <summary>
    /// Opens the map scene.
    /// </summary>
    public void openMap()
    {
        if (Object.FindObjectOfType<OptionsController>().isOptionsPanelOpen() == false)
        {
            save();
            Object.Destroy(Object.FindObjectOfType<GameController>().gameObject);
            //Application.LoadLevel("Map");
            Object.FindObjectOfType<SceneController>().loadLevel("Map");
        }
    }

    /// <summary>
    /// Play the specified sound effect.
    /// </summary>
    /// <param name="clip">The audio clip to play.</param>
    public void playSound(AudioClip clip)
    {
        Object.FindObjectOfType<MusicController>().playSoundEffect(clip);
    }
}
