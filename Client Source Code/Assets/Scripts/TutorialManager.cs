using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {
    public Toggle forceToggle;
    public int canLimit;
    public int pestLimit;
    public int smogLimit;
    public int accLimit;
    public int animLimit;
    public int canToggle;
    public int pestToggle;
    public int smogToggle;
    public int accToggle;
    public int animToggle;

    private string tutPath;
    private bool ignoreChange = false;

    private int force = 0;
    private int canDone = 0;
    private int pestDone = 0;
    private int smogDone = 0;
    private int accDone = 0;
    private int animDone = 0;

    private bool started = false;

	// Use this for initialization
	void Awake () {
        //Debug.Log("Tutorial manager started");
        tutPath = Application.persistentDataPath + "/tutorial";

        if (File.Exists(tutPath) == true)
        {
            StreamReader reader = new StreamReader(tutPath);

            force = int.Parse(reader.ReadLine());
            canDone = int.Parse(reader.ReadLine());
            pestDone = int.Parse(reader.ReadLine());
            smogDone = int.Parse(reader.ReadLine());
            accDone = int.Parse(reader.ReadLine());
            animDone = int.Parse(reader.ReadLine());

            reader.Close();
        }
        else
        {
            save();
        }

        updateForceToggle();
        started = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Check whether the watering can tutorial should be shown.
    /// </summary>
    /// <returns>True if the tutorial should be displayed, false otherwise.</returns>
    public bool showCanTutorial()
    {
        if (started == false)
        {
            return false;
        }

        //Debug.Log("Showcan: " + force + " " + canDone + " " + canLimit);
        if (force > 0)
        {
            return true;
        }
        else if (force < 0)
        {
            return false;
        }
        else if (canDone < canLimit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Check whether the pest clearing tutorial should be shown.
    /// </summary>
    /// <returns>True if the tutorial should be displayed, false otherwise.</returns>
    public bool showPestTutorial()
    {
        if (started == false)
        {
            return false;
        }

        if (force > 0)
        {
            return true;
        }
        else if (force < 0)
        {
            return false;
        }
        else if (pestDone < pestLimit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Check whether the smog clearing tutorial should be shown.
    /// </summary>
    /// <returns>True if the tutorial should be displayed, false otherwise.</returns>
    public bool showSmogTutorial()
    {
        if (started == false)
        {
            return false;
        }

        if (force > 0)
        {
            return true;
        }
        else if (force < 0)
        {
            return false;
        }
        else if (smogDone < smogLimit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Check whether the accessory tutorial should be shown.
    /// </summary>
    /// <returns>True if the tutorial should be displayed, false otherwise.</returns>
    public bool showAccTutorial()
    {
        if (started == false)
        {
            return false;
        }

        if (force > 0)
        {
            return true;
        }
        else if (force < 0)
        {
            return false;
        }
        else if (accDone < accLimit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Check whether the animal tutorial should be shown.
    /// </summary>
    /// <returns>True if the tutorial should be displayed, false otherwise.</returns>
    public bool showAnimTutorial()
    {
        if (started == false)
        {
            return false;
        }

        if (force > 0)
        {
            return true;
        }
        else if (force < 0)
        {
            return false;
        }
        else if (animDone < animLimit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Check whether the settings toggle should display as enabled or disabled.
    /// </summary>
    /// <returns>True if the toggle should appear as enabled, false otherwise.</returns>
    public bool settingsButton()
    {
        if (force > 0)
        {
            return true;
        }
        else if (force < 0)
        {
            return false;
        }
        else
        {
            if (canDone >= canToggle & pestDone >= pestToggle & smogDone >= smogToggle & accDone >= accToggle & animDone >= animToggle)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Mark the watering can tutorial as completed.
    /// </summary>
    public void setCanDone()
    {
        canDone++;
        save();
        updateForceToggle();
    }

    /// <summary>
    /// Mark the pest clearing tutorial as completed.
    /// </summary>
    public void setPestDone()
    {
        pestDone++;
        save();
        updateForceToggle();
    }

    /// <summary>
    /// Mark the smog clearing tutorial as completed.
    /// </summary>
    public void setSmogDone()
    {
        smogDone++;
        save();
        updateForceToggle();
    }

    /// <summary>
    /// Mark the accessory tutorial as completed.
    /// </summary>
    public void setAccDone()
    {
        accDone++;
        save();
        updateForceToggle();
    }

    /// <summary>
    /// Mark the animal tutorial as completed.
    /// </summary>
    public void setAnimDone()
    {
        animDone++;
        save();
        updateForceToggle();
    }

    /// <summary>
    /// Adjust the force on-off setting based on the settings toggle.
    /// </summary>
    public void setForce()
    {
        Debug.Log("Setting force level.");
        if (ignoreChange == true)
        {
            Debug.Log("Ignoring.");
            ignoreChange = false;
            return;
        }

        if (forceToggle.isOn == true)
        {
            Debug.Log("Is now 1.");
            force = 1;
        }
        else
        {
            Debug.Log("Is now -1.");
            force = -1;
        }
        save();
    }

    /// <summary>
    /// Set the enabled status of the settings toggle to the correct value.
    /// </summary>
    private void updateForceToggle()
    {
        ignoreChange = true;
        forceToggle.isOn = settingsButton();
        ignoreChange = false;
    }

    /// <summary>
    /// Write the current settings to file.
    /// </summary>
    private void save()
    {
        StreamWriter writer = new StreamWriter(tutPath);

        writer.WriteLine(force);
        writer.WriteLine(canDone);
        writer.WriteLine(pestDone);
        writer.WriteLine(smogDone);
        writer.WriteLine(accDone);
        writer.WriteLine(animDone);

        writer.Close();
    }
}
