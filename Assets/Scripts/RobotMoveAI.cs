using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotMoveAI : MonoBehaviour {

    private const float MIN_DAMAGE = 100;

	//警报管理器，获取警报信息
	private AlarmMangaer alarmManager;
    private PlayerHealth playerHealth;
	
    // 巡逻点
    public Transform[] wayPoints;
    private int index = 0;

    // 巡逻休息时间
    private float restTime = 2f;
    private float restTimer = 0;
    // 警报解除时间
    private float chaseTime = 4f;
    private float chaseTimer = 0;

    private NavMeshAgent navAgent;
    private RobotSight robotSight;
	private Animator anim;


	// Use this for initialization
	void Start () {
        alarmManager = GameController.GetAlarmMangaer();
        anim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        robotSight = GetComponent<RobotSight>();
        if(wayPoints != null) {
            // nav只管方向，不控制运动
            SetNavDestination(wayPoints[Mathf.Min(index, wayPoints.Length - 1)].position);
        }
        playerHealth = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerHealth>();
	}
	
	// Update is called once per frame
	void Update () {
        if(robotSight.IsPlayerInsight() && playerHealth.isAlive()) {
            //在视野内，射击
            Shootting();
        }else if(IsAlarmOn() && GetPlayerLastPostion() != Vector3.zero && playerHealth.isAlive()) {
            //全局警报，追捕警报点
            Chasing(GetPlayerLastPostion());
        }else if(robotSight.GetAlertPosition() != Vector3.zero && playerHealth.isAlive()) {
            //听到声音，追捕
            Chasing(robotSight.GetAlertPosition());
        }else {
            //巡逻
           Patrolling(); 
        }
	}

    //射击优化
	private void Shootting() {
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(1);
		if (info.IsName("WeaponShoot") && info.normalizedTime >= 1) {
			//计算伤害，距离越近伤害越大
			float damage = MIN_DAMAGE + 80 - 8 * (transform.position - playerHealth.transform.position).magnitude;
			damage = Mathf.Max(MIN_DAMAGE, damage);
			playerHealth.TakeDamage(damage);
			//hasShoot = true;
		}else {
			//hasShoot = false;
		}

		navAgent.isStopped = true;
	}

	private void Chasing(Vector3 position) {
		SetNavDestination(position);
		navAgent.speed = 5;
		
		// 这里也需要同步nav与robot位置
		if ((navAgent.nextPosition - transform.position).magnitude > 0.5) {
			navAgent.nextPosition = transform.position;
		}
		
		// 如果到达距离后，没有发现player，计时解除警报
		if (navAgent.remainingDistance < 1f) {
			chaseTimer += Time.deltaTime;
			if (chaseTimer > chaseTime) {
				GameController.Instance.LostPlayer();
                robotSight.HandleNoPlayer();
				chaseTimer = 0;
			}
		}
	}

    //巡逻
    private void Patrolling() {
        navAgent.speed = 2;
        navAgent.isStopped = false;
        //如果目的地不在巡逻点，说明可能是从追捕状态切换回来的，重新设置巡逻点
        if(navAgent.destination != wayPoints[index].position) {
            SetNavDestination(wayPoints[index].position);
        }

        //如果nav已经到达目的地了，robot可能还差一点，这里忽略，因为后面的补偿机制距离不超过0.5
        if(navAgent.remainingDistance <= 0.01) {
            //开始计时休息，达到时限后重新出发
            restTimer += Time.deltaTime;
            if(restTimer >= restTime) {
                index++;
                index %= wayPoints.Length;
                SetNavDestination(wayPoints[index].position);
                restTimer = 0;
            }
        }else {
            //正在巡逻，nav可能比robot实际走得快，同步nav与robot的位置
            if((navAgent.nextPosition - transform.position).magnitude > 0.5){
                navAgent.nextPosition = transform.position;
            }
        }
    }

    private void SetNavDestination(Vector3 pos) {
        navAgent.destination = pos;
        navAgent.isStopped = false;
        navAgent.updatePosition = false;
        navAgent.updateRotation = false;
    }

	//从全局控制器获取警报信息
	private bool IsAlarmOn() {
		if (alarmManager == null) {
			alarmManager = GameController.GetAlarmMangaer();
		}
		return alarmManager == null ? false : alarmManager.IsAlarmOn();
	}

    //获取玩家位置
    private Vector3 GetPlayerLastPostion() {
		if (alarmManager == null) {
			alarmManager = GameController.GetAlarmMangaer();
		}
        return alarmManager == null ? Vector3.zero : alarmManager.LastPlayerPosition();
	}
}
