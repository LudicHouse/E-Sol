using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine.UI;
using System.Net;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public System.Random rand;
    public GameObject accessoryPanel;
    public GameObject animalPanel;
    public GameObject optionsGroup;
    public GameObject creditsPanel;
    public float secondsPerAutosave;

    public string plantRegion;
    public int plantLanguage;
    public string localRegion;

    //public float swapSaveDuration;

    public List<string> animalNames;
    public List<GameObject> animalPrefabs;
    public List<Branch.Anchor> animalAnchors;

    public AudioClip returnSound;

    private int startSeed;
    //private Save toLoad;
    //private bool loaded = false;
    private Branch branchToEdit = null;

    private string localSavePath;
    private string regionPath;
    private string remoteSavePath;
    private string remoteDataPath;
    private long swapStartTicks = -1;
    private string savePath;

    private float autoSaveTimer;

    private OptionsController optionsController;

    // Use this for initialization
	void Awake () {
        Debug.Log("Game controller is awake");
        //UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
        autoSaveTimer = secondsPerAutosave;
        localSavePath = Application.persistentDataPath + "/localPlant.save";
        regionPath = Application.persistentDataPath + "/localregion";
        remoteSavePath = Application.persistentDataPath + "/remotePlant.save";
        remoteDataPath = Application.persistentDataPath + "/remotedata";
        savePath = localSavePath;

        /*if (File.Exists(remoteSavePath) == true & File.Exists(remoteDataPath) == true)
        {
            Debug.Log("Remote plant found!");
            StreamReader reader = new StreamReader(remoteDataPath); //Note: Error handling if file doesn't exist?
            string region = reader.ReadLine();
            long startedTicks = long.Parse(reader.ReadLine());
            reader.Close();

            Debug.Log("Downloaded at " + new DateTime(startedTicks).ToString());
            if (isSwapTimeUp(startedTicks) == false)
            {
                Debug.Log("Loading remote plant.");
                savePath = remoteSavePath;
                plantRegion = region;
                swapStartTicks = startedTicks;
            }
            else
            {
                Debug.Log("Remote timer expired, ignoring.");
                File.Delete(remoteSavePath);
                File.Delete(remoteDataPath);
            }
        }

        StreamReader lReader = new StreamReader(regionPath); //Note: Error handling if file doesn't exist?
        string lRegion = lReader.ReadLine();
        lReader.Close();
        localRegion = lRegion;
        if (swapStartTicks == -1)
        {
            plantRegion = lRegion;
        }

        foreach (GameController g in UnityEngine.Object.FindObjectsOfType<GameController>())
        {
            if (g != this)
            {
                g.accessoryPanel = this.accessoryPanel; //TODO: Sort out a better solution for this
                g.animalPanel = this.animalPanel; //And this.
                g.optionsGroup = this.optionsGroup;
            }
        }

        if (FindObjectsOfType<GameController>().Length > 1)
        {
            UnityEngine.Object.Destroy(this.gameObject);
            Debug.Log("Additional Game Controller found, using that instead.");
        }
        else if (File.Exists(savePath) == true)
        {
            Debug.Log("Save file exists, loading...");
            load();
        }
        else
        {
            rand = new System.Random();
            startSeed = rand.Next();
            rand = new System.Random(startSeed);
            Debug.Log("Started new game with seed " + startSeed);
        }*/
        StreamReader lReader = new StreamReader(regionPath); //Note: Error handling if file doesn't exist?
        string lRegion = lReader.ReadLine();
        lReader.Close();
        localRegion = lRegion;
        if (swapStartTicks == -1)
        {
            plantRegion = lRegion;
        }

        plantRegion = UnityEngine.Object.FindObjectOfType<SceneController>().plantRegion;
        plantLanguage = UnityEngine.Object.FindObjectOfType<SceneController>().plantLanguage;

        UnityEngine.Object.FindObjectOfType<SetFlag>().set(plantRegion);

        if (UnityEngine.Object.FindObjectOfType<SceneController>().loadPlant == true)
        {
            Debug.Log("Attempting to load save data...");
            //plantRegion = UnityEngine.Object.FindObjectOfType<SceneManager>().plantRegion;
            swapStartTicks = UnityEngine.Object.FindObjectOfType<SceneController>().swapStartTicks;
            savePath = UnityEngine.Object.FindObjectOfType<SceneController>().savePath;
            StartCoroutine("load");
        }
        else
        {
            rand = new System.Random();
            startSeed = rand.Next();
            rand = new System.Random(startSeed);
            Debug.Log("Started new game with seed " + startSeed);
            UnityEngine.Object.FindObjectOfType<FinishLoading>().ready();
        }

        optionsController = optionsGroup.GetComponent<OptionsController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            Application.Quit();
        }

        /*if (toLoad != null & loaded == true)
        {
            //Debug.Log("Beginning early load.");
            GameObject.FindObjectOfType<Plant>().transform.position = new Vector3(0, toLoad.height, 0); //This has to be done before the other portions to give the branches time to spawn correctly
            //Debug.Log("Early load done.");
        }*/

        if (autoSaveTimer <= 0)
        {
            save();
            autoSaveTimer = secondsPerAutosave;
        }
        else
        {
            autoSaveTimer -= Time.deltaTime;
        }

        if (swapStartTicks != -1)
        {
            if (UnityEngine.Object.FindObjectOfType<SceneController>().isSwapTimeUp(swapStartTicks) == true)
            {
                Debug.Log("Swap time up!");
                UnityEngine.Object.FindObjectOfType<MusicController>().playSoundEffect(returnSound);
                File.Delete(remoteSavePath);
                File.Delete(remoteDataPath);
                UnityEngine.Object.FindObjectOfType<SceneController>().loadMain();
            }
        }
	}

    /*void OnLevelWasLoaded(int level)
    {
        if (Application.loadedLevelName == "Main")
        {
            Debug.Log("About to start late load");
            loaded = true;
        }
    }*/

    /*void LateUpdate()
    {
        if (toLoad != null & loaded == true)
        {
            Debug.Log("Beginning late load.");
            GameObject.FindObjectOfType<SpawnSmog>().spawnTimer = toLoad.smogTimer;
            GameObject.FindObjectOfType<SpawnPests>().respawnCounter = toLoad.pestTimer;
            GameObject.FindObjectOfType<WateringCan>().water = toLoad.canLevel;
            GameObject.FindObjectOfType<Plant>().plantHydration = toLoad.hydrationLevel;

            foreach (Image option in accessoryPanel.GetComponentsInChildren<Image>(true))
            {
                if (option.tag == "AccessoryOption")
                {
                    /*if (option.name == toLoad.selectedAccessory)
                    {
                        //Debug.Log("Loaded accessory " + option.name);
                        UnityEngine.Object.FindObjectOfType<AccessoryController>().setAccessory(option.sprite, true); //Note: May cause issues if menu sprite is different to scripted button press parameter
                    }

                    if (toLoad.unlockedAccessories.Contains(option.name) == true)
                    {
                        option.GetComponent<Toggle>().interactable = true;
                    }
                    else
                    {
                        option.GetComponent<Toggle>().interactable = false;
                    }
                }
            }

            foreach (AccessoryController ac in UnityEngine.Object.FindObjectsOfType<AccessoryController>())
            {
                ac.setAccessory(toLoad.selectedAccessory);
            }

            foreach (Image option in animalPanel.GetComponentsInChildren<Image>(true))
            {
                if (option.tag == "AnimalOption")
                {
                    //Debug.Log(option.name);
                    foreach (KeyValuePair<int, string> pair in toLoad.selectedAnimals)
                    {
                        //Debug.Log("Branch " + pair.Key + " has animal " + pair.Value + ".");
                        if (pair.Value == option.name)
                        {
                            //Debug.Log(GameObject.FindObjectsOfType<Branch>().Length);
                            Transform branch = GameObject.FindGameObjectWithTag("AnimalBranches").GetComponent<GrowBranches>().sortBranches()[pair.Key];
                            foreach (SpriteRenderer s in branch.GetComponentsInChildren<SpriteRenderer>(true))
                            {
                                if (s.tag == "Animal")
                                {
                                    s.sprite = option.sprite;
                                    break;
                                }
                            }
                            //GameObject.FindGameObjectsWithTag("Animal")[pair.Key].GetComponent<SpriteRenderer>().sprite = option.sprite;
                        }
                    }

                    if (toLoad.unlockedAnimals.Contains(option.name) == true)
                    {
                        option.GetComponent<Toggle>().interactable = true;
                    }
                    else
                    {
                        option.GetComponent<Toggle>().interactable = false;
                    }
                }
            }

            toLoad = null;
            loaded = false;
            Debug.Log("Load complete.");
        }
    }*/

    void OnApplicationQuit()
    {
        save();
    }

    /// <summary>
    /// Save the current game state to file.
    /// </summary>
    public void save()
    {
        Save toSave = new Save();
        toSave.saveDate = System.DateTime.Now;
        toSave.randomSeed = startSeed;

        toSave.height = GameObject.FindObjectOfType<Plant>().transform.position.y;
        toSave.smogTimer = GameObject.FindObjectOfType<SpawnSmog>().spawnTimer;
        toSave.pestTimer = GameObject.FindObjectOfType<SpawnPests>().respawnCounter;
        toSave.canLevel = GameObject.FindObjectOfType<WateringCan>().water;
        toSave.hydrationLevel = GameObject.FindObjectOfType<Plant>().plantHydration;

        //Sprite accessorySprite = GameObject.FindGameObjectWithTag("Accessory").GetComponent<SpriteRenderer>().sprite;
        foreach (Image option in accessoryPanel.GetComponentsInChildren<Image>(true))
        {
            if (option.tag == "AccessoryOption")
            {
                /*if (accessorySprite != null)
                {
                    if (option.sprite == accessorySprite)
                    {
                        //Debug.Log("Saved accessory " + option.name);
                        toSave.selectedAccessory = option.name;
                    }
                }*/

                if (option.GetComponent<Toggle>().interactable == true)
                {
                    toSave.unlockedAccessories.Add(option.name);
                }
            }
        }
        /*if (accessorySprite != null & toSave.selectedAccessory == null)
        {
            Debug.LogError("Unable to identify current accessory!");
        }*/

        if (getAccessory() != null)
        {
            toSave.selectedAccessory = getAccessory();
        }
        Debug.Log("Saved accessory is " + toSave.selectedAccessory);

        int count = 0;
        foreach (Transform branch in GameObject.FindGameObjectWithTag("AnimalBranches").GetComponent<GrowBranches>().sortBranches())
        {
            /*foreach (SpriteRenderer rend in branch.GetComponentsInChildren<SpriteRenderer>(true))
            {
                if (rend.tag == "Animal")
                {
                    Sprite animalSprite = rend.sprite;

                    if (animalSprite != null)
                    {
                        foreach (Image option in animalPanel.GetComponentsInChildren<Image>(true))
                        {
                            if (option.tag == "AnimalOption")
                            {
                                if (option.sprite == animalSprite)
                                {
                                    toSave.selectedAnimals.Add(count, option.name);
                                    break;
                                }
                            }
                        }

                        if (toSave.selectedAnimals.ContainsKey(count) == false)
                        {
                            Debug.LogError("Unable to identify animal for branch " + count + "!");
                        }
                    }

                    break;
                }
            }*/

            if (branch.GetComponent<Branch>().getAnimal() != null)
            {
                toSave.selectedAnimals.Add(count, getAnimal(branch.GetComponent<Branch>().getAnimal()));
            }

            count++;
        }

        foreach (Image animal in animalPanel.GetComponentsInChildren<Image>(true))
        {
            if (animal.tag == "AnimalOption")
            {
                if (animal.GetComponent<Toggle>().interactable == true)
                {
                    toSave.unlockedAnimals.Add(animal.name);
                }
            }
        }

        //Debug.Log(toSave);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath);
        bf.Serialize(file, toSave);
        file.Close();
        Debug.Log("Saved.");

        if (swapStartTicks == -1)
        {
            Debug.Log("Uploading...");
            UnityEngine.Object.FindObjectOfType<NetworkController>().uploadSaveAsync(toSave);
        }
    }

    /// <summary>
    /// Load the save file stored in the scene manager.
    /// </summary>
    /// <returns></returns>
    public IEnumerator load()
    {
        Debug.Log("Beginning load");
        Save toLoad = UnityEngine.Object.FindObjectOfType<SceneController>().plantSave;

        System.TimeSpan timeDiff = System.DateTime.Now - toLoad.saveDate;
        //Debug.Log("It has been " + timeDiff.TotalSeconds + " seconds since the last save.");
        Plant p = UnityEngine.Object.FindObjectOfType<Plant>();
        float animalsMultiplier = Mathf.Floor((float)p.getNumAnimals() / (float)p.animalsPerMultiplier) + 1;
        float pestGrowthTime = Mathf.Clamp(toLoad.pestTimer, 0, (float)timeDiff.TotalSeconds);
        float pestGrowth = pestGrowthTime * p.insectsMultiplier;
        float smogGrowthTime = Mathf.Clamp(toLoad.smogTimer, 0, (float)timeDiff.TotalSeconds);
        float smogGrowth = smogGrowthTime * p.pollutionMultiplier;
        float newWater = toLoad.hydrationLevel - (p.hydroDrainRate * (float)timeDiff.TotalSeconds);
        float waterGrowth = 0;
        if (newWater <= 0)
        {
            float waterTime = toLoad.hydrationLevel / p.hydroDrainRate;
            waterGrowth = (0.5f * waterTime * toLoad.hydrationLevel) * p.waterMultiplier;
        }
        else
        {
            waterGrowth = (0.5f * (float)timeDiff.TotalSeconds * (toLoad.hydrationLevel + newWater)) * p.waterMultiplier;
        }
        //Debug.Log(pestGrowthTime);
        //Debug.Log(pestGrowth + " " + smogGrowth + " " + waterGrowth);

        toLoad.height += (pestGrowth + smogGrowth + waterGrowth) * animalsMultiplier * p.growthPerMoodPerSecond;
        toLoad.pestTimer = Mathf.Max(0, toLoad.pestTimer - (float)timeDiff.TotalSeconds);
        toLoad.smogTimer = Mathf.Max(0, toLoad.smogTimer - (float)timeDiff.TotalSeconds);
        toLoad.hydrationLevel = Mathf.Max(0, newWater);
        toLoad.canLevel = Math.Min(UnityEngine.Object.FindObjectOfType<WateringCan>().maxWater, toLoad.canLevel + (UnityEngine.Object.FindObjectOfType<WateringCan>().waterRegenSpeed * (float)timeDiff.TotalSeconds));

        startSeed = toLoad.randomSeed;
        rand = new System.Random(toLoad.randomSeed);
        GameObject.FindObjectOfType<Plant>().transform.position = new Vector3(0, toLoad.height, 0);

        Debug.Log("Accessory: " + toLoad.selectedAccessory);
        UnityEngine.Object.FindObjectOfType<MenuController>().setAccessory(toLoad.selectedAccessory, true);
        /*foreach (AccessoryController ac in UnityEngine.Object.FindObjectsOfType<AccessoryController>())
        {
            //Debug.Log(ac.name);
            ac.setAccessory(toLoad.selectedAccessory, true);
        }*/

        for (int loop = 0; loop < 5; loop++)
        {
            yield return null;
        }

        Debug.Log("Beginning late load.");
        GameObject.FindObjectOfType<SpawnSmog>().spawnTimer = toLoad.smogTimer;
        GameObject.FindObjectOfType<SpawnPests>().respawnCounter = toLoad.pestTimer;
        GameObject.FindObjectOfType<WateringCan>().water = toLoad.canLevel;
        GameObject.FindObjectOfType<Plant>().plantHydration = toLoad.hydrationLevel;

        foreach (Image option in accessoryPanel.GetComponentsInChildren<Image>(true))
        {
            if (option.tag == "AccessoryOption")
            {
                /*if (option.name == toLoad.selectedAccessory)
                {
                    //Debug.Log("Loaded accessory " + option.name);
                    UnityEngine.Object.FindObjectOfType<AccessoryController>().setAccessory(option.sprite, true); //Note: May cause issues if menu sprite is different to scripted button press parameter
                }*/

                if (toLoad.unlockedAccessories.Contains(option.name) == true)
                {
                    option.GetComponent<Toggle>().interactable = true;
                }
                else
                {
                    option.GetComponent<Toggle>().interactable = false;
                }
            }
        }

        foreach (Image option in animalPanel.GetComponentsInChildren<Image>(true))
        {
            if (option.tag == "AnimalOption")
            {
                //Debug.Log(option.name);
                foreach (KeyValuePair<int, string> pair in toLoad.selectedAnimals)
                {
                    //Debug.Log("Branch " + pair.Key + " has animal " + pair.Value + ".");
                    if (pair.Value == option.name)
                    {
                        //Debug.Log(GameObject.FindObjectsOfType<Branch>().Length);
                        Transform branch = GameObject.FindGameObjectWithTag("AnimalBranches").GetComponent<GrowBranches>().sortBranches()[pair.Key];
                        branch.GetComponent<Branch>().setAnimal(getAnimal(pair.Value), getAnchor(pair.Value));
                        foreach (SpriteRenderer s in branch.GetComponentsInChildren<SpriteRenderer>(true))
                        {
                            if (s.tag == "Animal")
                            {
                                s.sprite = option.sprite;
                                break;
                            }
                        }
                        //GameObject.FindGameObjectsWithTag("Animal")[pair.Key].GetComponent<SpriteRenderer>().sprite = option.sprite;
                    }
                }

                if (toLoad.unlockedAnimals.Contains(option.name) == true)
                {
                    option.GetComponent<Toggle>().interactable = true;
                }
                else
                {
                    option.GetComponent<Toggle>().interactable = false;
                }
            }
        }

        UnityEngine.Object.FindObjectOfType<SceneController>().loadPlant = false;
        UnityEngine.Object.FindObjectOfType<SceneController>().swapStartTicks = -1;
        Debug.Log("Load complete.");
        UnityEngine.Object.FindObjectOfType<FinishLoading>().ready();
    }

    /*/// <summary>
    /// Load a game from file.
    /// </summary>
    public void load()
    {
        if (File.Exists(savePath) == true)
        {
            Debug.Log("Loading...");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.Open);
            Save loadedSave = (Save)bf.Deserialize(file);
            file.Close();

            //Debug.Log("Seed is " + loadedSave.randomSeed);

            System.TimeSpan timeDiff = System.DateTime.Now - loadedSave.saveDate;
            //Debug.Log("It has been " + timeDiff.TotalSeconds + " seconds since the last save.");
            Plant p = UnityEngine.Object.FindObjectOfType<Plant>();
            float animalsMultiplier = Mathf.Floor((float)p.getNumAnimals() / (float)p.animalsPerMultiplier) + 1;
            float pestGrowthTime = Mathf.Clamp(loadedSave.pestTimer, 0, (float)timeDiff.TotalSeconds);
            float pestGrowth = pestGrowthTime * p.insectsMultiplier;
            float smogGrowthTime = Mathf.Clamp(loadedSave.smogTimer, 0, (float)timeDiff.TotalSeconds);
            float smogGrowth = smogGrowthTime * p.pollutionMultiplier;
            float newWater = loadedSave.hydrationLevel - (p.hydroDrainRate * (float)timeDiff.TotalSeconds);
            float waterGrowth = 0;
            if (newWater <= 0)
            {
                float waterTime = loadedSave.hydrationLevel / p.hydroDrainRate;
                waterGrowth = (0.5f * waterTime * loadedSave.hydrationLevel) * p.waterMultiplier;
            }
            else
            {
                waterGrowth = (0.5f * (float)timeDiff.TotalSeconds * (loadedSave.hydrationLevel + newWater)) * p.waterMultiplier;
            }
            //Debug.Log(pestGrowthTime);
            //Debug.Log(pestGrowth + " " + smogGrowth + " " + waterGrowth);

            loadedSave.height += (pestGrowth + smogGrowth + waterGrowth) * animalsMultiplier * p.growthPerMoodPerSecond;
            loadedSave.pestTimer = Mathf.Max(0, loadedSave.pestTimer - (float)timeDiff.TotalSeconds);
            loadedSave.smogTimer = Mathf.Max(0, loadedSave.smogTimer - (float)timeDiff.TotalSeconds);
            loadedSave.hydrationLevel = Mathf.Max(0, newWater);

            startSeed = loadedSave.randomSeed;
            rand = new System.Random(loadedSave.randomSeed);
            Debug.Log("Loading scene.");
            Application.LoadLevel("Main");
            //UnityEngine.Object.FindObjectOfType<SceneManager>().loadLevel("Main");
            Debug.Log("Scene loaded.");
            toLoad = loadedSave;
            //Debug.Log("Load done.");
        }
        else
        {
            Debug.LogError("Tried to load game when no save file exists!");
        }
    }*/

    /// <summary>
    /// Reset the current save data and start a new game.
    /// </summary>
    public void newGame() //TODO: Deprecated?
    {
        Debug.Log("Starting new game...");
        File.Delete(localSavePath);
        File.Delete(regionPath);
        Debug.Log("Deleting " + remoteSavePath + " and " + remoteDataPath);
        File.Delete(remoteSavePath);
        File.Delete(remoteDataPath);
        File.Delete(Application.persistentDataPath + "/token");
        UnityEngine.Object.Destroy(this.gameObject);
        SceneManager.LoadScene("Startup");
    }

    /// <summary>
    /// Delete the local region data and restart the game, prompting the user to select a new country.
    /// </summary>
    public void changeCountry()
    {
        Debug.Log("Restarting with new country...");
        File.Delete(regionPath);
        File.Delete(Application.persistentDataPath + "/token");
        UnityEngine.Object.Destroy(this.gameObject);
        SceneManager.LoadScene("Startup");
    }

    /// <summary>
    /// Checks whether any UI panel is open.
    /// </summary>
    /// <returns>True if a panel is open, false otherwise.</returns>
    public bool isPanelOpen()
    {
        if (accessoryPanel.activeSelf == true | animalPanel.activeSelf == true)
        {
            return true;
        }
        else
        {
            return optionsController.isOptionsPanelOpen();
        }
    }

    /// <summary>
    /// Checks whether the credits UI panel is open.
    /// </summary>
    /// <returns>True if the panel is open, false otherwise.</returns>
    public bool isCreditsPanelOpen()
    {
        if (creditsPanel.activeSelf == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Opens the animal selection panel.
    /// </summary>
    /// <param name="branch">The branch being edited.</param>
    public void openAnimalPanel(Branch branch)
    {
        foreach (Image option in animalPanel.GetComponentsInChildren<Image>(true))
        {
            if (option.tag == "AnimalOption")
            {
                if (option.name == getAnimal(branch.getAnimal()))
                {
                    option.GetComponent<Toggle>().isOn = true;
                }
                else
                {
                    option.GetComponent<Toggle>().isOn = false;
                }
            }
        }

        branchToEdit = branch;
        animalPanel.GetComponent<CloseIfTouchElsewhere>().ignoreNextTap = true;
        animalPanel.SetActive(true);

        UnityEngine.Object.FindObjectOfType<TutorialManager>().setAnimDone();
    }

    /// <summary>
    /// Applies an animal to the selected branch.
    /// </summary>
    /// <param name="newAnimal">The animal sprite to apply.</param>
    public void setAnimal(string newAnimal)
    {
        if (animalPanel.activeSelf == true)
        {
            if (getAnimal(branchToEdit.getAnimal()) == newAnimal)
            {
                branchToEdit.setAnimal(null, getAnchor(newAnimal));
            }
            else
            {
                branchToEdit.setAnimal(getAnimal(newAnimal), getAnchor(newAnimal));
            }
        }
    }

    /*/// <summary>
    /// Checks whether the swapped plant (if any) should be retuned.
    /// </summary>
    /// <param name="ticks">The timestamp at which the plant was collected.</param>
    /// <returns>True if the plant's timer has expired, false otherwise (or if there is no currently swapped plant).</returns>
    private bool isSwapTimeUp(long ticks)
    {
        DateTime target = new DateTime(ticks);
        target = target.AddSeconds(swapSaveDuration);

        //Debug.Log("Target: " + target.ToString() + ", Current: " + DateTime.Now.ToString());

        if (target.CompareTo(DateTime.Now) < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }*/

    /// <summary>
    /// Get the currently applied accessory.
    /// </summary>
    /// <returns>The name of the active accessory.</returns>
    public string getAccessory()
    {
        foreach (AccessoryController ac in UnityEngine.Object.FindObjectsOfType<AccessoryController>())
        {
            if (ac.getAccessory() != null)
            {
                return ac.getAccessory();
            }
        }

        return null;
    }

    /// <summary>
    /// Get the prefab for the specified animal.
    /// </summary>
    /// <param name="name">The name of the animal.</param>
    /// <returns>The animal prefab.</returns>
    private GameObject getAnimal(string name)
    {
        int index = animalNames.IndexOf(name);
        return animalPrefabs[index];
    }

    /// <summary>
    /// Get the position anchor to be used for the specified animal.
    /// </summary>
    /// <param name="name">The name of the animal.</param>
    /// <returns>The position on the branch to place the animal.</returns>
    private Branch.Anchor getAnchor(string name)
    {
        int index = animalNames.IndexOf(name);
        return animalAnchors[index];
    }

    /// <summary>
    /// Get the name of the specified animal.
    /// </summary>
    /// <param name="obj">The animal gameobject.</param>
    /// <returns>The name of the animal.</returns>
    private string getAnimal(GameObject obj)
    {
        if (obj == null)
        {
            return null;
        }

        //Debug.Log("Obj: " + obj.name);
        foreach (GameObject prefab in animalPrefabs)
        {
            //Debug.Log("Prefab: " + prefab.name);
            if (prefab.name + "(Clone)" == obj.name)
            {
                int index = animalPrefabs.IndexOf(prefab);
                return animalNames[index];
            }
        }

        return null;
    }
}
