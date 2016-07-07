using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class MapMenuController : MonoBehaviour {
    public GameObject countryPanel;
    public Text countryPanelName;
    public ProgressBar partBar;
    public Text partYear;
    public ProgressBar ozoneBar;
    public Text ozoneYear;
    public SetFlag localFlag;
    public SetFlag remoteFlag;

    public float particulateMax;
    public float ozoneMax;

    public GameObject easternPanel;
    public GameObject lowlandsPanel;

    public Animator plantAnim;

    public AudioClip successSound;
    public AudioClip failSound;

    private string localSavePath;
    private string localRegionPath;
    private string remoteSavePath;
    private string remoteDataPath;

	// Use this for initialization
	void Start () {
        localSavePath = Application.persistentDataPath + "/localPlant.save";
        localRegionPath = Application.persistentDataPath + "/localregion";
        remoteSavePath = Application.persistentDataPath + "/remotePlant.save";
        remoteDataPath = Application.persistentDataPath + "/remotedata";
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            back();
        }
	}

    /// <summary>
    /// Return to the main screen.
    /// </summary>
    public void back()
    {
        //Application.LoadLevel("Main");
        UnityEngine.Object.FindObjectOfType<SceneManager>().loadMain();
    }

    /// <summary>
    /// Opens the country panel for the specified country.
    /// </summary>
    /// <param name="countryCode">The country code.</param>
    public void selectCountry(string countryCode)
    {
        countryPanelName.text = Country.getName(countryCode);

        StreamReader lReader = new StreamReader(localRegionPath); //Note: Error handling if file doesn't exist?
        string lRegion = lReader.ReadLine();
        lReader.Close();
        localFlag.set(lRegion);
        remoteFlag.set(countryCode);

        partBar.set(Country.getParticulate(countryCode) / particulateMax);
        partYear.text = "(" + Country.getParticulateYear(countryCode) + ")";
        ozoneBar.set(Country.getOzone(countryCode) / ozoneMax);
        ozoneYear.text = "(" + Country.getOzoneYear(countryCode) + ")";

        if (Country.isExplored(countryCode) == true)
        {
            partBar.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            partBar.transform.parent.gameObject.SetActive(false);
        }

        countryPanel.SetActive(true);
        plantAnim.SetTrigger("reset");

        float part = (Country.getParticulate(countryCode) - 0) / (particulateMax - 0);
        float ozone = (Country.getOzone(countryCode) - 0) / (ozoneMax - 0);
        float average = (part + ozone) / 2;
        Debug.Log(part + " " + ozone + " " + average);
    }

    /// <summary>
    /// Closes the country panel.
    /// </summary>
    public void closeCountry()
    {
        countryPanel.SetActive(false);
    }

    /// <summary>
    /// Opens the eastern zoom panel.
    /// </summary>
    public void openEasternPanel()
    {
        easternPanel.SetActive(true);
    }

    /// <summary>
    /// Closes the eastern zoom panel.
    /// </summary>
    public void closeEasternPanel()
    {
        easternPanel.SetActive(false);
    }

    /// <summary>
    /// Opens the lowlands zoom panel.
    /// </summary>
    public void openLowlandsPanel()
    {
        lowlandsPanel.SetActive(true);
    }

    /// <summary>
    /// Closes the lowlands zoom panel.
    /// </summary>
    public void closeLowlandsPanel()
    {
        lowlandsPanel.SetActive(false);
    }

    /// <summary>
    /// Gets a plant from the currently selected country and returns to the main scene.
    /// </summary>
    public void swap()
    {
        StartCoroutine("swapAsync");
    }

    /// <summary>
    /// Gets a plant from the currently selected country and returns to the main scene.
    /// </summary>
    /// <returns></returns>
    private IEnumerator swapAsync()
    {
        plantAnim.SetTrigger("searchStart");
        yield return null;
        string countryCode = Country.getCode(countryPanelName.text);

        NetworkController.DownloadedSave remoteSave = UnityEngine.Object.FindObjectOfType<NetworkController>().downloadSave(countryCode);

        if (remoteSave != null)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(remoteSavePath);
            bf.Serialize(file, remoteSave.save);
            file.Close();

            StreamWriter writer = new StreamWriter(remoteDataPath);
            writer.WriteLine(countryCode);
            writer.WriteLine(remoteSave.language);
            writer.WriteLine(DateTime.Now.Ticks);
            writer.Close();

            file = File.Open(localSavePath, FileMode.Open);
            Save localSave = (Save)bf.Deserialize(file);
            file.Close();

            if (localSave.unlockedAnimals.Contains("country" + countryCode) == true)
            {
                localSave.unlockedAccessories.Add("country" + countryCode);
            }
            else
            {
                localSave.unlockedAnimals.Add("country" + countryCode);
            }

            file = File.Create(localSavePath);
            bf.Serialize(file, localSave);
            file.Close();

            Country.setExplored(countryCode);

            yield return null;
            plantAnim.SetTrigger("searchSuccess");
            UnityEngine.Object.FindObjectOfType<MusicController>().playSoundEffect(successSound);
            yield return new WaitForSeconds(2);

            //Application.LoadLevel("Main");
            UnityEngine.Object.FindObjectOfType<SceneManager>().loadMain();
        }
        else
        {
            yield return null;
            plantAnim.SetTrigger("searchFailed");
            UnityEngine.Object.FindObjectOfType<MusicController>().playSoundEffect(failSound);
            Debug.LogError("Unable to download save!");
        }
    }

    /// <summary>
    /// Play the specified sound effect.
    /// </summary>
    /// <param name="clip">The audio clip to play.</param>
    public void playSound(AudioClip clip)
    {
        UnityEngine.Object.FindObjectOfType<MusicController>().playSoundEffect(clip);
    }
}
