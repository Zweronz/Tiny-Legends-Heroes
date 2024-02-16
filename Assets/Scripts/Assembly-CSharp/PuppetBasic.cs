using UnityEngine;

public class PuppetBasic : MonoBehaviour
{
	public class Movement
	{
		public Vector3 move_start = Vector3.zero;

		public Vector3 move_target = Vector3.zero;

		public float move_velocity;

		public Vector3 last_move_position = Vector3.zero;

		public float frame_move_delta;

		public int block_count;

		public Quaternion rotate_start = Quaternion.identity;

		public Quaternion rotate_target = Quaternion.identity;

		public float rotate_during_time;

		public float rotate_eta_time;

		public float rotate_speed;

		public bool doing_rotation;

		public float correct_delta;
	}

	protected Movement puppetMovement = new Movement();

	protected CharacterController puppetController;

	public D3DProfileInstance profile_instance;

	public PuppetModelBuilder model_builder;

	public int cover_weapon_mask;

	public int current_animation_type;

	public bool DoingRotation
	{
		get
		{
			return puppetMovement.doing_rotation;
		}
	}

	public void RotateToScreen()
	{
		puppetMovement.rotate_target = base.transform.rotation;
		Vector3 forward = Vector3.forward;
		Quaternion quaternion = Quaternion.LookRotation(forward);
		if (!(Quaternion.Angle(base.transform.rotation, quaternion) < 1.5f))
		{
			puppetMovement.rotate_target = quaternion;
			puppetMovement.rotate_start = base.transform.rotation;
			puppetMovement.rotate_during_time = 0f;
			puppetMovement.rotate_eta_time = Mathf.Abs(Quaternion.Angle(puppetMovement.rotate_target, puppetMovement.rotate_start)) / puppetMovement.rotate_speed;
			puppetMovement.doing_rotation = true;
		}
	}

	protected bool SetRotationTarget(Vector3 target_pt)
	{
		puppetMovement.rotate_target = base.transform.rotation;
		Vector3 forward = target_pt - base.transform.position;
		Quaternion quaternion = Quaternion.LookRotation(forward);
		if (Quaternion.Angle(base.transform.rotation, quaternion) < 1.5f)
		{
			return false;
		}
		puppetMovement.rotate_target = quaternion;
		puppetMovement.rotate_start = base.transform.rotation;
		puppetMovement.rotate_during_time = 0f;
		puppetMovement.rotate_eta_time = Mathf.Abs(Quaternion.Angle(puppetMovement.rotate_target, puppetMovement.rotate_start)) / puppetMovement.rotate_speed;
		puppetMovement.doing_rotation = true;
		return true;
	}

	protected void RotateToTarget()
	{
		if (puppetMovement.doing_rotation)
		{
			base.transform.rotation = Quaternion.Slerp(puppetMovement.rotate_start, puppetMovement.rotate_target, puppetMovement.rotate_during_time / puppetMovement.rotate_eta_time);
			puppetMovement.rotate_during_time += Time.deltaTime;
			if (Quaternion.Angle(base.transform.rotation, puppetMovement.rotate_target) < 1.5f)
			{
				puppetMovement.doing_rotation = false;
			}
		}
	}

	protected bool CheckBlock()
	{
		Vector3 position = base.transform.position;
		float num = Vector3.Distance(position, puppetMovement.last_move_position);
		if (num <= puppetMovement.frame_move_delta * 0.5f)
		{
			puppetMovement.block_count++;
		}
		if (puppetMovement.block_count > 2)
		{
			puppetMovement.last_move_position = base.transform.position;
			puppetMovement.frame_move_delta = 0f;
			puppetMovement.block_count = 0;
			OnPositionTarget();
			return true;
		}
		return false;
	}

	public virtual void SetPuppetController()
	{
		puppetController = base.gameObject.AddComponent<CharacterController>();
		puppetController.height = model_builder.TransformCfg.character_controller_cfg.height;
		puppetController.radius = model_builder.TransformCfg.character_controller_cfg.radius;
		puppetController.center = model_builder.TransformCfg.character_controller_cfg.center;
		base.transform.localScale *= profile_instance.PuppetScale;
	}

	protected virtual void OnPositionTarget()
	{
		Debug.LogWarning("To override this function!");
	}

	private void Awake()
	{
		profile_instance = null;
		cover_weapon_mask = 0;
	}

	private void Start()
	{
	}

	public bool InitProfileInstance(D3DPuppetProfile profile, D3DGamer.D3DPuppetSaveData save_data)
	{
		profile_instance = new D3DProfileInstance();
		profile.SetPower(0);
		if (!profile_instance.InitInstance(profile, save_data))
		{
			return false;
		}
		string className = "PuppetModelBuilder";
		if (profile_instance.PuppetType == D3DPuppetProfile.ProfileType.AVATAR)
		{
			className = "PuppetAvatarBuilder";
		}
		else if (profile_instance.PuppetType == D3DPuppetProfile.ProfileType.SPECIAL)
		{
			className = "PuppetSpecialBuilder";
		}
		model_builder = UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(base.gameObject, "Assets/Scripts/Assembly-CSharp/PuppetBasic.cs (159,19)", className) as PuppetModelBuilder;
		model_builder.PuppetComponent = this;
		return true;
	}

