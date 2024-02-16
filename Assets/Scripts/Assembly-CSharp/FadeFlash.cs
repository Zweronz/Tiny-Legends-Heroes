using UnityEngine;

public class FadeFlash : MonoBehaviour
{
	private float fade_angle;

	private float fade_rate;

	private Color fade_color;

	private void Awake()
	{
		fade_angle = 0f;
		fade_rate = 10f;
		fade_color = base.transform.GetComponent<Renderer>().material.GetColor("_TintColor");
	}

	private void Start()
	{
	}

	private void Update()
	{
		fade_angle += fade_rate * Time.deltaTime;
		fade_color.a = Mathf.Abs(Mathf.Cos(fade_angle));
		base.transform.GetComponent<Renderer>().material.SetColor("_TintColor", fade_color);
	}
}
