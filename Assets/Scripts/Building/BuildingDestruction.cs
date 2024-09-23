using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDestruction : MonoBehaviour
{
    [SerializeField] private int _force;
    [SerializeField] List<GameObject> _pieces;
    private bool _isDestoyed;
    // Start is called before the first frame update
    void Start()
    {
        _isDestoyed = false;
        _pieces = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject piece = transform.GetChild(i).gameObject;
            _pieces.Add(piece);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mallet" && !_isDestoyed)
        {
            foreach (GameObject piece in _pieces)
            {
                piece.AddComponent<Rigidbody>();
                Vector3 direction = Random.insideUnitCircle.normalized;
                piece.GetComponent<Rigidbody>().AddForce(direction * _force, ForceMode.Impulse);
            }
            _isDestoyed = true;
        }
    }
}
