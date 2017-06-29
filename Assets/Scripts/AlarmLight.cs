using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLight : MonoBehaviour {

    //方便外部访问
    public static AlarmLight _instance;

	public bool alarmOn = false;
	private float lowIntentsity = 0;
	private float highIntentsity = 0.5f;

    private float animSpeed = 2;//speed for light on
	private float targetIntentsity;

    private Light myLight;

	void Awake() {
        _instance = this;
		targetIntentsity = highIntentsity;
        alarmOn = false;
	}

	// Use this for initialization
	void Start () {
		myLight = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		if (alarmOn) {
            

//			Mathf.Lerp (0, 10, t);//表示从0到10的比例值，如t=0.3，则返回值=0+0.3*（10-0）=3；
			myLight.intensity = Mathf.Lerp (myLight.intensity, targetIntentsity, Time.deltaTime * animSpeed); //一秒钟变到无限接近target亮度

            //如果达到了目标值，转换目标值
            if (Mathf.Abs(myLight.intensity - targetIntentsity) < 0.05f) {

				//浮点数不要直接比较，用相见小于某个精度
				if (Mathf.Abs(targetIntentsity - highIntentsity)< 0.01f) { 
                    targetIntentsity = lowIntentsity;
                }else if(Mathf.Abs(targetIntentsity - lowIntentsity) < 0.01f) {
                    targetIntentsity = highIntentsity;
                }

            }
        }else {
            //如果已经满足了就不要在做一次变换，减少渲染
            if ((Mathf.Abs(myLight.intensity - targetIntentsity) < 0.01f)) return;

            myLight.intensity = Mathf.Lerp(myLight.intensity, 0, Time.deltaTime * animSpeed); 
            targetIntentsity = highIntentsity;
        }


	}
}
