using UnityEngine;

public class PuppetAnimationEvent : MonoBehaviour
{
	private PuppetArena parent_puppet_script;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetParentPuppetScript(PuppetArena puppet)
	{
		parent_puppet_script = puppet;
	}

	private void OnSkillKeyFrame(int frame_index)
	{
		if (!(null == parent_puppet_script))
		{
			parent_puppet_script.OnSkillKeyFrame(frame_index);
		}
	}

	private void PlayEffect(string effect_config)
	{
		if (string.Empty == effect_config)
		{
			return;
		}
		string[] array = effect_config.Split(';');
		if (array.Length == 0)
		{
			return;
		}
		GameObject gameObject = (GameObject)Resources.Load("Dungeons3D/Prefabs/BattleEffects/" + array[0]);
		if (null == gameObject)
		{
			return;
		}
		GameObject gameObject2 = base.gameObject;
		if (array.Length > 1)
		{
			GameObject gameObject3 = D3DMain.Instance.FindGameObjectChild(gameObject2, array[1]);
			if (null != gameObject3)
			{
				gameObject2 = gameObject3;
			}
		}
		gameObject = (GameObject)Object.Instantiate(gameObject, gameObject2.transform.position, base.transform.rotation);
		gameObject.GetComponent<BasicEffectComponent>().Play(true, null, null, Vector3.zero, Vector2.one, 0f);
	}
}
