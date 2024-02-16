using UnityEngine;

public class ScrollInertiaBehaviour : MonoBehaviour
{
	private D3DScrollManager scroll_manager;

	private Vector2 inertia;

	public D3DScrollManager ScrollManager
	{
		set
		{
			scroll_manager = value;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		scroll_manager.Scroll(inertia);
		inertia -= inertia * 0.1f;
		if (Mathf.Abs(inertia.x) < 1f)
		{
			inertia = new Vector2(0f, inertia.y);
		}
		if (Mathf.Abs(inertia.y) < 1f)
		{
			inertia = new Vector2(inertia.x, 0f);
		}
		if (inertia == Vector2.zero)
		{
			base.enabled = false;
		}
	}

	public void StartInertia(Vector2 inertia)
	{
		this.inertia = inertia;
	}
}
