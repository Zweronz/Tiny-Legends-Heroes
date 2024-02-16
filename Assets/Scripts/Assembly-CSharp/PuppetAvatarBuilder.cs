using System.Collections.Generic;
using UnityEngine;

public class PuppetAvatarBuilder : PuppetModelBuilder
{
	private GameObject[] avatar_mount_obj;

	private List<GameObject> body_effects;

	private List<Material> _matColoredWhenHitted = new List<Material>();

	public List<Material> MaterialWhileAttacked
	{
		get
		{
			return _matColoredWhenHitted;
		}
	}

	private bool BindMatFromRes(Transform trans, int nMatCount, string strMaterialPrefix, List<string> strMaterial)
	{
		if (trans == null)
		{
			return false;
		}
		if (trans.GetComponent<Renderer>() == null)
		{
			return false;
		}
		int num = 0;
		Material[] materials = trans.GetComponent<Renderer>().materials;
		foreach (Material material in materials)
		{
			if (num > nMatCount - 1)
			{
				num = nMatCount - 1;
			}
			Material material2 = (Material)Resources.Load(strMaterialPrefix + strMaterial[num] + "_M");
			if (null == material2)
			{
				return false;
			}
			material.CopyPropertiesFromMaterial(material2);
			material.shader = material2.shader;
			material.name = material2.name;
			_matColoredWhenHitted.Add(material);
			num++;
		}
		return true;
	}

	private new void Awake()
	{
		base.Awake();
		avatar_mount_obj = new GameObject[4];
		body_effects = new List<GameObject>();
	}

	private new void Start()
	{
	}

	private new void Update()
	{
	}

	public override void BuildPuppetModel()
	{
		string empty = string.Empty;
		empty = empty + "Build Puppet,Type:Avatar,Id is:" + puppet_component.profile_instance.ProfileID + "\n";
		BuildAvatarBody(ref empty);
		BindParts(ref empty);
		BuildArmor(ref empty);
		BuildHead(ref empty);
		BuildHelm(ref empty);
		CheckHelmHide();
		D3DEquipment d3DEquipment = puppet_component.profile_instance.puppet_arms[0];
		if (d3DEquipment != null && d3DEquipment.equipment_type == D3DEquipment.EquipmentType.BOW_HAND)
		{
			BuildBowHand(d3DEquipment, ref empty);
		}
		else
		{
			BuildRightHand(ref empty);
			BuildLeftHand(ref empty);
		}
		foreach (string[] puppetEffect in puppet_component.profile_instance.PuppetEffects)
		{
			GameObject gameObject = D3DMain.Instance.FindGameObjectChild(base.gameObject, puppetEffect[0]);
			if (null != gameObject)
			{
				GameObject gameObject2 = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/ModEffect/" + puppetEffect[1]), gameObject.transform.position, Quaternion.identity);
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.rotation = Quaternion.identity;
			}
		}
		GetAnimationComponent();
		if (puppet_component.profile_instance.puppet_class != null)
		{
			AddAnimationClips(puppet_component.profile_instance.puppet_class.class_animations[0], ref empty);
			PlayPuppetAnimations(true, 6, WrapMode.Loop);
		}
		puppet_transform_cfg = D3DMain.Instance.DefaultPuppetTransform;
	}

	public override void BuildPuppetFaceFeatureModel()
	{
		string build_info = string.Empty;
		BuildAvatarBody(ref build_info);
		BindParts(ref build_info);
		BuildArmor(ref build_info);
		BuildHead(ref build_info);
		BuildHelm(ref build_info);
		CheckHelmHide();
		foreach (string[] puppetEffect in puppet_component.profile_instance.PuppetEffects)
		{
			GameObject gameObject = D3DMain.Instance.FindGameObjectChild(base.gameObject, puppetEffect[0]);
			if (null == gameObject)
			{
				gameObject = base.gameObject;
			}
			GameObject gameObject2 = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/ModEffect/" + puppetEffect[1]));
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.rotation = Quaternion.identity;
		}
		GetAnimationComponent();
		AddFaceFeatureAnimationClip();
		puppet_transform_cfg = D3DMain.Instance.DefaultPuppetTransform;
	}

