using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using FMODUnity;
public class CivilianDeath : MonoBehaviour
{
    [field: SerializeField] public EventReference DeathSFX { get; set; }

    [SerializeField] private GameObject _bloodEffect;
    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] private int _animNo;

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
        if (other.gameObject.tag == "Mallet")
        {
            _ragdollController.RagdollModeOn();

            _capsuleCollider.enabled = false;
            _navMeshAgent.enabled = false;

            GameObject blood = Instantiate(_bloodEffect, this.gameObject.transform.position, this.gameObject.transform.rotation);
            blood.GetComponent<VisualEffect>().Play();

            OnKilled?.Invoke(this, this.gameObject);

            _ragdollController.DeathBounce();

            if (!_pointGiven)
            {
                GameManager.LosePoint();
                _pointGiven= true;
            }
            if (!DeathSFX.IsNull)
            {
                RuntimeManager.PlayOneShot(DeathSFX, this.gameObject.transform.position);
            }
        }
        else if (other.gameObject.layer.Equals(14))
        {
            DeathByCriminal();

            if (!_pointGiven)
            {
                GameManager.LosePoint();
                _pointGiven = true;
            }
        }
    }

    public void DeathByCriminal()
    {
        int animNo = UnityEngine.Random.Range(0, _animNo);

        _animator.SetFloat("DeathNo",(float) animNo);
        _animator.SetTrigger("Death");

        Destroy(_rb);

        _capsuleCollider.enabled = false;
        _navMeshAgent.enabled = false;

        OnKilled?.Invoke(this, this.gameObject);

        if (!DeathSFX.IsNull)
        {
            RuntimeManager.PlayOneShot(DeathSFX, this.gameObject.transform.position);
        }
    }
}
