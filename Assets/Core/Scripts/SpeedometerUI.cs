using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedometerUI : MonoBehaviour
{
	public PlayerController pc;
	public Image imgSpeedometer;

	float velocity;

	private void FixedUpdate()
	{
		if (pc == null) return;
		imgSpeedometer.fillAmount = Mathf.SmoothDamp(imgSpeedometer.fillAmount, pc.GetSpeedometer(), ref velocity, 0.1f);
	}
}
