using UnityEngine;

public class PuppetComponents : MonoBehaviour
{
	private class BarChild
	{
		private enum EPoint
		{
			TopLeft = 0,
			BottomLeft = 1,
			BottomRight = 2,
			TopRight = 3
		}

		private MeshFilter mesh_filter;

		private Vector3[] mesh_vertices;

		private float bar_length;

		private int[] rightVerticesIndex = new int[2];

		private int[] leftVericesIndex = new int[2];

		public Vector3[] RightVertices
		{
			get
			{
				return new Vector3[2]
				{
					mesh_vertices[3],
					mesh_vertices[2]
				};
			}
		}

		public BarChild(MeshFilter mesh_filter)
		{
			this.mesh_filter = mesh_filter;
			mesh_vertices = this.mesh_filter.mesh.vertices;
			bar_length = Vector3.Distance(mesh_vertices[1], mesh_vertices[2]);
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < mesh_vertices.Length; i++)
			{
				if (mesh_vertices[i].x < 0f)
				{
					rightVerticesIndex[num++] = i;
				}
				else
				{
					leftVericesIndex[num2++] = i;
				}
			}
		}

		public void SetBarClip(float scale)
		{
			if (scale > 1f)
			{
				scale = 1f;
			}
			else if (scale < 0f)
			{
				scale = 0f;
			}
			mesh_vertices[rightVerticesIndex[0]].x = mesh_vertices[leftVericesIndex[0]].x - bar_length * scale;
			mesh_vertices[rightVerticesIndex[1]].x = mesh_vertices[leftVericesIndex[0]].x - bar_length * scale;
			mesh_filter.mesh.vertices = mesh_vertices;
		}

		public void SetLeftPoints(Vector3[] left_vertices)
		{
			mesh_vertices[leftVericesIndex[0]] = left_vertices[0];
			mesh_vertices[leftVericesIndex[1]] = left_vertices[1];
			mesh_filter.mesh.vertices = mesh_vertices;
		}

