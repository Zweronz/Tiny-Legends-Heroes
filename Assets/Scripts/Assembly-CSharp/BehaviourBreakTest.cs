using System.Collections;
using UnityEngine;

public class BehaviourBreakTest : MonoBehaviour
{
	private void Awake()
	{
	}

	private void Start()
	{
		StartCoroutine("Schedule");
		Invoke("InvokeTest", 3f);
	}

	private void Update()
	{
	}

	private IEnumerator Schedule()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.5f);
		}
	}

	private void InvokeTest()
	{
	}
}
