using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Custom/EnemyID")]
public class ID : ScriptableObject
{
    [Header("Uid")]
    [SerializeField] private string _id = Guid.NewGuid().ToString();
    public Guid Id { get; private set;}
    void OnValidate()
    {
        Id = Guid.Parse(_id);
    }
    
    void Awake()
    {
        OnValidate();
    }
}
