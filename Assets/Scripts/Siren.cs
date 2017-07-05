using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Siren : MonoBehaviour {

    AlarmMangaer alarmManager;
    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        alarmManager = GameController.GetAlarmMangaer();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if(IsAlarmOn()) {
            PlaySiren();
        }else {
            StopSiren();
        }
	}
	
	//从全局控制器获取警报信息
	private bool IsAlarmOn() {
		if (alarmManager == null) {
			alarmManager = GameController.GetAlarmMangaer();
		}
		return alarmManager == null ? false : alarmManager.IsAlarmOn();
	}

    private void PlaySiren() {
        if (!audioSource.isPlaying) {
            audioSource.Play();
        }
    }

    private void StopSiren() {
        if (audioSource.isPlaying) {
            audioSource.Stop();
        }
    }
}