		public void Decay(float decay_rate)
		{
			if (mesh_vertices[rightVerticesIndex[0]].x > mesh_vertices[leftVericesIndex[0]].x)
			{
				mesh_vertices[rightVerticesIndex[0]].x = mesh_vertices[leftVericesIndex[0]].x;
				mesh_vertices[rightVerticesIndex[1]].x = mesh_vertices[leftVericesIndex[0]].x;
				mesh_filter.mesh.vertices = mesh_vertices;
			}
			else
			{
				mesh_vertices[rightVerticesIndex[0]].x = mesh_vertices[rightVerticesIndex[0]].x + decay_rate;
				mesh_vertices[rightVerticesIndex[1]].x = mesh_vertices[rightVerticesIndex[1]].x + decay_rate;
				mesh_filter.mesh.vertices = mesh_vertices;
			}
		}
	}

	public enum BarType
	{
		PLAYER = 0,
		ENEMY = 1
	}

	public enum MpType
	{
		HIDE = 0,
		MP = 1,
		SP = 2
	}

	public enum RingAnimateState
	{
		ZOOM = 0,
		STOP = 1,
		FLASH = 2
	}

	private GameObject host_puppet;

	public GameObject BarsObj;

	public GameObject BoardObj;

	private BarChild[] Bars;

	public GameObject Ring;

	private Animation ring_animation;

	private Vector3 ring_default_scale;

	private Renderer ring_render;

	private Material normal_mat;

	private Material choosing_mat;

	public GameObject Shadow;

	private void Awake()
	{
		Bars = new BarChild[3];
	}

	private void Update()
	{
		base.transform.position = host_puppet.transform.position;
		if (null != BarsObj)
		{
			BarsObj.transform.LookAt(Camera.main.transform);
			Vector3 eulerAngles = BarsObj.transform.eulerAngles;
			BarsObj.transform.eulerAngles = new Vector3(eulerAngles.x, 0f, eulerAngles.z);
			if (Bars[2] != null)
			{
				Bars[2].Decay(Time.deltaTime * 0.25f);
			}
		}
	}

	public void Initialize(GameObject host_puppet, bool use_bars, bool use_ring, bool use_shadow)
	{
		this.host_puppet = host_puppet;
		if (use_bars)
		{
			BarsObj.SetActiveRecursively(false);
		}
		else
		{
			Object.Destroy(BarsObj);
		}
		if (use_ring)
		{
			Ring.SetActiveRecursively(false);
		}
		else
		{
			Object.Destroy(Ring);
		}
		if (use_shadow)
		{
			Shadow.SetActiveRecursively(false);
		}
		else
		{
			Object.Destroy(Shadow);
		}
		base.transform.position = this.host_puppet.transform.position;
		base.transform.rotation = Quaternion.identity;
	}

	public void InitializeBars(BarType bar_type, MpType mp_type)
	{
		BarsObj.SetActiveRecursively(true);
		GameObject gameObject = D3DMain.Instance.FindGameObjectChild(host_puppet, "mount_blood");
		if (null != gameObject)
		{
			BarsObj.transform.localPosition = gameObject.transform.position - host_puppet.transform.position;
		}
		Transform transform = BoardObj.transform.Find("Bar1");
		if (bar_type == BarType.ENEMY)
		{
			Material material = (Material)Resources.Load("Dungeons3D/Images/jingyantiaoditu_M");
			BoardObj.GetComponent<MeshRenderer>().material = material;
			BoardObj.transform.localScale = new Vector3(2f, 0.47f, 1f);
			transform.transform.localScale = new Vector3(0.91f, 0.3f, 1f);
			transform.transform.localPosition = new Vector3(0f, 0.2f, 0.015f);
			transform.GetComponent<MeshRenderer>().material = (Material)Resources.Load("Dungeons3D/Images/xuetiaoY_M");
			Shader shader = Shader.Find("Triniti/Character/COL_2S_AB");
			transform.GetComponent<MeshRenderer>().material.shader = shader;
			transform.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, 0.7f);
		}
		Bars[0] = new BarChild(transform.GetComponent<MeshFilter>());
		transform = BoardObj.transform.Find("Bar2");
		if (mp_type != 0)
		{
			Bars[1] = new BarChild(transform.GetComponent<MeshFilter>());
			if (mp_type == MpType.SP)
			{
				transform.GetComponent<MeshRenderer>().material = (Material)Resources.Load("Dungeons3D/Images/nengliangtiao_M");
			}
		}
		else
		{
			Object.Destroy(transform.gameObject);
		}
		transform = BoardObj.transform.Find("DmgBar");
		if (bar_type == BarType.ENEMY)
		{
			transform.transform.localScale = new Vector3(0.91f, 0.3f, 1f);
			transform.transform.localPosition = new Vector3(0f, 0.2f, 0.015f);
		}
		Bars[2] = new BarChild(transform.GetComponent<MeshFilter>());
		Bars[2].SetLeftPoints(Bars[0].RightVertices);
	}

	public void ClipBar(bool is_hp, float scale)
	{
		if (is_hp)
		{
			if (Bars[0] != null)
			{
				Bars[0].SetBarClip(scale);
				Bars[2].SetLeftPoints(Bars[0].RightVertices);
			}
		}
		else if (Bars[1] != null)
		{
			Bars[1].SetBarClip(scale);
		}
	}

	public void ClipBar(float hp_scale, float mp_scale)
	{
		if (Bars[0] != null)
		{
			Bars[0].SetBarClip(hp_scale);
			Bars[2].SetLeftPoints(Bars[0].RightVertices);
		}
		if (Bars[1] != null)
		{
			Bars[1].SetBarClip(mp_scale);
		}
	}

	public void BarVisible(bool visible)
	{
		BarsObj.SetActiveRecursively(visible);
	}

	public void RingVisible(bool visible)
	{
		Ring.SetActiveRecursively(visible);
	}

	public void InitializeRing(Vector3 scale, Material normal_mat, Material choosing_mat)
	{
		Ring.SetActiveRecursively(true);
		Ring.transform.localScale = scale;
		ring_default_scale = scale;
		ring_animation = Ring.GetComponentInChildren<Animation>();
		this.normal_mat = normal_mat;
		this.choosing_mat = choosing_mat;
		ring_render = ring_animation.GetComponentInChildren<Renderer>();
		ring_render.material = this.normal_mat;
	}

	public void SetRingState(RingAnimateState animate_state)
	{
		if (!(null == Ring))
		{
			switch (animate_state)
			{
			case RingAnimateState.ZOOM:
				ring_animation.Play("PuppetRingZoom");
				ring_render.material = choosing_mat;
				break;
			case RingAnimateState.STOP:
				ring_animation.Stop();
				ring_animation.transform.localScale = Vector3.one;
				Ring.transform.localScale = ring_default_scale;
				ring_render.material = normal_mat;
				break;
			case RingAnimateState.FLASH:
				ring_animation.Play("PuppetRingFlash");
				ring_render.material = normal_mat;
				break;
			}
		}
	}

	public void InitializeShadow(Vector3 scale)
	{
		Shadow.SetActiveRecursively(true);
		Shadow.transform.localScale = scale;
	}

	public void SetExpComponent()
	{
		Material material = (Material)Resources.Load("Dungeons3D/Images/jingyantiaoditu_M");
		BoardObj.GetComponent<MeshRenderer>().material = material;
		BoardObj.transform.localScale = new Vector3(2f, 0.47f, 1f);
		Transform transform = BoardObj.transform.Find("Bar1");
		transform.transform.localScale = new Vector3(0.91f, 0.3f, 1f);
		transform.transform.localPosition = new Vector3(0f, 0.2f, 0.015f);
		transform.GetComponent<MeshRenderer>().material = (Material)Resources.Load("Dungeons3D/Images/jingyantiao_M");
		if (Bars[1] != null)
		{
			Object.Destroy(BoardObj.transform.Find("Bar2").gameObject);
			Bars[1] = null;
		}
		if (Bars[2] != null)
		{
			BoardObj.transform.Find("DmgBar").gameObject.active = false;
		}
	}
}
