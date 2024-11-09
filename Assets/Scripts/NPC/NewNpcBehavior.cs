using CharacterMovement;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class NewNpcBehavior : CharacterMovement3D
{
    [Header("Throw Attributes")]
    [SerializeField] private float _upForce;
    [SerializeField] private float _forwardForce;

    [Header("NPC Attributes")]
    [SerializeField] private int point;
    [SerializeField] private float fadeOutTime;
    [SerializeField] private float _fadeSpeed;
    
    [Header("Escape Attributes")]
    [SerializeField] private EndTarget _endTarget;
    [SerializeField] private State _state;
    [SerializeField] private Pattern _pattern;
    [SerializeField] private float _escapeRangeFindRadius;

    [Header("Pattern Attributes")]
    [SerializeField] private float _zigzagHorizontalDistance;
    [SerializeField] private float _zigzagVerticalDistance;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _radius;

    private Renderer _objectRenderer;
    private Material _objectMaterial;

    private float _alpha;

    private float angle = 0;
    [SerializeField] private GameObject _target;
    [SerializeField] private CapsuleCollider _capsuleCollider;
    private Vector3 _newDirectionVector;

    LayerMask _doorLayerMask;
    LayerMask _civilianTargetLayerMask;
    [SerializeField] public GameObject _vacuum;
    private float _originalSpeed;
    float t = 0;
    float acceleration = 1;
    SpawnState _spawnState = SpawnState.REGULAR;
    void Start()
    {
        _originalSpeed = Speed;
        _objectRenderer = GetComponentInChildren<Renderer>();
        _objectMaterial = _objectRenderer.material;
        _doorLayerMask |= (1 << 25);
        if (_spawnState == SpawnState.REGULAR)
        {
            _civilianTargetLayerMask |= (1 << 6);
            _newDirectionVector = new Vector3(_zigzagHorizontalDistance, 0, _zigzagVerticalDistance);
            _endTarget = (EndTarget)Random.Range(0, 2);
            if (point < 0)
            {
                _endTarget = EndTarget.CIVILIAN;
            }
            if (_endTarget == EndTarget.NO_TARGET)
            {
                _pattern = (Pattern)Random.Range(0, 2);
            }
            else if (_endTarget == EndTarget.CIVILIAN)
            {
                SetCivilianTarget();
            }
            else
            {
                SetEscapeDestination();
            }
        }
        else
        {
            _endTarget = EndTarget.TARGET;
            GetComponent<Animator>().SetTrigger("Scatter");
            Speed = 20;
            
        }
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (_endTarget == EndTarget.CIVILIAN)
        {
            if(Speed>=_originalSpeed)
            {
                acceleration = -1;
            }
            else if(Speed <= _originalSpeed/2)
            {
                acceleration =1;
            }
            t += Time.fixedDeltaTime * acceleration;
            Speed = Mathf.Lerp(_originalSpeed/2,_originalSpeed,t/4);
        }  
    }

    public void BuildingSpawn()
    {
        _spawnState = SpawnState.BUILDING;
    }

    private void SetEscapeDestination()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _escapeRangeFindRadius, _doorLayerMask);
        if(hitColliders.Length !=0)
        {
            _target = hitColliders[Random.Range(0, hitColliders.Length)].gameObject;
            MoveTo(_target.transform.position);
        }
    }

    private void SetCivilianTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _escapeRangeFindRadius, _civilianTargetLayerMask);
        _target = hitColliders[Random.Range(0, hitColliders.Length)].gameObject;
        MoveTo(_target.transform.position);
    }

    public void AssignVacuumPos(GameObject vacuum)
    {
        if(vacuum != null)
        {
            _objectMaterial.SetVector("_Target", vacuum.transform.position);
        }
    }

    protected override void Update()
    {
        base.Update();
        angle += Time.deltaTime * _rotationSpeed;
        if ((_endTarget == EndTarget.TARGET || _endTarget == EndTarget.CIVILIAN) && _target!=null)
        {
            MoveTo(_target.transform.position);
        }
        else if (_endTarget == EndTarget.NO_TARGET)
        {
            if (_pattern == Pattern.ZIGZAG)
            {
                if (NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance)
                {
                    if (_newDirectionVector.x > 0)
                    {
                        _newDirectionVector.x = -1 * Random.Range(3, _zigzagHorizontalDistance);
                        _newDirectionVector.z = Random.Range(3, _zigzagVerticalDistance);
                    }
                    else
                    {
                        _newDirectionVector.x *= -1;
                        _newDirectionVector.z = 0;
                    }
                    MoveTo(transform.position + _newDirectionVector);
                }
            }
            else if (_pattern == Pattern.CIRCLE)
            {
                _newDirectionVector.x = Mathf.Cos(angle) * _radius;
                _newDirectionVector.z = Mathf.Sin(angle) * _radius;
                MoveTo(transform.position + _newDirectionVector);
            }
        }
    }

    private void AssignTarget()
    {
        if (point < 0)
        {
            _endTarget = EndTarget.CIVILIAN;
            SetCivilianTarget();
            return;
        }

        if (_endTarget == EndTarget.TARGET)
        {
            SetEscapeDestination();
        }
        else
        {
            if (_pattern == Pattern.ZIGZAG)
            {
                MoveTo(transform.position + _newDirectionVector);
            }
        }
    }

    public int GetPoint()
    {
        return point;
    }
    public enum EndTarget
    {
        TARGET,
        NO_TARGET,
        CIVILIAN
    }
    public enum Pattern
    {
        ZIGZAG,
        CIRCLE
    }

    public enum State
    {
        PRECISE,
        FRENZY
    }
    public enum SpawnState
    {
        BUILDING,
        REGULAR
    }
}
