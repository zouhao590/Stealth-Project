using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour {

    // 获取内外两扇门位置
    public Transform outerLeft;
    public Transform innerLeft;
	public Transform outerRight;
	public Transform innerRight;

    private float innerSpeed = 5;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        // 电梯内门跟随外门运动，保持一定滞后
        float leftPos = Mathf.Lerp(innerLeft.position.x, outerLeft.position.x, Time.deltaTime * innerSpeed);
        innerLeft.position = new Vector3(leftPos, innerLeft.position.y, innerLeft.position.z);
        float rightPos = Mathf.Lerp(innerRight.position.x, outerRight.position.x, Time.deltaTime * innerSpeed);
        innerRight.position = new Vector3(rightPos, innerRight.position.y, innerRight.position.z);
	}
}
