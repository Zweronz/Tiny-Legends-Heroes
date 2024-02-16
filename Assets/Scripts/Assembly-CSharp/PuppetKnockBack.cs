using System;
using UnityEngine;

public class PuppetKnockBack : MonoBehaviour
{
	private float knockback_rad;

	private float knockback_intensity;

	private Quaternion knockback_direction;

	private float knockback_coe;

	private void Awake()
	{
		knockback_rad = 0f;
		knockback_intensity = (float)Math.PI * 4f;
		knockback_coe = 1f;
	}

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.localPosition = Vector3.forward * Mathf.Cos(knockback_rad) * knockback_coe * 0.2f;
		knockback_coe *= -1f;
		knockback_rad += knockback_intensity * Time.deltaTime;
		if (knockback_rad >= (float)Math.PI / 2f)
		{
			base.transform.localPosition = Vector3.zero;
			UnityEngine.Object.Destroy(this);
		}
	}

	public void Reset(Quaternion direction)
	{
		knockback_direction = direction;
		knockback_rad = 0f;
	}
}
