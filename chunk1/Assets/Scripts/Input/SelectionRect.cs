using System;
using UnityEngine;

public class SelectionRect : MonoBehaviour
{
    public Action<Vector3> OnIdle;

	public Action<Vector3, Vector3> OnRectStart;
	public Action<Vector3, Vector3> OnRectFinish;
    public Action<Vector3, Vector3> OnRectUpdate;

	public Action<Vector3, Vector3> OnRectStartRight;
	public Action<Vector3, Vector3> OnRectFinishRight;
	public Action<Vector3, Vector3> OnRectUpdateRight;

	private Vector3 _startPosition;
	private Vector3 _finishPosition;
	private bool _started = false;
	private int _startedButton;

	public void Update()
	{
        if (!_started && Input.GetMouseButtonDown(0))
        {
            StartRect(Input.mousePosition, 0);
        }
        else if (!_started && Input.GetMouseButtonDown(1))
        {
            StartRect(Input.mousePosition, 1);
        }
        else if (_started && Input.GetMouseButtonUp(_startedButton))
        {
            FinishRect(Input.mousePosition);
        }
        else if (_started)
        {
            UpdateRect(Input.mousePosition);
            FireEvent(_startedButton == 0 ? OnRectUpdate : OnRectUpdateRight);
        }
        else
        {
            OnIdle(Input.mousePosition);
        }
	}

	public void StartRect(Vector3 position, int button)
	{
		_started = true;
		_startedButton = button;
		_startPosition = position;
		UpdateRect(_startPosition);
		FireEvent(_startedButton == 0 ? OnRectStart : OnRectStartRight);
	}

	public void FinishRect(Vector3 position)
	{
		UpdateRect(position);
        FireEvent(_startedButton == 0 ? OnRectFinish : OnRectFinishRight);
		_started = false;
	}

    private void FireEvent(Action<Vector3, Vector3> action)
    {
        var leftTop = new Vector3(Math.Min(_startPosition.x, _finishPosition.x), Math.Min(_startPosition.y, _finishPosition.y));
        var rightBottom = new Vector3(Math.Max(_startPosition.x, _finishPosition.x), Math.Max(_startPosition.y, _finishPosition.y));
        if (action != null)
            action(leftTop, rightBottom);
    }

	public void UpdateRect(Vector3 position)
	{
		_finishPosition = position;
    }
}
