using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetCloudLevel : MonoBehaviour {
    public string countryCode;
    public float partMin;
    public float partMax;
    public float ozoneMin;
    public float ozoneMax;
    public float cloudAlphaMin;

	// Use this for initialization
	void Start () {
        float part = (Country.getParticulate(countryCode) - partMin) / (partMax - partMin);
        float ozone = (Country.getOzone(countryCode) - ozoneMin) / (ozoneMax - ozoneMin);
        float average = (part + ozone) / 2;

        foreach (Transform child in transform)
        {
            Color newColor = child.GetComponent<Image>().color;
            newColor.a = cloudAlphaMin + ((1 - cloudAlphaMin) * average);
            child.GetComponent<Image>().color = newColor;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
