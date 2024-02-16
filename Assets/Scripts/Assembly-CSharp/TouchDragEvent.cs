using UnityEngine;

public class TouchDragEvent
{
	private enum LockType
	{
		FLOOR = 0,
		FRIEND = 1,
		ENEMY = 2
	}

	private SceneArena scene_arena;

	private int event_finger_id;

	private GameObject target_aureola;

	private GameObject drag_line;

	private LockType lock_type;

	private PuppetArena pick_puppet;

	private PuppetArena target_puppet;

	private Vector3 floor_point;

	private bool moved;

	private bool instant_touch;

	public bool invaild_touch;

	public PuppetArena PickPuppet
	{
		get
		{
			return pick_puppet;
		}
	}

	public bool InstantTouch
	{
		get
		{
			return instant_touch;
		}
	}

	public TouchDragEvent(SceneArena scene, int finger_id, PuppetArena puppet, bool instant)
	{
		scene_arena = scene;
		event_finger_id = finger_id;
		lock_type = LockType.FLOOR;
		moved = false;
		instant_touch = instant;
		invaild_touch = false;
		target_aureola = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/GamePlay/TargetPuppetRing"));
		target_aureola.SetActiveRecursively(false);
		drag_line = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/GamePlay/DragLine"));
		drag_line.SetActiveRecursively(false);
		D3DMain.Instance.SetGameObjectMeshColor(D3DMain.Instance.FindGameObjectChild(drag_line, "patch"), Color.green);
		pick_puppet = puppet;
		target_puppet = pick_puppet;
	}

	public bool TouchEquals(int finger_id)
	{
		if (null == pick_puppet || pick_puppet.IsDead())
		{
			return false;
		}
		return finger_id == event_finger_id;
	}

	public void TouchMove(Vector3 point, PuppetArena target)
	{
		if (null == pick_puppet || pick_puppet.IsDead())
		{
			DestroyDragComponents();
			return;
		}
		floor_point = point;
		if (null != target)
		{
			if (target == target_puppet)
			{
				return;
			}
			if (pick_puppet.CheckVaildTarget(target))
			{
				target_puppet = target;
				lock_type = LockType.FLOOR;
			}
			else
			{
				target_puppet = null;
			}
		}
		else
		{
			target_puppet = null;
		}
		if (!target_aureola.active)
		{
			target_aureola.SetActiveRecursively(true);
		}
		if (!drag_line.active)
		{
			drag_line.SetActiveRecursively(true);
		}
		moved = true;
		if (!target_aureola.active && !drag_line.active)
		{
			return;
		}
		if (null != target_puppet)
		{
			floor_point = new Vector3(target_puppet.transform.position.x, 0f, target_puppet.transform.position.z);
			bool flag = pick_puppet.tag != target_puppet.tag;
			if (lock_type != LockType.ENEMY && flag)
			{
				lock_type = LockType.ENEMY;
				SetDragComponentsColor();
				float num = target_puppet.ControllerRadius * 2f;
				target_aureola.transform.localScale = new Vector3(num, 1f, num);
			}
			else if (lock_type != LockType.FRIEND && !flag)
			{
				lock_type = LockType.FRIEND;
				SetDragComponentsColor();
				float num2 = target_puppet.ControllerRadius * 2f;
				target_aureola.transform.localScale = new Vector3(num2, 1f, num2);
			}
		}
		else if (lock_type != 0)
		{
			lock_type = LockType.FLOOR;
			SetDragComponentsColor();
			target_aureola.transform.localScale = new Vector3(1.5f, 1f, 1.5f);
		}
		Vector3 vector = new Vector3(pick_puppet.transform.position.x, 0f, pick_puppet.transform.position.z);
		Vector3 forward = floor_point - vector;
		Quaternion quaternion = Quaternion.LookRotation(forward);
		Vector3 pointInSphere = D3DPlaneGeometry.GetPointInSphere(vector, quaternion, pick_puppet.ControllerRadius);
		Vector3 pointInSphere2 = D3DPlaneGeometry.GetPointInSphere(floor_point, Quaternion.Euler(quaternion.eulerAngles + Vector3.right * 180f), target_aureola.transform.localScale.x * 0.5f);
		float z = Vector3.Distance(pointInSphere2, pointInSphere);
		drag_line.transform.localScale = new Vector3(0.08f, 1f, z);
		drag_line.transform.position = new Vector3((pointInSphere.x + pointInSphere2.x) * 0.5f, 0f, (pointInSphere.z + pointInSphere2.z) * 0.5f);
		drag_line.transform.transform.rotation = quaternion;
		target_aureola.transform.position = floor_point;
	}

