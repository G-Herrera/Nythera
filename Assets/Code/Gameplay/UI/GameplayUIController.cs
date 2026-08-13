using UnityEngine;

public class GameplayUIController : MonoBehaviour
{
    [Header("UI Modules")]
    [SerializeField] private HUDWeaponIcon hudWeaponIcon;
    [SerializeField] private WeaponWheelController weaponWheelController;
    [SerializeField] private HUDKeys hudKeys;

    private void Start()
    {
        InitializeWeapons();
        InitializeKeys();
    }

    private void InitializeWeapons()
    {
        PlayerWeaponSystem weaponSystem =
            FindObjectOfType<PlayerWeaponSystem>();

        if (weaponSystem == null)
        {
            Debug.LogWarning(
                "GameplayUIController no encontró PlayerWeaponSystem."
            );

            return;
        }

        hudWeaponIcon.Initialize(weaponSystem);
        weaponWheelController.Initialize(weaponSystem);
    }

    private void InitializeKeys()
    {
        GestorLlaves keyManager =
            FindObjectOfType<GestorLlaves>();

        if (keyManager == null)
        {
            Debug.LogWarning(
                "GameplayUIController no encontró GestorLlaves."
            );

            return;
        }

        if (hudKeys != null)
            hudKeys.Initialize(keyManager);
    }
}