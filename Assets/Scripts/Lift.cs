using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lift : MonoBehaviour {

    // 获取内外两扇门位置
    public Transform outerLeft;
    public Transform innerLeft;
	public Transform outerRight;
	public Transform innerRight;

    private float innerSpeed = 5;
    //电梯启动时间
    private float liftStartTime = 2;
    private float liftStartTimer = 0;
    private bool bPlayerIn = false;
    //电梯到达时间
    private float liftArriveTime = 6;
    private float liftArriveTimer = 0;
    private bool bWin = false;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        if (bWin) return;

        // 电梯内门跟随外门运动，保持一定滞后
        float leftPos = Mathf.Lerp(innerLeft.position.x, outerLeft.position.x, Time.deltaTime * innerSpeed);
        innerLeft.position = new Vector3(leftPos, innerLeft.position.y, innerLeft.position.z);
        float rightPos = Mathf.Lerp(innerRight.position.x, outerRight.position.x, Time.deltaTime * innerSpeed);
        innerRight.position = new Vector3(rightPos, innerRight.position.y, innerRight.position.z);

        if(bPlayerIn) {
            liftStartTimer += Time.deltaTime;
            if (liftStartTimer >= liftStartTime) {
                transform.Translate(Vector3.up * Time.deltaTime);
                liftArriveTimer += Time.deltaTime;
                // 电梯到达，游戏胜利
                if (liftArriveTimer >= liftArriveTime) {
                    //SceneManager.LoadScene("");
                    bWin = true;
                    Debug.LogError("游戏胜利！");
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == Tags.player) {
            bPlayerIn = true;   
        }
    }

    private void OnTriggerExit(Collider other) {
		if (other.tag == Tags.player) {
			bPlayerIn = false;
		}
    }
}
