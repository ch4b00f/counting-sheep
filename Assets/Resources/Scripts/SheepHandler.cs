using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;

public class SheepHandler : MonoBehaviour
{
    [SerializeField] private GameObject _sheepPrefab;
    [SerializeField] private EventReference _spawnSound;

    private List<Sheep> _allSheep;
    private int _totalSheep;

    // costs of sheep and upgrades
    private int _sheepCost = 1;
    [SerializeField] private int[] _upgradeCosts = new int[3];
    private int _currentLevel = 1;

    // ties to the UI click to spawn sheep
    public static UnityEvent OnSpawnClicked = new UnityEvent();
    public static UnityEvent<int> OnCostChanged = new UnityEvent<int>();

    public static UnityEvent OnUpgradeClicked = new UnityEvent();
    public static UnityEvent<int, int> OnLevelChanged = new UnityEvent<int, int>();

    private void Awake()
    {
        DOTween.SetTweensCapacity(1000000, 1000000);

        // assign events
        OnSpawnClicked.AddListener(SpawnSheep);
        OnUpgradeClicked.AddListener(UpgradeSheep);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnSheep();
            Debug.Log(_totalSheep);
        }
    }

    public void SpawnSheep()
    {
        if (_sheepCost > GameManager.jumpsCounted) return;

        // update the jump counter
        GameManager.jumpsCounted -= _sheepCost;
        GameManager.OnJumpCounted.Invoke();

        // update the spawn cost
        _sheepCost++;
        OnCostChanged.Invoke(_sheepCost);

        // instantiate a sheep
        GameObject newSheep = GameObject.Instantiate(_sheepPrefab, new Vector3(Random.Range(-5f, 5f), 15f, Random.Range(-5f, -2f)), Quaternion.identity);

        RuntimeManager.PlayOneShot(_spawnSound);
        _totalSheep++;
    }

    private void UpgradeSheep()
    {
        if (_upgradeCosts[_currentLevel - 1] > GameManager.jumpsCounted) return;

        if (_currentLevel >= 4) return;

        // update the jump counter
        GameManager.jumpsCounted -= _upgradeCosts[_currentLevel -1];
        GameManager.OnJumpCounted.Invoke();

        // update the upgrade cost
        _currentLevel++;
        Debug.Log(_currentLevel);
        OnLevelChanged.Invoke(_currentLevel, _upgradeCosts[_currentLevel-1]);

        // change the level of all the sheep
    }
}
