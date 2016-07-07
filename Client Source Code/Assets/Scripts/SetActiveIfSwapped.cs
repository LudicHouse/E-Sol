using UnityEngine;
using System.Collections;
using System.IO;

public class SetActiveIfSwapped : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string region = "none";

        if (File.Exists(Application.persistentDataPath + "/remotedata") == true)
        {
            StreamReader reader = new StreamReader(Application.persistentDataPath + "/remotedata");
            region = reader.ReadLine();
            reader.Close();
        }

        foreach (Transform child in transform)
        {
            if (Country.getName(region) == child.name)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
