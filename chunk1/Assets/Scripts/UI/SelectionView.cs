using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class SelectionView : MonoBehaviour
{
    [SerializeField]
    Selectable _selectable;
    MeshRenderer _visual;

    void Start()
    {
        _selectable.OnSelectionChange += UpdateSelection;
        _visual = GetComponent<MeshRenderer>();
        UpdateSelection(false);
    }

    private void UpdateSelection(bool selected)
    {
        _visual.enabled = selected;
    }
}
