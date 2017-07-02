using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotMoveAI : MonoBehaviour {


    public Transform[] wayPoints;
    private PlayerHealth playerHealth;
    private int index;

    // 巡逻休息时间
    private float restTime = 2f;
    private float restTimer = 0;
    // 警报解除时间
    private float chaseTime = 4f;
    private float chaseTimer = 0;

    private NavMeshAgent navAgent;
    private RobotSight robotSight;
	// Use this for initialization
	void Start () {
        navAgent = GetComponent<NavMeshAgent>();
        robotSight = GetComponent<RobotSight>();
        if(wayPoints != null) {
            // nav只管方向，不控制运动
            navAgent.destination = wayPoints[Mathf.Min(index, wayPoints.Length - 1)].position;
            navAgent.updatePosition = false;
            navAgent.updateRotation = false;
        }
        playerHealth = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerHealth>();
	}
	
	// Update is called once per frame
	void Update () {
        if(robotSight.bPlayerInsight && playerHealth.isAlive()) {
            //在视野内，射击
            Shootting();
        }else if(robotSight.alertPosition != Vector3.zero && playerHealth.isAlive()) {
            //追捕
            Chasing();
        }else {
            //巡逻
           Patrolling(); 
        }
	}

    //巡逻
    private void Patrolling() {
        navAgent.speed = 2;
        //如果nav已经到达目的地了，robot可能还差一点，这里忽略，因为后面的补偿机制距离不超过0.5
        if(navAgent.remainingDistance <= 0.01) {
            //开始计时休息，达到时限后重新出发
			restTimer += Time.deltaTime;
            if(restTimer >= restTime) {
                index++;
                index %= wayPoints.Length;
                navAgent.destination = wayPoints[index].position;
                navAgent.isStopped = false;
				navAgent.updatePosition = false;
				navAgent.updateRotation = false;
                restTimer = 0;
            }
        }else {
            //正在巡逻，nav可能比robot实际走得快，同步nav与robot的位置
            if((navAgent.nextPosition - transform.position).magnitude > 0.5){
                navAgent.nextPosition = transform.position;
            }
        }
    }

    private void Chasing() {
        navAgent.destination = robotSight.alertPosition;
        navAgent.isStopped = false;
		navAgent.updatePosition = false;
		navAgent.updateRotation = false;
        navAgent.speed = 5;

		// 这里也需要同步nav与robot位置
		if ((navAgent.nextPosition - transform.position).magnitude > 0.5) {
			navAgent.nextPosition = transform.position;
		}

        // 如果到达距离后，没有发现player，计时解除警报
        if (navAgent.remainingDistance < 1f) {
			chaseTimer += Time.deltaTime;
			if (chaseTimer > chaseTime) {
				GameController._instance.LostPlayer();
				chaseTimer = 0;
			}
        }
    }

    private void Shootting() {
        navAgent.isStopped = true;
    }
}
