using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {
    public float happyThreshold;
    public float veryHappyThreshold;

    private Animator anim;
    private AudioSource source;

    private GameController gc;
    private Plant p;

	// Use this for initialization
	void Start () {
        if (Object.FindObjectsOfType<MusicController>().Length > 1)
        {
            Object.Destroy(this.gameObject);
        }
        else
        {
            Object.DontDestroyOnLoad(this.gameObject);
            anim = GetComponent<Animator>();
            source = GetComponent<AudioSource>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Application.loadedLevelName == "Main" && gc.isCreditsPanelOpen() == false)
        {
            anim.SetBool("usemood", true);

            if (p.getMood() >= veryHappyThreshold)
            {
                anim.SetInteger("mood", 1);
            }
            else if (p.getMood() >= happyThreshold)
            {
                anim.SetInteger("mood", 0);
            }
            else
            {
                anim.SetInteger("mood", -1);
            }

            anim.SetBool("pollution", p.isPollution());
        }
        else
        {
            anim.SetBool("usemood", false);
        }
	}

    void OnLevelWasLoaded(int level)
    {
        if (Application.loadedLevelName == "Main")
        {
            gc = Object.FindObjectOfType<GameController>();
            p = Object.FindObjectOfType<Plant>();
        }
    }

    /// <summary>
    /// Play the specified sound effect.
    /// </summary>
    /// <param name="clip">The audio clip to play.</param>
    public void playSoundEffect(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}
