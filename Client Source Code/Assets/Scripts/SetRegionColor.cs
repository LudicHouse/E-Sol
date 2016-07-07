using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetRegionColor : MonoBehaviour {
    public string countryCode;
    public float partMin;
    public float partMax;
    public float ozoneMin;
    public float ozoneMax;

    public Color lowColor;
    public Color midColor;
    public Color highColor;

	// Use this for initialization
	void Start () {
        float part = (Country.getParticulate(countryCode) - partMin) / (partMax - partMin);
        float ozone = (Country.getOzone(countryCode) - ozoneMin) / (ozoneMax - ozoneMin);
        float average = (part + ozone) / 2;

        if (average < 0.5)
        {
            GetComponent<Image>().color = Color.Lerp(lowColor, midColor, average * 2);
        }
        else
        {
            GetComponent<Image>().color = Color.Lerp(midColor, highColor, (average - 0.5f) * 2);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
