using UnityEngine;

public class DungeonFloorEnemyStateRecord
{
	public Vector3 position;

	public Quaternion rotation;

	public PuppetDungeonEnmeyBehaviour.PostState behaviour_post_state;

	public DungeonFloorEnemyStateRecord(Vector3 position, Quaternion rotation, PuppetDungeonEnmeyBehaviour.PostState post_state)
	{
		this.position = position;
		this.rotation = rotation;
		behaviour_post_state = post_state;
	}
}