	public void TouchEnd()
	{
		DestroyDragComponents();
		if (null == pick_puppet || pick_puppet.IsDead())
		{
			return;
		}
		pick_puppet.ClearHatredPuppet();
		if (null != target_puppet)
		{
			if (target_puppet == pick_puppet && !moved)
			{
				if (null == scene_arena.activing_puppet || scene_arena.activing_puppet != pick_puppet)
				{
					if (null != scene_arena.activing_puppet)
					{
						scene_arena.activing_puppet.OnCancelChoosed();
					}
					scene_arena.activing_puppet = pick_puppet;
					scene_arena.activing_puppet.OnBeChoosed();
					scene_arena.ui_arena.SetFaceButtonDown(scene_arena.activing_puppet);
				}
			}
			else
			{
				pick_puppet.SetTarget(target_puppet);
			}
		}
		else
		{
			pick_puppet.SetTarget(floor_point);
		}
	}

	public void TouchUpdate()
	{
		if (null == pick_puppet || pick_puppet.IsDead())
		{
			DestroyDragComponents();
			invaild_touch = true;
		}
		else
		{
			if (!target_aureola.active && !drag_line.active)
			{
				return;
			}
			Vector3 vector = floor_point;
			if (null != target_puppet)
			{
				if (!target_puppet.IsDead() && pick_puppet.CheckVaildTarget(target_puppet))
				{
					vector = new Vector3(target_puppet.transform.position.x, 0f, target_puppet.transform.position.z);
				}
				else
				{
					target_puppet = null;
					lock_type = LockType.FLOOR;
					SetDragComponentsColor();
					target_aureola.transform.localScale = new Vector3(1.5f, 1f, 1.5f);
				}
			}
			Vector3 vector2 = new Vector3(pick_puppet.transform.position.x, 0f, pick_puppet.transform.position.z);
			Vector3 vector3 = vector - vector2;
			Quaternion quaternion;
			if (Vector3.zero == vector3)
			{
				quaternion = Quaternion.identity;
				drag_line.SetActiveRecursively(false);
			}
			else
			{
				quaternion = Quaternion.LookRotation(vector3);
				drag_line.SetActiveRecursively(true);
			}
			Vector3 pointInSphere = D3DPlaneGeometry.GetPointInSphere(vector2, quaternion, pick_puppet.ControllerRadius);
			Vector3 pointInSphere2 = D3DPlaneGeometry.GetPointInSphere(vector, Quaternion.Euler(quaternion.eulerAngles + Vector3.right * 180f), target_aureola.transform.localScale.x * 0.5f);
			float z = Vector3.Distance(pointInSphere2, pointInSphere);
			drag_line.transform.localScale = new Vector3(0.08f, 1f, z);
			drag_line.transform.position = new Vector3((pointInSphere.x + pointInSphere2.x) * 0.5f, 0f, (pointInSphere.z + pointInSphere2.z) * 0.5f);
			drag_line.transform.transform.rotation = quaternion;
			target_aureola.transform.position = vector;
		}
	}

	private void SetDragComponentsColor()
	{
		string empty = string.Empty;
		empty = ((lock_type == LockType.ENEMY) ? "red" : ((lock_type != LockType.FRIEND) ? "green" : "yellow"));
		GameObject gameObject = D3DMain.Instance.FindGameObjectChild(target_aureola, "patch");
		gameObject.GetComponentInChildren<Renderer>().material = (Material)Resources.Load("Dungeons3D/Images/" + empty + "_M");
		GameObject gameObject2 = D3DMain.Instance.FindGameObjectChild(drag_line, "patch");
		gameObject2.GetComponentInChildren<Renderer>().material = (Material)Resources.Load("Dungeons3D/Images/" + empty + "01_M");
		if (lock_type != 0)
		{
			Animation componentInChildren = target_aureola.GetComponentInChildren<Animation>();
			componentInChildren.Rewind("PuppetRingLockTarget");
			componentInChildren.Play("PuppetRingLockTarget");
			componentInChildren = drag_line.GetComponentInChildren<Animation>();
			componentInChildren.Rewind("DragLineLockTarget");
			componentInChildren.Play("DragLineLockTarget");
		}
	}

	public void DestroyDragComponents()
	{
		if (null != target_aureola)
		{
			if (target_aureola.active)
			{
				Animation componentInChildren = target_aureola.GetComponentInChildren<Animation>();
				componentInChildren.Play("PuppetRingWither");
				Object.Destroy(target_aureola, componentInChildren["PuppetRingWither"].length);
			}
			else
			{
				Object.DestroyImmediate(target_aureola);
			}
		}
		if (null != drag_line)
		{
			if (drag_line.active)
			{
				Animation componentInChildren2 = drag_line.GetComponentInChildren<Animation>();
				componentInChildren2.Rewind("DragLineWither");
				componentInChildren2.Play("DragLineWither");
				Object.Destroy(drag_line, componentInChildren2["DragLineWither"].length);
			}
			else
			{
				Object.DestroyImmediate(drag_line);
			}
		}
	}
}
