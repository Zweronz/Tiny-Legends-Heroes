using UnityEngine;

public class BasicEffectComponent : MonoBehaviour
{
	protected bool one_shot;

	protected GameObject effect_host;

	protected GameObject rotation_host;

	protected Vector3 effect_offset;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public virtual void Play(bool one_shot, GameObject effect_host, GameObject rotation_host, Vector3 effect_offset, Vector2 range, float delay)
	{
	}

	public virtual void Stop()
	{
	}

	public static BasicEffectComponent PlayEffect(string effect_name, GameObject effect_host, string mount_point, bool bind, Vector2 range, Vector3 offset, bool one_shot, float delay_time)
	{
		if (string.Empty == effect_name)
		{
			return null;
		}
		if (null == effect_host)
		{
			return null;
		}
		GameObject gameObject = Resources.Load("Dungeons3D/Prefabs/BattleEffects/" + effect_name) as GameObject;
		if (null == gameObject)
		{
			return null;
		}
		GameObject gameObject2 = effect_host;
		if (string.Empty != mount_point)
		{
			GameObject gameObject3 = D3DMain.Instance.FindGameObjectChild(effect_host, mount_point);
			if (null != gameObject3)
			{
				gameObject2 = gameObject3;
			}
		}
		Vector3 position = gameObject2.transform.position;
		Quaternion rotation = effect_host.transform.rotation;
		if (!bind)
		{
			gameObject2 = null;
			effect_host = null;
		}
		gameObject = (GameObject)Object.Instantiate(gameObject, position + rotation * offset, rotation);
		BasicEffectComponent component = gameObject.GetComponent<BasicEffectComponent>();
		component.Play(one_shot, gameObject2, effect_host, offset, range, delay_time);
		return component;
	}

	public static BasicEffectComponent PlayEffect(string effect_name, Vector3 effect_position, Quaternion effect_rotation, Vector2 range, Vector3 offset, bool one_shot, float delay_time)
	{
		if (string.Empty == effect_name)
		{
			return null;
		}
		GameObject gameObject = Resources.Load("Dungeons3D/Prefabs/BattleEffects/" + effect_name) as GameObject;
		if (null == gameObject)
		{
			return null;
		}
		gameObject = (GameObject)Object.Instantiate(gameObject, effect_position + effect_rotation * offset, effect_rotation);
		BasicEffectComponent component = gameObject.GetComponent<BasicEffectComponent>();
		component.Play(one_shot, null, null, offset, range, delay_time);
		return component;
	}
}
