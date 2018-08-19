using Assets.Scripts;
using Assets.Scripts.Units;
using UnityEngine;

public class SelectionView : MonoBehaviour
{
    [SerializeField]
    Unit _unit = null;
    MeshRenderer _visual;

    void Start()
    {
        _unit.Selectable.OnSelectionChange += UpdateSelection;
        _visual = GetComponent<MeshRenderer>();
        UpdateSelection(false);
    }

    private void UpdateSelection(bool selected)
    {
        _visual.enabled = selected;
    }
}
