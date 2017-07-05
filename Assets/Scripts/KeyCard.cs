using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard : MonoBehaviour {

    public AudioClip musicPickup;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == Tags.player)
		{
            Player player = other.GetComponent<Player>();
            if(player != null) {
                // 使用外部播放声音，否则对象销毁后声音立即停止
                AudioSource.PlayClipAtPoint(musicPickup, transform.position);
                player.PickupKey();
                Destroy(this.gameObject);
            }
		}
	}
}
