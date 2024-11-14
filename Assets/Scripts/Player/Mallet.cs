using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using Vector3 = UnityEngine.Vector3;

public class Mallet : MonoBehaviour
{
    [field: SerializeField] public EventReference AttackSFX { get; set; }

    [Header("VFX Prefab References")]
    [SerializeField] private GameObject _debrisVFX;

    [Header("Target Attributes")]
    [SerializeField] private GameObject _target;
    [SerializeField] private float _targetOffset;

    [Header("Mallet References")]
    [SerializeField] InputActionAsset inputActions;
    [SerializeField] private Animator _malletAnimator;
    [SerializeField] private GameObject _malletHandle;
    [SerializeField] private float _originalStartY;
    [SerializeField] private float _hungerExpense;
    [SerializeField] private float _switchCooldown;

    [Header("Vacuum References")]
    [SerializeField] private Vacuum _vacuum;

    [Header("Rotate Icon References")]
    [SerializeField] private RotateIcon _rotateIcon;

    private float _cooldown;
    private int _attackMode;
    private InputAction vacuumAction;
    private Vector3 _mousePos;
    private Vector3 hitpoint;
    private RaycastHit _hit;
    private LayerMask _layerMask;
    private Vector3 _hitTargetpos;

    private void Start()
    {
        _layerMask = LayerMask.GetMask("Floor");
        _attackMode = 0;
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
        _malletHandle.gameObject.transform.position = hitpoint;
    }
    private void OnEnable()
    {
        var playerActionMap = inputActions.FindActionMap("Player");
        vacuumAction = playerActionMap.FindAction("Vacuum");

        vacuumAction.canceled += OnMouseRelease;
        vacuumAction.Enable();
    }

    private void OnMouseRelease(InputAction.CallbackContext context)
    {
        _malletAnimator.SetBool("VacuumReleased", true);
        _vacuum.VacuumOff();
    }

    private void OnDisable()
    {
        vacuumAction.canceled -= OnMouseRelease;
        vacuumAction.Disable();
    }
    public void OnVacuum()
    {
        if (_attackMode == 1)
        {
            _vacuum.VacuumOn();
            _malletAnimator.SetTrigger("Vacuum");
            _malletAnimator.SetBool("VacuumReleased", false);
        }
    }

    public void OnFire()
    {
        if (PlayerStats.Hunger >= _hungerExpense && _attackMode == 0)
        {
            _malletAnimator.SetTrigger("Swing");
            PlayerStats.Hunger -= _hungerExpense;
            Mathf.Clamp(PlayerStats.Hunger, 0, 100);
        }
    }

    public void OnSwitchWeapon()
    {
        if (Time.time > _cooldown)
        {
            if (_attackMode == 0)
            {
                gameObject.tag = "Vacuum";
                _attackMode = 1;
                _malletAnimator.SetTrigger("SwitchVacuum");
                _rotateIcon.SwitchSides();
            }
            else if (_attackMode == 1)
            {
                gameObject.tag = "Mallet";
                _attackMode = 0;
                _malletAnimator.SetTrigger("SwitchMallet");
                _vacuum.VacuumOff();
                _rotateIcon.SwitchSides();
            }
            _cooldown = Time.time + _switchCooldown;
        }
    }

    public void ImpactEffects()
    { 
        GameObject impactVFX = ObjectPool.instance.GetPooledObject();

        if (impactVFX != null)
        {
            impactVFX.transform.position = _target.transform.position + Vector3.up;
            impactVFX.GetComponent<VisualEffect>().Play();
        }

        if (!AttackSFX.IsNull)
        {
            RuntimeManager.PlayOneShot(AttackSFX, this.gameObject.transform.position);
        }
    }
}

