using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSystem : MonoBehaviour
{

    [Header("Initial Weapon")]
    [SerializeField] private int initialWeaponIndex;

    [Header("Available Weapons")]
    [SerializeField] private WeaponSlotData[] weapons;

    public event Action<WeaponSlotData> OnWeaponChanged;

    public int CurrentWeaponIndex { get; private set; } = -1;
    public WeaponSlotData CurrentWeapon { get; private set; }


    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log($"Cantidad de armas configuradas: {weapons.Length}");
        Debug.Log($"Índice inicial: {initialWeaponIndex}");

        EquipWeapon(initialWeaponIndex);
    }

    public void EquipWeapon(int weaponIndex)
    {
        if (weapons == null || weapons.Length == 0)
        {
            Debug.LogError("PlayerWeaponSystem no tiene armas configuradas.");
            return;
        }

        if ( weaponIndex < 0 || weaponIndex >= weapons.Length)
        {
            Debug.LogError($"Invalid weapon index: {weaponIndex}");
            return;
        }

        WeaponSlotData weaponToEquip = weapons[weaponIndex];

        if (weaponToEquip == null)
        {
            Debug.LogError($"El espacio de arma {weaponIndex} no está configurado.");
            return;
        }

        DisableAllWeapons();

        CurrentWeaponIndex = weaponIndex;
        CurrentWeapon = weapons[weaponIndex];

        if (CurrentWeapon.WeaponObject != null)
            CurrentWeapon.WeaponObject.SetActive(true);

        OnWeaponChanged?.Invoke(CurrentWeapon);

        Debug.Log($"Arma equipada: {CurrentWeapon.WeaponName}");
    }

    private void DisableAllWeapons()
    {
        if (weapons == null)
            return;

        foreach (WeaponSlotData weapon in weapons)
        {
            if (weapon != null && weapon.WeaponObject != null)
                weapon.WeaponObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
