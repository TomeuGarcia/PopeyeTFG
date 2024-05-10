using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Enemies.General;
using Popeye.Modules.Enemies.Hazards;
using Popeye.Modules.PlayerAnchor.Player.PlayerEvents;
using Popeye.Modules.WorldElements.WorldInteractors;
using Unity.Mathematics;
using UnityEngine;
[System.Serializable]
public class TransformList
{
    public List<Transform> _list;
}
public class BossShooting : MonoBehaviour
{
    private IHazardFactory _hazardsFactory;
    private ServiceLocator _serviceLocator;
    private ParabolicProjectile _currentProjectile;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private List<TransformList> _shootPatterns;

    [SerializeField] private float _timeBetweenShots = 5f;
    private float _timer = 0;
    private bool _startShooting;
    private int _index = 0;
    private bool _shootPreview;
    [SerializeField] private InterfaceReference<IHazardDispenserView, MonoBehaviour> _view;
    [SerializeField] private HazardDispenserConfig _config;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private int _desiredWaveId = 2;
    [SerializeField] private int _finalWaveId = 2;
    [SerializeField] private AWorldInteractor _secondHead;
    [SerializeField] private bool _moves = false;
    private IEventSystemService _systemService;
    private IHazardDispenserView View => _view.Value;

    void Awake()
    {
        _hazardsFactory = ServiceLocator.Instance.GetService<IHazardFactory>();
        View.Configure(_config.ViewConfig);
        ResetTimer();
        _systemService = ServiceLocator.Instance.GetService<IEventSystemService>();
        _systemService.Subscribe<IPlayerEventsDispatcher.OnDieEvent>(OnPlayerDie);
        _enemySpawner.OnWaveFinished += OnWaveFinishedEvent;
    }

    private void OnEnable()
    {
            
            
        
    }

    private void OnDestroy()
    {
       
            _enemySpawner.OnWaveFinished -= OnWaveFinishedEvent;
            _systemService.Unsubscribe<IPlayerEventsDispatcher.OnDieEvent>(OnPlayerDie);
    }

    void OnPlayerDie(IPlayerEventsDispatcher.OnDieEvent eventData)
    {
        ResetTimer();
        StopShooting();
        if (_moves)
        {
            if(_secondHead.IsActivated())
            _secondHead.AddDeactivationInput();
        }
    }
    void OnWaveFinishedEvent(int id,bool hasDied)
    {
        if (id == _desiredWaveId)
            {
                if (_moves && !hasDied)
                {
                    _secondHead.AddActivationInput();
                    StartShooting();
                }
                ResetTimer();
            }

        if (id == _finalWaveId)
        {
            Explode();
        }

        
    }

    public void ResetTimer()
    {
        _timer = _timeBetweenShots;
    }
    public void Update()
    {
        if (_startShooting)
        {
            if (_timer<=0.5f && !_shootPreview)
            {
                _shootPreview = true; 
                View.PlayPrepareDispensingAnimation(0.5f);
            }
            else if (_timer <= 0)
            {
                StartShootingPattern(_shootPatterns[_index]._list);
                _timer = _timeBetweenShots;
                _shootPreview = false;
                _index++;
                _index = _index % _shootPatterns.Count;
            }

            _timer -= Time.deltaTime; 
        }
    }

    public void StartShooting()
    {
        Debug.Log("start shooting " + gameObject.name);
        _startShooting = true;
    }
    public void StopShooting()
    {
        Debug.Log("stopShooting");
        _startShooting = false;
    }
    public void StartShootingPattern(List<Transform> shootPattern)
    {
        foreach (var transform in shootPattern)
        {
            _currentProjectile = _hazardsFactory.CreateParabolicProjectile(_firePoint, transform,0,0);
            _currentProjectile.ShootWithoutPredict();
        }
    }

    public void Explode()
    {
        StopShooting();
        gameObject.SetActive(false);
        _hazardsFactory.CreateExplosion(transform.position, quaternion.identity, ExplosionSize.Big);
    }
}
