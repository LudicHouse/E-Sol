using UnityEngine;
using System.Collections;

public class WateringCan : MonoBehaviour {
    public float moveSpeed;
    public float startRotHeight;
    public float endRotHeight;
    public float maxRotation;
    public float rotThreshold;
    public float maxWater;
    public float waterDrainSpeed;
    public float waterRegenSpeed;
    public Plant flower;
    public bool clampX;
    public float minPosX;
    public float maxPosX;
    public float maxPosY;
    public bool forceMinMax;
    public GameObject waterFlowLeft;
    public GameObject waterFlowRight;
    public ManageWaterFlowPosition waterFlowManager;
    public Sprite foregroundActive;
    public Sprite foregroundInactive;
    public Sprite backgroundActive;
    public Sprite backgroundInactive;
    public SpriteRenderer backgroundRenderer;
    public AudioClip waterSound;

    public float water;
    private Vector3 startPos;
    //private int fingerId = -1;
    //private Vector2 fingerStart;
    private bool dragging = false;
    private Vector3 moveTarget;
    private bool hasThanked = false;

    private Collider2D col;
    private TouchManager tMan;
    private SpriteRenderer rend;

	// Use this for initialization
	void Start () {
        if (water == -1)
        {
            water = maxWater;
        }
        startPos = transform.localPosition;
        moveTarget = startPos;
        col = GetComponent<Collider2D>();

        tMan = Object.FindObjectOfType<TouchManager>();
        tMan.onDragStart += onDragStart;
        tMan.onDrag += onDrag;
        tMan.onDragStop += onDragStop;

        rend = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        /*Vector2 move = startPos - (Vector2)transform.localPosition;

        if (fingerId != -1)
        {
            if (Input.touchCount < fingerId + 1)
            {
                fingerId = -1;
            }
            else if (Input.GetTouch(fingerId).phase == TouchPhase.Canceled | Input.GetTouch(fingerId).phase == TouchPhase.Ended)
            {
                fingerId = -1;
            }
            else
            {
                Touch t = Input.GetTouch(fingerId);
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(t.position);

                move = (startPos + (worldPos - fingerStart)) - (Vector2)transform.localPosition;
            }
        }
        else
        {
            foreach (Touch t in Input.touches)
            {
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(t.position);

                if (col == Physics2D.OverlapPoint(worldPos) & t.phase == TouchPhase.Began & Object.FindObjectOfType<GameController>().isPanelOpen() == false)
                {
                    fingerId = t.fingerId;
                    fingerStart = worldPos;
                    break;
                }
            }
        }*/

        Vector3 move = moveTarget - transform.localPosition;
        transform.localPosition += move * moveSpeed * Time.deltaTime;

        if ((startPos.x < 0 & transform.localPosition.x > 0) | (startPos.x > 0 & transform.localPosition.x < 0))
        {
            transform.localScale = new Vector3(-1, 1, 1);

            /*foreach (FlipParticles flip in waterFlowLeft.GetComponentsInChildren<FlipParticles>())
            {
                flip.left = true;
            }

            foreach (FlipParticles flip in waterFlowRight.GetComponentsInChildren<FlipParticles>())
            {
                flip.left = true;
            }*/

            waterFlowManager.showLeft = true;
        }
        else
        {
            transform.localScale = Vector3.one;

            /*foreach (FlipParticles flip in waterFlowLeft.GetComponentsInChildren<FlipParticles>())
            {
                flip.left = false;
            }

            foreach (FlipParticles flip in waterFlowRight.GetComponentsInChildren<FlipParticles>())
            {
                flip.left = false;
            }*/

            waterFlowManager.showLeft = false;
        }

        float height = transform.localPosition.y - startPos.y;
        if (height < startRotHeight)
        {
            transform.localEulerAngles = Vector3.zero;
        }
        else if (height > endRotHeight)
        {
            transform.localEulerAngles = new Vector3(0, 0, maxRotation * transform.localScale.x);
        }
        else
        {
            float rot = ((height - startRotHeight) / (endRotHeight - startRotHeight)) * maxRotation * transform.localScale.x;
            transform.localEulerAngles = new Vector3(0, 0, rot);
        }

        float currentRot = transform.localEulerAngles.z;
        //Debug.Log(currentRot);
        if (currentRot > 180)
        {
            currentRot -= 360;
        }
        if (Mathf.Abs(currentRot) > Mathf.Abs(rotThreshold))
        {
            float drainAmount = Mathf.Min(water, waterDrainSpeed * Time.deltaTime);
            water -= drainAmount;
            flower.plantHydration += drainAmount;

            if (hasThanked == false & water > 0)
            {
                GameObject.FindObjectOfType<SpeechBubble>().show(Country.Message.ThankYou);
                GameObject.FindObjectOfType<TutorialManager>().setCanDone();
                GameObject.FindObjectOfType<MusicController>().playSoundEffect(waterSound);
                hasThanked = true;
            }

            if (water > 0)
            {
                //Debug.Log("Should flow");
                setWaterFlow(true);
            }
            else
            {
                //Debug.Log("No water");
                setWaterFlow(false);
            }
        }
        else
        {
            water = Mathf.Min(maxWater, water + waterRegenSpeed * Time.deltaTime);
            hasThanked = false;

            setWaterFlow(false);
        }

        if (forceMinMax == true & dragging == false & water <= 0)
        {
            moveTarget = startPos;
        }

        if (forceMinMax == true & water < maxWater & dragging == false)
        {
            rend.sprite = foregroundInactive;
            backgroundRenderer.sprite = backgroundInactive;
        }
        else
        {
            rend.sprite = foregroundActive;
            backgroundRenderer.sprite = backgroundActive;
        }
	}

