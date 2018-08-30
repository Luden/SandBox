using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Units;
using UnityEngine;

public class SelectionView : MonoBehaviour
{
    Unit _unit = null;
    MeshRenderer _visual;

    public List<Material> Materials = new List<Material>();

    public void Init(Unit unit)
    {
        _unit = unit;
        _visual = GetComponent<MeshRenderer>();
        _visual.material = Materials[(int)_unit.Player.Faction];
        _unit.Selectable.OnSelectionChange += UpdateSelection;
        UpdateSelection(false);
    }

    private void UpdateSelection(bool selected)
    {
        _visual.enabled = selected;
    }
}
