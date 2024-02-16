using System.Collections;
using UnityEngine;

public class PuppetDungeon : PuppetBasic
{
	public enum MapPuppetState
	{
		Idle = 0,
		Move = 1
	}

	private MapPuppetState puppetState;

	private PuppetComponents puppetComponents;

	private float ray_distance;

	public SceneDungeon scene_dungeon;

	private void Start()
	{
	}

	private void Awake()
	{
		puppetState = MapPuppetState.Idle;
		puppetMovement.move_velocity = 4f;
		puppetMovement.rotate_speed = 4f * D3DFormulas.RotateCoe;
		puppetMovement.doing_rotation = false;
		puppetMovement.correct_delta = 0f;
		ray_distance = 1.4f;
	}

	private void OnDestroy()
	{
		if (null != puppetComponents)
		{
			Object.Destroy(puppetComponents.gameObject);
		}
	}

	public void InitPuppetComponents(bool weak_enemy = false)
	{
		GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/GamePlay/PuppetComponentsPrefab"));
		puppetComponents = gameObject.GetComponent<PuppetComponents>();
		puppetComponents.Initialize(base.gameObject, false, true, true);
		Material normal_mat = null;
		if ("Player" == base.gameObject.tag)
		{
			normal_mat = (Material)Resources.Load("Dungeons3D/Images/ring_green_M");
		}
		else if ("Enemy" == base.gameObject.tag)
		{
			normal_mat = ((!weak_enemy) ? ((Material)Resources.Load("Dungeons3D/Images/ring_red_M")) : ((Material)Resources.Load("Dungeons3D/Images/ring_gray_M")));
		}
		else if ("InteractiveNPC" == base.gameObject.tag)
		{
			normal_mat = (Material)Resources.Load("Dungeons3D/Images/ring_hero_M");
		}
		puppetComponents.InitializeRing(Vector3.one * model_builder.TransformCfg.ring_size * profile_instance.PuppetScale, normal_mat, null);
		puppetComponents.InitializeShadow(Vector3.one * model_builder.TransformCfg.ring_size * profile_instance.PuppetScale);
	}

	public void PuppetRingVisible(bool visible)
	{
		puppetComponents.RingVisible(visible);
	}

	private void Update()
	{
		PuppetBehaviour();
	}

	private void PuppetBehaviour()
	{
		RotateToTarget();
		MapPuppetState mapPuppetState = puppetState;
		if (mapPuppetState != 0 && mapPuppetState == MapPuppetState.Move && !CheckBlock())
		{
			MoveToTarget();
		}
	}

	public void SetTarget(Vector3 target_pt)
	{
		puppetMovement.last_move_position = base.transform.position;
		puppetMovement.frame_move_delta = 0f;
		puppetMovement.block_count = 0;
		SetMoveTarget(target_pt);
	}

	public void SetTarget(Vector3 target_pt, float move_speed)
	{
		puppetMovement.last_move_position = base.transform.position;
		puppetMovement.frame_move_delta = 0f;
		puppetMovement.block_count = 0;
		puppetMovement.move_velocity = move_speed;
		SetMoveTarget(target_pt);
	}

	public void SetTarget(GameObject target_obj)
	{
		SetMoveTarget(target_obj.transform.position);
	}

	private void SetMoveTarget(Vector3 target_pt)
	{
		SetRotationTarget(target_pt);
		puppetMovement.move_start = base.transform.position;
		puppetMovement.move_target = target_pt;
		puppetMovement.correct_delta = 0f;
		puppetState = MapPuppetState.Move;
		model_builder.PlayPuppetAnimations(true, 5, WrapMode.Loop);
	}

	private void MoveToTarget()
	{
		puppetMovement.last_move_position = base.transform.position;
		puppetMovement.frame_move_delta = puppetMovement.move_velocity * Time.deltaTime;
		puppetController.Move(puppetMovement.rotate_target * Vector3.forward * puppetMovement.frame_move_delta);
		base.transform.position = new Vector3(base.transform.position.x, 0f, base.transform.position.z);
		if ((puppetMovement.move_target - base.transform.position).sqrMagnitude < 0.1f)
		{
			OnPositionTarget();
		}
		else
		{
			CorrectMove();
		}
	}

	public void CancelMove()
	{
		puppetMovement.doing_rotation = false;
		puppetState = MapPuppetState.Idle;
		if ("Player" == base.tag)
		{
			TAudioController component = GetComponent<TAudioController>();
			if (null != component)
			{
				component.SetAudioEventPlayDelegate(null);
			}
		}
		model_builder.PlayPuppetAnimations(false, 2, WrapMode.Loop);
	}

	public IEnumerator Hide(bool show, float delay)
	{
		yield return new WaitForSeconds(delay);
		base.gameObject.SetActiveRecursively(show);
		if (null != puppetComponents)
		{
			puppetComponents.gameObject.SetActiveRecursively(show);
		}
	}

	protected override void OnPositionTarget()
	{
		puppetState = MapPuppetState.Idle;
		model_builder.PlayPuppetAnimations(true, 2, WrapMode.Loop);
	}

	public void StopMove()
	{
		puppetState = MapPuppetState.Idle;
		model_builder.PlayPuppetAnimations(false, 2, WrapMode.Loop);
	}

	private void CorrectMove()
	{
		puppetMovement.correct_delta += Time.deltaTime;
		if (puppetMovement.correct_delta >= 0.3f)
		{
			puppetMovement.correct_delta = 0f;
			SetMoveTarget(puppetMovement.move_target);
		}
	}

	public MapPuppetState GetPuppetState()
	{
		return puppetState;
	}

	private bool RaycastHitTarget()
	{
		if ("Player" != base.tag)
		{
			return false;
		}
		Vector3 position = base.transform.position;
		Vector3 direction = puppetMovement.rotate_target * Vector3.forward;
		Ray ray = new Ray(position, direction);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, ray_distance, 512))
		{
			return true;
		}
		return false;
	}

	private void ChangeFootStep(ref string audioName)
	{
		if (D3DMain.Instance.exploring_dungeon.dungeon != null)
		{
			D3DDungeonModelPreset.ModelPreset modelPreset = ((D3DMain.Instance.exploring_dungeon.current_floor != 0) ? D3DDungeonModelPreset.Instance.GetModelPreset(D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_model_preset) : D3DDungeonModelPreset.Instance.GetModelPreset(D3DMain.Instance.exploring_dungeon.dungeon.dungeon_town.town_model_preset));
			audioName = D3DAudioManager.Instance.GetDungeonFootStep(modelPreset.ModelPostfix);
		}
	}

	public void ChangeFootStepInDungeon()
	{
		TAudioController component = model_builder.GetModelParts(0).GetComponent<TAudioController>();
		if (null != component)
		{
			component.SetAudioEventPlayDelegate(ChangeFootStep);
		}
	}
}
