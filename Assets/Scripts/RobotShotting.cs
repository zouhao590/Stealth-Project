using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotShotting : MonoBehaviour {

    private const float MIN_DAMAGE = 30;

    private Animator anim;
    private bool hasShoot = false;
    private PlayerHealth playerHealth;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
        playerHealth = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerHealth>();
	}
	
	// Update is called once per frame
	void Update () {
        //shot参数来自于射击动画的内部曲线参数，动画控制器同名参数会与之同步
        if (anim.GetFloat("Shot") > 0.5) {
            Shotting();    
        }else {
            hasShoot = false;
        }
	}

    private void Shotting() {
        if(!hasShoot) {
            //计算伤害，距离越近伤害越大
            float damage = MIN_DAMAGE + 80 - 8 * (transform.position - playerHealth.transform.position).magnitude;
            hasShoot = true;
        }    
    }
}
