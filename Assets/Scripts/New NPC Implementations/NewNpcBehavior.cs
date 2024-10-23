using CharacterMovement;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class NewNpcBehavior : CharacterMovement3D
{
    [Header("NPC Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float point;
    [SerializeField] private float fadeOutTime;
    [SerializeField] private float _fadeSpeed;
    
    private Renderer _objectRenderer;
    private Material _objectMaterial;
    private float _alpha;
    private float _fadeAmount = 1;
    private bool _fadingOut;
    private float _cycleCount = 0;
    private float _timer = 0;
    private float _timer2 = 0;
    void Start()
    {
        _objectRenderer = GetComponentInChildren<Renderer>();
        _objectMaterial = _objectRenderer.material;
        _alpha = _objectMaterial.GetFloat("_Alpha");
        Rigidbody.velocity *= speed;
        
    }

    protected override void Update()
    {
        base.Update();
        StartCoroutine(Fadeout());
        _timer += Time.deltaTime;
    }

    private IEnumerator Fadeout()
    {
        if (_timer < fadeOutTime)
        {
            
            _fadeAmount = Mathf.Lerp(1, 0, _timer / fadeOutTime);
            Debug.Log(_fadeAmount);
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
}
