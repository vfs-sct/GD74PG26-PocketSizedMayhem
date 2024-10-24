using CharacterMovement;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NewNpcBehavior : CharacterMovement3D
{
    [Header("NPC Attributes")]
    [SerializeField] private float point;
    [SerializeField] private float fadeOutTime;
    [SerializeField] private float _fadeSpeed;
    [SerializeField] private float _escapeRangeFindRadius;

    [Header("Escape Attributes")]
    [SerializeField] private EndTarget _endTarget;
    [SerializeField] private State _state;
    [SerializeField] private Pattern _pattern;

    [Header("Pattern Attributes")]
    [SerializeField] private float _zigzagHorizontalDistance;
    [SerializeField] private float _zigzagVerticalDistance;

    [Header("Fading Attributes")]
    private Renderer _objectRenderer;
    private Material _objectMaterial;

    private float _alpha;
    private float _fadeAmount = 1;
    private bool _fadingOut;
    private float _cycleCount = 0;
    private float _timer = 0;
    private float _timer2 = 0;
    LayerMask _layerMask;
    private Vector3 _newDirectionVector;
    void Start()
    {
        _layerMask |= (1 << 22);

        _objectRenderer = GetComponentInChildren<Renderer>();
        _objectMaterial = _objectRenderer.material;

        _alpha = _objectMaterial.GetFloat("_Alpha");
        _newDirectionVector = new Vector3(_zigzagHorizontalDistance,0, _zigzagVerticalDistance);

        if (_endTarget == EndTarget.TARGET)
        {
            SetEscapeDestination();
        }
        else
        {
            NavMeshAgent.SetDestination(transform.position + _newDirectionVector);
            Debug.Log(transform.position + _newDirectionVector);
        }
    }

    private void SetEscapeDestination()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _escapeRangeFindRadius, _layerMask);
        NavMeshAgent.SetDestination(hitColliders[Random.Range(0, hitColliders.Length)].transform.position);
    }

    protected override void Update()
    {
        base.Update();

        if(_endTarget == EndTarget.NO_TARGET)
        {
            
            if (_pattern == Pattern.ZIGZAG)
            {
                if(NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance)
                {
                    if(_newDirectionVector.x>0)
                    {
                        _newDirectionVector.x = -1 * Random.Range(3, _zigzagHorizontalDistance);
                        _newDirectionVector.z = Random.Range(3,_zigzagVerticalDistance);
                    }
                    else
                    {
                        _newDirectionVector.x *= -1;
                        _newDirectionVector.z = 0;
                    }
                    NavMeshAgent.SetDestination(transform.position + _newDirectionVector);
                }
            }
            else if (_pattern == Pattern.CIRCLE)
            {

            }
        }
    }

    private IEnumerator Fadeout()
    {
        if (_timer < fadeOutTime)
        {
            
            _fadeAmount = Mathf.Lerp(1, 0, _timer / fadeOutTime);
            _objectMaterial.SetFloat("_Alpha", _fadeAmount);
        }
        yield return null;
    }

    private IEnumerator ColorFadeOut()
    {
        float newTimer = 0;
        while (newTimer < _timer2)
        {
            _fadeAmount = Mathf.Lerp(1, 0, 1 - newTimer / _timer2);
            _objectMaterial.SetFloat("_Alpha", _fadeAmount);
            newTimer += Time.deltaTime;
        }
        yield return StartCoroutine(ColorFadeIn());
    }

    private IEnumerator ColorFadeIn()
    {
        float newTimer = 0;
        while (newTimer < _timer2)
        {
            _fadeAmount = Mathf.Lerp(0, 1,  newTimer / _timer2);
            _objectMaterial.SetFloat("_Alpha", _fadeAmount);
            newTimer += Time.deltaTime;
        }
        yield return StartCoroutine(ColorFadeOut());
    }

    public enum EndTarget
    {
        TARGET,
        NO_TARGET
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _escapeRangeFindRadius);
    }
}
