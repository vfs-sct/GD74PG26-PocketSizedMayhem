using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class CivilianDeath : MonoBehaviour
{
    [SerializeField] private GameObject _bloodEffect;

    private RagdollOnOffController _ragdollController;

    private void Start()
    {
        _ragdollController = GetComponent<RagdollOnOffController>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Mallet")
        {
            _ragdollController.RagdollModeOn();
            this.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            GameObject blood = Instantiate(_bloodEffect, this.gameObject.transform.position, this.gameObject.transform.rotation);
            blood.GetComponent<VisualEffect>().Play();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mallet")
        {
            _ragdollController.DeathBounce();
        }
    }
}
