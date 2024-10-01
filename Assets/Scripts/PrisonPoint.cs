using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(7))
        {
            GameManager.AddPoint();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(7))
        {
            GameManager.LosePoint();
        }
    }
}
