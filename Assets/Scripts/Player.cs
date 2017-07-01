using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //启动加速度,减速度
    private float moveSpeed = 4;
    private float stopSpeed = 10;
    private float rotateSpeed = 6;

    private Animator animator;

    //是否有开门钥匙
    public bool hasKey = false;

    private void Awake() {
        animator = this.GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 控制移动和方向
        if(Mathf.Abs(h) > 0.1 || Mathf.Abs(v) > 0.1) {
            float newSpeed = Mathf.Lerp(animator.GetFloat("walkSpeed"), 5, moveSpeed * Time.deltaTime);
            animator.SetFloat("walkSpeed", newSpeed);
            Debug.Log("go, speed:" + animator.GetFloat("walkSpeed"));

            // 旋转到指定角度
            Vector3 targetDir = new Vector3(h, 0, v);

            // 直接把朝向改成目标方向，出现闪转效果不好
            //transform.forward = targetDir;

            //创建一个朝向targetDir的旋转，并调用差值逐渐旋转，内部会计算最小角度
            Quaternion newRotation = Quaternion.LookRotation(targetDir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotateSpeed);

            //这样实现转向也可以
            //transform.forward = Vector3.Lerp(transform.forward, targetDir, Time.deltaTime * 50);

        }else {
            //太小了就相当于停止了，不需要再做计算
            if(animator.GetFloat("walkSpeed") >= 0.01) {
				float newSpeed = Mathf.Lerp(animator.GetFloat("walkSpeed"), 0, stopSpeed * Time.deltaTime);
				animator.SetFloat("walkSpeed", newSpeed);
				Debug.Log("stop, speed:" + animator.GetFloat("walkSpeed"));
            }
        }

        // 控制是否潜行
        if(Input.GetKey((KeyCode.LeftShift))) {
            animator.SetBool("sneak", true);
        }else {
            animator.SetBool("sneak", false);
        }

        // 控制脚步声
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) {
            PlayFootMusic();
        }else {
            StopFootMusic();
        }
	}

    private void PlayFootMusic() {
        AudioSource audioSource = GetComponent<AudioSource>();
        if(!audioSource.isPlaying) {
            audioSource.Play();
        }
    }

	private void StopFootMusic()
	{
		AudioSource audioSource = GetComponent<AudioSource>();
		if (audioSource.isPlaying)
		{
			audioSource.Stop();
		}
	}
}