	private void BuildAvatarBody(ref string build_info)
	{
		build_info += "------Build Body\n";
		Object @object = Resources.Load("Dungeons3D/Prefabs/Character/Avatar/Armor/armor_robe_000");
		if (null == @object)
		{
			build_info += "Build Avatar Body failed,other parts and animation will not work normal.\n";
			return;
		}
		model_parts[0] = (GameObject)Object.Instantiate(@object);
		Transform transform = model_parts[0].transform;
		transform.parent = base.transform;
		transform.localPosition = Vector3.zero;
		build_info += "Success!\n";
	}

	private void BuildArmor(ref string build_info)
	{
		int num = 0;
		build_info += "------Build Body\n";
		D3DEquipment d3DEquipment = puppet_component.profile_instance.puppet_arms[2];
		if (d3DEquipment == null)
		{
			build_info += "Puppet does not equip armor,use default body\n";
			d3DEquipment = puppet_component.profile_instance.puppet_default_body;
			if (d3DEquipment == null)
			{
				build_info += "Puppet's default body is null,create body failed\n";
				SetPartsVisible(num, false);
				return;
			}
		}
		Object @object = Resources.Load("Dungeons3D/Prefabs/Character/Avatar/Armor/" + d3DEquipment.use_model);
		if (null == @object)
		{
			build_info = build_info + "Can not find body model,the model id is:" + d3DEquipment.use_model + "\n";
			SetPartsVisible(num, false);
			return;
		}
		GameObject gameObject = (GameObject)Object.Instantiate(@object);
		model_parts[num].name = gameObject.name;
		Transform transform = gameObject.transform.Find("armor");
		if (null != transform)
		{
			int num2 = 0;
			int count = d3DEquipment.use_textures.Count;
			if (null != transform.GetComponent<Renderer>())
			{
				Material[] materials = transform.GetComponent<Renderer>().materials;
				foreach (Material material in materials)
				{
					if (num2 > count - 1)
					{
						num2 = count - 1;
					}
					Material material2 = (Material)Resources.Load("Dungeons3D/Models/Character/Avatar/Armor/Materials/" + d3DEquipment.use_textures[num2] + puppet_component.profile_instance.PuppetFeatureSkin + "_M");
					if (null == material2)
					{
						material2 = (Material)Resources.Load("Dungeons3D/Models/Character/Avatar/Armor/Materials/" + d3DEquipment.use_textures[num2] + "_M");
					}
					if (null != material2)
					{
						material.CopyPropertiesFromMaterial(material2);
						material.shader = material2.shader;
						material.name = material2.name;
						_matColoredWhenHitted.Add(material);
					}
					num2++;
				}
			}
		}
		SkinnedMeshRenderer componentInChildren = model_parts[num].GetComponentInChildren<SkinnedMeshRenderer>();
		SkinnedMeshRenderer componentInChildren2 = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
		Transform[] array = new Transform[componentInChildren2.bones.Length];
		for (int j = 0; j < componentInChildren2.bones.Length; j++)
		{
			Transform transform2 = null;
			Transform[] componentsInChildren = model_parts[num].GetComponentsInChildren<Transform>();
			foreach (Transform transform3 in componentsInChildren)
			{
				if (transform3.name == componentInChildren2.bones[j].name)
				{
					transform2 = transform3;
					break;
				}
			}
			if (null == transform2)
			{
				break;
			}
			array[j] = transform2;
		}
		componentInChildren.bones = array;
		componentInChildren.sharedMesh = componentInChildren2.sharedMesh;
		componentInChildren.sharedMaterials = componentInChildren2.sharedMaterials;
		Object.DestroyObject(gameObject);
		foreach (GameObject body_effect in body_effects)
		{
			Object.DestroyObject(body_effect);
		}
		body_effects.Clear();
		foreach (string[] equipment_effect in d3DEquipment.equipment_effects)
		{
			GameObject gameObject2 = D3DMain.Instance.FindGameObjectChild(model_parts[num], equipment_effect[0]);
			if (null != gameObject2)
			{
				GameObject gameObject3 = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/ModEffect/" + equipment_effect[1]));
				gameObject3.transform.parent = gameObject2.transform;
				gameObject3.transform.localPosition = Vector3.zero;
				gameObject3.transform.rotation = Quaternion.identity;
				body_effects.Add(gameObject3);
			}
		}
		build_info += "Success!\n";
	}

