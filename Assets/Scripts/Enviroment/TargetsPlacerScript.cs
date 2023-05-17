using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetsPlacerScript : MonoBehaviour
{
    [Header("====References====")]
    [SerializeField] Transform _targetPrefab;



    [Space(20)]
    [Header("====Settings====")]
    [Range(0, 10)]
    [SerializeField] int _columnCount;
    [Range(0, 10)]
    [SerializeField] int _rowCount;
    [Space(5)]
    [Range(0, 10)]
    [SerializeField] float _columnSpacing;
    [Range(0, 10)]
    [SerializeField] float _rowSpacing;
    [Space(5)]
    [SerializeField] SpawnMethodEnum _spawnMethodType;


    [SerializeField] List<Transform> _targets = new List<Transform>(); public List<Transform> Targets { get { return _targets; } }

    public enum SpawnMethodEnum
    {
        Even, Random
    }

    private delegate void SpawningMethod();
    private SpawningMethod[] _spawningMethods = new SpawningMethod[2];




    private void Start()
    {
        _spawningMethods[0] = SpawnEvenly;
        _spawningMethods[1] = SpawnRandomly;

        Spawn();
    }


    public void Spawn()
    {
        _spawningMethods[(int)_spawnMethodType]();
    }
    public void ResetTargets()
    {
        foreach (Transform target in _targets) Destroy(target.gameObject);
        _targets.Clear();
        Spawn();
    }





    private void SpawnRandomly()
    {
        Vector3 position = Vector3.zero;
        Vector3 offset = Vector3.zero;


        for (int column = 0; column < _columnCount; column++)
        {
            position.x += _columnSpacing;
            for (int row = 0; row < _rowCount; row++)
            {
                position.z += _rowSpacing;
                offset = new Vector3(Random.Range(0, 3), 0, Random.Range(0, 3));

                Transform newTarget = Instantiate(_targetPrefab, transform);
                newTarget.localPosition = position + offset;
                _targets.Add(newTarget);
            }
            position.z = 0;
        }
    }
    private void SpawnEvenly()
    {
        Vector3 position = Vector3.zero;

        for (int column = 0; column < _columnCount; column++)
        {
            position.x += _columnSpacing;
            for (int row = 0; row < _rowCount; row++)
            {
                position.z += _rowSpacing;

                Transform newTarget = Instantiate(_targetPrefab, transform);
                newTarget.localPosition = position;
                _targets.Add(newTarget);
            }
            position.z = 0;
        }
    }
}
