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

    private RagdollOnOffController _ragdollController;
    private Animator _animator;
    private Rigidbody _rb;
    private NavMeshAgent _navMeshAgent;

    public event EventHandler<GameObject> OnKilled;

    private bool _pointGiven;

    private void Start()
    {
        _ragdollController = GetComponent<RagdollOnOffController>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _pointGiven = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mallet" || other.gameObject.layer.Equals(17))
        {
            
            _ragdollController.RagdollModeOn();
            
            GameObject blood = Instantiate(_bloodEffect, this.gameObject.transform.position, this.gameObject.transform.rotation);
            blood.GetComponent<VisualEffect>().Play();

            OnKilled?.Invoke(this, this.gameObject);

            Destroy(_rb);

            _capsuleCollider.enabled = false;
            _navMeshAgent.enabled = false;

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
        }
    }
}