	private void BuildHead(ref string build_info)
	{
		build_info += "------Build Head\n";
		Object @object = Resources.Load("Dungeons3D/Prefabs/Character/Avatar/Head/" + puppet_component.profile_instance.PuppetFeatureModel);
		if (null == @object)
		{
			build_info = build_info + "Can not find head model,the model id is:" + puppet_component.profile_instance.PuppetFeatureModel + "\n";
			return;
		}
		int num = 1;
		model_parts[num] = (GameObject)Object.Instantiate(@object);
		Transform transform = model_parts[num].transform;
		transform.parent = avatar_mount_obj[num - 1].transform.parent;
		transform.localPosition = Vector3.zero;
		Transform trans = transform.Find("head");
		Transform trans2 = transform.Find("ears");
		Transform trans3 = transform.Find("hair");
		Transform trans4 = transform.Find("nose");
		Transform trans5 = transform.Find("beard");
		int count = puppet_component.profile_instance.PuppetFeatureTextures.Count;
		BindMatFromRes(trans, count, "Dungeons3D/Models/Character/Avatar/Head/Materials/", puppet_component.profile_instance.PuppetFeatureTextures);
		BindMatFromRes(trans2, count, "Dungeons3D/Models/Character/Avatar/Head/Materials/", puppet_component.profile_instance.PuppetFeatureTextures);
		BindMatFromRes(trans3, count, "Dungeons3D/Models/Character/Avatar/Head/Materials/", puppet_component.profile_instance.PuppetFeatureTextures);
		BindMatFromRes(trans4, count, "Dungeons3D/Models/Character/Avatar/Head/Materials/", puppet_component.profile_instance.PuppetFeatureTextures);
		BindMatFromRes(trans5, count, "Dungeons3D/Models/Character/Avatar/Head/Materials/", puppet_component.profile_instance.PuppetFeatureTextures);
		build_info += "Success!\n";
	}

	private void BuildHelm(ref string build_info)
	{
		build_info += "------Build Helm\n";
		D3DEquipment d3DEquipment = puppet_component.profile_instance.puppet_arms[3];
		if (d3DEquipment == null)
		{
			build_info += "Puppet does not equip helm\n";
			return;
		}
		Object @object = Resources.Load("Dungeons3D/Prefabs/Character/Avatar/Helm/" + d3DEquipment.use_model);
		if (null == @object)
		{
			build_info = build_info + "Can not find helm model,the model id is:" + d3DEquipment.use_model + "\n";
			return;
		}
		int num = 2;
		model_parts[num] = (GameObject)Object.Instantiate(@object);
		foreach (string[] equipment_effect in d3DEquipment.equipment_effects)
		{
			GameObject gameObject = D3DMain.Instance.FindGameObjectChild(model_parts[num], equipment_effect[0]);
			if (null != gameObject)
			{
				GameObject gameObject2 = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/ModEffect/" + equipment_effect[1]));
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.rotation = Quaternion.identity;
			}
		}
		Transform transform = model_parts[num].transform;
		transform.parent = avatar_mount_obj[num - 1].transform.parent;
		transform.localPosition = Vector3.zero;
		Transform trans = transform.Find("helm");
		int count = d3DEquipment.use_textures.Count;
		BindMatFromRes(trans, count, "Dungeons3D/Models/Character/Avatar/Helm/Materials/", d3DEquipment.use_textures);
		build_info += "Success!\n";
	}

