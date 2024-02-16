using UnityEngine;

public class ScheduleDestroy : MonoBehaviour
{
	public float waitTime;

	private void Start()
	{
		Object.Destroy(base.gameObject, waitTime);
	}

	private void Update()
	{
	}
}
