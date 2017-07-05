using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotAnimation : MonoBehaviour {

    private NavMeshAgent navAgent;
    private Animator anim;
    private RobotSight robotSight;
    private PlayerHealth playerHealth;

	// Use this for initialization
	void Start () {
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        robotSight = GetComponent<RobotSight>();
        playerHealth = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerHealth>();
	}
	
	// Update is called once per frame
	void Update () {
        //如果导航到了，设置速度为0，停止动画
        if(navAgent.desiredVelocity == Vector3.zero) {
            anim.SetFloat("walkSpeed", 0);
            anim.SetFloat("anglarSpeed", 0);
        }else {
			//desiredVelocity包含了目标方向和速度
			float angle = Vector3.Angle(transform.forward, navAgent.desiredVelocity);
            if(angle > 90) {
                anim.SetFloat("walkSpeed", 0);
            }else {
				//速度取值，取目标向量与当前向量的垂直投影，当两个向量平行时速度最大，同时与目标向量的大小有关
				Vector3 projection = Vector3.Project(navAgent.desiredVelocity, transform.forward);
                anim.SetFloat("walkSpeed", projection.magnitude);
            }

			float angleRad = angle * Mathf.Deg2Rad;
			// 判断目标在左边还是右边
			// a、b向量做cross运算，结果向量垂直于两向量平面
			// 根据左右定则，拇指a，食指b，中指res，res向外则b在a右侧，res向内反之
			Vector3 crossRes = Vector3.Cross(transform.forward, navAgent.desiredVelocity);
			if (crossRes.y < 0) {
				angleRad = -angleRad;
			}

            anim.SetFloat("anglarSpeed", angleRad * 10); //提高转速
        }

        //射击动画同步insight变量，活着才算
        anim.SetBool("bPlayerInsight", robotSight.IsPlayerInsight() && playerHealth.isAlive());
	}




}
