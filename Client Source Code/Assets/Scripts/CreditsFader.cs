using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreditsFader : MonoBehaviour {
    public GameObject group1;
    public GameObject group2;
    public float screenWaitDuration;
    public float blankWaitDuration;
    public float fadeDuration;

    private float timer;
    private int stage;

	// Use this for initialization
	void Start () {
        setTransparency(group1, 1);
        setTransparency(group2, 0);

        timer = screenWaitDuration;
        stage = 1;
	}
	
	// Update is called once per frame
	void Update () {
        if (timer <= 0)
        {
            if (stage == 1)
            {
                timer = fadeDuration;
                stage = 2;
                //Debug.Log("Moving to stage " + stage + " with timer set to " + timer);
            }
            else if (stage == 2)
            {
                timer = blankWaitDuration;
                stage = 3;
                //Debug.Log("Moving to stage " + stage + " with timer set to " + timer);
            }
            else if (stage == 3)
            {
                timer = fadeDuration;
                stage = 4;
                //Debug.Log("Moving to stage " + stage + " with timer set to " + timer);
            }
            else if (stage == 4)
            {
                timer = screenWaitDuration;
                stage = 5;
                //Debug.Log("Moving to stage " + stage + " with timer set to " + timer);
            }
            else if (stage == 5)
            {
                timer = fadeDuration;
                stage = 6;
                //Debug.Log("Moving to stage " + stage + " with timer set to " + timer);
            }
            else if (stage == 6)
            {
                timer = blankWaitDuration;
                stage = 7;
                //Debug.Log("Moving to stage " + stage + " with timer set to " + timer);
            }
            else if (stage == 7)
            {
                timer = fadeDuration;
                stage = 8;
                //Debug.Log("Moving to stage " + stage + " with timer set to " + timer);
            }
            else if (stage == 8)
            {
                timer = screenWaitDuration;
                stage = 1;
                //Debug.Log("Moving to stage " + stage + " with timer set to " + timer);
            }
        }
        else
        {
            if (stage == 1)
            {
                setTransparency(group1, 1);
                setTransparency(group2, 0);
            }
            else if (stage == 2)
            {
                setTransparency(group1, timer / fadeDuration);
                setTransparency(group2, 0);
            }
            else if (stage == 3)
            {
                setTransparency(group1, 0);
                setTransparency(group2, 0);
            }
            else if (stage == 4)
            {
                setTransparency(group1, 0);
                setTransparency(group2, 1 - (timer / fadeDuration));
            }
            else if (stage == 5)
            {
                setTransparency(group1, 0);
                setTransparency(group2, 1);
            }
            else if (stage == 6)
            {
                setTransparency(group1, 0);
                setTransparency(group2, timer / fadeDuration);
            }
            else if (stage == 7)
            {
                setTransparency(group1, 0);
                setTransparency(group2, 0);
            }
            else if (stage == 8)
            {
                setTransparency(group1, 1 - (timer /fadeDuration));
                setTransparency(group2, 0);
            }

            timer -= Time.deltaTime;
        }
	}

    /// <summary>
    /// Set the transparency of all Text objects in the group.
    /// </summary>
    /// <param name="group">The parent gameobject.</param>
    /// <param name="val">The alpha value to apply.</param>
    private void setTransparency(GameObject group, float val)
    {
        foreach (Text t in group.GetComponentsInChildren<Text>())
        {
            Color newColor = t.color;
            newColor.a = val;
            t.color = newColor;
        }
    }
}
