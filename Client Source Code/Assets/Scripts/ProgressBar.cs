using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {
    public RectTransform barImage;
    public Color lowColor;
    public Color midColor;
    public Color highColor;

    public float maxWidth;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    /// <summary>
    /// Sets the width and color of the progress bar to the specified value.
    /// </summary>
    /// <param name="value">The value of the progress bar, between 0 and 1.</param>
    public void set(float value)
    {
        Vector2 newSize = barImage.sizeDelta;
        newSize.x = Mathf.Clamp(maxWidth * value, 0, maxWidth);
        barImage.sizeDelta = newSize;

        if (value < 0.5)
        {
            barImage.GetComponent<Image>().color = Color.Lerp(lowColor, midColor, value * 2);
        }
        else
        {
            barImage.GetComponent<Image>().color = Color.Lerp(midColor, highColor, (value - 0.5f) * 2);
        }
    }
}
