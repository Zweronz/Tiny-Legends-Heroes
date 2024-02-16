using UnityEngine;

public class SmoothZoom : MonoBehaviour
{
	public Vector3 defaultScale;

	private Vector3 zoomSpeed;

	private Vector3 zoomAngle;

	private Vector3 extent;

	private bool sleep;

	private void Awake()
	{
		defaultScale = base.gameObject.transform.localScale;
		zoomSpeed = Vector3.zero;
		zoomAngle = Vector3.zero;
		extent = Vector3.zero;
		sleep = true;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (!sleep)
		{
			zoomAngle += zoomSpeed * Time.deltaTime;
			Vector3 vector = new Vector3(extent.x * Mathf.Cos(zoomAngle.x), extent.x * Mathf.Cos(zoomAngle.y), extent.x * Mathf.Cos(zoomAngle.z));
			base.gameObject.transform.localScale = defaultScale + vector;
		}
	}

	public void StartZoom(Vector3 zoomSpeed, Vector3 extent_percent)
	{
		defaultScale = base.gameObject.transform.localScale;
		this.zoomSpeed = zoomSpeed;
		extent = new Vector3(defaultScale.x * extent_percent.x, defaultScale.y * extent_percent.y, defaultScale.z * extent_percent.z);
		sleep = false;
	}

	public void StopZoom(bool reset)
	{
		sleep = true;
		if (reset)
		{
			base.gameObject.transform.localScale = defaultScale;
			zoomAngle = Vector3.zero;
		}
	}
}
