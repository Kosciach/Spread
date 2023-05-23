using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeExecutionDelayer : MonoBehaviour
{
    public static CodeExecutionDelayer Instance;
    private IEnumerator _delayedCoroutine;


    private void Awake()
    {
        Instance = this;
    }

    public void ExecuteAfterDelay(float delay, System.Action afterDelay)
    {
        if(_delayedCoroutine != null)
        {
            StopCoroutine(_delayedCoroutine);
        }
        _delayedCoroutine = DelayExecution(delay, afterDelay);
        StartCoroutine(_delayedCoroutine);
    }


    private IEnumerator DelayExecution(float delay, System.Action afterDelay)
    {
        yield return new WaitForSeconds(delay);

        afterDelay.Invoke();
    }
}
