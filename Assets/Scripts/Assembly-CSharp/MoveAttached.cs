using UnityEngine;

public class MoveAttached
{
	public delegate void OnMoveStop();

	public delegate void OnMove(Rect rect);

	private UIControl _MoveTarget;

	private float _fTolMoveTime;

	private Vector2 _vDesPos;

	private float _fMoveTimeLeft;

	private Vector2 _vStartPos;

	private OnMoveStop _onMoveStopFunc;

	private OnMove _onMoveFunc;

	public bool IsMoving
	{
		get
		{
			return _fMoveTimeLeft > 0f;
		}
	}

	public MoveAttached(UIButtonBase moveTarget, OnMoveStop onMoveStop, OnMove onMove)
	{
		_MoveTarget = moveTarget;
		_onMoveStopFunc = onMoveStop;
		_onMoveFunc = onMove;
	}

	public void Tick()
	{
		if (IsMoving)
		{
			_fMoveTimeLeft -= Time.deltaTime;
			Step();
			float f = _MoveTarget.Rect.x - _vDesPos.x * (float)D3DMain.Instance.HD_SIZE;
			float f2 = _MoveTarget.Rect.y - _vDesPos.y * (float)D3DMain.Instance.HD_SIZE;
			if (Mathf.Abs(f) < 1f && Mathf.Abs(f2) < 1f)
			{
				OnStop();
			}
			else if (!IsMoving)
			{
				OnStop();
			}
		}
	}

	public void StartMove(float fMoveTime, Vector2 vDes)
	{
		if (!IsMoving)
		{
			_fMoveTimeLeft = (_fTolMoveTime = fMoveTime);
			_vDesPos = vDes;
			_vStartPos = new Vector2(_MoveTarget.Rect.x, _MoveTarget.Rect.y) / D3DMain.Instance.HD_SIZE;
		}
	}

	private void OnStop()
	{
		_MoveTarget.Rect = D3DMain.Instance.ConvertRectAutoHD(_vDesPos.x, _vDesPos.y, 60f, 60f);
		_onMoveStopFunc();
	}

	private void Step()
	{
		if (_fTolMoveTime <= 0f)
		{
			Debug.LogError("divisor can less than or equals to ZERO!");
			return;
		}
		float fRatio = (_fTolMoveTime - _fMoveTimeLeft) / _fTolMoveTime;
		Vector2 vector = Tween.CalcCur(_vStartPos, _vDesPos, fRatio, Tween.TweenType.Back);
		_MoveTarget.Rect = D3DMain.Instance.ConvertRectAutoHD(vector.x, vector.y, 60f, 60f);
		_onMoveFunc(_MoveTarget.Rect);
	}
}
