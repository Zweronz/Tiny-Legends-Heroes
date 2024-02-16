using UnityEngine;

public class D3DPuppetTransformCfg
{
	public class CharacterControllerCfg
	{
		public Vector3 center;

		public float radius;

		public float height;
	}

	public class CameraCfg
	{
		public Vector3 offset;

		public Vector3 rotation;

		public float size;
	}

	public float ring_size;

	public CharacterControllerCfg character_controller_cfg;

	public CameraCfg stash_camera_cfg;

	public CameraCfg face_camera_cfg;

	public CameraCfg tavern_hire_camera_cfg;

	public CameraCfg tavern_camp_camera_cfg;

	public CameraCfg tavern_battle_team_cfg;
}
