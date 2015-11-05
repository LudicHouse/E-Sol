using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlowerLoader : MonoBehaviour {
    public Transform petalContainer;
    public Image foregroundImage;
    public Color zeroColor;
    public Color oneColor;
    public float initialValue;

	// Use this for initialization
	void Start () {
        set(initialValue);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Sets the loading indicator to the specified value.
    /// </summary>
    /// <param name="value">The value to display, between 0 and 1.</param>
    public void set(float value)
    {
        //Debug.Log("Setting " + value);
        foregroundImage.color = Color.Lerp(zeroColor, oneColor, value);

        int petalsToFill = (int)Mathf.Floor(value * petalContainer.childCount);
        for (int loop = 0; loop < petalsToFill; loop++)
        {
            petalContainer.GetChild(loop).gameObject.SetActive(true);
        }
        for (int loop = petalsToFill; loop < petalContainer.childCount; loop++)
        {
            petalContainer.GetChild(loop).gameObject.SetActive(false);
        }
    }
}
