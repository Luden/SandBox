using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectionRectView : MonoBehaviour
{
	[SerializeField]
	private Image _rectBackground = null;
	[SerializeField]
	private Image _rectBorder = null;
	[SerializeField]
	private RectTransform _rectTransform = null;

	[SerializeField]
	private SelectionRect _selectionRect = null;

	public void Start()
	{
        _rectTransform.anchoredPosition = Vector2.zero;
        _rectTransform.sizeDelta = Vector2.zero;

        SetVisible(false);

		_selectionRect.OnRectStart += OnRectStart;
		_selectionRect.OnRectFinish += OnRectFinish;
		_selectionRect.OnRectUpdate += OnRectUpdate;
	}

    public void Dispose()
    {
        _selectionRect.OnRectStart -= OnRectStart;
        _selectionRect.OnRectFinish -= OnRectFinish;
        _selectionRect.OnRectUpdate -= OnRectUpdate;
    }

	private void OnRectUpdate(Vector3 start, Vector3 finish)
	{
		_rectTransform.anchoredPosition = new Vector2(Math.Min(start.x, finish.x), Math.Min(start.y, finish.y));
		_rectTransform.sizeDelta = new Vector2(Math.Abs(start.x - finish.x), Math.Abs(start.y - finish.y));
	}

	private void OnRectFinish(Vector3 start, Vector3 finish)
	{
		SetVisible(false);
		_rectTransform.anchoredPosition = Vector2.zero;
		_rectTransform.sizeDelta = Vector2.zero;
	}

	private void OnRectStart(Vector3 start, Vector3 finish)
	{
		SetVisible(true);
	}

	private void SetVisible(bool visible)
	{
		_rectBackground.enabled = visible;
		_rectBorder.enabled = visible;
	}
}
