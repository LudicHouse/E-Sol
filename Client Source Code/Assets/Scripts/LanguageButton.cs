using UnityEngine;
using System.Collections;

public class LanguageButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Select the clicked language.
    /// </summary>
    public void click()
    {
        Object.FindObjectOfType<StartupController>().selectLanguage(int.Parse(gameObject.name));
    }
}
