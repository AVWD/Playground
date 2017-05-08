using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour {

    AudioSource Audio;
    [Header("Audio")]
    public AudioClip SndShot;
    public AudioClip SndLoop;
    public AudioClip SndShot2;
    public AudioClip SndSprint;

    public static GameManager instance { get; set; }

    // Use this for initialization
    void Start () {
        // not truly singleton
        instance = this;

        Audio = GetComponent<AudioSource>();
        Audio.loop = true;
        Audio.clip = SndLoop;
        Audio.Play();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void shoot()
    {
        Audio.PlayOneShot(SndShot, 1);
    }

    public void sprint()
    {
        Audio.PlayOneShot(SndSprint, 1);
    }

    public void enemyShot()
    {
        Audio.PlayOneShot(SndShot2, 1);
    }
}
