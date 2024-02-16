using UnityEngine;

public class PuppetMonitorBasic : MonoBehaviour
{
	protected PuppetArena puppet_component;

	protected void Awake()
	{
		puppet_component = GetComponent<PuppetArena>();
		puppet_component.puppet_monitor = this;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public virtual void OnEnter()
	{
	}

	public virtual void OnDead()
	{
	}

	public virtual void OnBodyRecycle()
	{
	}

	public virtual void OnHPDecrease()
	{
	}

	public virtual void OnFriendCountChange(int count)
	{
	}

	public virtual void OnEnemyCountChange(int count)
	{
	}

	public virtual void OnSummonedCountChange(int count)
	{
	}

	public virtual void OnIdle()
	{
	}

	public virtual void OnMove()
	{
	}

	public virtual void OnExcuteSkillOver()
	{
	}

	public virtual void OnRevive()
	{
	}

	protected virtual void Reset()
	{
	}
}
