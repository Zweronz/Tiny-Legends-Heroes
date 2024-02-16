using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneArena : MonoBehaviour
{
	private enum DragEvent
	{
		DOWN = 0,
		DRAG = 1,
		END = 2
	}

	private DragEvent arena_drag_event;

	private Vector3 last_mouse_point;

	private Touch Win32Touch;

	public PuppetArena activing_puppet;

	private List<TouchDragEvent> touch_drag_events;

	private bool is_battle_win_behaviour;

	public UIArena ui_arena;

	public List<PuppetArena> playerList = new List<PuppetArena>();

	public List<PuppetArena> playerTallyList = new List<PuppetArena>();

	public List<PuppetArena> playerRecycleList = new List<PuppetArena>();

	public List<PuppetArena> playerSummonedList = new List<PuppetArena>();

	public List<PuppetArena> playerWinnerList = new List<PuppetArena>();

	public List<PuppetArena> enemyList = new List<PuppetArena>();

	public List<PuppetArena> enemyTallyList = new List<PuppetArena>();

	public List<PuppetArena> enemyRecycleList = new List<PuppetArena>();

	public List<PuppetArena> enemySummonedList = new List<PuppetArena>();

	public List<PuppetArena> playerTeamData = new List<PuppetArena>();

	public List<PuppetArena> playerGrave = new List<PuppetArena>();

	public List<PuppetArena> enemyGrave = new List<PuppetArena>();

	public List<PuppetArena> enemyBodyDestroyer = new List<PuppetArena>();

	public GameObject playerGraveObj;

	public GameObject enemyGraveObj;

	public int kill_count;

	public int kill_require;

	public int player_rest;

	public int BattleResult;

	public List<AureoleBehaviour> AureoleManager = new List<AureoleBehaviour>();

	public GameObject camera_lookAt;

	private bool camera_transform_change;

	private bool lookat_transform_change;

	private bool camera_fov_change;

	private int CurrentCameraType;

	public Vector3[] CameraTransform = new Vector3[2]
	{
		new Vector3(0f, 49.5f, 49.4f),
		new Vector3(0f, 17.1f, 48.2f)
	};

	public Vector3[] CameraLookAtTransform = new Vector3[2]
	{
		new Vector3(0f, 0f, 0.6f),
		new Vector3(0f, 0f, -0.794f)
	};

	public float[] CameraFov = new float[2] { 11f, 14f };

	private Vector3 camera_direction;

	private Vector3 lookat_direction;

	private Quaternion camera_rotate_target;

	private Quaternion lookat_rotate_target;

	private float camera_velocity;

	private float lookat_velocity;

	private float fov_velocity;

	public Vector2[] WorldLimit;

	public D3DBattlePreset.SpawnerConfig SpawnerConfig;

	public int battle_exp_basic;

	public int battle_gold_basic;

	public int battle_gained_exp;

	public int battle_gained_gold;

	private bool bIsBoss;

	private float _fTimeToStopBulletTime;

	private readonly float _fBulletTimeScale = 0.1f;

	private readonly float _fMaxBulletTime = 3f;

	private bool _bUsingBulletTime;

	public bool IsBattleWinBehaviour
	{
		get
		{
			return is_battle_win_behaviour;
		}
	}

	public bool IsBoss
	{
		get
		{
			return bIsBoss;
		}
	}

	private void Awake()
	{
		if (null != D3DMain.Instance.HD_BOARD_OBJ)
		{
			D3DMain.Instance.HD_BOARD_OBJ.SetActiveRecursively(false);
		}
		D3DMain.Instance.TriggerApplicationPause = false;
		kill_count = 0;
		playerGraveObj = new GameObject("PlayerGrave");
		playerGraveObj.active = false;
		enemyGraveObj = new GameObject("EnemyGrave");
		enemyGraveObj.active = false;
		Win32Touch = default(Touch);
		Input.multiTouchEnabled = true;
		touch_drag_events = new List<TouchDragEvent>();
		D3DDungeonModelPreset.ModelPreset modelPreset = D3DDungeonModelPreset.Instance.GetModelPreset(D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_model_preset);
		GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/Arena/arena_" + modelPreset.ModelPostfix));
		gameObject.isStatic = true;
		TAudioManager.instance.AudioListener.transform.position = gameObject.transform.position;
		TAudioManager.instance.AudioListener.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
		Physics.IgnoreLayerCollision(8, 9, true);
		Physics.IgnoreLayerCollision(9, 9, true);
		if (D3DMain.Instance.BattleType == ArenaBattleType.DUNGEON_BATTLE)
		{
			base.gameObject.AddComponent<ArenaDungeonBattle>();
		}
		Camera.main.pixelRect = new Rect(((float)Screen.width - 960f * ((float)Screen.height / 640f)) * 0.5f, 0f, 960f * ((float)Screen.height / 640f), Screen.height);
		base.transform.position = CameraTransform[0];
		Camera.main.fov = CameraFov[0];
		camera_lookAt.transform.position = CameraLookAtTransform[0];
		camera_velocity = Vector3.Distance(CameraTransform[0], CameraTransform[1]) / 0.5f;
		lookat_velocity = Vector3.Distance(CameraLookAtTransform[0], CameraLookAtTransform[1]) / 0.5f;
		Camera.main.transform.LookAt(camera_lookAt.transform.position);
		GameObject gameObject2 = (GameObject)Object.Instantiate(Resources.Load("Dungeons3D/Prefabs/GamePlay/WorldLimitPlane"));
		gameObject2.transform.position = gameObject.transform.position;
		WorldLimit = new Vector2[4];
		SpawnerConfig = D3DBattlePreset.Instance.GetSpawnerConfig(modelPreset.ModelPostfix);
		float num = 0f;
		Ray ray = Camera.main.ScreenPointToRay(Vector3.zero + new Vector3(num, num, 0f));
		RaycastHit hitInfo;
		Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, 8192);
		WorldLimit[0] = new Vector2(hitInfo.point.x, hitInfo.point.z);
		float num2 = (float)Screen.height / 640f;
		ray = Camera.main.ScreenPointToRay(new Vector3(960f * num2, 0f, 0f));
		Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, 8192);
		WorldLimit[1] = new Vector2(hitInfo.point.x, hitInfo.point.z);
		ray = Camera.main.ScreenPointToRay(new Vector3(960f * num2, Screen.height, 0f));
		Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, 8192);
		WorldLimit[2] = new Vector2(hitInfo.point.x, hitInfo.point.z);
		ray = Camera.main.ScreenPointToRay(new Vector3(0f, Screen.height, 0f));
		Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, 8192);
		WorldLimit[3] = new Vector2(hitInfo.point.x, hitInfo.point.z);
		Object.Destroy(gameObject2);
		battle_exp_basic = D3DFormulas.GetExpBonus(D3DMain.Instance.exploring_dungeon.player_battle_group_data.group_level);
		battle_gold_basic = D3DFormulas.GetGoldBonus(D3DMain.Instance.exploring_dungeon.player_battle_group_data.group_level);
		battle_gained_exp = 0;
		battle_gained_gold = 0;
		Object.Destroy(GetComponent<FPS>());
	}

	private void Start()
	{
		int spawner_id = D3DMain.Instance.exploring_dungeon.player_battle_group_data.spawner_id;
		if (D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_spawners.ContainsKey(spawner_id))
		{
			bIsBoss = D3DMain.Instance.exploring_dungeon.CurrentFloor.floor_spawners[spawner_id].bShowCDTime;
		}
		string group_id = D3DMain.Instance.exploring_dungeon.player_battle_group_data.temp_group.group_id;
		CreatePlayerPawns(D3DGamer.Instance.PlayerBattleTeamData);
		ui_arena.FadeInArena();
	}

	private void OnDestroy()
	{
		if (null != D3DMain.Instance.HD_BOARD_OBJ)
		{
			D3DMain.Instance.HD_BOARD_OBJ.SetActiveRecursively(true);
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (D3DMain.Instance.TriggerApplicationPause)
		{
			ui_arena.GamePause();
		}
	}

	private void Update()
	{
		CheckingBulletTime();
		if (camera_transform_change || lookat_transform_change)
		{
			DoChangeCameraTransform();
		}
		if (is_battle_win_behaviour)
		{
			return;
		}
		ArenaDragEvent();
		int num = 0;
		while (num < touch_drag_events.Count)
		{
			TouchDragEvent touchDragEvent = touch_drag_events[num];
			touchDragEvent.TouchUpdate();
			if (touchDragEvent.invaild_touch)
			{
				touch_drag_events.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
	}

	private void ReadyToStartBulletThisTime()
	{
		_fTimeToStopBulletTime = _fMaxBulletTime * _fBulletTimeScale;
	}

	private void UseBulletTime(bool bUse)
	{
		if (bUse)
		{
			if (Time.timeScale == 1f && _fTimeToStopBulletTime > 0f)
			{
				_bUsingBulletTime = true;
				Time.timeScale = _fBulletTimeScale;
			}
		}
		else if (_bUsingBulletTime)
		{
			Time.timeScale = 1f;
			_bUsingBulletTime = false;
		}
	}

	private void CheckingBulletTime()
	{
		if (_fTimeToStopBulletTime > 0f && _bUsingBulletTime)
		{
			_fTimeToStopBulletTime -= Time.deltaTime;
			if (_fTimeToStopBulletTime < 0f)
			{
				UseBulletTime(false);
			}
		}
	}

	public void FreeAllTouch()
	{
		foreach (TouchDragEvent touch_drag_event in touch_drag_events)
		{
			touch_drag_event.TouchEnd();
		}
		touch_drag_events.Clear();
	}

	private PuppetArena CreatePuppetBySaveData(D3DGamer.D3DPuppetSaveData save_data)
	{
		GameObject gameObject = new GameObject();
		PuppetArena puppetArena = gameObject.AddComponent<PuppetArena>();
		puppetArena.scene_arena = this;
		if (!puppetArena.InitProfileInstance(D3DMain.Instance.GetProfileClone(save_data.pupet_profile_id), save_data))
		{
			Object.Destroy(gameObject);
			return null;
		}
		puppetArena.profile_instance.InitSkillLevel(save_data);
		puppetArena.profile_instance.InitSkillSlots(save_data);
		puppetArena.model_builder.BuildPuppetModel();
		puppetArena.model_builder.AddAnimationEvent();
		puppetArena.model_builder.AddAudioController(true);
		puppetArena.model_builder.PlayPuppetAnimations(true, puppetArena.model_builder.ChangeableIdleClip, WrapMode.Loop, true);
		puppetArena.CheckPuppetWeapons();
		puppetArena.SetBattleSkill();
		puppetArena.profile_instance.InitProperties();
		puppetArena.InitCommonCD();
		puppetArena.SetPuppetController();
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.rotation = Quaternion.identity;
		D3DMain.Instance.SetGameObjectGeneralLayer(puppetArena.gameObject, 9);
		return puppetArena;
	}

	private PuppetArena CreatePuppetByLevel(string profile_id, int puppet_level, int enemy_power = 0)
	{
		if (string.Empty == profile_id)
		{
			return null;
		}
		GameObject gameObject = new GameObject();
		PuppetArena puppetArena = gameObject.AddComponent<PuppetArena>();
		puppetArena.scene_arena = this;
		if (!puppetArena.InitProfileInstance(D3DMain.Instance.GetProfileClone(profile_id), puppet_level, enemy_power))
		{
			Object.Destroy(gameObject);
			return null;
		}
		puppetArena.profile_instance.FillSkillLevel();
		puppetArena.profile_instance.InitSkillSlots();
		puppetArena.model_builder.BuildPuppetModel();
		puppetArena.model_builder.AddAnimationEvent();
		puppetArena.model_builder.AddAudioController(true);
		puppetArena.model_builder.PlayPuppetAnimations(true, puppetArena.model_builder.ChangeableIdleClip, WrapMode.Loop, true);
		puppetArena.CheckPuppetWeapons();
		puppetArena.SetBattleSkill();
		puppetArena.profile_instance.InitProperties();
		puppetArena.InitCommonCD();
		puppetArena.SetPuppetController();
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.rotation = Quaternion.identity;
		D3DMain.Instance.SetGameObjectGeneralLayer(puppetArena.gameObject, 9);
		return puppetArena;
	}

	private void ClearAllPlayer()
	{
		playerTeamData.Clear();
		playerList.Clear();
		playerTallyList.Clear();
		ui_arena.ClearFaceButtons();
	}

	public void CallCreatePlayerPawnsAsync()
	{
		StartCoroutine(CreatePlayerPawnsAsyn(D3DTutorialTeam.Instance.PlayerDatas));
	}

	private IEnumerator CreatePlayerPawnsAsyn(List<D3DGamer.D3DPuppetSaveData> TeamData)
	{
		int index = 0;
		List<PuppetArena> spawned_puppets = new List<PuppetArena>();
		foreach (D3DGamer.D3DPuppetSaveData save_data in TeamData)
		{
			PuppetArena puppet = CreatePlayerPuppet(save_data, ref spawned_puppets);
			BasicEffectComponent.PlayEffect("level up", puppet.gameObject, string.Empty, true, Vector2.one, Vector3.zero, true, 0f);
			Time.timeScale = 0.3f;
			if (null != puppet)
			{
				puppet.transform.position = new Vector3(D3DBattlePreset.Instance.HeroPoints[index].x, 0f, D3DBattlePreset.Instance.HeroPoints[index].y);
				index++;
			}
			yield return new WaitForSeconds(1.5f * Time.timeScale);
		}
		Time.timeScale = 1f;
		playerTeamData.AddRange(playerTallyList);
	}

	private void CreatePlayerPawns(List<D3DGamer.D3DPuppetSaveData> TeamData)
	{
		int num = 0;
		List<PuppetArena> spawned_puppets = new List<PuppetArena>();
		foreach (D3DGamer.D3DPuppetSaveData TeamDatum in TeamData)
		{
			PuppetArena puppetArena = CreatePlayerPuppet(TeamDatum, ref spawned_puppets);
			if (null != puppetArena)
			{
				puppetArena.transform.position = new Vector3(D3DBattlePreset.Instance.HeroPoints[num].x, 0f, D3DBattlePreset.Instance.HeroPoints[num].y);
				num++;
			}
		}
		playerTeamData.AddRange(playerTallyList);
	}

	private PuppetArena CreatePlayerPuppet(D3DGamer.D3DPuppetSaveData save_data, ref List<PuppetArena> spawned_puppets)
	{
		PuppetArena puppetArena = CreatePuppetBySaveData(save_data);
		if (null == puppetArena)
		{
			return null;
		}
		puppetArena.gameObject.AddComponent<PuppetMonitorNull>();
		puppetArena.gameObject.name = "Puppet_" + save_data.pupet_profile_id;
		PuppetAutoSpot puppetAutoSpot = puppetArena.gameObject.AddComponent<PuppetAutoSpot>();
		puppetAutoSpot.Init(enemyList, 5f);
		puppetArena.tag = "Player";
		playerList.Add(puppetArena);
		playerTallyList.Add(puppetArena);
		player_rest++;
		puppetArena.InitPuppetComponents();
		ui_arena.CreateFaceButtonForPuppet(puppetArena);
		if (spawned_puppets != null)
		{
			spawned_puppets.Add(puppetArena);
		}
		return puppetArena;
	}

	public PuppetArena CreateEnemyPuppet(string profile_id, int puppet_level, ref List<PuppetArena> spawned_puppets)
	{
		PuppetArena puppetArena = CreatePuppetByLevel(profile_id, puppet_level, D3DMain.Instance.exploring_dungeon.player_battle_group_data.enemy_power);
		if (null == puppetArena)
		{
			return null;
		}
		puppetArena.gameObject.AddComponent<PuppetMonitorAI>();
		puppetArena.tag = "Enemy";
		enemyList.Add(puppetArena);
		enemyTallyList.Add(puppetArena);
		puppetArena.InitPuppetComponents();
		if (spawned_puppets != null)
		{
			spawned_puppets.Add(puppetArena);
		}
		return puppetArena;
	}

	public void SummonPuppet(PuppetArena summoner, string summon_id, int summon_level, int summon_count, float summon_life, string summon_effect, ref List<PuppetArena> summomed_puppets)
	{
		if (summon_level < 0)
		{
			summon_level = summoner.profile_instance.puppet_level;
		}
		for (int i = 0; i < summon_count; i++)
		{
			PuppetArena puppetArena = CreatePuppetByLevel(summon_id, summon_level);
			if (!(null == puppetArena))
			{
				SetPuppetRandomPosition(puppetArena);
				if ("Player" == summoner.tag)
				{
					puppetArena.tag = "Player";
					playerList.Add(puppetArena);
					playerSummonedList.Add(puppetArena);
				}
				else
				{
					puppetArena.tag = "Enemy";
					enemyList.Add(puppetArena);
					enemySummonedList.Add(puppetArena);
				}
				puppetArena.gameObject.AddComponent<PuppetMonitorAI>();
				if (summon_life != -1f)
				{
					PuppetSummonLife puppetSummonLife = puppetArena.gameObject.AddComponent<PuppetSummonLife>();
					puppetSummonLife.SummonLife(summoner, summon_life);
				}
				puppetArena.InitPuppetComponents();
				if (string.Empty != summon_effect)
				{
					BasicEffectComponent.PlayEffect(summon_effect, puppetArena.transform.position, Quaternion.identity, Vector2.zero, Vector3.zero, true, 0f);
				}
				if (summomed_puppets != null)
				{
					summomed_puppets.Add(puppetArena);
				}
			}
		}
	}

	public void ReviveGravePuppet(string faction_tag, float hp_recover_percent, float mp_recover_percent)
	{
		if ((BattleResult == -1 && "TriggerPlayer" == faction_tag) || (BattleResult == 1 && "TriggerEnemy" == faction_tag) || (hp_recover_percent <= 0f && mp_recover_percent <= 0f))
		{
			return;
		}
		PuppetArena puppetArena = null;
		PuppetArena.ArenaPuppetState revive_state = PuppetArena.ArenaPuppetState.Idle;
		if ("TriggerPlayer" == faction_tag)
		{
			if (playerGrave.Count > 0)
			{
				puppetArena = playerGrave[playerGrave.Count - 1];
				if (!puppetArena.Revive(hp_recover_percent, mp_recover_percent, ref revive_state))
				{
					puppetArena = null;
				}
			}
		}
		else if (enemyGrave.Count > 0)
		{
			puppetArena = enemyGrave[enemyGrave.Count - 1];
			if (!puppetArena.Revive(hp_recover_percent, mp_recover_percent, ref revive_state))
			{
				puppetArena = null;
			}
		}
		if (null != puppetArena)
		{
			if (revive_state == PuppetArena.ArenaPuppetState.Grave)
			{
				SetPuppetRandomPosition(puppetArena);
			}
			BasicEffectComponent.PlayEffect("resurrection", puppetArena.transform.position, Quaternion.identity, Vector2.zero, Vector3.zero, true, 0f);
		}
	}

	public void OnPlayerTallyCountChange(List<PuppetArena> ignore_list = null)
	{
		foreach (PuppetArena player in playerList)
		{
			if ((ignore_list == null || !ignore_list.Contains(player)) && null != player.puppet_monitor)
			{
				player.puppet_monitor.OnFriendCountChange(playerTallyList.Count);
			}
		}
		foreach (PuppetArena enemy in enemyList)
		{
			if (null != enemy.puppet_monitor)
			{
				enemy.puppet_monitor.OnEnemyCountChange(playerTallyList.Count);
			}
		}
	}

	public void OnEnemyTallyCountChange(List<PuppetArena> ignore_list = null)
	{
		foreach (PuppetArena enemy in enemyList)
		{
			if ((ignore_list == null || !ignore_list.Contains(enemy)) && null != enemy.puppet_monitor)
			{
				enemy.puppet_monitor.OnFriendCountChange(enemyTallyList.Count);
			}
		}
		foreach (PuppetArena player in playerList)
		{
			if (null != player.puppet_monitor)
			{
				player.puppet_monitor.OnEnemyCountChange(enemyTallyList.Count);
			}
		}
	}

	public void OnPlayerSummonedCountChange(List<PuppetArena> ignore_list = null)
	{
		foreach (PuppetArena player in playerList)
		{
			if ((ignore_list == null || !ignore_list.Contains(player)) && null != player.puppet_monitor)
			{
				player.puppet_monitor.OnSummonedCountChange(playerSummonedList.Count);
			}
		}
	}

	public void OnEnemySummonedCountChange(List<PuppetArena> ignore_list = null)
	{
		foreach (PuppetArena enemy in enemyList)
		{
			if ((ignore_list == null || !ignore_list.Contains(enemy)) && null != enemy.puppet_monitor)
			{
				enemy.puppet_monitor.OnSummonedCountChange(enemySummonedList.Count);
			}
		}
	}

	private bool CheckOverlap(GameObject obj)
	{
		foreach (PuppetArena player in playerList)
		{
			if (obj == player.gameObject || !((double)Vector3.Distance(obj.transform.position, player.transform.position) <= (double)(obj.GetComponent<CharacterController>().radius + player.GetComponent<CharacterController>().radius) * 1.2))
			{
				continue;
			}
			return true;
		}
		foreach (PuppetArena enemy in enemyList)
		{
			if (obj == enemy.gameObject || !((double)Vector3.Distance(obj.transform.position, enemy.transform.position) <= (double)(obj.GetComponent<CharacterController>().radius + enemy.GetComponent<CharacterController>().radius) * 1.2))
			{
				continue;
			}
			return true;
		}
		return false;
	}

	public void SetEnemyPositionBySpawner(PuppetArena puppet)
	{
		string effect_name = "born_L";
		if (D3DBattlePreset.Instance.CustomSpawnEffect.ContainsKey(puppet.profile_instance.ProfileID))
		{
			effect_name = D3DBattlePreset.Instance.CustomSpawnEffect[puppet.profile_instance.ProfileID];
		}
		if (SpawnerConfig == null)
		{
			SetPuppetRandomPosition(puppet);
			BasicEffectComponent.PlayEffect(effect_name, puppet.gameObject, string.Empty, false, Vector2.one, Vector3.zero, true, 0f);
			return;
		}
		int num = 0;
		do
		{
			if (num > 30)
			{
				SetPuppetRandomPosition(puppet);
				BasicEffectComponent.PlayEffect(effect_name, puppet.gameObject, string.Empty, false, Vector2.one, Vector3.zero, true, 0f);
				return;
			}
			Vector2[] array = SpawnerConfig.SpawnerLine[Random.Range(0, SpawnerConfig.SpawnerLine.Count)];
			Vector2 randomPointOnLine = D3DPlaneGeometry.GetRandomPointOnLine(array[0], array[1]);
			Vector3 vector = new Vector3(randomPointOnLine.x, 0f, randomPointOnLine.y);
			if (!D3DPlaneGeometry.PointInpolygon(WorldLimit, new Vector2(vector.x, vector.z)))
			{
				float num2 = 99999f;
				int[] array2 = new int[2];
				for (int i = 0; i < WorldLimit.Length; i++)
				{
					int num3 = i;
					int num4 = ((i != 0) ? (i - 1) : (WorldLimit.Length - 1));
					float num5 = D3DPlaneGeometry.DistanceLine(WorldLimit[num4], WorldLimit[num3], new Vector2(vector.x, vector.z));
					if (num5 < num2)
					{
						num2 = num5;
						array2[0] = num3;
						array2[1] = num4;
					}
				}
				RaycastHit hitInfo;
				do
				{
					randomPointOnLine = D3DPlaneGeometry.GetRandomPointOnLine(WorldLimit[array2[0]], WorldLimit[array2[1]]);
					vector = new Vector3(randomPointOnLine.x, 0f, randomPointOnLine.y);
				}
				while (!Physics.Raycast(vector + Vector3.up, -Vector3.up, out hitInfo, float.PositiveInfinity, 256));
			}
			puppet.transform.position = vector;
			num++;
		}
		while (CheckOverlap(puppet.gameObject));
		puppet.SyncComponentsPosition();
		BasicEffectComponent.PlayEffect(effect_name, puppet.gameObject, string.Empty, false, Vector2.one, Vector3.zero, true, 0f);
	}

	public void SetPuppetRandomPosition(PuppetArena puppet)
	{
		while (true)
		{
			float x = ((!(Vector2.Distance(WorldLimit[0], WorldLimit[1]) > Vector2.Distance(WorldLimit[2], WorldLimit[3]))) ? Random.Range(WorldLimit[0].x, WorldLimit[1].x) : Random.Range(WorldLimit[2].x, WorldLimit[3].x));
			float z = ((!(Vector2.Distance(WorldLimit[0], WorldLimit[3]) > Vector2.Distance(WorldLimit[1], WorldLimit[2]))) ? Random.Range(WorldLimit[0].y, (WorldLimit[3].y - WorldLimit[0].y) * 0.5f) : Random.Range(WorldLimit[1].y, (WorldLimit[2].y - WorldLimit[1].y) * 0.5f));
			Vector3 vector = new Vector3(x, 0f, z);
			RaycastHit hitInfo;
			if (D3DPlaneGeometry.PointInpolygon(WorldLimit, new Vector2(vector.x, vector.z)) && Physics.Raycast(vector + Vector3.up, -Vector3.up, out hitInfo, float.PositiveInfinity, 256))
			{
				puppet.transform.position = vector;
				if (!CheckOverlap(puppet.gameObject))
				{
					break;
				}
			}
		}
		puppet.SyncComponentsPosition();
	}

	private void ArenaDragEvent()
	{
		if (ui_arena.isFadeing || ui_arena.HitUI || !ui_arena.battle_ui_event || Time.timeScale < _fBulletTimeScale)
		{
			return;
		}
		Touch[] touches = Input2.touches;
		for (int i = 0; i < touches.Length; i++)
		{
			Touch touch = touches[i];
			if (touch.phase == TouchPhase.Began)
			{
				Vector3[] array = new Vector3[5]
				{
					new Vector3(0f, 0f, 0f),
					new Vector3(0f, 3f, 0f),
					new Vector3(0f, -3f, 0f),
					new Vector3(-3f, 0f, 0f),
					new Vector3(3f, 0f, 0f)
				};
				bool flag = false;
				ReadyToStartBulletThisTime();
				for (int j = 0; j < 5; j++)
				{
					Ray ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0f) + array[j]);
					RaycastHit[] array2 = Physics.RaycastAll(ray, float.PositiveInfinity, 512);
					for (int k = 0; k < array2.Length; k++)
					{
						RaycastHit raycastHit = array2[k];
						if (!("Player" == raycastHit.collider.tag))
						{
							continue;
						}
						PuppetArena component = raycastHit.collider.GetComponent<PuppetArena>();
						if (null != component)
						{
							if (!PuppetHasTouchEvent(component))
							{
								TouchDragEvent item = new TouchDragEvent(this, touch.fingerId, component, false);
								touch_drag_events.Add(item);
								UseBulletTime(true);
							}
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (flag || !(null != activing_puppet))
				{
					continue;
				}
				bool flag2 = false;
				foreach (TouchDragEvent touch_drag_event in touch_drag_events)
				{
					if (activing_puppet == touch_drag_event.PickPuppet)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					TouchDragEvent item2 = new TouchDragEvent(this, Win32Touch.fingerId, activing_puppet, true);
					touch_drag_events.Add(item2);
					UseBulletTime(true);
				}
			}
			else if (touch.phase == TouchPhase.Moved)
			{
				if (touch.deltaPosition.magnitude < 1.5f)
				{
					continue;
				}
				TouchDragEvent touchDragEvent = GetTouchDragEvent(touch.fingerId);
				if (touchDragEvent == null)
				{
					continue;
				}
				bool flag3 = false;
				Ray ray2 = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0f));
				RaycastHit hitInfo;
				if (Physics.Raycast(ray2, out hitInfo, float.PositiveInfinity, 512))
				{
					PuppetArena component2 = hitInfo.collider.gameObject.GetComponent<PuppetArena>();
					if (null != component2 && touchDragEvent.PickPuppet.CheckVaildTarget(component2))
					{
						flag3 = true;
						touchDragEvent.TouchMove(component2.transform.position, component2);
					}
				}
				if (!flag3)
				{
					if (Physics.Raycast(ray2, out hitInfo, float.PositiveInfinity, 256))
					{
						Vector3 point = new Vector3(hitInfo.point.x, 0f, hitInfo.point.z);
						PuppetArena targetPuppet = GetTargetPuppet(point);
						touchDragEvent.TouchMove(point, targetPuppet);
					}
					else
					{
						TouchOutGroud(touchDragEvent, touch.position);
					}
				}
			}
			else
			{
				if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
				{
					continue;
				}
				TouchDragEvent touchDragEvent2 = GetTouchDragEvent(touch.fingerId);
				if (touchDragEvent2 == null)
				{
					continue;
				}
				if (touchDragEvent2.InstantTouch)
				{
					Ray ray3 = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0f));
					RaycastHit[] array3 = Physics.RaycastAll(ray3, float.PositiveInfinity, 512);
					RaycastHit hitInfo2;
					if (array3.Length > 0)
					{
						touchDragEvent2.TouchMove(array3[0].transform.position, array3[0].transform.GetComponent<PuppetArena>());
					}
					else if (Physics.Raycast(ray3, out hitInfo2, float.PositiveInfinity, 256))
					{
						Vector3 point2 = new Vector3(hitInfo2.point.x, 0f, hitInfo2.point.z);
						PuppetArena targetPuppet2 = GetTargetPuppet(point2);
						touchDragEvent2.TouchMove(point2, targetPuppet2);
					}
					else
					{
						TouchOutGroud(touchDragEvent2, touch.position);
					}
				}
				UseBulletTime(false);
				touchDragEvent2.TouchEnd();
				touch_drag_events.Remove(touchDragEvent2);
			}
		}
	}

	private void TouchOutGroud(TouchDragEvent drag_event, Vector2 touch_point)
	{
		if (!(null != drag_event.PickPuppet))
		{
			return;
		}
		Vector2 vector = Camera.main.WorldToScreenPoint(drag_event.PickPuppet.transform.position);
		RaycastHit hitInfo = default(RaycastHit);
		while (!(Vector2.Distance(vector, touch_point) < 1f))
		{
			for (int i = 1; i < 11; i++)
			{
				Vector2 pointOnLine = D3DPlaneGeometry.GetPointOnLine(vector, touch_point, (float)(i - 1) * 0.1f);
				Vector2 pointOnLine2 = D3DPlaneGeometry.GetPointOnLine(vector, touch_point, (float)i * 0.1f);
				bool flag = Physics.Raycast(Camera.main.ScreenPointToRay(pointOnLine), out hitInfo, float.PositiveInfinity, 256);
				bool flag2 = Physics.Raycast(Camera.main.ScreenPointToRay(pointOnLine2), float.PositiveInfinity, 256);
				if (flag && !flag2)
				{
					vector = pointOnLine;
					touch_point = pointOnLine2;
					break;
				}
				if (i == 10)
				{
					return;
				}
			}
		}
		Vector3 point = new Vector3(hitInfo.point.x, 0f, hitInfo.point.z);
		PuppetArena targetPuppet = GetTargetPuppet(point);
		drag_event.TouchMove(point, targetPuppet);
	}

	private TouchDragEvent GetTouchDragEvent(int finger_id)
	{
		foreach (TouchDragEvent touch_drag_event in touch_drag_events)
		{
			if (touch_drag_event.TouchEquals(finger_id))
			{
				return touch_drag_event;
			}
		}
		return null;
	}

	private PuppetArena GetTargetPuppet(Vector3 point)
	{
		foreach (PuppetArena player in playerList)
		{
			if (Vector3.Distance(point, player.transform.position) <= player.ControllerRadius * 1.5f)
			{
				return player;
			}
		}
		foreach (PuppetArena enemy in enemyList)
		{
			if (Vector3.Distance(point, enemy.transform.position) <= enemy.ControllerRadius * 1.5f)
			{
				return enemy;
			}
		}
		return null;
	}

	public void ChangeCameraTransform()
	{
		if (CurrentCameraType == 0)
		{
			camera_direction = CameraTransform[1] - CameraTransform[0];
			lookat_direction = CameraLookAtTransform[1] - CameraLookAtTransform[0];
			camera_rotate_target = Quaternion.LookRotation(CameraTransform[1] - CameraTransform[0]);
			lookat_rotate_target = Quaternion.LookRotation(CameraLookAtTransform[1] - CameraLookAtTransform[0]);
			fov_velocity = (CameraFov[1] - CameraFov[0]) / 0.5f;
			CurrentCameraType = 1;
		}
		else
		{
			camera_direction = CameraTransform[0] - CameraTransform[1];
			lookat_direction = CameraLookAtTransform[0] - CameraLookAtTransform[1];
			camera_rotate_target = Quaternion.LookRotation(CameraTransform[0] - CameraTransform[1]);
			lookat_rotate_target = Quaternion.LookRotation(CameraLookAtTransform[0] - CameraLookAtTransform[1]);
			fov_velocity = (CameraFov[0] - CameraFov[1]) / 0.5f;
			CurrentCameraType = 0;
		}
		camera_transform_change = true;
		lookat_transform_change = true;
		camera_fov_change = true;
	}

	private void DoChangeCameraTransform()
	{
		if (camera_transform_change)
		{
			base.transform.position += camera_rotate_target * Vector3.forward * Time.deltaTime * camera_velocity;
			Vector3 rhs = CameraTransform[CurrentCameraType] - base.transform.position;
			if (Vector3.Dot(camera_direction, rhs) < 0f)
			{
				base.transform.position = CameraTransform[CurrentCameraType];
				camera_transform_change = false;
			}
		}
		if (lookat_transform_change)
		{
			camera_lookAt.transform.position += lookat_rotate_target * Vector3.forward * Time.deltaTime * lookat_velocity;
			Vector3 rhs2 = CameraLookAtTransform[CurrentCameraType] - camera_lookAt.transform.position;
			if (Vector3.Dot(lookat_direction, rhs2) < 0f)
			{
				camera_lookAt.transform.position = CameraLookAtTransform[CurrentCameraType];
				lookat_transform_change = false;
			}
		}
		if (camera_fov_change)
		{
			Camera.main.fov += Time.deltaTime * fov_velocity;
			if ((fov_velocity > 0f && Camera.main.fov > CameraFov[CurrentCameraType]) || (fov_velocity <= 0f && Camera.main.fov < CameraFov[CurrentCameraType]))
			{
				Camera.main.fov = CameraFov[CurrentCameraType];
				camera_fov_change = false;
			}
		}
		Camera.main.transform.LookAt(camera_lookAt.transform.position);
	}

	public void UIPickPuppet(PuppetArena pick_puppet)
	{
		if (playerTallyList.Contains(pick_puppet) && !(pick_puppet == activing_puppet))
		{
			if (null != activing_puppet)
			{
				activing_puppet.OnCancelChoosed();
			}
			activing_puppet = pick_puppet;
			activing_puppet.OnBeChoosed();
		}
	}

	private bool PuppetHasTouchEvent(PuppetArena puppet)
	{
		foreach (TouchDragEvent touch_drag_event in touch_drag_events)
		{
			if (touch_drag_event.PickPuppet == puppet)
			{
				return true;
			}
		}
		return false;
	}

	public AureoleBehaviour CheckAureoleExisting(PuppetArena caster, AureoleConfig config)
	{
		foreach (AureoleBehaviour item in AureoleManager)
		{
			if (item.AureoleExisting(caster, config))
			{
				return item;
			}
		}
		return null;
	}

	public IEnumerator EnableBattleWinBehaviour()
	{
		playerWinnerList.Clear();
		foreach (PuppetArena puppet in playerTallyList)
		{
			if (null != puppet && !puppet.IsDead() && !puppet.IsInGrave())
			{
				puppet.OnCancelChoosed();
				playerWinnerList.Add(puppet);
			}
		}
		if (D3DGamer.Instance.TutorialState[0])
		{
			int vip_gold = 0;
			if (D3DGamer.Instance.ExpBonus == 0.2f && D3DGamer.Instance.GoldBonus == 0.1f)
			{
				vip_gold = Mathf.RoundToInt((float)battle_gained_gold * D3DGamer.Instance.GoldBonus);
				if (vip_gold < 1)
				{
					vip_gold = 1;
				}
			}
			D3DGamer.Instance.UpdateCurrency(battle_gained_gold + vip_gold);
			D3DShopRuleEx.Instance.RestBattleCount--;
		}
		if (playerWinnerList.Count <= 0)
		{
			if (null != D3DAudioManager.Instance.ArenaAudio)
			{
				D3DAudioManager.Instance.ArenaAudio.Stop();
				D3DAudioManager.Instance.ArenaAudio = null;
			}
			D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BATTLE_WIN), null, false, false);
			ui_arena.PopBattleResultUI(true);
			yield return 0;
			yield break;
		}
		foreach (PuppetArena summoned_puppet in playerSummonedList)
		{
			if (null == summoned_puppet || summoned_puppet.IsDead())
			{
				summoned_puppet.Variable(TriggerVariable.VariableType.HP_DAMAGE, summoned_puppet.profile_instance.puppet_property.hp_max, false);
			}
		}
		foreach (TouchDragEvent touch_event in touch_drag_events)
		{
			touch_event.DestroyDragComponents();
		}
		touch_drag_events.Clear();
		is_battle_win_behaviour = true;
		yield return new WaitForSeconds(0.3f);
		if (null != D3DAudioManager.Instance.ArenaAudio)
		{
			D3DAudioManager.Instance.ArenaAudio.Stop();
			D3DAudioManager.Instance.ArenaAudio = null;
		}
		D3DAudioManager.Instance.PlayAudio(D3DAudioManager.Instance.GetCommonAudio(D3DAudioManager.CommonAudio.BATTLE_WIN), null, false, false);
		BattleWinBehaviour win_behaviour = Camera.main.gameObject.AddComponent<BattleWinBehaviour>();
		win_behaviour.Init(this);
	}

	public void OnPlayerDead(PuppetArena puppet)
	{
		playerRecycleList.Add(puppet);
		playerList.Remove(puppet);
		if (playerTallyList.Contains(puppet))
		{
			player_rest--;
			ui_arena.RemoveFaceButton(puppet);
			playerGrave.Add(puppet);
			OnPlayerTallyCountChange();
		}
		if (player_rest == 0 && playerList.Count == 0 && BattleResult == 0)
		{
			BattleResult = -1;
		}
		if (playerSummonedList.Contains(puppet))
		{
			playerSummonedList.Remove(puppet);
			OnPlayerSummonedCountChange();
		}
		playerRecycleList.Remove(puppet);
		if (BattleResult == -1 && playerRecycleList.Count == 0)
		{
			if (!D3DGamer.Instance.TutorialState[0])
			{
				BattleResult = 0;
				ClearAllPlayer();
				ui_arena.ShowTutorialNewHeroComin(D3DTutorialHintCfg.DamaoHintData.HintCondition.NewHeroComin, 0, 2f);
			}
			else
			{
				ui_arena.StartCoroutine("EndBattleDelay");
			}
		}
	}

	public void OnEnemyDead(PuppetArena puppet)
	{
		enemyRecycleList.Add(puppet);
		enemyList.Remove(puppet);
		if (enemyTallyList.Contains(puppet))
		{
			enemyTallyList.Remove(puppet);
			kill_count++;
			ui_arena.kill_text.SetText(kill_count + " / " + kill_require);
			enemyGrave.Add(puppet);
			battle_gained_exp += Mathf.RoundToInt(puppet.profile_instance.Profile.percent_bonus[0] * (float)battle_exp_basic) + puppet.profile_instance.Profile.fixed_bonus[0];
			battle_gained_gold += Mathf.RoundToInt(puppet.profile_instance.Profile.percent_bonus[1] * (float)battle_gold_basic) + puppet.profile_instance.Profile.fixed_bonus[1];
			OnEnemyTallyCountChange();
		}
		if (kill_count >= kill_require && BattleResult == 0 && enemyList.Count == 0)
		{
			BattleResult = 1;
		}
		if (enemySummonedList.Contains(puppet))
		{
			enemySummonedList.Remove(puppet);
			OnEnemySummonedCountChange();
		}
		enemyRecycleList.Remove(puppet);
		if (BattleResult == 1 && enemyRecycleList.Count == 0)
		{
			ui_arena.StartCoroutine("EndBattleDelay");
		}
	}

	private void OnDrawGizmos()
	{
		for (int i = 0; i < 4; i++)
		{
			Gizmos.DrawSphere(new Vector3(WorldLimit[i].x, 0f, WorldLimit[i].y), 0.1f);
			if (i > 0)
			{
				Gizmos.DrawLine(new Vector3(WorldLimit[i].x, 0f, WorldLimit[i].y), new Vector3(WorldLimit[i - 1].x, 0f, WorldLimit[i - 1].y));
			}
			else
			{
				Gizmos.DrawLine(new Vector3(WorldLimit[0].x, 0f, WorldLimit[0].y), new Vector3(WorldLimit[3].x, 0f, WorldLimit[3].y));
			}
		}
	}
}
