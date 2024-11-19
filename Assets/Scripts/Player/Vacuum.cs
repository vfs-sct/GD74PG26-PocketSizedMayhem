using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class Vacuum : MonoBehaviour
{
    [field: SerializeField] public EventReference VacuumSFX { get; set; }
    [field: SerializeField] public EventReference SuckSFX { get; set; }
    [field: SerializeField] public EventReference VacuumOffSFX { get; set; }

    [Header("Colliders")]
    [SerializeField] private MeshCollider _rayCollider;
    [SerializeField] private CapsuleCollider _storeCollider;

    [Header("Vacuum Ray")]
    [SerializeField] private float _rayChangeSpeed;

    [Header("Tim Hunger Gain")]
    [SerializeField] private float _hungerGain;

    private bool _vacuumOn = false;
    private Vector3 _rayStartScale;
    [SerializeField] private List<GameObject> _pulledObjects;
    LayerMask _layerMask;
   
    private void Start()
    {
        _layerMask |= (1 << LayerMask.NameToLayer("EasyCivilian"));
        _layerMask |= (1 << LayerMask.NameToLayer("MediumCivilian"));
        _layerMask |= (1 << LayerMask.NameToLayer("HardCivilian"));
        _layerMask |= (1 << LayerMask.NameToLayer("NegativeCivilian"));
        
        _rayStartScale = gameObject.transform.localScale;
        gameObject.transform.localScale = Vector3.zero;
        _pulledObjects = new List<GameObject>();
    }

    private void Update()
    {
        if (_vacuumOn)
        {
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, _rayStartScale, _rayChangeSpeed * Time.deltaTime);
        }
        else
        {
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, Vector3.zero, _rayChangeSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (_vacuumOn)
        {
            foreach (GameObject enemy in _pulledObjects)
            {
                Vector3 pullForce = (this.gameObject.transform.position - enemy.transform.position).normalized/ Vector3.Distance(this.gameObject.transform.position, enemy.transform.position) * 50;
                if(enemy.activeInHierarchy)
                {
                    enemy.GetComponent<Rigidbody>().velocity = (new Vector3(pullForce.x * 4, pullForce.y * 6, pullForce.z * 4));
                    enemy.GetComponent<NewNpcBehavior>().AssignVacuumPos(this.gameObject);
                }
                else
                {
                    _pulledObjects.Remove(enemy);
                }
            }
        }
    }

    public void VacuumOn()
    {
        RuntimeManager.PlayOneShot(VacuumSFX, this.gameObject.transform.position);
        _vacuumOn = true;
        _rayCollider.enabled = true;
    }

    public void VacuumOff()
    {
        RuntimeManager.PlayOneShot(VacuumOffSFX, this.gameObject.transform.position);
        _pulledObjects.Clear();
        _vacuumOn = false;
        _rayCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<NewNpcBehavior>(out NewNpcBehavior civilian))
        {
            _pulledObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
       
        if (other.TryGetComponent<NewNpcBehavior>(out NewNpcBehavior civilian))
        {
            civilian.AssignVacuumPos(null);
            _pulledObjects.Remove(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((_layerMask.value & (1 << collision.transform.gameObject.layer)) != 0)
        {
            collision.gameObject.GetComponent<NewNpcBehavior>().AssignVacuumPos(null);
            _pulledObjects.Remove(collision.gameObject);
            collision.gameObject.gameObject.SetActive(false);
            PlayerStats.Hunger += _hungerGain;
            RuntimeManager.PlayOneShot(SuckSFX, this.gameObject.transform.position);
        }
    }

}
