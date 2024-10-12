
using CharacterMovement;
using FMODUnity;
using PrimeTween;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Mallet : Weapon
{
    [field: SerializeField] public EventReference AttackSFX { get; set; }
    [SerializeField] private GameObject _debrisVFX;
    [SerializeField] private GameObject _impact;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _originalStartY;
    [SerializeField] private float _targetOffset;

    [SerializeField] private Animator _malletAnimator;
    [SerializeField] private GameObject _impactPos;
    [SerializeField] private GameObject _malletHandle;
    [SerializeField] private GameObject _floor;
    [SerializeField] InputActionAsset inputActions;
    private InputAction vacuumAction;
    private Vector3 _mousePos;
    private Vector3 hitpoint;
    private RaycastHit _hit;
    private LayerMask _layerMask;
    private LayerMask _vacuumLayerMask;
    private Vector3 _hitTargetpos;
    [SerializeField] private float _impactRadius;
    [SerializeField] private float _malletMovementSpeed ;
    private bool _isAttacking = false;
    private int layerAsLayerMask;
    private int _attackMode;
    private bool isVacuuming = false;
    private void OnEnable()
    {
        // Get the action map and the specific action for the mouse click
        var playerActionMap = inputActions.FindActionMap("Player");
        vacuumAction = playerActionMap.FindAction("Vacuum");

        vacuumAction.canceled += OnMouseRelease;

        // Enable the action
        vacuumAction.Enable();
    }
    private void OnMouseRelease(InputAction.CallbackContext context)
    {
        _malletAnimator.SetBool("VacuumReleased", true);
        isVacuuming = false;
    }
    private void OnDisable()
    {
        // Unsubscribe from the events and disable the action
        vacuumAction.canceled -= OnMouseRelease;
        vacuumAction.Disable();
    }
    private void Start()
    {
        _attackMode = 0;
        _vacuumLayerMask |= (1 << LayerMask.NameToLayer("Enemy"));
        _vacuumLayerMask |= (1 << LayerMask.NameToLayer("Civilian"));
    }
    public override void Fire()
    {
        if(!_isAttacking && _attackMode == 0)
        {
            if (!AttackSFX.IsNull)
            {
                RuntimeManager.PlayOneShot(AttackSFX, this.gameObject.transform.position);
            }
            _malletAnimator.SetTrigger("Swing");
            _layerMask = LayerMask.GetMask("Floor");
        }
    }

    public void OnVacuum()
    {
        if(_attackMode==1)
        {
            isVacuuming = true;
            _malletAnimator.SetTrigger("Vacuum");
            _malletAnimator.SetBool("VacuumReleased", false);
        }
        
    }
    public void OnSelectMallet()
    {
        if (_attackMode == 1)
        {
            _attackMode = 0;
            _malletAnimator.SetTrigger("SwitchMallet");
        }
    }

    public void OnSelectClaw()
    {
        if (_attackMode == 0)
        {
            _attackMode = 1;
            _malletAnimator.SetTrigger("SwitchVacuum");
        }
    }
    
    private void Update()
    {
        _mousePos = Input.mousePosition;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(_mousePos), out _hit, Mathf.Infinity, _layerMask))
        {
            return;
        }
        
        _hitTargetpos = _hit.point;
        _target.transform.position = _hitTargetpos;

        this.gameObject.transform.position = _hit.point;
        hitpoint = _hit.point;
        hitpoint.z -= _targetOffset;
        hitpoint.y = _originalStartY;
        _malletHandle.gameObject.transform.position = Vector3.MoveTowards(_malletHandle.gameObject.transform.position, hitpoint, _malletMovementSpeed);
        if(isVacuuming)
        {
            Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, 15, _vacuumLayerMask);
            foreach (Collider collider in hitColliders)
            {
                collider.gameObject.GetComponent<CharacterMovement3D>().enabled = false;
                collider.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                Vector3 direction = this.gameObject.transform.position - collider.gameObject.transform.position;

                // apply gravitational force of attraction to attracted body
                collider.gameObject.GetComponent<Rigidbody>().AddForce(direction.x * 15, direction.y * 15, direction.z * 15, ForceMode.Impulse);
            }
        }
    }

    public void DisableColliders()
    {
        GetComponentInChildren<Collider>().enabled = false;
        _isAttacking = false;
    }

    public void EnableColldiers()
    {
        GetComponentInChildren<Collider>().enabled = true;
        _isAttacking = true;

    }
    public void ImpactEffects()
    {
        GameObject impact = Instantiate(_impact, _impactPos.transform.position+Vector3.up, _impact.transform.rotation);
        impact.GetComponent<VisualEffect>().Play();
        //layerAsLayerMask |= (1 << 6);   
        //layerAsLayerMask |= (1 << 7);
        //Collider[] hitColliders = new Collider[25];
        //Physics.OverlapSphereNonAlloc(transform.position, 10, hitColliders, layerAsLayerMask);
        //foreach (Collider collider in hitColliders)
        //{
        //    if(collider.enabled)
        //    {
        //        if (collider.gameObject.TryGetComponent<Animator>(out Animator component))
        //        {
        //            Tween.PunchLocalPosition(collider.gameObject.transform, strength: Vector3.up * 10, duration: 0.7f, frequency: 1);
        //        }
        //    }
        //}
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Debris"))
        {
            Instantiate(_debrisVFX, other.transform.position, _debrisVFX.transform.rotation);
            Destroy(other.gameObject);
        }
    }
}

