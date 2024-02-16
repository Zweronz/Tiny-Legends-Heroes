using System;
using UnityEngine;

public class MissileComponent : MonoBehaviour
{
	private PuppetArena shooter;

	private ActiveSkillTriggerBehaviour skill_trigger;

	private ActiveSkillTriggerExecuter skill_executer;

	private Vector3 start_point;

	private Vector3 end_point;

	private float distance;

	public bool direction_missile = true;

	public float z_velocity;

	public float x_curvature_min;

	public float x_curvature_max;

	private float x_curvature;

	private float x_curvature_velocity;

	private float x_curvature_degree;

	public float y_curvature_min;

	public float y_curvature_max;

	private float y_curvature;

	private float y_curvature_velocity;

	private float y_curvature_degree;

	public string AwakenSfx;

	public string DestorySfx;

	public BasicEffectComponent TriggerEffect;

	public bool PlayEffectOnGround;

	private Quaternion direction;

	private Vector3 direction_vector;

	private void Update()
	{
		Vector3 vector = direction * Vector3.forward * z_velocity * Time.deltaTime;
		start_point += vector;
		float num = x_curvature * Mathf.Sin(x_curvature_degree * ((float)Math.PI / 180f));
		Vector3 vector2 = direction * Vector3.right * num;
		x_curvature_degree += x_curvature_velocity * Time.deltaTime;
		float num2 = y_curvature * Mathf.Sin(y_curvature_degree * ((float)Math.PI / 180f));
		Vector3 vector3 = direction * Vector3.up * num2;
		y_curvature_degree += y_curvature_velocity * Time.deltaTime;
		base.transform.eulerAngles = direction.eulerAngles + Vector3.up * (Mathf.Atan2((x_curvature - num) * Mathf.Cos(x_curvature_degree * ((float)Math.PI / 180f)), z_velocity) * 57.29578f) + Vector3.right * (Mathf.Atan2((y_curvature - num2) * (0f - Mathf.Cos(y_curvature_degree * ((float)Math.PI / 180f))), z_velocity) * 57.29578f);
		base.transform.position = start_point;
		base.transform.position += vector2;
		base.transform.position += vector3;
		if (null != shooter)
		{
			shooter.transform.position = base.transform.position;
			if (shooter.IsBadState())
			{
				shooter.transform.position = new Vector3(shooter.transform.position.x, 0f, shooter.transform.position.z);
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	private void LateUpdate()
	{
		Vector3 rhs = end_point - start_point;
		if (Vector3.Dot(direction_vector, rhs) < 0f)
		{
			base.transform.position = end_point;
			OnTargetPosition();
		}
	}

	public void InitMissile(Vector3 start, Vector3 end, PuppetArena shooter, ActiveSkillTriggerBehaviour trigger, ActiveSkillTriggerExecuter executer)
	{
		start_point = start;
		end_point = end;
		direction = Quaternion.LookRotation(end_point - start_point);
		base.transform.rotation = direction;
		distance = Vector3.Distance(end_point, start_point);
		float num = distance / z_velocity;
		x_curvature = UnityEngine.Random.Range(x_curvature_min, x_curvature_max);
		x_curvature_velocity = 180f / num;
		x_curvature_degree = 0f;
		y_curvature = UnityEngine.Random.Range(y_curvature_min, y_curvature_max);
		y_curvature_velocity = 180f / num;
		y_curvature_degree = 0f;
		base.transform.position = start_point;
		direction_vector = end_point - start_point;
		this.shooter = shooter;
		skill_trigger = trigger;
		skill_executer = executer;
		D3DAudioManager.Instance.PlayAudio(AwakenSfx, base.gameObject, false, true);
		if (z_velocity >= 10000f || end_point == start_point)
		{
			base.transform.position = end_point;
			OnTargetPosition();
		}
	}

	private void OnTargetPosition()
	{
		D3DAudioManager.Instance.PlayAudio(DestorySfx, base.gameObject, false, true);
		if (null != TriggerEffect)
		{
			Vector3 position = base.transform.position;
			if (PlayEffectOnGround)
			{
				position = new Vector3(position.x, 0f, position.z);
			}
			BasicEffectComponent basicEffectComponent = (BasicEffectComponent)UnityEngine.Object.Instantiate(TriggerEffect, position, direction);
			basicEffectComponent.Play(true, null, null, Vector3.zero, Vector2.one, 0f);
		}
		if (null != shooter)
		{
			shooter.transform.position = new Vector3(shooter.transform.position.x, 0f, shooter.transform.position.z);
		}
		if (null != skill_trigger)
		{
			skill_trigger.ResumeTrigger();
		}
		if (null != skill_executer)
		{
			skill_executer.Execute();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
