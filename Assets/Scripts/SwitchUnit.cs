using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchUnit : MonoBehaviour {

    public GameObject laser;
    public Material unlockMaterial;
    public GameObject screen;

    private AudioSource myAudio;


	// Use this for initialization
	void Start () {
        myAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == Tags.player) {
            if(Input.GetKeyDown(KeyCode.Z)) {
                if (laser) laser.SetActive(false);
				myAudio.Play();
				screen.GetComponent<Renderer>().material = unlockMaterial;
            }
		}
	}
}
