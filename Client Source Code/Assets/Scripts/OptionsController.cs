using UnityEngine;
using System.Collections;
using System.IO;

public class OptionsController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Delete the local region data and restart the game, prompting the user to select a new country.
    /// </summary>
    public void changeCountry()
    {
        Debug.Log("Restarting with new country...");
        File.Delete(Application.persistentDataPath + "/localregion");
        File.Delete(Application.persistentDataPath + "/token");
        if (Object.FindObjectOfType<GameController>() != null)
        {
            Object.FindObjectOfType<GameController>().save();
        }
        Application.LoadLevel("Startup");
    }

    /// <summary>
    /// Reset the current save data and start a new game.
    /// </summary>
    public void newGame()
    {
        Debug.Log("Starting new game...");
        File.Delete(Application.persistentDataPath + "/localPlant.save");
        File.Delete(Application.persistentDataPath + "/localregion");
        File.Delete(Application.persistentDataPath + "/remotePlant.save");
        File.Delete(Application.persistentDataPath + "/remotedata");
        File.Delete(Application.persistentDataPath + "/token");
        File.Delete(Application.persistentDataPath + "/explored");
        File.Delete(Application.persistentDataPath + "/tutorial");
        Application.LoadLevel("Startup");
    }
    
    /// <summary>
    /// Close the game.
    /// </summary>
    public void quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Play the specified sound effect.
    /// </summary>
    /// <param name="clip">The audio clip to play.</param>
    public void playSound(AudioClip clip)
    {
        Object.FindObjectOfType<MusicController>().playSoundEffect(clip);
    }

    public bool isOptionsPanelOpen()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf == true)
            {
                return true;
            }
        }

        return false;
    }
}
