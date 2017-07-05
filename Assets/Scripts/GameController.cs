﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface AlarmReceiver {
    void SeePlayer(Transform player);
    void LostPlayer();
}

public interface AlarmMangaer {
    bool IsAlarmOn();
    Vector3 LastPlayerPosition();
}

public class GameController : MonoBehaviour,AlarmReceiver,AlarmMangaer {

    private static GameController _instance;

    //警报信息
    public bool alarmOn = false;
    public Vector3 lastPlayerPosition = Vector3.zero;

    //两个背景音乐
    public AudioSource musicNormal;
    public AudioSource musicPanic;
    //音乐变换速度
    private const float musicChangeSpeed = 10f;

	public static GameController Instance {
		get {
			return _instance;
		}
	}

    private void Awake() {
		_instance = this;
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if(alarmOn) {
            //改变背景音乐
            ChangeToPanicMusic();
        }else {
            ChangeToNormalMusic();
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

	
	public static AlarmReceiver GetAlarmReceiver() {
		return Instance;
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


	public static AlarmMangaer GetAlarmMangaer() {
		return Instance;
	}

    public bool IsAlarmOn() {
        return alarmOn;
    }

    public Vector3 LastPlayerPosition() {
        return lastPlayerPosition;
    }


    //电梯到达回调
    public void HandleLiftArrived() {
        PlayerHealth health = GameObject.FindWithTag(Tags.player).GetComponent<PlayerHealth>();
        if(health && health.isAlive()) {
			Debug.Log("游戏胜利！");
        }
    }

    //角色死亡回调
    public void HandlePlayerDead() {
        Debug.Log("Game Over！");
        //StartCoroutine(ReloadScene());
    }

	private IEnumerator ReloadScene() {
		yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
	}
}
