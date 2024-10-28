using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDestruction : MonoBehaviour
{
    [SerializeField] private int _force;
    [SerializeField] List<GameObject> _pieces;
    [SerializeField] GameObject _civilians;
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
            this.GetComponent<Rigidbody>().isKinematic = false;
            foreach (GameObject piece in _pieces)
            {
                piece.AddComponent<Rigidbody>();
                Vector3 direction = Random.insideUnitCircle.normalized;
                piece.GetComponent<Rigidbody>().AddForce(direction * _force, ForceMode.Impulse);
            }
            _isDestoyed = true;
            StartCoroutine(AssignDebri());
            int random = Random.Range(0, 4);
            for(int i =0;i< random;i++)
            {
                GameObject civilian = Instantiate(_civilians, this.gameObject.transform.position + Vector3.up*5,_civilians.transform.rotation);
                civilian.GetComponent<RagdollOnOffController>().RagdollModeOn();
                civilian.GetComponent<RagdollOnOffController>().DeathBounce();
            }
        }
    }

    IEnumerator AssignDebri()
    {
        yield return new WaitForSeconds(1f);
        foreach (GameObject piece in _pieces)
        {
            piece.layer = LayerMask.NameToLayer("Debris");
        }
    }
}
