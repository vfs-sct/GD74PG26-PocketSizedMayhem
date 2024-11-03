using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using FMODUnity;
using System.Collections;
using Unity.VisualScripting;
public class CivilianDeath : MonoBehaviour
{
    [field: SerializeField] public EventReference DeathSFX { get; set; }

    [SerializeField] private GameObject _bloodEffect;
    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] private CapsuleCollider _triggerCollider;
    [SerializeField] private RagdollOnOffController _ragdollController;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rb;
    //[SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private NewNpcBehavior _civilianBehaviour;
    [SerializeField] private int _animNo;
    
    public event EventHandler<GameObject> OnKilled;
    private bool _pointGiven;
    private bool _isFading = false;
    private float _fadeTime;
    private float _fadeThresholdTime;
    private float _fadeAmount;

    private Renderer _objectRenderer;
    private Material _objectMaterial;
    private Color _objectColor;
    private void Start()
    {
        _objectRenderer = GetComponentInChildren<Renderer>();
        _objectMaterial = _objectRenderer.material;
        _objectColor = _objectMaterial.color;
        _fadeTime = 0;
        _fadeThresholdTime = 8;
        _pointGiven = false;
    }
    private void Update()
    {
        if(_isFading)
        {
            if(_fadeTime >= _fadeThresholdTime)
            {
                this.gameObject.SetActive(false);
                _ragdollController.RagdollModeOff();
                _capsuleCollider.enabled = true;
                _civilianBehaviour.enabled = true;
                _triggerCollider.enabled = true;
                _fadeTime = 0;
                _isFading = false;
                _objectMaterial.SetFloat("_Alpha", 1);
            }
            else
            {
                _fadeTime += Time.deltaTime;
                _fadeAmount = Mathf.Lerp(1, 0, _fadeTime / _fadeThresholdTime);
                _objectMaterial.SetFloat("_Alpha", _fadeAmount);
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mallet")
        {
            _capsuleCollider.enabled = false;
            _civilianBehaviour.enabled = false;
            _ragdollController.RagdollModeOn();
            _ragdollController.DeathBounce();

            GameObject impactVFX = BloodVFXPool.instance.GetPooledObject();

            if (impactVFX != null)
            {
                impactVFX.transform.position = transform.position;
                impactVFX.GetComponent<VisualEffect>().Play();
            }

            OnKilled?.Invoke(this, this.gameObject);
            
            if (!_pointGiven)
            {
                PlayerStats.Points += _civilianBehaviour.GetPoint();
                _pointGiven= true;
            }
            if (!DeathSFX.IsNull)
            {
                RuntimeManager.PlayOneShot(DeathSFX, this.gameObject.transform.position);
            }
            _triggerCollider.enabled = false;
            StartCoroutine(StartFading());
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    { 
        if (collision.gameObject.tag == "Vacuum")
        {
            this.gameObject.SetActive(false);
            PlayerStats.Hunger += 5;
        }
    }

    IEnumerator StartFading()
    {
        yield return new WaitForSeconds(2);
        _isFading = true;
    }
}
