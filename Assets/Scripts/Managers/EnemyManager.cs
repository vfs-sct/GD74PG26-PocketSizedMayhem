using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private CivilianManager _civilianManager;

    [SerializeField] private List<RegularCriminalBehaviour> _regularEnemies;

    private void Update()
    {
        foreach (RegularCriminalBehaviour criminal in _regularEnemies)
        {
            if(!criminal.HasTarget())
            {
                criminal.SetTarget(ClosestCivilian(criminal));
            }
        }
    }

    public GameObject ClosestCivilian(RegularCriminalBehaviour enemy)
    {
        float distance = enemy.GetDetectionRadius();
        GameObject closest = null;
        foreach(GameObject civilian in _civilianManager._civilians)
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
