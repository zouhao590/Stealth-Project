﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController _instance;

    public bool alarmOn = false;
    public Vector3 lastPlayerPosition = Vector3.zero;
    private GameObject[] sirens;//警报器

    //两个背景音乐
    public AudioSource musicNormal;
    public AudioSource musicPanic;

    private float musicChangeSpeed = 10f;

    private void Awake() {
		_instance = this;
        alarmOn = false;
    }

    // Use this for initialization
    void Start () {
        sirens = GameObject.FindGameObjectsWithTag(Tags.siren);
	}
	
	// Update is called once per frame
	void Update () {
        //同步给警报灯
        AlarmLight._instance.alarmOn = this.alarmOn;

        if(alarmOn) {
            //改变背景音乐，同时响起警报
            ChangeToPanicMusic();
            PlaySiren();
        }else {
            ChangeToNormalMusic();
            StopSiren();
        }	
    }

    private void ChangeToPanicMusic() {
        if(musicNormal.volume > 0.01) {
            musicNormal.volume = Mathf.Lerp(musicNormal.volume, 0, Time.deltaTime * musicChangeSpeed);
        }
        //panic声音不用太大
        if(musicPanic.volume < 0.49) {
            musicPanic.volume = Mathf.Lerp(musicPanic.volume, 0.5f, Time.deltaTime * musicChangeSpeed);
        }
    }

	private void ChangeToNormalMusic() {
		if (musicNormal.volume < 0.49) {
			musicNormal.volume = Mathf.Lerp(musicNormal.volume, 0.5f, Time.deltaTime * musicChangeSpeed);
		}
		if (musicPanic.volume > 0.01) {
			musicPanic.volume = Mathf.Lerp(musicPanic.volume, 0, Time.deltaTime * musicChangeSpeed);
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

    //when cam or laser or robot find player
    public void SeePlayer(Transform player) {
        alarmOn = true;
        lastPlayerPosition = player.position;
    }


	public void LostPlayer() {
        alarmOn = false;
        lastPlayerPosition = Vector3.zero;
	}
}
