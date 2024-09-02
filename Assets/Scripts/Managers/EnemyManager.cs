using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _civilians;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject ClosestCivilian(GameObject enemy)
    {
        float distance;
        GameObject closest = null;
        if (_civilians.Count > 0)
        {
            distance  = Vector3.Distance(_civilians[0].transform.position, enemy.transform.position); ;
        }
        else
        {
            return null;
        }
        foreach(GameObject gameobject in _civilians)
        {
            float compareDist;
            compareDist = Vector3.Distance(gameobject.transform.position, enemy.transform.position);
            if(compareDist < distance)
            {
                distance = compareDist;
                closest = gameobject;
            }
        }
        return closest;
    }
}