	private void BuildRightHand(ref string build_info)
	{
		build_info += "------Build Right Hand\n";
		D3DEquipment d3DEquipment = puppet_component.profile_instance.puppet_arms[0];
		if (d3DEquipment == null)
		{
			build_info += "Puppet does not equip right hand,use default weapon\n";
			d3DEquipment = puppet_component.profile_instance.puppet_default_weapon;
			if (d3DEquipment == null)
			{
				build_info += "Puppet's default weapon is null,create right hand failed";
				return;
			}
		}
		if (d3DEquipment.equipment_type == D3DEquipment.EquipmentType.BOW_HAND)
		{
			return;
		}
		int num = 4;
		Object @object = Resources.Load("Dungeons3D/Prefabs/Character/Avatar/Weapon/" + d3DEquipment.use_model);
		if (null == @object)
		{
			build_info = build_info + "Can not find right hand weapon model,the model id is:" + d3DEquipment.use_model + "\n";
			return;
		}
		model_parts[num] = (GameObject)Object.Instantiate(@object);
		foreach (string[] equipment_effect in d3DEquipment.equipment_effects)
		{
			GameObject gameObject = D3DMain.Instance.FindGameObjectChild(model_parts[num], equipment_effect[0]);
			if (null != gameObject)
			{
				GameObject gameObject2 = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/ModEffect/" + equipment_effect[1]));
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.rotation = Quaternion.identity;
			}
		}
		Transform transform = model_parts[num].transform;
		transform.parent = avatar_mount_obj[num - 1].transform.parent;
		transform.localPosition = Vector3.zero;
		Transform trans = transform.Find("weapon");
		int count = d3DEquipment.use_textures.Count;
		BindMatFromRes(trans, count, "Dungeons3D/Models/Character/Avatar/Weapon/Materials/", d3DEquipment.use_textures);
		build_info += "Success!\n";
	}

	private void BuildBowHand(D3DEquipment bow_equipment, ref string build_info)
	{
		Object @object = Resources.Load("Dungeons3D/Prefabs/Character/Avatar/Weapon/" + bow_equipment.use_model);
		if (null == @object)
		{
			build_info = build_info + "Can not find left hand weapon model,the model id is:" + bow_equipment.use_model + "\n";
			return;
		}
		int num = 3;
		model_parts[num] = (GameObject)Object.Instantiate(@object);
		foreach (string[] equipment_effect in bow_equipment.equipment_effects)
		{
			GameObject gameObject = D3DMain.Instance.FindGameObjectChild(model_parts[num], equipment_effect[0]);
			if (null != gameObject)
			{
				GameObject gameObject2 = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/ModEffect/" + equipment_effect[1]));
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.rotation = Quaternion.identity;
			}
		}
		Transform transform = model_parts[num].transform;
		transform.parent = avatar_mount_obj[num - 1].transform.parent;
		transform.localPosition = Vector3.zero;
		Transform trans = transform.Find("weapon");
		int count = bow_equipment.use_textures.Count;
		BindMatFromRes(trans, count, "Dungeons3D/Models/Character/Avatar/Weapon/Materials/", bow_equipment.use_textures);
		build_info += "Success!\n";
	}

