using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

    //todo 调试方便暂时public
    public float hp = 100;
    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void TakeDamage(float damage) {
        hp -= damage < 0 ? 0 : damage;
        if(hp <= 0) {
            anim.SetBool("dead", true);
            GameController.Instance.HandlePlayerDead();
        }
    }

    public bool isAlive() {
        return hp > 0;
    }

}
