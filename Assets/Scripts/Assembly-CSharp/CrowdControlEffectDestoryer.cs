using System.Collections;
using UnityEngine;

public class CrowdControlEffectDestoryer : MonoBehaviour
{
	public IEnumerator StopControlEffect(BasicEffectComponent effect, D3DAudioBehaviour sfx, float delay)
	{
		yield return new WaitForSeconds(delay);
		if (null != effect)
		{
			effect.Stop();
		}
		if (null != sfx)
		{
			sfx.Stop();
		}
		Object.Destroy(base.gameObject);
	}
}
