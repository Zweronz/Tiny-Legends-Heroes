using UnityEngine;

public class TestPuppetConfig : MonoBehaviour
{
	public int SaveDataIndex;

	private void Start()
	{
		D3DMain.Instance.LoadD3DEquipmentsBatch("Dungeons3D/Docs/D3DEquipmentCfg");
		D3DMain.Instance.LoadD3DClassesBatch("Dungeons3D/Docs/D3DClassCfg");
		D3DMain.Instance.LoadD3DPuppetProfilesBatch("Dungeons3D/Docs/D3DPuppetProfileCfg");
		D3DLootFromula.Instance.InitLootFormula();
		D3DMain.Instance.LoadD3DFormula("Dungeons3D/Docs/D3DOthersCfg/D3Dgongshi");
		if (!Utils.CheckFileExists("team.tlh"))
		{
			D3DGamer.Instance.DefaultTeamData();
			D3DGamer.Instance.SaveTeamData();
		}
		D3DGamer.Instance.LoadTeamData();
		PuppetBasic puppetBasic = base.gameObject.AddComponent<PuppetBasic>();
		if (puppetBasic.InitProfileInstance(D3DMain.Instance.GetProfileClone(D3DGamer.Instance.PlayerBattleTeamData[SaveDataIndex].pupet_profile_id), D3DGamer.Instance.PlayerBattleTeamData[SaveDataIndex]))
		{
			puppetBasic.model_builder.BuildPuppetModel();
			puppetBasic.model_builder.AddAnimationEvent();
			puppetBasic.model_builder.PlayPuppetAnimations(true, puppetBasic.model_builder.ChangeableIdleClip, WrapMode.Loop, true);
			puppetBasic.CheckPuppetWeapons();
		}
	}

	private void Update()
	{
	}
}
