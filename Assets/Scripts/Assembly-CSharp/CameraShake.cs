using System;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	private float current_shake_time;

	private float shake_time = 0.3f;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.localPosition = new Vector3(Mathf.Sin(UnityEngine.Random.Range(0f, (float)Math.PI * 2f)) * 0.2f, 0f, Mathf.Sin(UnityEngine.Random.Range(0f, (float)Math.PI * 2f)) * 0.2f);
		current_shake_time += Time.deltaTime;
		if (current_shake_time > shake_time)
		{
			base.transform.localPosition = Vector3.zero;
			UnityEngine.Object.Destroy(this);
		}
	}

	public void Reset()
	{
		current_shake_time = 0f;
	}

	public void Reset(float shake_time)
	{
		current_shake_time = 0f;
		this.shake_time = shake_time;
	}
}
