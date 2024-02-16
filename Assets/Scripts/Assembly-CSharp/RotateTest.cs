using UnityEngine;

public class RotateTest : MonoBehaviour
{
	private GameObject arrow1;

	private GameObject arrow2;

	private GameObject arrow3;

	private void Start()
	{
		arrow1 = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/Projectile/arrow_test"));
		arrow2 = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/Projectile/arrow_test"));
		arrow3 = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/Projectile/arrow_test"));
		arrow1.transform.localScale = new Vector3(1f, 1f, 5f);
		arrow2.transform.localScale = new Vector3(1f, 1f, 3f);
		arrow2.transform.Rotate(Vector3.up * 90f);
		arrow3.transform.Rotate(Vector3.up * (Mathf.Atan2(3f, 5f) * 57.29578f));
	}

	private void Update()
	{
	}
}
