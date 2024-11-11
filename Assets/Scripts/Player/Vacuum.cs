using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.VFX;
using static UnityEditorInternal.ReorderableList;

public class Vacuum : MonoBehaviour
{
    [field: SerializeField] public EventReference VacuumSFX { get; set; }

    [Header("Colliders")]
    [SerializeField] private MeshCollider _rayCollider;
    [SerializeField] private CapsuleCollider _storeCollider;

    [Header("Vacuum Ray")]
    [SerializeField] private float _rayChangeSpeed;

    private bool _vacuumOn = false;
    private Vector3 _rayStartScale;
    private List<GameObject> _pulledObjects;
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
                enemy.GetComponent<Rigidbody>().velocity  = (new Vector3(pullForce.x * 2, pullForce.y *4, pullForce.z * 2));
                enemy.GetComponent<NewNpcBehavior>().AssignVacuumPos(this.gameObject);
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
        _pulledObjects.Clear();
        _vacuumOn = false;
        _rayCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((_layerMask.value & (1 << other.transform.gameObject.layer)) != 0)
        {
            _pulledObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
       
        if ((_layerMask.value & (1 << other.transform.gameObject.layer)) !=0)
        {
            other.GetComponent<NewNpcBehavior>().AssignVacuumPos(null);
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
            PlayerStats.Hunger += 5;
            Mathf.Clamp(PlayerStats.Hunger, 0, 100);
        }
    }

}
