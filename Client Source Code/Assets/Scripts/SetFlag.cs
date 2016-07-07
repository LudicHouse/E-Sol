using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SetFlag : MonoBehaviour {
    public List<string> countryCodes;
    public List<Sprite> flags;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Apply the specified flag sprite.
    /// </summary>
    /// <param name="countryCode">The country code of the flag to apply.</param>
    public void set(string countryCode)
    {
        int index = countryCodes.IndexOf(countryCode);

        if (GetComponent<Image>() != null)
        {
            Image img = GetComponent<Image>();
            img.sprite = flags[index];
        }
        else if (GetComponent<SpriteRenderer>() != null)
        {
            SpriteRenderer rend = GetComponent<SpriteRenderer>();
            rend.sprite = flags[index];
        }
    }
}
