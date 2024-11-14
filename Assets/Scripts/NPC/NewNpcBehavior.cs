using CharacterMovement;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NewNpcBehavior : CharacterMovement3D
{
    [Header("Mallet Attributes")]
    [SerializeField] private GameObject _target;
    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] public GameObject _vacuum;

    [Header("Throw Attributes")]
    [SerializeField] private float _upForce;
    [SerializeField] private float _forwardForce;

    [Header("NPC Attributes")]
    [SerializeField] private int point;
    [SerializeField] private TypeDifficulty _type;
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
    
    private Vector3 _newDirectionVector;

    private float _originalSpeed;
    private float t = 0;
    private float acceleration = 1;
    private float angle = 0;

    LayerMask _doorLayerMask;
    LayerMask _civilianTargetLayerMask;

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
            _pattern = (Pattern)Random.Range(0, 2);
        }
        else
        {
            _endTarget = EndTarget.TARGET;
            GetComponent<Animator>().SetTrigger("Scatter");
            Speed = 20;
        }
    }
    private void OnEnable()
    {
        StartCoroutine(PatternStart());
    }
    public void BuildingSpawn()
    {
        _spawnState = SpawnState.BUILDING;
    }

    public void AssignVacuumPos(GameObject vacuum)
    {
        if(vacuum != null)
        {
            _objectMaterial.SetVector("_Target", vacuum.transform.position);
        }
    }
    IEnumerator PatternStart()
    {
        yield return new WaitForSeconds(Random.Range(3, 6));
        _endTarget = EndTarget.NO_TARGET;
        yield return new WaitForSeconds(Random.Range(1, 4));
        _endTarget = EndTarget.TARGET;
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

    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    public  bool HasTarget()
    {
        if (_target != null && _target.activeInHierarchy != false)
        {
            return true;
        }
        return false;
    }


    public int GetPoint()
    {
        return point;
    }
    public TypeDifficulty GetDifficultyType()
    {
        return _type;
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

    public enum TypeDifficulty
    {
        EASY,
        NORMAL,
        HARD,
        NEGATIVE
    }
}
