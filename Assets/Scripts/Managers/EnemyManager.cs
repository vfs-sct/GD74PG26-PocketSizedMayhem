using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<CivilianBehaviour> _civilians;
    [SerializeField] private List<RegularCriminalBehaviour> _regularEnemies;
    [SerializeField] private CivilianBehaviour[] _civilianBehaviors;
    [SerializeField] private RegularCriminalBehaviour[] _regularCriminalBehaviors;
    private void Awake()
    {
        _civilianBehaviors = FindObjectsOfType<CivilianBehaviour>();
        _regularCriminalBehaviors = FindObjectsOfType<RegularCriminalBehaviour>();
        foreach (CivilianBehaviour obj in _civilianBehaviors)
        {
            _civilians.Add(obj);
        }
        foreach (RegularCriminalBehaviour obj in _regularCriminalBehaviors)
        {
            _regularEnemies.Add(obj);
        }
    }

    private void Update()
    {
        foreach (RegularCriminalBehaviour gameObject in _regularEnemies)
        {
            if(!gameObject.HasTarget())
            {
                gameObject.SetTarget(ClosestCivilian(gameObject));
            }
        }
    }

    public GameObject ClosestCivilian(RegularCriminalBehaviour enemy)
    {
        float distance = enemy.GetDetectionRadius();
        GameObject closest = null;
        foreach(CivilianBehaviour civilian in _civilians)
        {
            float compareDist;
            compareDist = Vector3.Distance(enemy.gameObject.transform.position, civilian.gameObject.transform.position);
            if(compareDist < distance)
            {
                distance = compareDist;
                closest = civilian.gameObject;
            }
        }
        return closest;
    }
}
