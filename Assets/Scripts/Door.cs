using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    private Animator myAnimator;
    public AudioSource myOpenAudio;
    public AudioSource myDenidAudio;

    // 是否需要钥匙开门
    public bool needKey = false;

    private void Awake() {
        myAnimator = GetComponent<Animator>();
        //myOpenAudio = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // 在开关门动画切换的时候播放，不需要手动stop，否自播一帧就没了
        if (myAnimator.IsInTransition(0)) {
            if (!myOpenAudio.isPlaying)
            {
                myOpenAudio.Play();
            }
        }
	}

	void OnTriggerEnter(Collider other) {

        // 需要钥匙的时候检查是否是player，且是否有钥匙
        if(needKey) {
            Player player = other.GetComponent<Player>();
            if(player == null || !player.hasKey) {
                if (myDenidAudio != null) myDenidAudio.Play();
                return;
            }
        }

        // 不需要钥匙的时候直接开门
        if (other.tag == Tags.player || other.tag == Tags.enemy) {
            myAnimator.SetBool("bClose", false);
		}
	}

	void OnTriggerExit(Collider other)
	{
        // 关门不管有没有钥匙，离开就关
		if (other.tag == Tags.player || other.tag == Tags.enemy) {
            myAnimator.SetBool("bClose", true);
		}
	}
}
