using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTVCam : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void OntriggerStay(Collider other) {
		// only foucs on player
		if (other.tag == "Player") {
			GameController._instance.alarmOn = true;
			GameController._instance.lastPosition = other.transform.position;
		}
	}
}
