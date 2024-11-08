using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    [SerializeField] private GameObject _spark;
    [SerializeField] private List<GameObject> _shields;
    [SerializeField] private float health;

    float fade = 0;
    float timer = 0;
    void Start()
    {
        health = 100;
    }

    void Update()
    {
        if (health <= 0)
        {
            health = 0;
            _spark.SetActive(false);
            fade = Mathf.Lerp(0,1,timer);
            _shields[0].GetComponent<Renderer>().material.SetFloat("_Fade", fade );
            timer += Time.deltaTime/2;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        health--;
        Mathf.Clamp(health, 0, 100);
    }


}
