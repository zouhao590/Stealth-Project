using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    
    // 是否需要钥匙开门
    public bool needKey = false;
	
    private Animator myAnimator;
    // 外部赋值
	public AudioSource myOpenAudio;
	public AudioSource myDenidAudio;

    private void Awake() {
        myAnimator = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // 在开关门动画切换的时候播放，不需要手动stop，否自播一帧就没了
        if (myAnimator.IsInTransition(0)) {
            if (myOpenAudio && !myOpenAudio.isPlaying) {
                myOpenAudio.Play();
            }
        }
	}

	void OnTriggerEnter(Collider other) {

        // 需要钥匙的时候检查是否是player，且是否有钥匙
        if(needKey) {
            if(other.tag == Tags.player) {
				Player player = other.GetComponent<Player>();
                if (player == null || !player.HasKey()) {
					if (myDenidAudio != null) myDenidAudio.Play();
					return;
				}
            }
        }

        // 不需要钥匙的时候直接开门
        if (other.tag == Tags.player) {
            myAnimator.SetBool("bClose", false);
        }else if(other.tag == Tags.enemy && !other.isTrigger) {
            //机器人的脚步声探测collider不算
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
