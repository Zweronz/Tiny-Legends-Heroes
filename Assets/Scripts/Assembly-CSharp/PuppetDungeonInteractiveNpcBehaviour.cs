using UnityEngine;

public class PuppetDungeonInteractiveNpcBehaviour : MonoBehaviour
{
	private PuppetDungeon puppet_dungeon;

	private SceneDungeon scene_dungeon;

	private D3DInteractiveNpc.NpcFunction npc_function;

	private void Awake()
	{
		puppet_dungeon = GetComponent<PuppetDungeon>();
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (!(Vector3.Distance(base.transform.position, scene_dungeon.map_player.transform.position) <= puppet_dungeon.model_builder.TransformCfg.character_controller_cfg.radius + scene_dungeon.map_player.model_builder.TransformCfg.character_controller_cfg.radius + 0.3f))
		{
			return;
		}
		if (scene_dungeon.map_player.GetPuppetState() == PuppetDungeon.MapPuppetState.Move)
		{
			scene_dungeon.map_player.StopMove();
		}
		else if (!scene_dungeon.map_player.DoingRotation)
		{
			scene_dungeon.picking_interactive_npc.PuppetRingVisible(false);
			scene_dungeon.picking_interactive_npc = null;
			switch (npc_function)
			{
			case D3DInteractiveNpc.NpcFunction.HERO_HIRE:
				scene_dungeon.ui_dungeon.OpenTavern();
				break;
			case D3DInteractiveNpc.NpcFunction.SKILL_SCHOOL:
				scene_dungeon.ui_dungeon.OpenSkillSchool();
				break;
			case D3DInteractiveNpc.NpcFunction.SHOP:
				scene_dungeon.ui_dungeon.OpenShop();
				break;
			}
			base.enabled = false;
		}
	}

	public void Init(SceneDungeon scene_dungeon, D3DInteractiveNpc.NpcFunction npc_function)
	{
		this.scene_dungeon = scene_dungeon;
		this.npc_function = npc_function;
	}
}
