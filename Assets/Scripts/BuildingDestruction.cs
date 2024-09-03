using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDestruction : MonoBehaviour
{
    [SerializeField] List<GameObject> _pieces;
    // Start is called before the first frame update
    void Start()
    {
        _pieces = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject piece = transform.GetChild(i).gameObject;
            _pieces.Add(piece);

        }
    }
}
