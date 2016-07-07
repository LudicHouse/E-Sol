using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class DisableIfSwapped : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (File.Exists(Application.persistentDataPath + "/remotedata") == true | File.Exists(Application.persistentDataPath + "/remotePlant.save") == true)
        {
            GetComponent<Button>().interactable = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