    /// <summary>
    /// Called when the player begins a drag touch event.
    /// </summary>
    /// <param name="startPos">The starting position of the drag.</param>
    private void onDragStart(Vector2 startPos)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(startPos);

        if (col == Physics2D.OverlapPoint(worldPos) & Object.FindObjectOfType<GameController>().isPanelOpen() == false)
        {
            if (forceMinMax == false | water >= maxWater)
            {
                dragging = true;
                moveTarget = transform.localPosition;
            }
        }
    }

    /// <summary>
    /// Called as part of a drag touch event.
    /// </summary>
    /// <param name="delta">The amount the finger has moved since the last event.</param>
    private void onDrag(Vector2 delta)
    {
        if (dragging == true)
        {
            Vector2 diff = Camera.main.ScreenToWorldPoint(delta) - Camera.main.ScreenToWorldPoint(Vector2.zero);

            if (forceMinMax == true)
            {
                float heightThreshold = ((rotThreshold / maxRotation) * (endRotHeight - startRotHeight)) + startRotHeight + startPos.y;
                if (moveTarget.y >= heightThreshold)
                {
                    moveTarget += (Vector3)diff;
                    moveTarget.y = Mathf.Max(moveTarget.y, heightThreshold);
                }
                else
                {
                    moveTarget += (Vector3)diff;
                }
            }
            else
            {
                moveTarget += (Vector3)diff;
            }

            if (clampX == true)
            {
                moveTarget.x = startPos.x;
            }

            moveTarget.x = Mathf.Clamp(moveTarget.x, minPosX, maxPosX);
            moveTarget.y = Mathf.Clamp(moveTarget.y, startPos.y, maxPosY);
        }
    }

    /// <summary>
    /// Called when the player finished a drag touch event.
    /// </summary>
    private void onDragStop()
    {
        dragging = false;

        if (forceMinMax == true)
        {
            float heightThreshold = ((rotThreshold / maxRotation) * (endRotHeight - startRotHeight)) + startRotHeight + startPos.y;
            if (moveTarget.y < heightThreshold)
            {
                moveTarget = startPos;
            }
        }
        else
        {
            moveTarget = startPos;
        }
    }

    /// <summary>
    /// Enable or disable the water flow animation.
    /// </summary>
    /// <param name="val">Whether the water flow should be on or off.</param>
    private void setWaterFlow(bool val)
    {
        //Debug.Log("Setting flow to " + val);
        foreach (ParticleSystem p in waterFlowLeft.GetComponentsInChildren<ParticleSystem>())
        {
            //Debug.Log("Psystem found");
            if (val == true & p.enableEmission == false)
            {
                p.enableEmission = true;
                p.Play();
            }
            else
            {
                p.enableEmission = val;
            }
        }

        foreach (ParticleSystem p in waterFlowRight.GetComponentsInChildren<ParticleSystem>())
        {
            //Debug.Log("Psystem found");
            if (val == true & p.enableEmission == false)
            {
                p.enableEmission = true;
                p.Play();
            }
            else
            {
                p.enableEmission = val;
            }
        }
    }
}
