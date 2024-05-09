using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Enemies.Hazards;
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
    private IHazardDispenserView View => _view.Value;

    void Start()
    {
        _hazardsFactory = ServiceLocator.Instance.GetService<IHazardFactory>();
        View.Configure(_config.ViewConfig);
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
                Debug.Log(_index);
            }

            _timer -= Time.deltaTime; 
        }
    }

    public void StartShooting()
    {
        _startShooting = true;
    }
    public void StartShootingPattern(List<Transform> shootPattern)
    {
        foreach (var transform in shootPattern)
        {
            _currentProjectile = _hazardsFactory.CreateParabolicProjectile(_firePoint, transform,0,0);
            _currentProjectile.ShootWithoutPredict();
        }
    }
    
}
