using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _civilians;
    [SerializeField] private CivilianBehaviour[] allObjects;
    private void Awake()
    {
        allObjects = FindObjectsOfType<CivilianBehaviour>();
        foreach (var obj in allObjects)
        {
            _civilians.Add(obj.gameObject);
        }
    }

    public GameObject ClosestCivilian(GameObject enemy)
    {
        float distance;
        GameObject closest = null;
        if (_civilians.Count > 0)
        {
            distance  = Vector3.Distance(enemy.transform.position, _civilians[0].transform.position);
            closest = _civilians[0];
        }
        else
        {
            return null;
        }
        foreach(GameObject civilian in _civilians)
        {
            float compareDist;
            compareDist = Vector3.Distance(enemy.transform.position, civilian.transform.position);
            if(compareDist < distance)
            {
                distance = compareDist;
                closest = civilian;
            }
        }
        return closest;
    }

    public void EliminateCivilian(GameObject civilian)
    {
        _civilians.Remove(civilian);
    }
}
