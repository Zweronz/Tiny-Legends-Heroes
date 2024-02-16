using System.Collections;
using UnityEngine;

public class RangeTest : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(0.5f);
		Object.Destroy(base.gameObject);
	}

	private void Update()
	{
	}
}
