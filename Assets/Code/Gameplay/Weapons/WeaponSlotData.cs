using System;
using UnityEngine;

[Serializable]
public class WeaponSlotData
{
    [SerializeField] private string weaponName;
    [SerializeField] private Sprite weaponIcon;
    [SerializeField] private GameObject weaponObject;

    public string WeaponName => weaponName;
    public Sprite WeaponIcon => weaponIcon;
    public GameObject WeaponObject => weaponObject;
}