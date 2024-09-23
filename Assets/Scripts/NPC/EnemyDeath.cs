using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using FMODUnity;
public class EnemyDeath : MonoBehaviour
{
    [field: SerializeField] public EventReference DeathSFX { get; set; }

    [SerializeField] private GameObject _bloodEffect;
    [SerializeField] private CapsuleCollider _capsuleCollider;

    [SerializeField] private RagdollOnOffController _ragdollController;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private RegularCriminalBehaviour _regularCriminalBehaviour;

    public event EventHandler<GameObject> OnKilled;

    private bool _pointGiven;

    private void Start()
    {
        _pointGiven = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mallet" || other.gameObject.layer.Equals(17))
        {
            _capsuleCollider.enabled = false;
            _navMeshAgent.enabled = false;
            _ragdollController.RagdollModeOn();
            
            GameObject blood = Instantiate(_bloodEffect, this.gameObject.transform.position, this.gameObject.transform.rotation);
            blood.GetComponent<VisualEffect>().Play();

            OnKilled?.Invoke(this, this.gameObject);
            
            _ragdollController.DeathBounce();

            if (!_pointGiven)
            {
                GameManager.AddPoint();
                _pointGiven = true;
            }

            PlayerStats.CriminalKilled++;

            if (!DeathSFX.IsNull)
            {
                RuntimeManager.PlayOneShot(DeathSFX, this.gameObject.transform.position);
            }
            _regularCriminalBehaviour.enabled = false;
        }
    }
}
