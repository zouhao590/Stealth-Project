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
		
	void OnTriggerStay(Collider other)
	{
		if (other.tag == Tags.player)
		{
			GameController._instance.SeePlayer(other.transform);
		}
	}
}
