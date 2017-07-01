using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public bool isFlicker = false;

    public float onTime = 2;
    public float offTime = 2;
    private float timer = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(isFlicker) {
            timer += Time.deltaTime;
            Renderer myRender = GetComponent<Renderer>();
            if(myRender.enabled) {//is on
                if(timer >= onTime) {
                    timer = 0;
                    myRender.enabled = false;
                }
            }else {
                if (timer >= offTime) {
                    timer = 0;
                    myRender.enabled = true;
                }
            }
        }
	}

    void OnTriggerStay(Collider other) {
        if(other.tag == Tags.player) {
            GameController._instance.SeePlayer(other.transform);
        }
    }
}
