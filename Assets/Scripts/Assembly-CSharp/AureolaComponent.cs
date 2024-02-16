using UnityEngine;

public class AureolaComponent : MonoBehaviour
{
	public enum AureolaState
	{
		ZOOM = 0,
		FLASH = 1,
		STOP = 2
	}

	private GameObject aureola;

	private Animation animation_component;

	private Vector3 default_scale;

	private void Awake()
	{
		aureola = base.transform.Find("aureola").gameObject;
		animation_component = aureola.GetComponent<Animation>();
	}

	public void SetAureolaDefaultSize(Vector3 scale)
	{
		base.transform.localScale = scale;
		default_scale = scale;
	}

	public void SetAureolaMaterial(Material mat)
	{
		aureola.GetComponentInChildren<Renderer>().material = mat;
	}

	public void SetAureolaState(AureolaState state)
	{
		switch (state)
		{
		case AureolaState.ZOOM:
			animation_component.Play("PuppetRingZoom");
			aureola.GetComponentInChildren<Renderer>().material = (Material)Resources.Load("Dungeons3D/Images/ring_hero_M");
			break;
		case AureolaState.STOP:
			animation_component.Stop();
			aureola.transform.localScale = Vector3.one;
			base.transform.localScale = default_scale;
			aureola.GetComponentInChildren<Renderer>().material = (Material)Resources.Load("Dungeons3D/Images/ring_green_M");
			break;
		case AureolaState.FLASH:
			animation_component.Play("PuppetRingFlash");
			aureola.GetComponentInChildren<Renderer>().material = (Material)Resources.Load("Dungeons3D/Images/ring_green_M");
			break;
		}
	}
}
