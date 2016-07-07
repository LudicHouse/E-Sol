using UnityEngine;
using System.Collections;

public class ManageWaterFlowPosition : MonoBehaviour {
    public Transform anchor;
    public Transform scale;

    public GameObject leftGroup;
    public GameObject rightGroup;

    public bool showLeft;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = anchor.position;

        /*Vector3 newRot = transform.localEulerAngles;
        if (scale.localScale.x < 0)
        {
            newRot.y = -90;
        }
        else
        {
            newRot.y = 90;
        }
        transform.localEulerAngles = newRot;*/

        if (showLeft == true)
        {
            setAlpha(1, leftGroup);
            setAlpha(0, rightGroup);
        }
        else
        {
            setAlpha(0, leftGroup);
            setAlpha(1, rightGroup);
        }
	}

    /// <summary>
    /// Set the transparency of the water flow.
    /// </summary>
    /// <param name="val">The alpha value to apply.</param>
    /// <param name="group">The parent gameobject of the group.</param>
    private void setAlpha(float val, GameObject group)
    {
        foreach (ParticleSystem system in group.GetComponentsInChildren<ParticleSystem>())
        {
            Color newColor = system.startColor;
            newColor.a = val;
            system.startColor = newColor;
        }
    }
}
