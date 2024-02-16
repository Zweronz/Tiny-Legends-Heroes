using UnityEngine;

public class ArrowComponent : MonoBehaviour
{
	public void StartAnimation()
	{
		base.gameObject.SetActiveRecursively(true);
		Animation component = GetComponent<Animation>();
		component.Rewind();
		component.Play("ArrowUV");
	}

	private void OnAnimationEnd()
	{
		base.gameObject.SetActiveRecursively(false);
	}
}