	private void BuildLeftHand(ref string build_info)
	{
		build_info += "------Build Left Hand\n";
		D3DEquipment d3DEquipment = puppet_component.profile_instance.puppet_arms[1];
		if (d3DEquipment == null)
		{
			build_info += "Puppet does not equip left hand,use default weapon\n";
			d3DEquipment = puppet_component.profile_instance.puppet_default_weapon;
			if (d3DEquipment == null)
			{
				build_info += "Puppet's default weapon is null,create left hand failed";
				return;
			}
			if (d3DEquipment.equipment_type != D3DEquipment.EquipmentType.BOW_HAND)
			{
				return;
			}
		}
		Object @object = Resources.Load("Dungeons3D/Prefabs/Character/Avatar/Weapon/" + d3DEquipment.use_model);
		if (null == @object)
		{
			build_info = build_info + "Can not find left hand weapon model,the model id is:" + d3DEquipment.use_model + "\n";
			return;
		}
		int num = 3;
		model_parts[num] = (GameObject)Object.Instantiate(@object);
		foreach (string[] equipment_effect in d3DEquipment.equipment_effects)
		{
			GameObject gameObject = D3DMain.Instance.FindGameObjectChild(model_parts[num], equipment_effect[0]);
			if (null != gameObject)
			{
				GameObject gameObject2 = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/ModEffect/" + equipment_effect[1]));
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.rotation = Quaternion.identity;
			}
		}
		Transform transform = model_parts[num].transform;
		transform.parent = avatar_mount_obj[num - 1].transform.parent;
		transform.localPosition = Vector3.zero;
		Transform trans = transform.Find("weapon");
		int count = d3DEquipment.use_textures.Count;
		BindMatFromRes(trans, count, "Dungeons3D/Models/Character/Avatar/Weapon/Materials/", d3DEquipment.use_textures);
		build_info += "Success!\n";
	}

	private void BindParts(ref string bind_info)
	{
		bind_info += "Try to bind parts\n";
		for (int i = 0; i < 4; i++)
		{
			avatar_mount_obj[i] = new GameObject("Mount Point " + (ModelParts)(i + 1));
			avatar_mount_obj[i].transform.parent = base.transform;
			avatar_mount_obj[i].transform.localPosition = Vector3.zero;
		}
		if (null == model_parts[0])
		{
			bind_info += "Avatar body is null,bind failed";
			return;
		}
		string[] array = new string[4] { "Bone/Pelvis/Spine/Head", "Bone/Pelvis/Spine/Head/Helm", "Bone/Pelvis/Spine/Left_Shoulder/Left_Hand/Sheild", "Bone/Pelvis/Spine/Right_Shoulder/Right_Hand/Weapon" };
		for (int j = 0; j < 4; j++)
		{
			GameObject gameObject = model_parts[0].transform.Find(array[j]).gameObject;
			bind_info = bind_info + "Try to bind mount point " + (ModelParts)(j + 1);
			if (null != gameObject)
			{
				avatar_mount_obj[j].transform.parent = gameObject.transform;
				avatar_mount_obj[j].transform.localPosition = Vector3.zero;
				bind_info += "Bind success!\n";
			}
			else
			{
				bind_info += "Bind point is not found,bind failed\n";
			}
		}
	}

