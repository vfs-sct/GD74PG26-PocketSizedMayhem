using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
public class CivilianDeath : MonoBehaviour
{
    [SerializeField] private GameObject _bloodEffect;
    [SerializeField] private int _animNo;

    private RagdollOnOffController _ragdollController;
    private Animator _animator;
    private BoxCollider _boxCollider;
    private Rigidbody _rb;
    private NavMeshAgent _navMeshAgent;

    public event EventHandler<GameObject> OnKilled;

    private bool _pointGiven;
    private void Start()
    {
        _ragdollController = GetComponent<RagdollOnOffController>();
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider>();
        _rb = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _pointGiven = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mallet")
        {
            _ragdollController.RagdollModeOn();
            this.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            GameObject blood = Instantiate(_bloodEffect, this.gameObject.transform.position, this.gameObject.transform.rotation);
            blood.GetComponent<VisualEffect>().Play();
            OnKilled?.Invoke(this, this.gameObject);
            _ragdollController.DeathBounce();
            if (!_pointGiven)
            {
                GameManager.LosePoint();
                _pointGiven= true;
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
        int boredAnimation = UnityEngine.Random.Range(0, _animNo);

        _animator.SetFloat("DeathNo",(float) boredAnimation);
        _animator.SetTrigger("Death");

        Destroy(_rb);
        Destroy(_boxCollider);
        Destroy(_navMeshAgent);
        OnKilled?.Invoke(this, this.gameObject);
    }
}
