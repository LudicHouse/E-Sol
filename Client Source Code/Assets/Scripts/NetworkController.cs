using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System;
using System.Threading;

public class NetworkController : MonoBehaviour {
    public string serverUrl;

    private string token;

    public class DownloadedSave
    {
        public Save save;
        public int language;
    }

	// Use this for initialization
	void Start () {
        string tokenPath = Application.persistentDataPath + "/token";
        if (File.Exists(tokenPath) == true)
        {
            //Debug.Log("Token file found.");
            StreamReader reader = new StreamReader(tokenPath);
            token = reader.ReadLine();
            reader.Close();
        }
        else
        {
            //Debug.Log("Token file not found, registering...");
            WebClient client = new WebClient();
            token = client.DownloadString(serverUrl + "/register.php?region=" + UnityEngine.Object.FindObjectOfType<GameController>().localRegion + "&language=" + UnityEngine.Object.FindObjectOfType<GameController>().plantLanguage);

            StreamWriter writer = new StreamWriter(tokenPath);
            writer.Write(token);
            writer.Close();
        }

        //Debug.Log("Token is " + token);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Uploads a save file to the server asynchronously.
    /// </summary>
    /// <param name="toUpload">The save file to upload.</param>
    public void uploadSaveAsync(Save toUpload)
    {
        Thread t = new Thread(new ParameterizedThreadStart(this.uploadSave));
        t.Start(toUpload);
    }

    /// <summary>
    /// Uploads a save file to the server.
    /// </summary>
    /// <param name="toUpload">The save file to upload.</param>
    private void uploadSave(object toUpload)
    {
        uploadSave((Save)toUpload);
    }

    /// <summary>
    /// Uploads a save file to the server.
    /// </summary>
    /// <param name="toUpload">The save file to upload.</param>
    public void uploadSave(Save toUpload)
    {
        string url = serverUrl + "/uploadsave.php?token=" + token;
        url += "&savedate=" + toUpload.saveDate.ToUniversalTime().Ticks;
        url += "&randomseed=" + toUpload.randomSeed;
        url += "&height=" + toUpload.height;
        url += "&smogtimer=" + toUpload.smogTimer;
        url += "&pesttimer=" + toUpload.pestTimer;
        url += "&canlevel=" + toUpload.canLevel;
        url += "&hydrationlevel=" + toUpload.hydrationLevel;
        url += "&selectedaccessory=" + toUpload.selectedAccessory;

        url += "&numaccessories=" + toUpload.unlockedAccessories.Count;
        for (int loop = 0; loop < toUpload.unlockedAccessories.Count; loop++)
        {
            url += "&accessory" + loop + "=" + toUpload.unlockedAccessories[loop];
        }

        url += "&numselanim=" + toUpload.selectedAnimals.Count;
        int count = 0;
        foreach (KeyValuePair<int, string> pair in toUpload.selectedAnimals)
        {
            url += "&selbranch" + count + "=" + pair.Key;
            url += "&selanimal" + count + "=" + pair.Value;
            count++;
        }

        url += "&numunanim=" + toUpload.unlockedAnimals.Count;
        for (int loop = 0; loop < toUpload.unlockedAnimals.Count; loop++)
        {
            url += "&unanimal" + loop + "=" + toUpload.unlockedAnimals[loop];
        }

        WebClient client = new WebClient();
        Debug.Log(client.DownloadString(url));
    }

    /// <summary>
    /// Downloads a save at random from the server.
    /// </summary>
    /// <param name="countryCode">The country code to download saves from.</param>
    /// <returns>The downloaded save file.</returns>
    public DownloadedSave downloadSave(string countryCode)
    {
        string url = serverUrl + "/downloadsave.php?region=" + countryCode;
        WebClient client = new WebClient();
        string result = "";
        try
        {
            result = client.DownloadString(url);
        }
        catch
        {
            return null;
        }

        if (result == "false")
        {
            return null; //Throw exception?
        }

        string[] seperators = { "\n\r" };
        string[] lines = result.Split(seperators, System.StringSplitOptions.None);

        DownloadedSave dl = new DownloadedSave();
        dl.language = int.Parse(lines[0]);

        Save output = new Save();
        Debug.Log(result);
        output.saveDate = new DateTime(long.Parse(lines[1]), DateTimeKind.Utc).ToLocalTime();
        Debug.Log(output.saveDate.ToString());
        output.randomSeed = int.Parse(lines[2]);
        output.height = float.Parse(lines[3]);
        output.smogTimer = float.Parse(lines[4]);
        output.pestTimer = float.Parse(lines[5]);
        output.canLevel = float.Parse(lines[6]);
        output.hydrationLevel = float.Parse(lines[7]);
        output.selectedAccessory = lines[8];

        int pos = 9;

        int numAccessory = int.Parse(lines[pos]);
        pos++;
        for (int loop = 0; loop < numAccessory; loop++)
        {
            Debug.Log(lines[pos]);
            output.unlockedAccessories.Add(lines[pos]);

            pos++;
        }

        int numSelAnim = int.Parse(lines[pos]);
        pos++;
        for (int loop = 0; loop < numSelAnim; loop++)
        {
            int branch = int.Parse(lines[pos]);
            string animal = lines[pos + 1];
            output.selectedAnimals.Add(branch, animal);

            pos += 2;
        }
        pos--;

        int numUnAnim = int.Parse(lines[pos]);
        pos++;
        for (int loop = 0; loop < numUnAnim; loop++)
        {
            output.unlockedAnimals.Add(lines[pos]);

            pos++;
        }

        dl.save = output;

        return dl;
    }
}
