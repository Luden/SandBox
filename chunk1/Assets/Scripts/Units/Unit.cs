using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Commands;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public ISelectable Selectable;
	public CommandProcessor CommandProcessor;
	public NavMeshAgent NavMeshAgent;
    public NavMeshObstacle NavMeshObstacle;

    void Start()
	{
        Selectable = GetComponent<Selectable>();
		NavMeshAgent = GetComponent<NavMeshAgent>();
        //NavMeshAgent.enabled = false;
        NavMeshObstacle = GetComponent<NavMeshObstacle>();

        var provider = ManagerProvider.Instance;
		CommandProcessor = new CommandProcessor(provider.CommandManager.CommandFactory, provider.TimeManager, provider.GameSettings.UnitCommandsUpdatePeriod, this);
	}

	public void Stop()
	{
		CommandProcessor.Stop();
	}

    HashSet<Unit> _neighbours = new HashSet<Unit>();
    GameObject _reachedTarget = null;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Unit")
            _neighbours.Add(other.gameObject.GetComponent<Unit>());
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Unit")
            _neighbours.Remove(other.gameObject.GetComponent<Unit>());
    }

    int _visitorHash = -1;
    List<Unit> _cacheNeighbours = new List<Unit>();
    List<Unit> GetNeighbours(bool isMoving)
    {
        _cacheNeighbours.Clear();
        foreach (var unit in _neighbours)
            if (unit.CommandProcessor.IsMoving() == isMoving)
                _cacheNeighbours.Add(unit);
        return _cacheNeighbours;
    }

    bool IsUnitOrNeighboursReachedTarget(Vector3 point, int visitorHash)
    {
        if (visitorHash == _visitorHash)
            return false;

        _visitorHash = visitorHash;

        return IsUnitReachedTarget(point) || IsNeighboursReachedTarget(point, visitorHash);
    }

    public bool IsUnitReachedTarget(Vector3 point)
    {
        return (transform.position - point).sqrMagnitude < 1f;
    }

    public bool IsNeighboursReachedTarget(Vector3 point)
    {
        _visitorHash = Random.Range(0, int.MaxValue);
        return IsNeighboursReachedTarget(point, _visitorHash) && _neighbours.Count > 1;
    }

    bool IsNeighboursReachedTarget(Vector3 point, int visitorHash)
    {
        var staticNeighbours = GetNeighbours(false);
        if (staticNeighbours.Count == 0)
            return false;

        foreach (var unit in staticNeighbours)
            if (unit.IsUnitOrNeighboursReachedTarget(point, visitorHash))
                return true;

        return false;
    }

    public Unit GetOppositeNeighbour()
    {
        var direction = NavMeshAgent.steeringTarget - transform.position;
        foreach (var unit in GetNeighbours(true))
        {
            var unitDirection = unit.NavMeshAgent.steeringTarget - unit.transform.position;
            if (Vector3.Dot(unitDirection, direction) < 0.1f)
                return unit;
        }
        return null;
    }
}
