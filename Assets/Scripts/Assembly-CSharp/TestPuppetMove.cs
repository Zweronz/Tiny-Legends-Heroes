using UnityEngine;

public class TestPuppetMove : MonoBehaviour
{
	private Vector3 left_position;

	private Vector3 right_position;

	private bool left_move;

	private void Start()
	{
		left_position = base.transform.position + Vector3.right * -6f;
		right_position = base.transform.position + Vector3.right * 6f;
		left_move = true;
	}

	private void Update()
	{
		if (left_move)
		{
			base.transform.position -= Vector3.right * 3f * Time.deltaTime;
			if (base.transform.position.x <= left_position.x)
			{
				left_move = false;
			}
		}
		else
		{
			base.transform.position += Vector3.right * 3f * Time.deltaTime;
			if (base.transform.position.x >= right_position.x)
			{
				left_move = true;
			}
		}
	}
}
