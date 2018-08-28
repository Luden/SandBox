using Assets.Scripts;
using Assets.Scripts.Units;
using UnityEngine;

public class SelectionView : MonoBehaviour
{
    Unit _unit = null;
    MeshRenderer _visual;

    public void Init(Unit unit)
    {
        _unit = unit;
        _unit.Selectable.OnSelectionChange += UpdateSelection;
        _visual = GetComponent<MeshRenderer>();
        UpdateSelection(false);
    }

    private void UpdateSelection(bool selected)
    {
        _visual.enabled = selected;
    }
}