	public bool InitProfileInstance(D3DPuppetProfile profile, int level, int power_level = 0)
	{
		profile_instance = new D3DProfileInstance();
		profile.SetPower(power_level);
		if (!profile_instance.InitInstance(profile, level))
		{
			return false;
		}
		string className = "PuppetModelBuilder";
		if (profile_instance.PuppetType == D3DPuppetProfile.ProfileType.AVATAR)
		{
			className = "PuppetAvatarBuilder";
		}
		else if (profile_instance.PuppetType == D3DPuppetProfile.ProfileType.SPECIAL)
		{
			className = "PuppetSpecialBuilder";
		}
		model_builder = UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(base.gameObject, "Assets/Scripts/Assembly-CSharp/PuppetBasic.cs (181,19)", className) as PuppetModelBuilder;
		model_builder.PuppetComponent = this;
		return true;
	}

	public void ChangeArms(D3DPuppetProfile.PuppetArms arm_part, D3DEquipment equipment)
	{
		profile_instance.ChangeArms(arm_part, equipment);
		if (!(null == model_builder))
		{
			model_builder.ChangeModelParts(arm_part);
			if (arm_part == D3DPuppetProfile.PuppetArms.RIGHT_HAND || arm_part == D3DPuppetProfile.PuppetArms.LEFT_HAND)
			{
				CheckPuppetWeapons();
			}
		}
	}

	public void ChangeFaceFeatureArms(D3DPuppetProfile.PuppetArms arm_part, D3DEquipment equipment)
	{
		profile_instance.ChangeArms(arm_part, equipment);
		if (!(null == model_builder))
		{
			model_builder.ChangeFaceFeatureModel(arm_part);
		}
	}

	public void RemoveArms(D3DPuppetProfile.PuppetArms arm_part, bool bUseDefaultArms = true)
	{
		profile_instance.ChangeArms(arm_part, null);
		if (!(null == model_builder))
		{
			model_builder.RemoveModelParts(arm_part, bUseDefaultArms);
			if (arm_part == D3DPuppetProfile.PuppetArms.RIGHT_HAND || arm_part == D3DPuppetProfile.PuppetArms.LEFT_HAND)
			{
				CheckPuppetWeapons();
			}
		}
	}

	public void RemoveFaceFeatureArms(D3DPuppetProfile.PuppetArms arm_part)
	{
		profile_instance.ChangeArms(arm_part, null);
		if (!(null == model_builder))
		{
			model_builder.RemoveFaceFeatureModelParts(arm_part);
		}
	}

	public void CheckPuppetWeapons()
	{
		if (profile_instance.PuppetType == D3DPuppetProfile.ProfileType.SPECIAL)
		{
			return;
		}
		D3DEquipment d3DEquipment = profile_instance.puppet_arms[0];
		D3DEquipment d3DEquipment2 = profile_instance.puppet_arms[1];
		string text = profile_instance.puppet_class.class_animations[0];
		current_animation_type = 0;
		cover_weapon_mask = 0;
		if (d3DEquipment == null)
		{
			if (d3DEquipment2 != null)
			{
				if (d3DEquipment2.equipment_type == D3DEquipment.EquipmentType.TWO_HAND)
				{
					text = profile_instance.puppet_class.class_animations[1];
					current_animation_type = 1;
					profile_instance.ChangeArms(D3DPuppetProfile.PuppetArms.RIGHT_HAND, d3DEquipment2);
					profile_instance.ChangeArms(D3DPuppetProfile.PuppetArms.LEFT_HAND, null);
					model_builder.ChangeModelParts(D3DPuppetProfile.PuppetArms.RIGHT_HAND);
					model_builder.RemoveModelParts(D3DPuppetProfile.PuppetArms.LEFT_HAND);
					if (!profile_instance.GetTitanPower())
					{
						cover_weapon_mask = 1;
					}
				}
				else if (d3DEquipment2.equipment_type == D3DEquipment.EquipmentType.ONE_HAND)
				{
					profile_instance.ChangeArms(D3DPuppetProfile.PuppetArms.RIGHT_HAND, d3DEquipment2);
					profile_instance.ChangeArms(D3DPuppetProfile.PuppetArms.LEFT_HAND, null);
					model_builder.ChangeModelParts(D3DPuppetProfile.PuppetArms.RIGHT_HAND);
					model_builder.RemoveModelParts(D3DPuppetProfile.PuppetArms.LEFT_HAND);
				}
			}
			else if (profile_instance.puppet_default_weapon.equipment_type == D3DEquipment.EquipmentType.TWO_HAND)
			{
				text = profile_instance.puppet_class.class_animations[1];
				current_animation_type = 1;
			}
		}
		else if (d3DEquipment2 != null)
		{
			if (d3DEquipment2.equipment_type == D3DEquipment.EquipmentType.ONE_HAND)
			{
				if (profile_instance.GetTitanPower() || profile_instance.GetDualWield())
				{
					text = profile_instance.puppet_class.class_animations[2];
					current_animation_type = 2;
				}
			}
			else if (d3DEquipment2.equipment_type == D3DEquipment.EquipmentType.TWO_HAND && profile_instance.GetTitanPower())
			{
				text = profile_instance.puppet_class.class_animations[2];
				current_animation_type = 2;
			}
		}
		else if (d3DEquipment.equipment_type == D3DEquipment.EquipmentType.TWO_HAND)
		{
			text = profile_instance.puppet_class.class_animations[1];
			current_animation_type = 1;
			if (!profile_instance.GetTitanPower())
			{
				cover_weapon_mask = 1;
			}
		}
		else if (d3DEquipment.equipment_type == D3DEquipment.EquipmentType.BOW_HAND)
		{
			text = profile_instance.puppet_class.class_animations[1];
			current_animation_type = 1;
			cover_weapon_mask = 1;
		}
		if (string.Empty == text)
		{
			text = profile_instance.puppet_class.class_animations[0];
			current_animation_type = 0;
		}
		model_builder.ChangeAnimation(text);
	}
}
