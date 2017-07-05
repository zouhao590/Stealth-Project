using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotSight : MonoBehaviour {
    
	// 视线可视角度
	private const float fieldOfView = 120;

    // 视线范围内，可以射击
    public bool bPlayerInsight = false;
    // 要追踪player的位置(可能是看到或听到)
    public Vector3 alertPosition = Vector3.zero;
    // 全局发现的player位置
    public Vector3 lastPlayerPosition;

    private NavMeshAgent navAgent;
    private SphereCollider sphereCollider;

	// Use this for initialization
	void Start () {
        navAgent = GetComponent<NavMeshAgent>();
        sphereCollider = GetComponent<SphereCollider>();
	}
	
	// Update is called once per frame
	void Update () {

        // only called once
        if(lastPlayerPosition == null) {
            lastPlayerPosition = GameController.Instance.lastPlayerPosition;
        }

        if(GameController.Instance.lastPlayerPosition != lastPlayerPosition) {
            lastPlayerPosition = GameController.Instance.lastPlayerPosition;
            alertPosition = lastPlayerPosition;
        }

	}

    private void OnTriggerStay(Collider other) {
		if (other.tag != Tags.player) {
            return;
		}
        Player player = other.GetComponent<Player>();
        if (!player) return;

        // 如果玩家已经死了，就不管了
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (!health || !health.isAlive()) {
            GameController.Instance.LostPlayer();
            return;
        }

		//先检测是否能看到player
        //两个条件：1.发射射线能射到玩家  2.在眼睛可视角范围内
		RaycastHit hitInfo;
        bool bCasted = Physics.Raycast(transform.position + Vector3.up, other.transform.position - transform.position, out hitInfo);
        float angle = Vector3.Angle(transform.forward, other.transform.position - transform.position);

        if ((bCasted && hitInfo.collider.tag == Tags.player) &&  angle <= fieldOfView / 2f) {
            bPlayerInsight = true;
            alertPosition = other.transform.position;
            //拉起全局警报
            GameController.Instance.SeePlayer(other.transform);
			//能看到就直接返回了
			return;
        }else {
            bPlayerInsight = false;
        }


        //检测能否听到脚步声，考虑到有墙的情况，根据最短导航路径判长度断是否能听到
        //首先要player发出脚步声
        if (player.IsStepMusicPlaying()) {
            //然后计算最短导航路径
            NavMeshPath path = new NavMeshPath();
            if (navAgent.CalculatePath(other.transform.position, path))
            {
                //路径上所有点，包括robot和player
                Vector3[] wayPoint = new Vector3[path.corners.Length + 2];
                wayPoint[0] = transform.position;
                wayPoint[wayPoint.Length - 1] = other.transform.position;
                for (int i = 0; i < path.corners.Length; i++)
                {
                    wayPoint[i + 1] = path.corners[i];
                }
                //计算所有点连线长度
                float length = 0;
                for (int i = 1; i < wayPoint.Length; i++)
                {
                    length += (wayPoint[i] - wayPoint[i - 1]).magnitude;
                }
                //如果距离小于最大距离，认为可以听到脚步声
                //此处只是听到，自己追踪，不拉起全局警报
                alertPosition = length <= sphereCollider.radius ? other.transform.position : alertPosition;
            }
        }
	}

    private void OnTriggerExit(Collider other) {
        if (other.tag == Tags.player) {
            bPlayerInsight = false;
        }
    }

    public bool IsPlayerInsight() {
        return bPlayerInsight;    
    }

}
