using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Vacuum : MonoBehaviour
{
    [SerializeField] List<GameObject> pulledObjects;
    [SerializeField] List<GameObject> capturedObjects;
    private LayerMask _vacuumableObjects;
    private bool _vacuumOn = false;
    [SerializeField]private CapsuleCollider _capsuleCollider;
    [SerializeField]private CapsuleCollider _capsuleColliderb;
    [SerializeField]private GameObject _Ray;
    Vector3 endScale;
    Vector3 RayStartScale;
    [SerializeField] float raychangespeed;
    private void Start()
    {
        RayStartScale = _Ray.transform.localScale;
        _Ray.transform.localScale = Vector3.zero;
        endScale = new Vector3(3,1,3);
        pulledObjects = new List<GameObject>();
        capturedObjects = new List<GameObject>();
        _vacuumableObjects |= (1 << LayerMask.NameToLayer("Enemy"));
        _vacuumableObjects |= (1 << LayerMask.NameToLayer("Debris"));
        _vacuumableObjects |= (1 << LayerMask.NameToLayer("Civilian"));
        _vacuumableObjects |= (1 << LayerMask.NameToLayer("Bomb"));
    }
    private void Update()
    {
        if (_vacuumOn)
        {
            _Ray.transform.localScale = Vector3.Lerp(_Ray.transform.localScale, RayStartScale, raychangespeed * Time.deltaTime);
        }
        else
        {
            _Ray.transform.localScale = Vector3.Lerp(_Ray.transform.localScale, Vector3.zero, raychangespeed * Time.deltaTime);
        }
    }
    private void FixedUpdate()
    {
        if (_vacuumOn)
        {
            foreach (GameObject enemy in pulledObjects)
            {
                Vector3 pullForce = (this.gameObject.transform.position - enemy.transform.position).normalized/ Vector3.Distance(this.gameObject.transform.position, enemy.transform.position) * 100;
                enemy.GetComponent<Rigidbody>().velocity  = (new Vector3(pullForce.x, pullForce.y, pullForce.z));
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_vacuumableObjects == (_vacuumableObjects | (1 << other.gameObject.layer)))
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
        if (_vacuumableObjects == (_vacuumableObjects | (1 << collision.gameObject.layer)) && _vacuumOn)
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
        _capsuleCollider.enabled = true;
        _capsuleColliderb.enabled = true;
    }

    public void VacuumOff()
    {
        _vacuumOn = false;
        _capsuleCollider.enabled = false;
        _capsuleColliderb.enabled = false;
    }
}
