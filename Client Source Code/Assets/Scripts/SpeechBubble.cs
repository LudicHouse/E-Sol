using UnityEngine;
using System.Collections;

public class SpeechBubble : MonoBehaviour {
    public float messageDuration;
    public GameObject showHide;
    public Transform bubbleMiddle;
    public Transform bubbleLeft;
    public Transform bubbleRight;
    public Transform bubbleTail;
    public TextMesh text;
    public Transform anchorTo;
    public float minDistFromTop;
    public int textSortingLayerOrder;
    public string textSortingLayer;
    public AudioClip bubbleShowSound;
    public Camera mainCamera;

    private float timer = 0;
    private float tailPos;

	// Use this for initialization
	void Start () {
        show(Country.Message.Hello);
        tailPos = bubbleTail.localPosition.y;
        text.GetComponent<Renderer>().sortingOrder = textSortingLayerOrder;
        text.GetComponent<Renderer>().sortingLayerName = textSortingLayer;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector2 newPos = anchorTo.position;
        //Debug.Log(getDistFromTop(newPos));
        if (getDistFromTop(newPos) < minDistFromTop)
        {
            newPos.y = mainCamera.transform.position.y + mainCamera.orthographicSize - minDistFromTop;
        }
        transform.position = newPos;

        if (bubbleMiddle.position.y < anchorTo.position.y)
        {
            Vector3 newMiddle = bubbleMiddle.localScale;
            newMiddle.y = -1;
            bubbleMiddle.localScale = newMiddle;

            Vector3 newLeft = bubbleLeft.localScale;
            newLeft.y = -1;
            bubbleLeft.localScale = newLeft;

            Vector3 newRight = bubbleRight.localScale;
            newRight.y = -1;
            bubbleRight.localScale = newRight;

            Vector3 newTailScale = bubbleTail.localScale;
            newTailScale.y = -1;
            bubbleTail.localScale = newTailScale;
            Vector3 newTailPos = bubbleTail.localPosition;
            newTailPos.y = -tailPos;
            bubbleTail.localPosition = newTailPos;
        }
        else
        {
            Vector3 newMiddle = bubbleMiddle.localScale;
            newMiddle.y = 1;
            bubbleMiddle.localScale = newMiddle;

            Vector3 newLeft = bubbleLeft.localScale;
            newLeft.y = 1;
            bubbleLeft.localScale = newLeft;

            Vector3 newRight = bubbleRight.localScale;
            newRight.y = 1;
            bubbleRight.localScale = newRight;

            Vector3 newTailScale = bubbleTail.localScale;
            newTailScale.y = 1;
            bubbleTail.localScale = newTailScale;
            Vector3 newTailPos = bubbleTail.localPosition;
            newTailPos.y = tailPos;
            bubbleTail.localPosition = newTailPos;
        }

        if (showHide.activeSelf == true)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                showHide.SetActive(false);
            }
        }
	}

    /// <summary>
    /// Show the speech bubble.
    /// </summary>
    /// <param name="message">The message code to display.</param>
    public void show(string message)
    {
        show(Object.FindObjectOfType<GameController>().plantRegion, Object.FindObjectOfType<GameController>().plantLanguage, message, messageDuration);
    }

    /// <summary>
    /// Show the speech bubble.
    /// </summary>
    /// <param name="language">The country code to display the message in.</param>
    /// <param name="message">The message code to display.</param>
    /// <param name="duration">The duration to display the message, in seconds.</param>
    private void show(string region, int language, string message, float duration)
    {
        text.text = Country.getTranslation(region, language, message);
        showHide.SetActive(true);
        timer = duration;
        setBubbleWidth();
        Object.FindObjectOfType<MusicController>().playSoundEffect(bubbleShowSound);
    }

    /// <summary>
    /// Display raw text in the speech bubble (useful for debugging, otherwise should use translations).
    /// </summary>
    /// <param name="message">The string to display.</param>
    /// <param name="duration">The duration to display the message, in seconds.</param>
    public void showRaw(string message, float duration)
    {
        text.text = message;
        showHide.SetActive(true);
        timer = duration;
        setBubbleWidth();
        Object.FindObjectOfType<MusicController>().playSoundEffect(bubbleShowSound);
    }

    /// <summary>
    /// Set the width of the speech bubblle to match the current message.
    /// </summary>
    private void setBubbleWidth()
    {
        float width = text.GetComponent<Renderer>().bounds.size.x;
        float scale = width / bubbleMiddle.GetComponent<Renderer>().bounds.size.x;
        bubbleMiddle.localScale = new Vector3(bubbleMiddle.transform.localScale.x * scale, 1, 1);

        bubbleLeft.localPosition = new Vector3(-(width / 2) - bubbleLeft.GetComponent<Renderer>().bounds.extents.x, 0, 0);
        bubbleRight.localPosition = new Vector3((width / 2) + bubbleRight.GetComponent<Renderer>().bounds.extents.x, 0, 0);
        bubbleTail.localPosition = new Vector3(-(width / 2) + bubbleTail.GetComponent<Renderer>().bounds.extents.x, bubbleTail.localPosition.y, 0);

        if (bubbleTail.position.x < 0)
        {
            bubbleTail.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            bubbleTail.localScale = Vector3.one;
        }
    }

    /// <summary>
    /// Get the distance of the parameter from the top of the screen.
    /// </summary>
    /// <param name="position">Position to check.</param>
    /// <returns>The world-space distance from the top of the main camera.</returns>
    private float getDistFromTop(Vector2 position)
    {
        float top = mainCamera.transform.position.y + mainCamera.orthographicSize;
        return top - position.y;
    }
}
