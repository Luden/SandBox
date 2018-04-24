using Assets.Scripts;
using UnityEngine;

public class SelectionView : MonoBehaviour
{
    [SerializeField]
    Selectable _selectable = null;
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
