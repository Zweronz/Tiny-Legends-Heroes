using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDungeon : MonoBehaviour
{
	public PuppetDungeon map_player;

	private GameObject targetArrow;

	private Vector3 camera_vector_offset;

	public UIDungeon ui_dungeon;

	public List<GameObject> map_obj_manager;

	public List<PuppetDungeon> dungeon_enemy_groups;

	private MapSwitchTrigger[] map_triggers;

	public MapSwitchTrigger target_room_trigger;

	public bool first_enter = true;

	public bool enter_forward = true;

	public static float player_protect_time = 0f;

	public static Dictionary<int, DungeonFloorEnemyStateRecord> floor_enemy_records = new Dictionary<int, DungeonFloorEnemyStateRecord>();

	public static int CampBackLastLevel = -1;

	public static Vector3 CampBackLastPosition = Vector3.zero;

	private Dictionary<int, GameObject> test_counters = new Dictionary<int, GameObject>();

	private Dictionary<string, GameObject> test_treasureCounters = new Dictionary<string, GameObject>();

	private Dictionary<int, GameObject> _respawnTimeCounters = new Dictionary<int, GameObject>();

	private bool doing_portal;

	private float touch_down_time;

	public PuppetDungeon picking_interactive_npc;

	public string CurrentModelPostfix = string.Empty;

	private bool play_dungeon_bgm;

	private D3DDungeonModelPreset.ModelPreset dungeon_model_preset;

	public bool SpawnedGrave;

	public bool DoingPortal
	{
		get
		{
			return doing_portal;
		}
		set
		{
			doing_portal = value;
		}
	}

	private void Awake()
	{
		if (null != D3DMain.Instance.HD_BOARD_OBJ)
		{
			D3DMain.Instance.HD_BOARD_OBJ.SetActiveRecursively(false);
		}
		D3DMain.Instance.TriggerApplicationPause = false;
		TAudioManager.instance.AudioListener.transform.position = Vector3.zero;
		TAudioManager.instance.AudioListener.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
		Physics.IgnoreLayerCollision(8, 11, true);
		Physics.IgnoreLayerCollision(8, 9, true);
		Physics.IgnoreLayerCollision(9, 9, true);
		map_obj_manager = new List<GameObject>();
		dungeon_enemy_groups = new List<PuppetDungeon>();
		targetArrow = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/GamePlay/TargetArrow"));
		targetArrow.SetActiveRecursively(false);
		camera_vector_offset = base.transform.position - Vector3.zero;
	}

	private void Start()
	{
		Camera.main.pixelRect = new Rect(((float)Screen.width - 960f * ((float)Screen.height / 640f)) * 0.5f, 0f, 960f * ((float)Screen.height / 640f), Screen.height);
	}

	private void OnApplicationPause(bool pause)
	{
		if (D3DMain.Instance.TriggerApplicationPause)
		{
			ui_dungeon.ApplicationPause();
		}
	}

	private void OnDestroy()
	{
		if (null != D3DMain.Instance.HD_BOARD_OBJ)
		{
			D3DMain.Instance.HD_BOARD_OBJ.SetActiveRecursively(true);
		}
	}

	private void Update()
	{
		if (null != map_player)
		{
			Camera.main.transform.LookAt(map_player.transform.position);
			base.transform.position = map_player.transform.position + camera_vector_offset;
			TAudioManager.instance.AudioListener.transform.position = map_player.transform.position;
		}
		if (!doing_portal)
		{
			CheckFloorSpawner(false);
			DungeonTouchEvent();
		}
	}

	public void ReloadScene()
	{
		D3DMain.Instance.TriggerApplicationPause = false;
		Time.timeScale = 0.0001f;
		foreach (GameObject item in map_obj_manager)
		{
			Object.Destroy(item);
		}
		map_obj_manager.Clear();
		dungeon_enemy_groups.Clear();
		D3DMain.Instance.exploring_dungeon.floor_spawn_points.Clear();
		picking_interactive_npc = null;
		ui_dungeon.UIActiving = false;
		SpawnedGrave = false;
		first_enter = true;
		if (D3DMain.Instance.exploring_dungeon.current_floor - D3DMain.Instance.exploring_dungeon.dungeon.explored_level == 1)
		{
			D3DMain.Instance.exploring_dungeon.dungeon.explored_level = D3DMain.Instance.exploring_dungeon.current_floor;
			D3DGamer.Instance.SaveDungeonProgress();
		}
		ui_dungeon.SetPortalVisible(D3DMain.Instance.exploring_dungeon.dungeon.explored_level > 0);
		if (D3DMain.Instance.exploring_dungeon.current_floor == 0 && D3DMain.Instance.exploring_dungeon.dungeon.dungeon_town == null)
		{
			D3DMain.Instance.exploring_dungeon.floor_transfer_type = ExploringDungeon.FloorTransferType.PREVIOUS;
			D3DMain.Instance.exploring_dungeon.current_floor = 1;
		}
		GameObject gameObject;
		if (D3DMain.Instance.exploring_dungeon.current_floor == 0)
		{
			dungeon_model_preset = D3DDungeonModelPreset.Instance.GetModelPreset(D3DMain.Instance.exploring_dungeon.dungeon.dungeon_town.town_model_preset);
			gameObject = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/Maps/dungeon_" + dungeon_model_preset.ModelPostfix));
			play_dungeon_bgm = true;
			CurrentModelPostfix = dungeon_model_preset.ModelPostfix;
		}
		else
		{
			play_dungeon_bgm = false;
			dungeon_model_preset = D3DDungeonModelPreset.Instance.GetModelPreset(D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_model_preset);
			gameObject = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/Maps/dungeon_" + dungeon_model_preset.ModelPostfix));
			int num = 0;
			while (num < gameObject.transform.GetChildCount())
			{
				Transform child = gameObject.transform.GetChild(num);
				if ("Obstacle" == child.tag && !dungeon_model_preset.EnabledObstacles.Contains(child.name))
				{
					Object.DestroyImmediate(child.gameObject);
				}
				else
				{
					num++;
				}
			}
			if (!D3DMain.Instance.exploring_dungeon.CurrentFloor.open_room1)
			{
				if (null != gameObject.transform.Find("door_01"))
				{
					gameObject.transform.Find("door_01").gameObject.SetActiveRecursively(false);
				}
				if (null != gameObject.transform.Find("Room1Trigger"))
				{
					gameObject.transform.Find("Room1Trigger").gameObject.SetActiveRecursively(false);
				}
				if (null != gameObject.transform.Find("Room1ExitTrigger"))
				{
					gameObject.transform.Find("Room1ExitTrigger").gameObject.SetActiveRecursively(false);
				}
			}
			if (!D3DMain.Instance.exploring_dungeon.CurrentFloor.open_room2)
			{
				if (null != gameObject.transform.Find("door_02"))
				{
					gameObject.transform.Find("door_02").gameObject.SetActiveRecursively(false);
				}
				if (null != gameObject.transform.Find("Room2Trigger"))
				{
					gameObject.transform.Find("Room2Trigger").gameObject.SetActiveRecursively(false);
				}
				if (null != gameObject.transform.Find("Room2ExitTrigger"))
				{
					gameObject.transform.Find("Room2ExitTrigger").gameObject.SetActiveRecursively(false);
				}
			}
			GameObject gameObject2 = Resources.Load("Dungeons3D/Prefabs/Maps/" + D3DMain.Instance.exploring_dungeon.CurrentFloor.spawn_points_prefab) as GameObject;
			if (null != gameObject2)
			{
				for (int i = 0; i < gameObject2.transform.GetChildCount(); i++)
				{
					Transform child2 = gameObject2.transform.GetChild(i);
					if (null != child2.GetComponent<MapSpawner>() && !D3DMain.Instance.exploring_dungeon.floor_spawn_points.ContainsKey(child2.GetComponent<MapSpawner>().spawner_id))
					{
						ExploringDungeon.FloorSpawnerCfg floorSpawnerCfg = new ExploringDungeon.FloorSpawnerCfg();
						floorSpawnerCfg.spawner_position = child2.position;
						floorSpawnerCfg.random_rotation_minY = child2.GetComponent<MapSpawner>().random_rotation_minY;
						floorSpawnerCfg.random_rotation_maxY = child2.GetComponent<MapSpawner>().random_rotation_maxY;
						floorSpawnerCfg.chest_rotation = child2.GetComponent<MapSpawner>().chest_rotation;
						D3DMain.Instance.exploring_dungeon.floor_spawn_points.Add(child2.GetComponent<MapSpawner>().spawner_id, floorSpawnerCfg);
					}
				}
			}
			CurrentModelPostfix = dungeon_model_preset.ModelPostfix;
		}
		gameObject.isStatic = true;
		map_obj_manager.Add(gameObject);
		map_triggers = gameObject.GetComponentsInChildren<MapSwitchTrigger>();
		MapSwitchTrigger[] array = map_triggers;
		foreach (MapSwitchTrigger mapSwitchTrigger in array)
		{
			mapSwitchTrigger.scene_dungeon = this;
		}
		D3DGamer.D3DPuppetSaveData d3DPuppetSaveData = D3DGamer.Instance.PlayerBattleTeamData[0];
		GameObject gameObject3 = new GameObject();
		map_obj_manager.Add(gameObject3);
		map_player = gameObject3.AddComponent<PuppetDungeon>();
		map_player.InitProfileInstance(D3DMain.Instance.GetProfileClone(d3DPuppetSaveData.pupet_profile_id), d3DPuppetSaveData);
		map_player.profile_instance.InitSkillLevel(d3DPuppetSaveData);
		map_player.profile_instance.InitSkillSlots(d3DPuppetSaveData);
		map_player.model_builder.BuildPuppetModel();
		map_player.model_builder.AddAudioController(true);
		map_player.model_builder.PlayPuppetAnimations(true, 2, WrapMode.Loop, true);
		map_player.CheckPuppetWeapons();
		map_player.scene_dungeon = this;
		gameObject3.transform.rotation = Quaternion.identity;
		if (D3DMain.Instance.exploring_dungeon.current_floor != 0)
		{
			gameObject3.AddComponent<PuppetDungeonPlayerBehaviour>();
		}
		map_player.ChangeFootStepInDungeon();
		MapSwitchTrigger mapSwitchTrigger2 = null;
		mapSwitchTrigger2 = ((D3DMain.Instance.exploring_dungeon.floor_transfer_type == ExploringDungeon.FloorTransferType.WORLD_MAP) ? GetTriggerByType(MapSwitchTrigger.TriggerType.MAP_TRIGGER) : ((D3DMain.Instance.exploring_dungeon.floor_transfer_type != ExploringDungeon.FloorTransferType.PREVIOUS) ? GetTriggerByType(MapSwitchTrigger.TriggerType.NEXT_TRIGGER) : GetTriggerByType(MapSwitchTrigger.TriggerType.PREVIOUS_TRIGGER)));
		if (null != mapSwitchTrigger2)
		{
			Vector3 position = mapSwitchTrigger2.transform.Find("TransferPoint").position;
			gameObject3.transform.position = new Vector3(position.x, 0f, position.z);
			gameObject3.transform.Rotate(Vector3.up, mapSwitchTrigger2.player_face_direction);
		}
		else
		{
			gameObject3.transform.position = Vector3.zero;
		}
		if (D3DMain.Instance.exploring_dungeon.player_last_transform != null)
		{
			gameObject3.transform.position = D3DMain.Instance.exploring_dungeon.player_last_transform.position;
			gameObject3.transform.localRotation = D3DMain.Instance.exploring_dungeon.player_last_transform.rotation;
		}
		D3DMain.Instance.SetGameObjectGeneralLayer(map_player.gameObject, 9);
		map_player.tag = "Player";
		map_player.SetPuppetController();
		map_player.InitPuppetComponents();
		CheckFloorSpawner(true);
		if (floor_enemy_records.Keys.Count > 0)
		{
			foreach (PuppetDungeon dungeon_enemy_group in dungeon_enemy_groups)
			{
				PuppetDungeonEnmeyBehaviour component = dungeon_enemy_group.GetComponent<PuppetDungeonEnmeyBehaviour>();
				if (floor_enemy_records.ContainsKey(component.floor_spawner_id))
				{
					DungeonFloorEnemyStateRecord dungeonFloorEnemyStateRecord = floor_enemy_records[component.floor_spawner_id];
					dungeon_enemy_group.transform.position = dungeonFloorEnemyStateRecord.position;
					dungeon_enemy_group.transform.rotation = dungeonFloorEnemyStateRecord.rotation;
					component.record_state = dungeonFloorEnemyStateRecord.behaviour_post_state;
				}
			}
		}
		floor_enemy_records.Clear();
		DungeonNpcConfig.LevelNpcConfig npcConfig = DungeonNpcConfig.Instance.GetNpcConfig(D3DMain.Instance.exploring_dungeon.dungeon.dungeon_id, D3DMain.Instance.exploring_dungeon.current_floor);
		if (npcConfig != null && npcConfig.level_interactive_npc != null)
		{
			foreach (D3DInteractiveNpc item2 in npcConfig.level_interactive_npc)
			{
				gameObject3 = new GameObject();
				map_obj_manager.Add(gameObject3);
				PuppetDungeon puppetDungeon = gameObject3.AddComponent<PuppetDungeon>();
				puppetDungeon.InitProfileInstance(D3DMain.Instance.GetProfileClone(item2.PuppetID), 1);
				puppetDungeon.model_builder.BuildPuppetModel();
				puppetDungeon.model_builder.AddAudioController(false);
				puppetDungeon.model_builder.PlayPuppetAnimations(true, 2, WrapMode.Loop, true);
				puppetDungeon.CheckPuppetWeapons();
				puppetDungeon.SetPuppetController();
				puppetDungeon.transform.position = new Vector3(item2.NpcPosition.x, 0f, item2.NpcPosition.y);
				puppetDungeon.transform.Rotate(Vector3.up * item2.NpcRotationY);
				puppetDungeon.scene_dungeon = this;
				D3DMain.Instance.SetGameObjectGeneralLayer(puppetDungeon.gameObject, 12);
				puppetDungeon.tag = "InteractiveNPC";
				puppetDungeon.InitPuppetComponents();
				puppetDungeon.PuppetRingVisible(false);
				PuppetDungeonInteractiveNpcBehaviour puppetDungeonInteractiveNpcBehaviour = gameObject3.AddComponent<PuppetDungeonInteractiveNpcBehaviour>();
				puppetDungeonInteractiveNpcBehaviour.Init(this, item2.npcFunction);
				puppetDungeonInteractiveNpcBehaviour.enabled = false;
			}
		}
		if (D3DDungeonProgerssManager.Instance.CurrentDungeonProgress != null && D3DDungeonProgerssManager.Instance.CurrentDungeonProgress.ContainsKey(D3DMain.Instance.exploring_dungeon.current_floor))
		{
			D3DDungeonProgerssManager.LevelProgress levelProgress = D3DDungeonProgerssManager.Instance.CurrentDungeonProgress[D3DMain.Instance.exploring_dungeon.current_floor];
			bool flag = false;
			foreach (int key in levelProgress.UnlockBattleList.Keys)
			{
				if (!levelProgress.UnlockBattleList[key].win_target)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				mapSwitchTrigger2 = GetTriggerByType(MapSwitchTrigger.TriggerType.NEXT_TRIGGER);
				if (null != mapSwitchTrigger2)
				{
					GameObject gameObject4 = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/ModEffect/door_close"), mapSwitchTrigger2.transform.parent.transform.position, Quaternion.identity);
					gameObject4.transform.parent = mapSwitchTrigger2.transform.parent.parent;
					gameObject4.transform.position = mapSwitchTrigger2.transform.parent.transform.position;
					Object.Destroy(mapSwitchTrigger2.transform.parent.gameObject);
				}
			}
			if (string.Empty != levelProgress.on_first_enter_story && !levelProgress.read)
			{
				GameObject gameObject5 = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UIStory"));
				gameObject5.GetComponent<UIStory>().Init(levelProgress.on_first_enter_story, StoryEnterLevel);
				return;
			}
		}
		if (play_dungeon_bgm)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetTownMusic(D3DMain.Instance.exploring_dungeon.dungeon.dungeon_id), ref D3DAudioManager.Instance.DungeonTownAudio, null, false, false);
		}
		if (null == D3DAudioManager.Instance.DungeonAmbAudio)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetDungeonAmb(dungeon_model_preset.ModelPostfix), ref D3DAudioManager.Instance.DungeonAmbAudio, null, false, false);
		}
		ui_dungeon.EnterDungeon();
		if (doing_portal)
		{
			map_player.StartCoroutine(map_player.Hide(false, 0f));
			if (CampBackLastLevel > 0 && CampBackLastLevel == D3DMain.Instance.exploring_dungeon.current_floor)
			{
				map_player.transform.position = CampBackLastPosition;
				CampBackLastLevel = -1;
			}
		}
	}

	private void StoryEnterLevel()
	{
		if (null != D3DMain.Instance.HD_BOARD_OBJ)
		{
			D3DMain.Instance.HD_BOARD_OBJ.GetComponentInChildren<Camera>().depth = 0.5f;
			D3DMain.Instance.HD_BOARD_OBJ.SetActiveRecursively(false);
		}
		D3DDungeonProgerssManager.Instance.CurrentDungeonProgress[D3DMain.Instance.exploring_dungeon.current_floor].read = true;
		D3DGamer.Instance.SaveDungeonProgress();
		if (play_dungeon_bgm)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetTownMusic(D3DMain.Instance.exploring_dungeon.dungeon.dungeon_id), ref D3DAudioManager.Instance.DungeonTownAudio, null, false, false);
		}
		if (null == D3DAudioManager.Instance.DungeonAmbAudio)
		{
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetDungeonAmb(dungeon_model_preset.ModelPostfix), ref D3DAudioManager.Instance.DungeonAmbAudio, null, false, false);
		}
		ui_dungeon.EnterDungeon();
		if (doing_portal)
		{
			map_player.StartCoroutine(map_player.Hide(false, 0f));
		}
	}

	private void CheckFloorSpawner(bool bIsFirstCalled)
	{
		if (D3DMain.Instance.exploring_dungeon.current_floor == 0 || D3DMain.Instance.exploring_dungeon.floor_spawn_points.Count == 0)
		{
			return;
		}
		foreach (int key in D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_spawners.Keys)
		{
			D3DDungeonFloorSpawner d3DDungeonFloorSpawner = D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_spawners[key];
			if (d3DDungeonFloorSpawner.ReadySpawn)
			{
				if (null == D3DMain.Instance.exploring_dungeon.floor_spawn_points[d3DDungeonFloorSpawner.spawner_id].spawner_gameobj)
				{
					if (test_counters.ContainsKey(key))
					{
						Object.Destroy(test_counters[key]);
						test_counters.Remove(key);
					}
					CheckRemoveRespawnCounter(key);
					CreateEnemyByGroup(d3DDungeonFloorSpawner.spawner_id, (!bIsFirstCalled) ? true : false);
				}
			}
			else if (bIsFirstCalled && d3DDungeonFloorSpawner.bShowCDTime)
			{
				GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/GamePlay/RespawnTimeCounter"));
				gameObject.GetComponentInChildren<RespawnTimeCounter>().SetSpawner(D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_spawners[key]);
				gameObject.transform.position = D3DMain.Instance.exploring_dungeon.floor_spawn_points[key].spawner_position;
				_respawnTimeCounters.Add(key, gameObject);
				SpawnedGrave = true;
			}
		}
		foreach (D3DDungeonFloorTreasureChest floor_treasure in D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_treasures)
		{
			if (null == floor_treasure.chest_obj && (floor_treasure.SpawnedChest || (floor_treasure.ReadySpawn && floor_treasure.SpawnChest())))
			{
				floor_treasure.chest_obj = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/GamePlay/TreasureChest"));
				floor_treasure.chest_obj.GetComponent<DungeonTreasureChest>().Init(floor_treasure, this);
				if (test_treasureCounters.ContainsKey(floor_treasure.treasure_id))
				{
					Object.Destroy(test_treasureCounters[floor_treasure.treasure_id]);
					test_treasureCounters.Remove(floor_treasure.treasure_id);
				}
			}
		}
	}

	private void CheckRemoveRespawnCounter(int spawner_key)
	{
		if (_respawnTimeCounters.ContainsKey(spawner_key))
		{
			Object.Destroy(_respawnTimeCounters[spawner_key]);
			_respawnTimeCounters.Remove(spawner_key);
		}
	}

	public void SwitchRoom()
	{
		foreach (PuppetDungeon dungeon_enemy_group in dungeon_enemy_groups)
		{
			PuppetDungeonEnmeyBehaviour component = dungeon_enemy_group.GetComponent<PuppetDungeonEnmeyBehaviour>();
			component.CancelPursue();
		}
		first_enter = true;
		Time.timeScale = 0.0001f;
		map_player.transform.position = target_room_trigger.transform.position;
		map_player.transform.localRotation = new Quaternion(0f, target_room_trigger.player_face_direction, 0f, 0f);
		map_player.model_builder.PlayPuppetAnimations(false, 2, WrapMode.Loop, true);
		ui_dungeon.EnterDungeon();
	}

	private MapSwitchTrigger GetTriggerByType(MapSwitchTrigger.TriggerType type)
	{
		MapSwitchTrigger[] array = map_triggers;
		foreach (MapSwitchTrigger mapSwitchTrigger in array)
		{
			if (type == mapSwitchTrigger.trigger_type)
			{
				return mapSwitchTrigger;
			}
		}
		return null;
	}

	private void DungeonTouchEvent()
	{
		if (!Input.GetMouseButton(0))
		{
			touch_down_time = 1f;
		}
		else
		{
			if (ui_dungeon.HitUI || !ui_dungeon.HandleEventValid())
			{
				return;
			}
			touch_down_time += Time.deltaTime;
			if (!(touch_down_time > 0.3f))
			{
				return;
			}
			touch_down_time = 0f;
			if (!Camera.main.pixelRect.Contains((Vector2)Input.mousePosition))
			{
				return;
			}
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, 131072))
			{
				GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/UIPrefab/UIRespawnNow"));
				UIRespawnNow component = gameObject.GetComponent<UIRespawnNow>();
				RespawnTimeCounter componentInChildren = hitInfo.transform.GetComponentInChildren<RespawnTimeCounter>();
				if (componentInChildren != null)
				{
					component.Respawn = componentInChildren;
				}
			}
			else if (Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, 4096))
			{
				PuppetDungeon component2 = hitInfo.collider.gameObject.GetComponent<PuppetDungeon>();
				PuppetDungeonInteractiveNpcBehaviour component3;
				if (null != picking_interactive_npc)
				{
					if (picking_interactive_npc == component2)
					{
						return;
					}
					picking_interactive_npc.PuppetRingVisible(false);
					component3 = picking_interactive_npc.GetComponent<PuppetDungeonInteractiveNpcBehaviour>();
					component3.enabled = false;
				}
				picking_interactive_npc = component2;
				picking_interactive_npc.PuppetRingVisible(true);
				component3 = picking_interactive_npc.GetComponent<PuppetDungeonInteractiveNpcBehaviour>();
				component3.enabled = true;
				map_player.SetTarget(picking_interactive_npc.transform.position);
			}
			else
			{
				if (!Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, 256) || null == hitInfo.collider.gameObject)
				{
					return;
				}
				switch (hitInfo.collider.gameObject.tag)
				{
				case "SceneFloor":
					if (null != map_player)
					{
						if (null != picking_interactive_npc)
						{
							picking_interactive_npc.PuppetRingVisible(false);
							PuppetDungeonInteractiveNpcBehaviour component4 = picking_interactive_npc.GetComponent<PuppetDungeonInteractiveNpcBehaviour>();
							component4.enabled = false;
							picking_interactive_npc = null;
						}
						Vector3 vector = new Vector3(hitInfo.point.x, 0f, hitInfo.point.z);
						targetArrow.transform.position = vector;
						targetArrow.GetComponent<ArrowComponent>().StartAnimation();
						map_player.SetTarget(vector);
						ui_dungeon._subUIDungeonStash.ShowIcons(false);
					}
					break;
				}
			}
		}
	}

	private void CreateEnemyByGroup(int spawner_id, bool use_effect = false)
	{
		D3DEnemyGroup enemyGroup = D3DMain.Instance.GetEnemyGroup(D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_spawners[spawner_id].group_id);
		if (enemyGroup == null)
		{
			return;
		}
		D3DMain.Instance.exploring_dungeon.floor_spawn_points[spawner_id].spawner_gameobj = new GameObject();
		map_obj_manager.Add(D3DMain.Instance.exploring_dungeon.floor_spawn_points[spawner_id].spawner_gameobj);
		PuppetDungeon puppetDungeon = D3DMain.Instance.exploring_dungeon.floor_spawn_points[spawner_id].spawner_gameobj.AddComponent<PuppetDungeon>();
		puppetDungeon.InitProfileInstance(D3DMain.Instance.GetProfileClone(enemyGroup.leader_id), 1, D3DMain.Instance.exploring_dungeon.CurrentFloor.enmey_power);
		puppetDungeon.profile_instance.FillSkillLevel();
		puppetDungeon.profile_instance.InitSkillSlots();
		puppetDungeon.model_builder.BuildPuppetModel();
		puppetDungeon.model_builder.AddAudioController(false);
		puppetDungeon.model_builder.PlayPuppetAnimations(true, 2, WrapMode.Loop, true);
		puppetDungeon.CheckPuppetWeapons();
		puppetDungeon.SetPuppetController();
		puppetDungeon.transform.Rotate(Vector3.up * Random.Range(D3DMain.Instance.exploring_dungeon.floor_spawn_points[spawner_id].random_rotation_minY, D3DMain.Instance.exploring_dungeon.floor_spawn_points[spawner_id].random_rotation_maxY));
		puppetDungeon.scene_dungeon = this;
		D3DMain.Instance.SetGameObjectGeneralLayer(puppetDungeon.gameObject, 9);
		puppetDungeon.tag = "Enemy";
		PuppetDungeonEnmeyBehaviour puppetDungeonEnmeyBehaviour = puppetDungeon.gameObject.AddComponent<PuppetDungeonEnmeyBehaviour>();
		puppetDungeonEnmeyBehaviour.Init(spawner_id, enemyGroup, D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_spawners[spawner_id].spawned_group_level);
		dungeon_enemy_groups.Add(puppetDungeon);
		if (D3DDungeonProgerssManager.Instance.CurrentDungeonProgress != null && D3DDungeonProgerssManager.Instance.CurrentDungeonProgress.ContainsKey(D3DMain.Instance.exploring_dungeon.current_floor))
		{
			D3DDungeonProgerssManager.LevelProgress levelProgress = D3DDungeonProgerssManager.Instance.CurrentDungeonProgress[D3DMain.Instance.exploring_dungeon.current_floor];
			if (levelProgress.UnlockBattleList.ContainsKey(spawner_id) && (string.Empty == levelProgress.UnlockBattleList[spawner_id].target_group || levelProgress.UnlockBattleList[spawner_id].target_group == enemyGroup.group_id) && !levelProgress.UnlockBattleList[spawner_id].win_target)
			{
				GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/ModEffect/skull"));
				GameObject gameObject2 = D3DMain.Instance.FindGameObjectChild(puppetDungeon.gameObject, "mount_effect");
				if (null == gameObject2)
				{
					gameObject2 = puppetDungeon.gameObject;
				}
				gameObject.transform.parent = gameObject2.transform;
				gameObject.transform.localPosition = Vector3.zero;
			}
		}
		if (use_effect)
		{
			BasicEffectComponent.PlayEffect("born_L", puppetDungeon.gameObject, string.Empty, false, Vector2.one, Vector3.zero, true, 0f);
		}
	}

	public void ClearOldFloor()
	{
		foreach (GameObject value in test_counters.Values)
		{
			Object.Destroy(value);
		}
		test_counters.Clear();
		foreach (GameObject value2 in test_treasureCounters.Values)
		{
			Object.Destroy(value2);
		}
		test_treasureCounters.Clear();
		foreach (GameObject value3 in _respawnTimeCounters.Values)
		{
			Object.Destroy(value3);
		}
		_respawnTimeCounters.Clear();
		if (D3DMain.Instance.exploring_dungeon.current_floor > 0)
		{
			foreach (D3DDungeonFloorTreasureChest floor_treasure in D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_treasures)
			{
				if (null != floor_treasure.chest_obj)
				{
					Object.Destroy(floor_treasure.chest_obj);
					floor_treasure.chest_obj = null;
				}
			}
		}
		D3DMain.Instance.exploring_dungeon.floor_spawn_points.Clear();
	}

	public IEnumerator DisableFirstEnterByTime()
	{
		if (first_enter)
		{
			yield return new WaitForSeconds(0.3f);
			first_enter = false;
		}
	}

	public void UpdatePlayerAvatar()
	{
		if (map_player.profile_instance.PuppetType == D3DPuppetProfile.ProfileType.AVATAR)
		{
			Quaternion localRotation = map_player.transform.localRotation;
			int currentClip = map_player.model_builder.CurrentClip;
			float currentClipPlayedTime = map_player.model_builder.GetCurrentClipPlayedTime();
			WrapMode currentClipWarpMode = map_player.model_builder.GetCurrentClipWarpMode();
			Object.Destroy(map_player.model_builder.GetModelParts(4));
			Object.Destroy(map_player.model_builder.GetModelParts(3));
			Object.Destroy(map_player.model_builder.GetModelParts(2));
			Object.Destroy(map_player.model_builder.GetModelParts(1));
			Object.Destroy(map_player.model_builder.GetModelParts(0));
			map_player.transform.rotation = Quaternion.identity;
			map_player.InitProfileInstance(D3DMain.Instance.GetProfileClone(D3DGamer.Instance.PlayerBattleTeamData[0].pupet_profile_id), D3DGamer.Instance.PlayerBattleTeamData[0]);
			map_player.profile_instance.InitSkillLevel(D3DGamer.Instance.PlayerBattleTeamData[0]);
			map_player.profile_instance.InitSkillSlots(D3DGamer.Instance.PlayerBattleTeamData[0]);
			map_player.model_builder.BuildPuppetModel();
			map_player.model_builder.AddAudioController(true);
			map_player.CheckPuppetWeapons();
			map_player.model_builder.PlayPuppetAnimations(false, currentClip, currentClipWarpMode, true, 0f, currentClipPlayedTime);
			map_player.transform.rotation = localRotation;
		}
	}

	public void EnterBattle()
	{
		foreach (D3DDungeonFloorTreasureChest floor_treasure in D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_treasures)
		{
			if (null != floor_treasure.chest_obj)
			{
				Object.Destroy(floor_treasure.chest_obj);
				floor_treasure.chest_obj = null;
			}
		}
		floor_enemy_records.Clear();
		player_protect_time = 1.5f;
		foreach (PuppetDungeon dungeon_enemy_group in dungeon_enemy_groups)
		{
			PuppetDungeonEnmeyBehaviour component = dungeon_enemy_group.GetComponent<PuppetDungeonEnmeyBehaviour>();
			DungeonFloorEnemyStateRecord value = new DungeonFloorEnemyStateRecord(dungeon_enemy_group.transform.position, dungeon_enemy_group.transform.rotation, component.post_state);
			floor_enemy_records.Add(component.floor_spawner_id, value);
		}
		D3DMain.Instance.LoadingScene = 2;
		D3DMain.Instance.BattleType = ArenaBattleType.DUNGEON_BATTLE;
		Time.timeScale = 0.0001f;
		ui_dungeon.StopDungeonMusic(false);
		ClearOldFloor();
		ui_dungeon.StartCoroutine(ui_dungeon.BattleFade());
	}

	public void TriggerTreasureChest()
	{
		player_protect_time = 1.5f;
		map_player.CancelMove();
		floor_enemy_records.Clear();
		foreach (PuppetDungeon dungeon_enemy_group in dungeon_enemy_groups)
		{
			dungeon_enemy_group.CancelMove();
			PuppetDungeonEnmeyBehaviour component = dungeon_enemy_group.GetComponent<PuppetDungeonEnmeyBehaviour>();
			DungeonFloorEnemyStateRecord value = new DungeonFloorEnemyStateRecord(dungeon_enemy_group.transform.position, dungeon_enemy_group.transform.rotation, component.post_state);
			floor_enemy_records.Add(component.floor_spawner_id, value);
		}
		ui_dungeon.EnableUIFade(UIFade.FadeState.FADE_OUT, Color.black, TreasureLoot, false);
		Time.timeScale = 0.0001f;
	}

	private void TreasureLoot()
	{
		D3DMain.Instance.exploring_dungeon.player_trigger_chest.chest_obj.SetActiveRecursively(false);
		targetArrow.SetActiveRecursively(false);
		if (null != D3DMain.Instance.HD_BOARD_OBJ)
		{
			D3DMain.Instance.HD_BOARD_OBJ.SetActiveRecursively(true);
		}
		ui_dungeon.GetManager(0).gameObject.SetActiveRecursively(false);
		GameObject original = Resources.Load("Dungeons3D/Prefabs/UIPrefab/UILoot") as GameObject;
		original = (GameObject)Object.Instantiate(original);
	}

	public void StartDungeonPortal()
	{
		doing_portal = true;
		map_player.CancelMove();
		PuppetDungeonPlayerBehaviour component = map_player.GetComponent<PuppetDungeonPlayerBehaviour>();
		if (null != component)
		{
			component.SetPlayerInvincibilityOnPortal();
		}
		foreach (PuppetDungeon dungeon_enemy_group in dungeon_enemy_groups)
		{
			dungeon_enemy_group.CancelMove();
			PuppetDungeonEnmeyBehaviour component2 = dungeon_enemy_group.GetComponent<PuppetDungeonEnmeyBehaviour>();
			component2.SetBreakStateOnPlayerPortal();
		}
		map_obj_manager.Add(BasicEffectComponent.PlayEffect("home", map_player.gameObject, string.Empty, false, Vector2.one, Vector3.zero, true, 0f).gameObject);
		map_player.StartCoroutine(map_player.Hide(false, 0.75f));
	}

	private void OutputDungeonInfo()
	{
		string empty = string.Empty;
		string text = empty;
		empty = text + "Current Dungeon:" + D3DMain.Instance.exploring_dungeon.dungeon.dungeon_id + " ,and it's name is:" + D3DMain.Instance.exploring_dungeon.dungeon.dungeon_name + " ,it has " + D3DMain.Instance.exploring_dungeon.dungeon.dungeon_floors.Count + " floor(s)\n";
		if (D3DMain.Instance.exploring_dungeon.current_floor == 0)
		{
			empty = empty + "Now you're at town,this town use mode number " + D3DDungeonModelPreset.Instance.GetModelPreset(D3DMain.Instance.exploring_dungeon.dungeon.dungeon_town.town_model_preset).ModelPostfix;
			return;
		}
		text = empty;
		empty = text + "Now you're at Floor " + D3DMain.Instance.exploring_dungeon.current_floor + "\n";
		text = empty;
		empty = text + "This floor using model number " + D3DDungeonModelPreset.Instance.GetModelPreset(D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_model_preset).ModelPostfix + " ,Door1 Open? " + D3DMain.Instance.exploring_dungeon.CurrentFloor.open_room1 + " ,Door2 Open? " + D3DMain.Instance.exploring_dungeon.CurrentFloor.open_room2 + "\n";
		empty = empty + "Spawner prefab:" + D3DMain.Instance.exploring_dungeon.CurrentFloor.spawn_points_prefab + "\n";
		text = empty;
		empty = text + "This floor level is " + D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_level_min + " to " + D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_level_max + "\n";
		text = empty;
		empty = text + "Kill require:" + D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_battle_kill_require + " ,Spawn an enemy " + D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_battle_spawn_interval + " per seconds ,Fight most " + D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_battle_spawn_limit + " enemy(s) on screen\n";
		empty += "Floor random group(s):\n";
		foreach (string floor_random_group in D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_random_groups)
		{
			empty = empty + floor_random_group + "\n";
		}
		empty += "Floor spawners:\n";
		foreach (D3DDungeonFloorSpawner value in D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_spawners.Values)
		{
			text = empty;
			empty = text + "Spawner id:" + value.spawner_id + " ,Spawner group:" + value.group_id + " ,Will be spawned after " + value.spawn_interval + " seconds\n";
		}
	}

	private void OutputCheckGroupInfo(int spawner_id)
	{
		string empty = string.Empty;
		D3DEnemyGroup enemyGroup = D3DMain.Instance.GetEnemyGroup(D3DMain.Instance.exploring_dungeon.CurrentFloor.GetSpawnGroupID(spawner_id));
		if (enemyGroup == null)
		{
			empty += "The group you want to check is null!";
			return;
		}
		empty += "Group infomation:\n";
		empty = empty + "Group id:" + enemyGroup.group_id;
		empty += "\n==============In Dungeon:";
		empty = empty + "\nLeader id:" + enemyGroup.leader_id;
		string text = empty;
		empty = text + "\nPatrol radius:" + enemyGroup.patrol_radius + " ,and patrol speed:" + enemyGroup.patrol_speed;
		empty = empty + "\nSight radius" + enemyGroup.sight_radius;
		text = empty;
		empty = text + "\nPursue radius:" + enemyGroup.pursue_radius + " ,and pursue speed:" + enemyGroup.pursue_speed;
		text = empty;
		empty = text + "\nThis group will be respawned after " + enemyGroup.map_spawn_interval + " seconds\n";
		empty += "================In Battle:\n";
		empty = empty + "Kill require:" + enemyGroup.kill_require;
		text = empty;
		empty = text + "\nSpawn an enemy " + enemyGroup.battle_spawn_interval + " per seconds\n";
		text = empty;
		empty = text + "You will fight most " + enemyGroup.battle_spawn_limit + " enemy(s) on screen\n";
		text = empty;
		empty = text + "This group has " + enemyGroup.spawn_phases.Count + " phase(s)\n";
		for (int i = 0; i < enemyGroup.spawn_phases.Count; i++)
		{
			text = empty;
			empty = text + "========Phase" + (i + 1) + "===========\n";
			text = empty;
			empty = text + "Random spawn?" + enemyGroup.spawn_phases[i].random_spawn + " ,Wait previous phase over?" + enemyGroup.spawn_phases[i].is_wait;
			text = empty;
			empty = text + "\nThis phase will spawn " + enemyGroup.spawn_phases[i].phase_spawn_count + " enemy(s) ,and " + enemyGroup.spawn_phases[i].once_spawn_count + " enemy(s) will appear at once\n";
			empty += "Enemy List:\n";
			foreach (D3DEnemyGroupSpawnPhase.EnemySpawner phase_enemy_spawner in enemyGroup.spawn_phases[i].phase_enemy_spawners)
			{
				text = empty;
				empty = text + "Id:" + phase_enemy_spawner.enemy_id + " ,Odds:" + phase_enemy_spawner.odds + " ,LvDiff:" + phase_enemy_spawner.level_diff + "\n";
			}
		}
	}
}
