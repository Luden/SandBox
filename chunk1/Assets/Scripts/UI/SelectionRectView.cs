using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionRectView : MonoBehaviour
{
	[SerializeField]
	private Image _rectBackground;
	[SerializeField]
	private Image _rectBorder;
	[SerializeField]
	private RectTransform _rectTransform;

	[SerializeField]
	private SelectionRect _selectionRect;
	[SerializeField]
	private int _mouseButton;

	public void Start()
	{
		SetVisible(false);

		_selectionRect.OnRectStart += OnRectStart;
		_selectionRect.OnRectFinish += OnRectFinish;
		_selectionRect.OnRectUpdate += OnRectUpdate;
	}

	private void OnRectUpdate(Vector3 start, Vector3 finish, int button)
	{
		if (button != _mouseButton)
			return;
		_rectTransform.anchoredPosition = new Vector2(Math.Min(start.x, finish.x), Math.Min(start.y, finish.y));
		_rectTransform.sizeDelta = new Vector2(Math.Abs(start.x - finish.x), Math.Abs(start.y - finish.y));
	}

	private void OnRectFinish(Vector3 start, Vector3 finish, int button)
	{
		if (button != _mouseButton)
			return;
		SetVisible(false);
		_rectTransform.anchoredPosition = Vector2.zero;
		_rectTransform.sizeDelta = Vector2.zero;
	}

	private void OnRectStart(Vector3 start, Vector3 finish, int button)
	{
		if (button != _mouseButton)
			return;
		SetVisible(true);
	}

	private void SetVisible(bool visible)
	{
		_rectBackground.enabled = visible;
		_rectBorder.enabled = visible;
	}
}
