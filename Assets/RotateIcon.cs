using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class RotateIcon : MonoBehaviour
{
    [SerializeField] GameObject malletIcon;
    [SerializeField] GameObject vacuumIcon;
    [SerializeField] GameObject IconHolder;
    // Start is called before the first frame update
    private int direction = 1;
    private int _multiplier = 0;
    [SerializeField] private float _rotateDuration;
    float degreesPerSecond;
    private void Start()
    {
        degreesPerSecond = 180 / _rotateDuration;
    }

    public void SwitchSides()
    {
        _multiplier += direction;
        StartCoroutine(RotateOverTime());
    }

    private IEnumerator RotateOverTime()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation =  Quaternion.Euler(0, _multiplier * 180, 90); 

        float t = 0f;
        while (t < _rotateDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t / _rotateDuration);
            t += Time.deltaTime;
            if(t>=_rotateDuration/2 && direction ==1)
            {
                malletIcon.SetActive(false);
                vacuumIcon.SetActive(true);
            }
            else if(t >= _rotateDuration / 2 && direction == -1)
            {
                malletIcon.SetActive(true);
                vacuumIcon.SetActive(false);
            }
            yield return null;
        }

        transform.rotation = endRotation; // Set final rotation to ensure accuracy
        direction *= -1;
    }
}
