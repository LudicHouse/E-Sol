using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CloudCluster : MonoBehaviour {
    public Image cloud1;
    public Image cloud2;
    public Image cloud3;
    public Image cloud4;

    public float cloud1Min;
    public float cloud1Max;
    public float cloud2Min;
    public float cloud2Max;
    public float cloud3Min;
    public float cloud3Max;
    public float cloud4Min;
    public float cloud4Max;

    public string countryCode;

	// Use this for initialization
	void Start () {
        set(Country.getParticulate(countryCode));
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    /// <summary>
    /// Adjusts the visibility of the clouds depending on the pollution level.
    /// </summary>
    /// <param name="pollution">The pollution level to use.</param>
    public void set(float pollution)
    {
        Color new1 = cloud1.color;
        new1.a = Mathf.Clamp((pollution - cloud1Min) / (cloud1Max - cloud1Min), 0, 1);
        cloud1.color = new1;

        Color new2 = cloud2.color;
        new2.a = Mathf.Clamp((pollution - cloud2Min) / (cloud2Max - cloud2Min), 0, 1);
        cloud2.color = new2;

        Color new3 = cloud3.color;
        new3.a = Mathf.Clamp((pollution - cloud3Min) / (cloud3Max - cloud3Min), 0, 1);
        cloud3.color = new3;

        Color new4 = cloud4.color;
        new4.a = Mathf.Clamp((pollution - cloud4Min) / (cloud4Max - cloud4Min), 0, 1);
        cloud4.color = new4;
    }
}
