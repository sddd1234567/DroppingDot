using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour {
    private AudioSource ad;
    // Use this for initialization
    void Awake() {
        ad = GetComponent<AudioSource>();
        ad.Play();
        DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        if (!ad.isPlaying)
            Destroy(gameObject);
	}
    
}
