using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    //相机移动和旋转的速度
    public float moveSpeed = 3;
    public float rotateSpeed = 3;

    // 相机和主角的偏移
    private Vector3 offset;
    private Transform player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        offset = transform.position - player.position;
        //x轴保持一致
        offset = new Vector3(0, offset.y, offset.z);
	}
	
	// Update is called once per frame
	void Update () {

        AjustPosition();

        //this.transform.position = player.position + offset;
	}

    void AjustPosition() {
        //起始点
        Vector3 beginPositon = player.position + offset;
        //plyer正上方
        Vector3 endPosition = player.position + offset.magnitude * Vector3.up;

        //从相机起始位置到player头顶n+1个点中，寻找合适的点作为相机位置
        //默认最斜视角，有物体遮挡是垂直视角
        float n = 5;
        for (int i = 0; i <= n; i++) {
            Vector3 tmpPosition = Vector3.Lerp(beginPositon, endPosition, i/n);
            RaycastHit hitInfo;
            if(Physics.Raycast(tmpPosition, player.position - tmpPosition, out hitInfo)) {
                if(hitInfo.collider.tag == Tags.player) {
                    //有射线碰撞，且第一个碰撞体是player，说明当前位置可用
                    transform.position = Vector3.Lerp(transform.position, tmpPosition, Time.deltaTime * moveSpeed);

                    // 相机视角也缓动
                    Quaternion curRot = transform.rotation;
                    transform.LookAt(player.position);
                    Quaternion.Lerp(curRot, transform.rotation, Time.deltaTime*rotateSpeed);
                    break;
                }
            }
        }
    }
}
