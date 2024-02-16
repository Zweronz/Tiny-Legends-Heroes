using UnityEngine;

public class LaunchingPt : MonoBehaviour
{
	public GameObject TargetPoint;

	public MissileComponent Missile;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Work();
		}
	}

	private void Work()
	{
		GameObject gameObject = (GameObject)Object.Instantiate(Missile.gameObject, base.transform.position, Quaternion.LookRotation(TargetPoint.transform.position - base.transform.position));
		MissileComponent component = gameObject.GetComponent<MissileComponent>();
		component.InitMissile(base.transform.position, TargetPoint.transform.position, null, null, null);
	}
}
