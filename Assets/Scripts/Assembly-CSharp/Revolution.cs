using UnityEngine;

public class Revolution : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		base.transform.RotateAround(base.transform.parent.transform.position, Vector3.up, 360f * Time.deltaTime);
	}
}
