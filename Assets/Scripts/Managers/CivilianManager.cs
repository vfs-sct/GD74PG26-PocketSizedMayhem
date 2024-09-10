using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CivilianManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> _civilians;

    public void RemoveCivilian(object sender, GameObject civilian)
    {
        if (!_civilians.Contains(civilian))
        {
            return;
        }
        _civilians.Remove(civilian);
        civilian.GetComponent<CivilianDeath>().OnKilled -= RemoveCivilian;
    }

    public void AddToCivilianList(GameObject civilian)
    {
        civilian.GetComponent<CivilianDeath>().OnKilled += RemoveCivilian;
        _civilians.Add(civilian);
    }
}
