using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLight : MonoBehaviour {

    //警报管理器，获取警报信息
    private AlarmMangaer alarmManager;
    //亮暗值
    private const float lowIntentsity = 0;
    private const float highIntentsity = 0.5f;
    private float targetIntentsity;
	//闪烁速度
	private const float animSpeed = 2;

    private Light myLight;

	// Use this for initialization
	void Start () {
		targetIntentsity = highIntentsity;
		myLight = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        if (IsAlarmOn()) {
            TurnOnAlarmLight();
        }else {
            TurnOffAlarmLight();
        }
	}

    //从全局控制器获取警报信息
    private bool IsAlarmOn() {
        if(alarmManager == null) {
            alarmManager = GameController.GetAlarmMangaer();
        }
        return alarmManager == null ? false : alarmManager.IsAlarmOn();
    }

    // 打开警报灯
    private void TurnOnAlarmLight() {
		//Mathf.Lerp (0, 10, t);//表示从0到10的比例值，如t=0.3，则返回值=0+0.3*（10-0）=3；
		myLight.intensity = Mathf.Lerp(myLight.intensity, targetIntentsity, Time.deltaTime * animSpeed); //一秒钟变到无限接近target亮度

		//如果达到了目标值，转换目标值
		if (Mathf.Abs(myLight.intensity - targetIntentsity) < 0.05f) {
			//浮点数不要直接比较，用相见小于某个精度
			if (Mathf.Abs(targetIntentsity - highIntentsity) < 0.01f) {
				targetIntentsity = lowIntentsity;
			}
			else if (Mathf.Abs(targetIntentsity - lowIntentsity) < 0.01f) {
				targetIntentsity = highIntentsity;
			}
		}
    }

    // 关闭警报灯
    private void TurnOffAlarmLight() {
		//如果已经满足了就不要在做一次变换，减少渲染
		if ((Mathf.Abs(myLight.intensity - targetIntentsity) < 0.01f)) return;

		myLight.intensity = Mathf.Lerp(myLight.intensity, 0, Time.deltaTime * animSpeed);
		targetIntentsity = highIntentsity;
	}
}
