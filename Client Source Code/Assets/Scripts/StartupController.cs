using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class StartupController : MonoBehaviour {
    public float logoFadeDuration;
    public float logoHoldDuration;
    public float blankHoldDuration;
    public GameObject logoContainer;
    public GameObject disclaimerPanel;
    public GameObject regionPanel;
    public GameObject languagePanel;
    public Transform languageButtonContainer;
    public GameObject languageButtonPrefab;

    private int logoStage;
    private float logoTimer;

    private string localPlantPath;
    private string regionPath;

    private string selectedRegion;

	// Use this for initialization
	void Start () {
        setLogoTransparency(0);

        logoStage = 0;
        logoTimer = logoFadeDuration;
        localPlantPath = Application.persistentDataPath + "/localPlant.save";
        regionPath = Application.persistentDataPath + "/localregion";

        if (PlayerPrefs.HasKey("quality") == true)
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("quality"));
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (logoStage < 4)
        {
            if (logoTimer <= 0)
            {
                logoStage++;

                if (logoStage == 1)
                {
                    logoTimer = logoHoldDuration;
                }
                else if (logoStage == 2)
                {
                    logoTimer = logoFadeDuration;
                }
                else if (logoStage == 3)
                {
                    logoTimer = blankHoldDuration;
                }
                else if (logoStage == 4)
                {
                    if (File.Exists(regionPath) == true & File.Exists(localPlantPath) == true)
                    {
                        //Application.loadLevel("Main");
                        Object.FindObjectOfType<SceneManager>().loadMain();
                    }
                    else
                    {
                        disclaimerPanel.SetActive(true);
                    }
                }
            }
            else
            {
                logoTimer -= Time.deltaTime;
            }
        }

        if (logoStage == 0)
        {
            setLogoTransparency(1 - (logoTimer / logoFadeDuration));
        }
        else if (logoStage == 1)
        {
            setLogoTransparency(1);
        }
        else if (logoStage == 2)
        {
            setLogoTransparency(logoTimer / logoFadeDuration);
        }
        else if (logoStage == 3)
        {
            setLogoTransparency(0);
        }
	}

    /// <summary>
    /// Sets the transparency of the logo images.
    /// </summary>
    /// <param name="val">The new transparency, between 0 and 1.</param>
    private void setLogoTransparency(float val)
    {
        foreach (Image i in logoContainer.GetComponentsInChildren<Image>())
        {
            Color newColor = i.color;
            newColor.a = val;
            i.color = newColor;
        }
    }

    /// <summary>
    /// Select the specified region, and prompt for a language if necessary.
    /// </summary>
    /// <param name="countryCode">The country code to select.</param>
    public void selectRegion(string countryCode)
    {
        selectedRegion = countryCode;

        if (Country.getNumLanguages(countryCode) > 1)
        {
            regionPanel.SetActive(false);

            foreach (Transform child in languageButtonContainer)
            {
                Object.Destroy(child.gameObject);
            }

            for (int loop = 1; loop <= Country.getNumLanguages(countryCode); loop++)
            {
                GameObject newButton = Object.Instantiate(languageButtonPrefab);
                newButton.name = loop.ToString();
                newButton.GetComponentInChildren<Text>().text = Country.getTranslation(countryCode, loop, Country.Message.LanguageName);
                newButton.transform.SetParent(languageButtonContainer);
                newButton.transform.localScale = Vector3.one;
            }

            languagePanel.SetActive(true);
        }
        else
        {
            saveRegion(selectedRegion, 1);
        }
    }

    /// <summary>
    /// Select the specified language.
    /// </summary>
    /// <param name="language">The language to select.</param>
    public void selectLanguage(int language)
    {
        saveRegion(selectedRegion, language);
    }

    /// <summary>
    /// Selects the local region for the new player and loads the main scene.
    /// </summary>
    /// <param name="countryCode">The selected country code.</param>
    /// <param name="language">The selected language ID.</param>
    public void saveRegion(string countryCode, int language)
    {
        StreamWriter writer = new StreamWriter(regionPath);
        writer.WriteLine(countryCode);
        writer.WriteLine(language);
        writer.Close();

        //Application.LoadLevel("Main");
        Object.FindObjectOfType<SceneManager>().loadMain();
    }
}
