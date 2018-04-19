using InputHelpers;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField]
	private float _borderMovementSpeed = 1f;
	[SerializeField]
	private float _borderMovementSpeedupSteps = 10f;
	[SerializeField]
	private int _borderMovementZoneWidth = 5;
	[SerializeField]
	private int _borderMovementZoneOuterWidth = 10;
	[SerializeField]
	private bool _borderMovementEnabled = true;

	[Space, SerializeField]
	private float _arrowMovementSpeed = 1f;
	[SerializeField]
	private bool _arrowMovementEnabled = true;

	[Space, SerializeField]
	private float _mouseMovementSpeed = 1f;
	[SerializeField]
	private bool _mouseMovementEnabled = true;

	[Space, SerializeField]
	private float _mouseScrollSpeed = 1f;
	[SerializeField]
	private bool _mouseScrollEnabled = true;
	[SerializeField]
	private float _scrollStep = 0.1f;


	[Space, SerializeField]
	private float MinX = -100;
	[SerializeField]
	private float MaxX = 100;
	[SerializeField]
	private float MinZ = -100;
	[SerializeField]
	private float MaxZ = 100;
	[SerializeField]
	private float MaxY = 10;
	[SerializeField]
	private float MinY = 5;
	[SerializeField]
	private float MaxHeightPitch = 60;
	[SerializeField]
	private float MinHeightPitch = 40;

	void Start()
	{

	}

	void Update()
	{
		UpdateMovement();
	}

	private void UpdateMovement()
	{
		var movement = UpdateMiddleMouseMovement();
		if (movement == Vector3.zero && !_wasMouseDown)
			movement = UpdateBorderMovement();
		if (movement == Vector3.zero && !_wasMouseDown)
			movement = UpdateArrowMovement();

		var heightMovement = UpdateScrollMovement();
		movement += heightMovement;

		if (movement != Vector3.zero)
		{
			transform.position = ClampPosition(transform.position + movement);
			if (heightMovement != Vector3.zero)
			{
				var angle = Mathf.Lerp(MinHeightPitch, MaxHeightPitch, (transform.position.y - MinY) / (MaxY - MinY));
				transform.eulerAngles = new Vector3(angle, transform.eulerAngles.y, transform.eulerAngles.z);
			}
		}
	}

	private Vector3 ClampPosition(Vector3 position)
	{
		if (position.x < MinX)
			position.x = MinX;
		if (position.x > MaxX)
			position.x = MaxX;
		if (position.y < MinY)
			position.y = MinY;
		if (position.y > MaxY)
			position.y = MaxY;
		if (position.z < MinZ)
			position.z = MinZ;
		if (position.z > MaxZ)
			position.z = MaxZ;
		return position;
	}

	bool _wasMouseDown;
	Vector3 _lastMousePos;
	Vector3 UpdateMiddleMouseMovement()
	{
		var result = Vector3.zero;
		if (!_mouseMovementEnabled)
			return result;

		var mouseDown = Input.GetMouseButton(2);
		if (!mouseDown)
		{
			if (_wasMouseDown)
				_wasMouseDown = false;
			return result;
		}
		else
		{
			if (!_wasMouseDown)
			{
				_lastMousePos = Input.mousePosition;
				_wasMouseDown = true;
			}
			else
			{
				var mousePos = Input.mousePosition;
				var delta = mousePos - _lastMousePos;
				result.x = delta.x;
				result.z = delta.y;
				_lastMousePos = mousePos;
			}
		}
		return result * _mouseMovementSpeed;
	}

	Vector3 UpdateArrowMovement()
	{
		var result = Vector3.zero;
		if (!_arrowMovementEnabled)
			return result;

		if (Input.GetButton(InputKeys.CameraLeft))
			result.x = -1;
		if (Input.GetButton(InputKeys.CameraRight))
			result.x = 1;
		if (Input.GetButton(InputKeys.CameraUp))
			result.z = 1;
		if (Input.GetButton(InputKeys.CameraDown))
			result.z = -1;

		return result * _arrowMovementSpeed;
	}

	float _borderMovementCurrentSpeed;
	Vector3 UpdateBorderMovement()
	{
		var result = Vector3.zero;
		if (!_borderMovementEnabled)
			return result;

		if (Input.mousePosition.x <= _borderMovementZoneWidth && Input.mousePosition.x > -_borderMovementZoneOuterWidth)
			result.x = -1;
		else if (Input.mousePosition.x >= Screen.width - _borderMovementZoneWidth && Input.mousePosition.x < Screen.width + _borderMovementZoneOuterWidth)
			result.x = 1;
		if (Input.mousePosition.y <= _borderMovementZoneWidth && Input.mousePosition.y > -_borderMovementZoneOuterWidth)
			result.z = -1;
		else if (Input.mousePosition.y >= Screen.height - _borderMovementZoneWidth && Input.mousePosition.y < Screen.height + _borderMovementZoneOuterWidth)
			result.z = 1;

		if (result != Vector3.zero)
		{
			_borderMovementCurrentSpeed += _borderMovementSpeed / _borderMovementSpeedupSteps;
			_borderMovementCurrentSpeed = Mathf.Min(_borderMovementSpeed, _borderMovementCurrentSpeed);
			result = result * _borderMovementCurrentSpeed;
		}
		else if (_borderMovementCurrentSpeed != 0f)
		{
			_borderMovementCurrentSpeed = 0f;
		}
		return result;
	}

	float _scrollTarget;
	Vector3 UpdateScrollMovement()
	{
		var result = Vector3.zero;
		if (!_mouseScrollEnabled)
			return result;

		var delta = Input.mouseScrollDelta;
		if (delta != Vector2.zero)
			_scrollTarget += delta.y * _mouseScrollSpeed;

		if (_scrollTarget == 0)
			return result;

		var step = _scrollStep;
		if (Mathf.Abs(_scrollTarget) > Mathf.Abs(_mouseScrollSpeed))
			step *= Mathf.Abs(_scrollTarget / _mouseScrollSpeed);
		if (Mathf.Abs(_scrollTarget) < _scrollStep)
			step = Mathf.Abs(_scrollTarget);

		result.y = step * Mathf.Sign(_scrollTarget);
		_scrollTarget -= (step * Mathf.Sign(_scrollTarget));
		
		return result;
	}
}
