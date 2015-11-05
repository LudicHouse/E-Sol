using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QualityManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(QualitySettings.GetQualityLevel());
        foreach (Toggle option in GetComponentsInChildren<Toggle>())
        {
            if (option.name == QualitySettings.names[QualitySettings.GetQualityLevel()])
            {
                option.isOn = true;
            }
            else
            {
                option.isOn = false;
            }
        }
	}

    /// <summary>
    /// Change the quality level of the game.
    /// </summary>
    /// <param name="level">The quality level to apply.</param>
    public void set(int level)
    {
        QualitySettings.SetQualityLevel(level);
        PlayerPrefs.SetInt("quality", level);
        PlayerPrefs.Save();
    }
}
