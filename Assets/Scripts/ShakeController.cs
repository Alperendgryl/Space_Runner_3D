using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeController : MonoBehaviour
{
    private Transform _cam;
    private Vector3 _startPos;

    [SerializeField] private float _shakePower = 0.2f;
    [SerializeField] private float _shakeDuration = 0.1f;
    private float _initialDuration;
    [SerializeField] private float _downAmount = 1f;
    public static bool _isShake = false;

    void Start()
    {
        _cam = Camera.main.transform;
        _startPos = _cam.localPosition;
        _initialDuration = _shakeDuration;
    }

    void Update()
    {
        if (_isShake)
        {
            if (_shakeDuration > 0)
            {
                _cam.localPosition = _startPos + Random.insideUnitSphere * _shakePower;
                _shakeDuration -= _downAmount * Time.deltaTime;
            }
            else
            {
                _isShake = false;
                _cam.localPosition = _startPos;
                _shakeDuration = _initialDuration;
            }
        }
    }
}
