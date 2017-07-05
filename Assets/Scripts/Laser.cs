using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    //是否是闪烁型的
    public bool isFlicker = false;
    //闪烁时长
    private const float onTime = 2;
    private const float offTime = 2;
    private float timer = 0;

    Renderer myRenderer;

	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if(isFlicker) {
            timer += Time.deltaTime;
            if (timer >= offTime) {
                timer = 0;
                myRenderer.enabled = !myRenderer.enabled;
            }

        }
	}

    void OnTriggerStay(Collider other) {
        if(other.tag == Tags.player) {
            GameController.GetAlarmReceiver().SeePlayer(other.transform);
        }
    }
}
