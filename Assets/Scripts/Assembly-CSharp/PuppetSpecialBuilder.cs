using UnityEngine;

public class PuppetSpecialBuilder : PuppetModelBuilder
{
	private new void Start()
	{
	}

	private new void Update()
	{
	}

	public override void BuildPuppetModel()
	{
		string puppetFeatureModel = puppet_component.profile_instance.PuppetFeatureModel;
		string text = puppetFeatureModel + puppet_component.profile_instance.PuppetFeatureSkin;
		if (!D3DMain.Instance.PuppetTransformManager.ContainsKey(puppetFeatureModel))
		{
			puppet_transform_cfg = D3DMain.Instance.DefaultPuppetTransform;
		}
		else
		{
			puppet_transform_cfg = D3DMain.Instance.PuppetTransformManager[puppetFeatureModel];
		}
		Object @object = Resources.Load("Dungeons3D/Prefabs/Character/Special/" + puppetFeatureModel + "/" + text);
		if (null == @object)
		{
			return;
		}
		int num = 0;
		model_parts[num] = (GameObject)Object.Instantiate(@object);
		Transform transform = model_parts[num].transform;
		transform.parent = base.transform;
		transform.localPosition = Vector3.zero;
		Transform transform2 = transform.Find("body");
		if (null != transform2)
		{
			int num2 = 0;
			int count = puppet_component.profile_instance.PuppetFeatureTextures.Count;
			if (null != transform2.GetComponent<Renderer>())
			{
				Material[] materials = transform2.GetComponent<Renderer>().materials;
				foreach (Material material in materials)
				{
					if (num2 > count - 1)
					{
						num2 = count - 1;
					}
					Material material2 = (Material)Resources.Load("Dungeons3D/Models/Character/Special/" + puppetFeatureModel + "/Materials/" + puppet_component.profile_instance.PuppetFeatureTextures[num2] + "_M");
					if (null != material2)
					{
						material.CopyPropertiesFromMaterial(material2);
						material.shader = material2.shader;
						material.name = material2.name;
					}
					num2++;
				}
				foreach (string puppetOtherTexture in puppet_component.profile_instance.PuppetOtherTextures)
				{
					int num3 = puppetOtherTexture.IndexOf(';');
					int length = puppetOtherTexture.Length;
					string find_name = puppetOtherTexture.Substring(0, num3);
					GameObject gameObject = D3DMain.Instance.FindGameObjectChild(transform.gameObject, find_name);
					if (!(null == gameObject))
					{
						string text2 = puppetOtherTexture.Substring(num3 + 1, length - num3 - 1);
						Material material3 = (Material)Resources.Load("Dungeons3D/Models/Character/Special/" + puppetFeatureModel + "/Materials/" + text2 + "_M");
						if (null != material3)
						{
							gameObject.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(material3);
							gameObject.GetComponent<Renderer>().material.shader = material3.shader;
						}
					}
				}
			}
		}
		foreach (string[] puppetEffect in puppet_component.profile_instance.PuppetEffects)
		{
			GameObject gameObject2 = D3DMain.Instance.FindGameObjectChild(base.gameObject, puppetEffect[0]);
			if (null != gameObject2)
			{
				GameObject gameObject3 = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/ModEffect/" + puppetEffect[1]), gameObject2.transform.position, Quaternion.identity);
				gameObject3.transform.parent = gameObject2.transform;
				gameObject3.transform.localPosition = Vector3.zero;
				gameObject3.transform.rotation = Quaternion.identity;
			}
		}
		GetAnimationComponent();
		Object[] array = Resources.LoadAll("Dungeons3D/Prefabs/Character/Special/" + puppetFeatureModel + "/Animation", typeof(AnimationClip));
		Object[] array2 = array;
		foreach (Object object2 in array2)
		{
			animation_component.AddClip((AnimationClip)object2, object2.name);
		}
		TextAsset textAsset = Resources.Load("Dungeons3D/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("Docs", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt("AnimationIndexs", D3DGamer.Instance.Sk[0])) + "/" + D3DMain.Instance.RemoveIllegalCharacter(XXTEAUtils.Encrypt(puppetFeatureModel + "_animation", D3DGamer.Instance.Sk[0]))) as TextAsset;
		if (!(null == textAsset))
		{
			string text3 = XXTEAUtils.Decrypt(textAsset.text, D3DGamer.Instance.Sk[1]);
			while (text3 != string.Empty)
			{
				int num4 = text3.IndexOf('\n');
				int num5 = text3.IndexOf('>');
				string item = text3.Substring(num5 + 1, num4 - num5 - 2);
				animation_clip_names.Add(item);
				text3 = text3.Remove(0, num4 + 1);
			}
		}
	}

	public override void BuildPuppetFaceFeatureModel()
	{
		BuildPuppetModel();
	}

	public override void ChangeModelParts(D3DPuppetProfile.PuppetArms arm_part)
	{
	}

	public override void ChangeFaceFeatureModel(D3DPuppetProfile.PuppetArms arm_part)
	{
	}

	public override void RemoveModelParts(D3DPuppetProfile.PuppetArms arm_part, bool bUseDefaultPart = true)
	{
	}

	public override void RemoveFaceFeatureModelParts(D3DPuppetProfile.PuppetArms arm_part)
	{
	}

	public override void ChangeAnimation(string animation)
	{
	}
}
