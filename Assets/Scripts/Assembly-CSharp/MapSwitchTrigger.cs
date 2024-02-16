using UnityEngine;

public class MapSwitchTrigger : MonoBehaviour
{
	public enum TriggerType
	{
		PREVIOUS_TRIGGER = 0,
		NEXT_TRIGGER = 1,
		ROOM_TRIGGER = 2,
		MAP_TRIGGER = 3
	}

	private enum TriggerPath
	{
		QUIT_DUNGEON = 0,
		SWITCH_FLOOR = 1,
		SWITCH_ROOM = 2
	}

	public SceneDungeon scene_dungeon;

	public TriggerType trigger_type;

	public MapSwitchTrigger target_trigger;

	public float player_face_direction;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!("Player" == other.tag))
		{
			return;
		}
		if (scene_dungeon.first_enter)
		{
			scene_dungeon.first_enter = false;
			return;
		}
		other.GetComponent<PuppetDungeon>().CancelMove();
		int current_floor = D3DMain.Instance.exploring_dungeon.current_floor;
		TriggerPath triggerPath;
		if (trigger_type == TriggerType.PREVIOUS_TRIGGER)
		{
			if (current_floor < 1)
			{
				return;
			}
			current_floor--;
			if (current_floor == 0 && D3DMain.Instance.exploring_dungeon.dungeon.dungeon_town == null)
			{
				D3DMain.Instance.LoadingScene = 1;
				triggerPath = TriggerPath.QUIT_DUNGEON;
			}
			else
			{
				D3DMain.Instance.exploring_dungeon.floor_transfer_type = ExploringDungeon.FloorTransferType.NEXT;
				triggerPath = TriggerPath.SWITCH_FLOOR;
				D3DMain.Instance.exploring_dungeon.player_last_transform = null;
				scene_dungeon.ClearOldFloor();
			}
			D3DMain.Instance.exploring_dungeon.current_floor = current_floor;
			scene_dungeon.ui_dungeon.StopDungeonMusic(true);
		}
		else if (trigger_type == TriggerType.NEXT_TRIGGER)
		{
			current_floor++;
			if (current_floor > D3DMain.Instance.exploring_dungeon.dungeon.dungeon_floors.Count)
			{
				return;
			}
			D3DMain.Instance.exploring_dungeon.floor_transfer_type = ExploringDungeon.FloorTransferType.PREVIOUS;
			triggerPath = TriggerPath.SWITCH_FLOOR;
			D3DMain.Instance.exploring_dungeon.player_last_transform = null;
			scene_dungeon.ClearOldFloor();
			D3DMain.Instance.exploring_dungeon.current_floor = current_floor;
			scene_dungeon.ui_dungeon.StopDungeonMusic(true);
		}
		else
		{
			if (trigger_type == TriggerType.MAP_TRIGGER || null == target_trigger)
			{
				return;
			}
			triggerPath = TriggerPath.SWITCH_ROOM;
			scene_dungeon.target_room_trigger = target_trigger;
		}
		switch (triggerPath)
		{
		case TriggerPath.QUIT_DUNGEON:
			scene_dungeon.ui_dungeon.EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, scene_dungeon.ui_dungeon.QuitDungeon, false);
			break;
		case TriggerPath.SWITCH_FLOOR:
			scene_dungeon.ui_dungeon._subUIDungeonStash.ShowIcons(false, 0.01f);
			scene_dungeon.ui_dungeon.EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, scene_dungeon.ui_dungeon.SwitchFloor, false);
			SceneDungeon.CampBackLastLevel = -1;
			break;
		default:
			scene_dungeon.ui_dungeon.EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, scene_dungeon.SwitchRoom, false);
			break;
		}
		Time.timeScale = 0.0001f;
	}
}
