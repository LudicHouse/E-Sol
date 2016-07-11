using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEngine.UI;

public class SwapCountdownTimer : MonoBehaviour {
    private DateTime end;
    private Text text;
    private bool showCountdown = false;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();

        if (File.Exists(Application.persistentDataPath + "/remotedata") == true)
        {
            StreamReader reader = new StreamReader(Application.persistentDataPath + "/remotedata");
            reader.ReadLine();
            reader.ReadLine();
            DateTime start = new DateTime(long.Parse(reader.ReadLine()));
            reader.Close();

            end = start.AddSeconds(UnityEngine.Object.FindObjectOfType<SceneController>().swapSaveDuration);
            showCountdown = true;
        }
        else
        {
            text.text = "";
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (showCountdown == true)
        {
            TimeSpan diff = end - DateTime.Now;

            string timer = "";
            if (diff.Days > 0)
            {
                timer += Mathf.Max(0, diff.Days).ToString("D2") + ":";
            }
            timer += Mathf.Max(0, diff.Hours).ToString("D2") + ":" + Mathf.Max(0, diff.Minutes).ToString("D2") + ":" + Mathf.Max(0, diff.Seconds).ToString("D2");

            text.text = timer;
        }
	}
}
