using Assets.Scripts;
using UnityEngine;

public class PreselectionView : MonoBehaviour
{
    [SerializeField]
    Selectable _selectable = null;
    MeshRenderer _visual;
    Animation _animation;

    void Start()
    {
        _selectable.OnPreselectionChange += UpdatePreselection;
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
