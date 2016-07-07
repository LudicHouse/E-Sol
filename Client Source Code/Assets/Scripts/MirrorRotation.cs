using UnityEngine;
using System.Collections;

public class MirrorRotation : MonoBehaviour {
    public Transform toMirror;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        int reflect = 1;
        if (getDirection(transform) != getDirection(toMirror))
        {
            reflect = -1;
        }

        transform.localEulerAngles = new Vector3(0, 0, toMirror.localEulerAngles.z * reflect);
	}

    /// <summary>
    /// Get the direction of the x scaling of the target.
    /// </summary>
    /// <param name="target">The object to check.</param>
    /// <returns>1 if the local scale of x is greater than 0, -1 otherwise.</returns>
    private int getDirection(Transform target)
    {
        if (target.localScale.x > 0)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
}
