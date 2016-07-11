using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    public float swapSaveDuration;

    public bool loadPlant = false;
    public Save plantSave;
    public string plantRegion;
    public int plantLanguage = 1;
    public long swapStartTicks = -1;
    public string savePath;

    private AsyncOperation loader;
    private string toLoad;

    private string localSavePath;
    private string regionPath;
    private string remoteSavePath;
    private string remoteDataPath;

	// Use this for initialization
	void Awake () {
        localSavePath = Application.persistentDataPath + "/localPlant.save";
        regionPath = Application.persistentDataPath + "/localregion";
        remoteSavePath = Application.persistentDataPath + "/remotePlant.save";
        remoteDataPath = Application.persistentDataPath + "/remotedata";

        UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
        if (FindObjectsOfType<SceneController>().Length > 1)
        {
            UnityEngine.Object.Destroy(this.gameObject);
            Debug.Log("Additional Scene Manager found, using that instead.");
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Application.loadedLevelName == "Loading")
        {
            if (loader != null)
            {
                UnityEngine.Object.FindObjectOfType<FlowerLoader>().set(loader.progress);
                //Debug.Log(loader.progress);
            }
            else if (toLoad != null)
            {
                Debug.Log("In loading scene and toLoad is not null, loading new scene.");
                StartCoroutine("loadLevelAsync");
                toLoad = null;
            }
        }
        else
        {
            loader = null;
        }

        /*if (loader != null)
        {
            Debug.Log(loader.progress);
        }*/
	}

    void OnLevelWasLoaded(int level)
    {
        /*if (Application.loadedLevelName == "Loading")
        {
            Debug.Log("In loading scene, now attempting to load " + toLoad);
            StartCoroutine("loadLevelAsync");
            Debug.Log(loader.progress + " " + loader.isDone + " " + loader.allowSceneActivation);
        }
        else
        {
            Debug.Log("Not in loading scene, cleaning up.");
            loader = null;
        }*/
    }

    /// <summary>
    /// Load a new scene.
    /// </summary>
    /// <param name="levelName">The scene name to load.</param>
    public void loadLevel(string levelName)
    {
        Debug.Log("Attempting to load " + levelName);
        toLoad = levelName;
        Debug.Log("Going to loading scene first.");
        SceneManager.LoadScene("Loading");
    }

    /// <summary>
    /// Load the main scene (including any relevant save files).
    /// </summary>
    public void loadMain()
    {
        Debug.Log("Loading Main specifically.");
        if (File.Exists(regionPath) == true)
        {
            StreamReader lReader = new StreamReader(regionPath); //Note: Error handling if file doesn't exist?
            plantRegion = lReader.ReadLine();
            plantLanguage = int.Parse(lReader.ReadLine());
            lReader.Close();
        }

        if (File.Exists(localSavePath) == true)
        {
            Debug.Log("Loading save.");
            savePath = localSavePath;
            StreamReader lReader = new StreamReader(regionPath); //Note: Error handling if file doesn't exist?
            plantRegion = lReader.ReadLine();
            lReader.Close();

            if (File.Exists(remoteSavePath) == true & File.Exists(remoteDataPath) == true)
            {
                Debug.Log("Remote plant found!");
                StreamReader reader = new StreamReader(remoteDataPath); //Note: Error handling if file doesn't exist?
                string region = reader.ReadLine();
                int language = int.Parse(reader.ReadLine());
                long startedTicks = long.Parse(reader.ReadLine());
                reader.Close();

                Debug.Log("Downloaded at " + new DateTime(startedTicks).ToString());
                if (isSwapTimeUp(startedTicks) == false)
                {
                    Debug.Log("Loading remote plant.");
                    savePath = remoteSavePath;
                    plantRegion = region;
                    plantLanguage = language;
                    swapStartTicks = startedTicks;
                }
                else
                {
                    Debug.Log("Remote timer expired, ignoring.");
                    File.Delete(remoteSavePath);
                    File.Delete(remoteDataPath);
                }
            }

            plantSave = Save.load(savePath);
            loadPlant = true;
        }

        if (File.Exists(regionPath) == false)
        {
            Debug.Log("Couldn't find region file, returning to start screen.");
            loadLevel("Startup");
        }

        Debug.Log("Loading scene.");
        loadLevel("Main");
    }

    /// <summary>
    /// Load the new scene asynchronously.
    /// </summary>
    /// <returns></returns>
    private IEnumerator loadLevelAsync()
    {
        Debug.Log("Starting async level load.");
        loader = SceneManager.LoadSceneAsync(toLoad);
        Debug.Log("Load begun.");
        yield return loader;
        Debug.Log("Level load complete.");
    }

    /// <summary>
    /// Checks whether the swapped plant (if any) should be retuned.
    /// </summary>
    /// <param name="ticks">The timestamp at which the plant was collected.</param>
    /// <returns>True if the plant's timer has expired, false otherwise (or if there is no currently swapped plant).</returns>
    public bool isSwapTimeUp(long ticks)
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
    }
}
