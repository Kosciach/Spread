using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class WeaponBobbingController : MonoBehaviour
{
    [SerializeField] Transform _testBob;


    [Space(20)]
    [Header("====Debugs====")]
    [SerializeField] float _currentPosX;
    [SerializeField] float _targetPosX;


    [Space(20)]
    [Header("====Settings====")]
    [SerializeField] float _ax;



    private void Start()
    {
        _targetPosX = _ax;
        StartCoroutine(Elo());
    }
    private void Update()
    {
        _currentPosX = Mathf.Lerp(_currentPosX, _targetPosX, 1 * Time.deltaTime);
        _testBob.localPosition = new Vector3(_currentPosX, 0, 0);
    }


    private IEnumerator Elo()
    {
        yield return new WaitForSeconds(2);

        _targetPosX = -_ax;
        StartCoroutine(Elo2());
    }
    private IEnumerator Elo2()
    {
        yield return new WaitForSeconds(2);

        _targetPosX = _ax;
        StartCoroutine(Elo());
    }
}
