using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrackHeight : MonoBehaviour {
    public Transform toTrack;
    public float unitsPerRegion;
    public string unitOfMeasurement;
    public int decimalPlaces;
    public float fixedUnitsPerRegion;
    public string fixedUnitOfMeasurement;
    public int fixedDecimalPlaces;
    public float regionsPerFixed;
    public float fixedOffset;
    public GameObject fixedPrefab;
    public Transform fixedContainer;

    public float nextFixed;

	// Use this for initialization
	void Start () {
        nextFixed = regionsPerFixed;
	}
	
	// Update is called once per frame
    void LateUpdate () {
        Vector2 guiPos = Camera.main.WorldToScreenPoint(toTrack.position);
        Vector2 newPos = transform.position;
        newPos.y = guiPos.y;
        transform.position = newPos;

        float unitHeight = toTrack.position.y * unitsPerRegion;
        GetComponent<Text>().text = unitHeight.ToString("F" + decimalPlaces) + unitOfMeasurement;

        while (toTrack.position.y > nextFixed + fixedOffset)
        {
            GameObject newFixed = Object.Instantiate(fixedPrefab);
            newFixed.transform.SetParent(fixedContainer, false);
            newFixed.GetComponent<FixedHeightTracker>().region = nextFixed;
            newFixed.GetComponent<Text>().text = (nextFixed * fixedUnitsPerRegion).ToString("F" + fixedDecimalPlaces) + fixedUnitOfMeasurement;

            nextFixed += regionsPerFixed;
        }
	}
}
