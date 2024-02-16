using System.Collections;
using UnityEngine;

public class PuppetDungeonEnmeyBehaviour : MonoBehaviour
{
	public enum PostState
	{
		IDLE = 0,
		PATROL = 1,
		PURSUE = 2,
		RETURN = 3,
		BREAK = 4
	}

	public PostState post_state;

	public PostState record_state;

	private PuppetDungeon puppet_dungeon;

	private PuppetDungeon player_target;

	private float[] idle_time_interval;

	private float idle_delta;

	private float idle_time;

	private float sight_delta;

	private float sight_check_time;

	private float pursue_delta;

	private float pursue_check_time;

	public int floor_spawner_id;

	public D3DEnemyGroup enemy_group;

	private Vector3 post_position;

	private Vector3 pursue_start_position;

	private int group_level;

	private bool weak_enemy;

	public bool WeakEnemy
	{
		get
		{
			return weak_enemy;
		}
	}

	public int GroupLevel
	{
		get
		{
			return group_level;
		}
	}

	private void Awake()
	{
		puppet_dungeon = GetComponent<PuppetDungeon>();
		post_state = PostState.IDLE;
		record_state = PostState.IDLE;
		idle_time_interval = new float[2] { 1f, 3f };
		idle_delta = 0f;
		idle_time = Random.Range(idle_time_interval[0], idle_time_interval[1]);
		sight_delta = 0f;
		sight_check_time = 0.1f;
		pursue_delta = 0f;
		pursue_check_time = 0.2f;
	}

	private void Start()
	{
	}

	private void Update()
	{
		switch (post_state)
		{
		case PostState.IDLE:
			idle_delta += Time.deltaTime;
			if (idle_delta >= idle_time)
			{
				PatrolBehaviour();
			}
			PursueBehaviour();
			break;
		case PostState.PATROL:
			if (puppet_dungeon.GetPuppetState() == PuppetDungeon.MapPuppetState.Idle)
			{
				post_state = PostState.IDLE;
				idle_delta = 0f;
				idle_time = Random.Range(idle_time_interval[0], idle_time_interval[1]);
			}
			PursueBehaviour();
			break;
		case PostState.PURSUE:
			pursue_delta += Time.deltaTime;
			if (pursue_delta >= pursue_check_time)
			{
				puppet_dungeon.SetTarget(player_target.transform.position);
				pursue_delta = 0f;
			}
			ReturnBehaviour();
			break;
		case PostState.RETURN:
			if (puppet_dungeon.GetPuppetState() == PuppetDungeon.MapPuppetState.Idle)
			{
				post_state = PostState.IDLE;
				idle_delta = 0f;
				idle_time = Random.Range(idle_time_interval[0], idle_time_interval[1]);
			}
			break;
		}
	}

	public void Init(int spawner_id, D3DEnemyGroup group, int group_level)
	{
		floor_spawner_id = spawner_id;
		enemy_group = group;
		if (enemy_group.patrol_radius <= 0f)
		{
			idle_time_interval[0] = 999999f;
			idle_time_interval[1] = 999999f;
			idle_time = 999999f;
		}
		if (enemy_group.sight_radius <= 0f)
		{
			sight_check_time = 999999f;
		}
		post_position = D3DMain.Instance.exploring_dungeon.floor_spawn_points[floor_spawner_id].spawner_position;
		base.transform.position = post_position;
		player_target = puppet_dungeon.scene_dungeon.map_player;
		this.group_level = group_level;
		if (D3DGamer.Instance.BattleTeamMaxLevel > this.group_level)
		{
			weak_enemy = true;
		}
		GetComponent<PuppetDungeon>().InitPuppetComponents(weak_enemy);
	}

	private void PatrolBehaviour()
	{
		if (!(enemy_group.patrol_radius <= 0f))
		{
			Vector3 target_pt = post_position + new Vector3(Random.Range(0f - enemy_group.patrol_radius, enemy_group.patrol_radius), 0f, Random.Range(0f - enemy_group.patrol_radius, enemy_group.patrol_radius));
			puppet_dungeon.SetTarget(target_pt, enemy_group.patrol_speed);
			post_state = PostState.PATROL;
		}
	}

	private void PursueBehaviour()
	{
		sight_delta += Time.deltaTime;
		if (sight_delta >= sight_check_time)
		{
			if (enemy_group.sight_radius > 0f && Vector3.Distance(player_target.transform.position, base.transform.position) <= enemy_group.sight_radius)
			{
				post_state = PostState.PURSUE;
				pursue_start_position = base.transform.position;
				puppet_dungeon.SetTarget(player_target.transform.position, enemy_group.pursue_speed);
				pursue_delta = 0f;
			}
			sight_delta = 0f;
		}
	}

	private void ReturnBehaviour()
	{
		if (Vector3.Distance(base.transform.position, pursue_start_position) >= enemy_group.pursue_radius || puppet_dungeon.GetPuppetState() == PuppetDungeon.MapPuppetState.Idle)
		{
			post_state = PostState.RETURN;
			puppet_dungeon.SetTarget(post_position, enemy_group.patrol_speed);
		}
	}

	public IEnumerator BreakBehaviour(float break_time)
	{
		post_state = PostState.BREAK;
		yield return new WaitForSeconds(break_time);
		ResumeBehaviour();
	}

	public void SetBreakStateOnPlayerPortal()
	{
		post_state = PostState.BREAK;
		StopAllCoroutines();
	}

	private void ResumeBehaviour()
	{
		post_state = record_state;
		switch (post_state)
		{
		case PostState.IDLE:
			idle_delta = 0f;
			break;
		case PostState.PATROL:
		case PostState.PURSUE:
			post_state = PostState.IDLE;
			idle_delta = 0f;
			break;
		case PostState.RETURN:
			puppet_dungeon.SetTarget(post_position, enemy_group.patrol_speed);
			break;
		}
	}

	public void CancelPursue()
	{
		if (post_state == PostState.PURSUE)
		{
			post_state = PostState.RETURN;
			puppet_dungeon.SetTarget(post_position, enemy_group.patrol_speed);
		}
	}
}
