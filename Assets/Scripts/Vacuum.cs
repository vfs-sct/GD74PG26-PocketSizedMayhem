using CharacterMovement;
using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vacuum : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies;
    private bool _vacuumOn = false;
    private void Start()
    {
        enemies = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        if (_vacuumOn)
        {
            foreach (GameObject enemy in enemies)
            {
                /// Vector3.Distance(this.gameObject.transform.position, enemy.transform.position)
                Vector3 pullForce = (this.gameObject.transform.position - enemy.transform.position).normalized  * 300;
                enemy.GetComponent<Rigidbody>().AddForce(new Vector3(pullForce.x, pullForce.y, pullForce.z));
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemies.Add(other.gameObject);
            Tween.Rotation(other.gameObject.transform, endValue: Quaternion.Euler(180, 180, 180), duration: 6);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        enemies.Remove(other.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        enemies.Remove(collision.gameObject);
        collision.rigidbody.velocity = Vector3.zero;
        collision.gameObject.AddComponent<FixedJoint>();
        collision.gameObject.GetComponent<FixedJoint>().connectedBody = this.GetComponent<Rigidbody>();
        collision.gameObject.transform.parent = this.gameObject.transform;
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
