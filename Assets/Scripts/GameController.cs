using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public bool alarmOn = false;
    private GameObject[] sirens;//警报器

    private void Awake() {
        alarmOn = false;
    }

    // Use this for initialization
    void Start () {
        sirens = GameObject.FindGameObjectsWithTag("Siren");
	}
	
	// Update is called once per frame
	void Update () {
        //同步给警报灯
        AlarmLight._instance.alarmOn = this.alarmOn;

        if(alarmOn) {
            PlaySiren();
        }else {
            StopSiren();
        }
	}

    private void PlaySiren() {
        if (sirens == null) return;
        foreach(GameObject siren in sirens) {
            AudioSource audioSource = siren.GetComponent<AudioSource>();
            if(!audioSource.isPlaying) { //TODO:为啥不能直接siren.audio
                audioSource.Play();
            }
        }
    }

    private void StopSiren() {
		foreach (GameObject siren in sirens)
		{
			AudioSource audioSource = siren.GetComponent<AudioSource>();
            audioSource.Stop();
		}
    }
}
