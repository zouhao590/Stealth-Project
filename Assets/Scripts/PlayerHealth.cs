using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

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
        hp -= damage;
        if(hp <= 0) {
            anim.SetBool("dead", true);
            StartCoroutine(ReloadScene());
        }
    }

    public bool isAlive() {
        return hp > 0;
    }

    IEnumerator ReloadScene() {
        yield return new WaitForSeconds(2f);
        //Application.LoadLevel(0);
        SceneManager.LoadScene("MainScene",LoadSceneMode.Single);
    }

}
