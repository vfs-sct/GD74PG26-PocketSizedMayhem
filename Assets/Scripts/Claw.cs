using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Claw : Weapon
{
    [SerializeField] private GameObject grabbedObject;
    [SerializeField] private FixedJoint joint;
    [SerializeField] private bool fired = false;
    [SerializeField] private bool grabbed = false;
    [SerializeField] private int descendSpeed;
    [SerializeField] private float _originalStartY;

    private bool touching = false;
    private Vector3 _mousePos;
    private Vector3 hitpoint;
    private RaycastHit hit;

    private void Start()
    {
        Vector3 startPos = transform.position;
        startPos.y = _originalStartY;
        transform.position = startPos;
    }
    public override void Fire()
    {
        fired = true;
    }
    private void Update()
    {
        if (fired)
        {
            if (grabbedObject == null)
            {
                transform.Translate(Vector3.down * Time.deltaTime * descendSpeed);
            }
            else
            {
                Destroy(grabbedObject.GetComponent<FixedJoint>());
                //grabbedObject.GetComponent<NPCPathing>().enabled = true;
                grabbedObject.GetComponent<NavMeshAgent>().enabled = false;
                grabbedObject.GetComponent<Animator>().enabled = true;
                grabbedObject.GetComponent<Rigidbody>().useGravity = true;
                grabbedObject.GetComponent<RagdollOnOffController>().enabled = true;
                grabbedObject = null;
                fired = false;
            }
        }
        else if (grabbed)
        {
            transform.Translate(-Vector3.down * Time.deltaTime * descendSpeed);
            if (transform.position.y > _originalStartY)
                grabbed = false;
        }
        else
        {
            _mousePos = Input.mousePosition;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(_mousePos), out hit))
            {
                return;
            }
            hitpoint = hit.point;
            hitpoint.y = transform.position.y;
            transform.position = hitpoint;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        fired = false;
        grabbed = true;

        if (collision.gameObject.GetComponent<RagdollOnOffController>())
        {
            grabbedObject = collision.gameObject;
            //grabbedObject.GetComponent<NPCPathing>().enabled = false;
            grabbedObject.GetComponent<Rigidbody>().useGravity = false;
            grabbedObject.GetComponent<NavMeshAgent>().enabled = false;
            grabbedObject.GetComponent<Animator>().enabled = false;
            grabbedObject.AddComponent<FixedJoint>();
            grabbedObject.GetComponent<FixedJoint>().connectedBody = this.GetComponent<Rigidbody>();
            grabbedObject.GetComponent<RagdollOnOffController>().enabled = false;
        }
    }
}
