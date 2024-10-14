using System.Collections.Generic;
using UnityEngine;

public class Vacuum : MonoBehaviour
{
    [SerializeField] List<GameObject> pulledObjects;
    [SerializeField] List<GameObject> capturedObjects;
    private bool _vacuumOn = false;
    private void Start()
    {
        pulledObjects = new List<GameObject>();
        capturedObjects = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        if (_vacuumOn)
        {
            foreach (GameObject enemy in pulledObjects)
            {
                Vector3 pullForce = (this.gameObject.transform.position - enemy.transform.position).normalized/ Vector3.Distance(this.gameObject.transform.position, enemy.transform.position) * 300;
                enemy.GetComponent<Rigidbody>().velocity  = (new Vector3(pullForce.x, pullForce.y, pullForce.z));
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            pulledObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        pulledObjects.Remove(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && _vacuumOn)
        {
            capturedObjects.Add(collision.gameObject);
            collision.rigidbody.velocity = Vector3.zero;
            if (!collision.gameObject.TryGetComponent<FixedJoint>(out FixedJoint joint))
            {
                collision.gameObject.AddComponent<FixedJoint>();
                collision.gameObject.GetComponent<FixedJoint>().connectedBody = this.GetComponent<Rigidbody>();
                collision.gameObject.transform.parent = this.gameObject.transform;
            }
        }
    }
    public void ReleaseAll()
    {
        foreach (GameObject enemy in capturedObjects)
        {
            Destroy(enemy.GetComponent<FixedJoint>());
            enemy.transform.parent = null;
        }
    }
    public void VacuumOn()
    {
        _vacuumOn = true;
    }

    public void VacuumOff()
    {
        _vacuumOn = false;
    }
}