	private void AddAnimationClips(string animation_type, ref string build_info)
	{
		build_info = build_info + "------Try to add puppet animation clips,animation type:" + animation_type + "\n";
		if (null == animation_component)
		{
			build_info += "Animation component is null,add clips failed\n";
			return;
		}
		foreach (string animation_clip_name in animation_clip_names)
		{
			if (string.Empty != animation_clip_name && null != animation_component.GetClip(animation_clip_name))
			{
				animation_component.RemoveClip(animation_clip_name);
			}
		}
		animation_clip_names.Clear();
		string text = animation_type + "_animation";
		TextAsset textAsset = Resources.Load("Dungeons3D/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("Docs", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("AnimationIndexs", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt(text, D3DGamer.Instance.Sk[0]))) as TextAsset;
		if (null == textAsset)
		{
			build_info = build_info + "animation index config file can not be found,file:" + text + "\n";
			return;
		}
		current_animation = animation_type;
		string text2 = XXTEAUtils.Decrypt(textAsset.text, D3DGamer.Instance.Sk[1]);
		while (text2 != string.Empty)
		{
			int num = text2.IndexOf('\n');
			int num2 = text2.IndexOf('>');
			string item = text2.Substring(num2 + 1, num - num2 - 2);
			animation_clip_names.Add(item);
			text2 = text2.Remove(0, num + 1);
		}
		Object[] array = Resources.LoadAll("Dungeons3D/Prefabs/Character/Avatar/Animation/" + animation_type, typeof(AnimationClip));
		Object[] array2 = array;
		foreach (Object @object in array2)
		{
			animation_component.AddClip((AnimationClip)@object, @object.name);
		}
		build_info += "Add clips success!\n";
	}

	private void AddFaceFeatureAnimationClip()
	{
		AnimationClip animationClip = Resources.Load("Dungeons3D/Prefabs/Character/Avatar/Animation/idle_icon/idle_icon") as AnimationClip;
		animation_component.clip = animationClip;
		animation_component.AddClip(animationClip, animationClip.name);
		animation_clip_names.Add(animationClip.name);
	}

	public override void ChangeModelParts(D3DPuppetProfile.PuppetArms arm_part)
	{
		string build_info = string.Empty;
		int num = -1;
		switch (arm_part)
		{
		default:
			return;
		case D3DPuppetProfile.PuppetArms.ARMOR:
			num = 0;
			BuildArmor(ref build_info);
			break;
		case D3DPuppetProfile.PuppetArms.HELM:
			num = 2;
			Object.DestroyImmediate(model_parts[num]);
			BuildHelm(ref build_info);
			model_parts[num].transform.localRotation = avatar_mount_obj[num - 1].transform.localRotation;
			CheckHelmHide();
			break;
		case D3DPuppetProfile.PuppetArms.RIGHT_HAND:
		{
			num = 4;
			D3DEquipment d3DEquipment = puppet_component.profile_instance.puppet_arms[0];
			if (d3DEquipment == null)
			{
				d3DEquipment = puppet_component.profile_instance.puppet_default_weapon;
				if (d3DEquipment == null)
				{
					Object.DestroyImmediate(model_parts[num]);
					model_parts[num].transform.localRotation = avatar_mount_obj[num - 1].transform.localRotation;
					break;
				}
			}
			if (d3DEquipment.equipment_type == D3DEquipment.EquipmentType.BOW_HAND)
			{
				num = 3;
				Object.DestroyImmediate(model_parts[num]);
				BuildBowHand(d3DEquipment, ref build_info);
			}
			else
			{
				Object.DestroyImmediate(model_parts[num]);
				BuildRightHand(ref build_info);
			}
			model_parts[num].transform.localRotation = avatar_mount_obj[num - 1].transform.localRotation;
			break;
		}
		case D3DPuppetProfile.PuppetArms.LEFT_HAND:
			num = 3;
			Object.DestroyImmediate(model_parts[num]);
			BuildLeftHand(ref build_info);
			model_parts[num].transform.localRotation = avatar_mount_obj[num - 1].transform.localRotation;
			break;
		}
		D3DMain.Instance.SetGameObjectGeneralLayer(model_parts[num], base.gameObject.layer);
	}

	public override void ChangeFaceFeatureModel(D3DPuppetProfile.PuppetArms arm_part)
	{
		string build_info = string.Empty;
		int num = -1;
		switch (arm_part)
		{
		default:
			return;
		case D3DPuppetProfile.PuppetArms.ARMOR:
			num = 0;
			BuildArmor(ref build_info);
			break;
		case D3DPuppetProfile.PuppetArms.HELM:
			num = 2;
			Object.DestroyImmediate(model_parts[num]);
			BuildHelm(ref build_info);
			model_parts[num].transform.localRotation = avatar_mount_obj[num - 1].transform.localRotation;
			CheckHelmHide();
			break;
		}
		D3DMain.Instance.SetGameObjectGeneralLayer(model_parts[num], base.gameObject.layer);
	}

	public override void RemoveModelParts(D3DPuppetProfile.PuppetArms arm_part, bool bUseDefaultPart = true)
	{
		string build_info = string.Empty;
		int num = -1;
		switch (arm_part)
		{
		default:
			return;
		case D3DPuppetProfile.PuppetArms.ARMOR:
			num = 0;
			BuildArmor(ref build_info);
			break;
		case D3DPuppetProfile.PuppetArms.HELM:
			num = 2;
			Object.DestroyImmediate(model_parts[num]);
			CheckHelmHide();
			return;
		case D3DPuppetProfile.PuppetArms.RIGHT_HAND:
			num = 4;
			if (puppet_component.profile_instance.puppet_default_weapon != null && puppet_component.profile_instance.puppet_default_weapon.equipment_type == D3DEquipment.EquipmentType.BOW_HAND)
			{
				num = 3;
				Object.DestroyImmediate(model_parts[num]);
				if (bUseDefaultPart)
				{
					BuildBowHand(puppet_component.profile_instance.puppet_default_weapon, ref build_info);
					model_parts[num].transform.localRotation = avatar_mount_obj[num - 1].transform.localRotation;
				}
			}
			else
			{
				Object.DestroyImmediate(model_parts[num]);
				if (bUseDefaultPart)
				{
					BuildRightHand(ref build_info);
					model_parts[num].transform.localRotation = avatar_mount_obj[num - 1].transform.localRotation;
				}
			}
			break;
		case D3DPuppetProfile.PuppetArms.LEFT_HAND:
			num = 3;
			Object.DestroyImmediate(model_parts[num]);
			return;
		}
		if (model_parts[num] != null)
		{
			D3DMain.Instance.SetGameObjectGeneralLayer(model_parts[num], base.gameObject.layer);
		}
	}

	public override void RemoveFaceFeatureModelParts(D3DPuppetProfile.PuppetArms arm_part)
	{
		string build_info = string.Empty;
		int num = -1;
		switch (arm_part)
		{
		case D3DPuppetProfile.PuppetArms.ARMOR:
			num = 0;
			BuildArmor(ref build_info);
			D3DMain.Instance.SetGameObjectGeneralLayer(model_parts[num], base.gameObject.layer);
			break;
		case D3DPuppetProfile.PuppetArms.HELM:
			num = 2;
			Object.DestroyImmediate(model_parts[num]);
			CheckHelmHide();
			break;
		}
	}

	public override void ChangeAnimation(string animation)
	{
		if (!(string.Empty == animation) && !(animation == current_animation))
		{
			StopCurrentClip();
			string build_info = string.Empty;
			AddAnimationClips(animation, ref build_info);
			PlayPuppetAnimations(false, current_clip, WrapMode.Loop, true);
		}
	}

	public override void CheckHelmHide()
	{
		Transform transform = model_parts[1].transform.Find("ears");
		Transform transform2 = model_parts[1].transform.Find("hair");
		Transform transform3 = model_parts[1].transform.Find("nose");
		Transform transform4 = model_parts[1].transform.Find("beard");
		D3DEquipment d3DEquipment = puppet_component.profile_instance.puppet_arms[3];
		if (d3DEquipment == null)
		{
			if (null != transform)
			{
				transform.gameObject.active = true;
			}
			if (null != transform2)
			{
				transform2.gameObject.active = true;
			}
			if (null != transform3)
			{
				transform3.gameObject.active = true;
			}
			if (null != transform4)
			{
				transform4.gameObject.active = true;
			}
		}
		else if (D3DMain.Instance.D3DHelmConfigManager.ContainsKey(d3DEquipment.use_model))
		{
			D3DHelmConfig d3DHelmConfig = D3DMain.Instance.D3DHelmConfigManager[d3DEquipment.use_model];
			if (null != transform)
			{
				transform.gameObject.active = d3DHelmConfig.draw_ears;
			}
			if (null != transform2)
			{
				transform2.gameObject.active = d3DHelmConfig.draw_hair;
			}
			if (null != transform3)
			{
				transform3.gameObject.active = d3DHelmConfig.draw_nose;
			}
			if (null != transform4)
			{
				transform4.gameObject.active = d3DHelmConfig.draw_beard;
			}
		}
	}
}
