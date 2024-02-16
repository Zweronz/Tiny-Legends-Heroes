using UnityEngine;

public class QuadsTest : MonoBehaviour
{
	public float width = 1f;

	public float height = 1f;

	public float radius = 1f;

	public GameObject rect;

	public GameObject circle;

	private void Start()
	{
		rect.transform.localScale = new Vector3(width, 0f, height);
		circle.transform.localScale = new Vector3(radius * 2f, 0f, radius * 2f);
	}

	private void Update()
	{
	}
}
