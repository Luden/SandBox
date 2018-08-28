using Assets.Scripts.Units;
using UnityEngine;

public class PreselectionView : MonoBehaviour
{
    Unit _unit;
    MeshRenderer _visual;
    Animation _animation;

    public void Init(Unit unit)
    {
        _unit = unit;
        _unit.Selectable.OnPreselectionChange += UpdatePreselection;
        _visual = GetComponent<MeshRenderer>();
        _animation = GetComponent<Animation>();
        _animation.Stop();
        UpdatePreselection(false);
    }

    private void UpdatePreselection(bool selected)
    {
        _visual.enabled = selected;
        if (selected)
            _animation.Play();
        else
            _animation.Stop();
    }
}
