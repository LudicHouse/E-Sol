using UnityEngine;
using System.Collections;

public class BackgroundCameraController : MonoBehaviour {
    public SpriteRenderer background;
    public GameObject cloudContainer;
    public Camera mainCamera;

    private Camera thisCamera;
    private float lowerLimit;
    private float upperLimit;

    private float mainCameraStart;
    private float mainCameraLastY;

	// Use this for initialization
	void Start () {
        thisCamera = GetComponent<Camera>();
        float ratio = (float)Screen.width / (float)Screen.height;
        thisCamera.orthographicSize = (background.bounds.size.x / 2) / ratio;
        //thisCamera.orthographicSize = mainCamera.orthographicSize;

        lowerLimit = background.bounds.min.y + thisCamera.orthographicSize; //TODO: Make inclusive rather than exclusive
        upperLimit = background.bounds.max.y - thisCamera.orthographicSize;
        Vector3 newPos = transform.position;
        newPos.y = lowerLimit;
        transform.position = newPos;

        mainCameraStart = getCameraLowerBound();
        mainCameraLastY = getCameraLowerBound();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        float diff = getCameraLowerBound() - mainCameraStart;
        float scaleMultiplier = mainCamera.orthographicSize / thisCamera.orthographicSize;

        Vector3 newPos = transform.position;
        newPos.y = Mathf.Clamp(lowerLimit + (diff / scaleMultiplier), lowerLimit, upperLimit); //TODO: This doesn't quite match the movement of the main camera, look into this later
        transform.position = newPos;

        //Debug.Log(diff + " " + lowerLimit + " " + (lowerLimit + (diff / scaleMultiplier)) + " " + newPos.y);

        //Debug.Log((lowerLimit + diff) + " " + newPos.y + " " + (lowerLimit + diff == newPos.y));
        if ((int)((lowerLimit + (diff / scaleMultiplier)) * 100) != (int)(newPos.y * 100))
        {
            float change = getCameraLowerBound() - mainCameraLastY;
            //Debug.Log(change);

            foreach (Transform cloud in cloudContainer.transform)
            {
                Vector2 newCloudPos = cloud.position;
                newCloudPos.y -= change / scaleMultiplier;
                cloud.position = newCloudPos;
            }
        }

        mainCameraLastY = getCameraLowerBound();
	}

    /// <summary>
    /// Get the lower bound of the camera.
    /// </summary>
    /// <returns>The world-space Y value of the camera's lower bound.</returns>
    private float getCameraLowerBound()
    {
        return mainCamera.transform.position.y - mainCamera.orthographicSize;
    }
}
