using UnityEngine;

public class GameplayUIController : MonoBehaviour
{
    [Header("UI Modules")]
    [SerializeField] private HUDWeaponIcon hudWeaponIcon;
    [SerializeField] private WeaponWheelController weaponWheelController;

    private void Start()
    {
        PlayerWeaponSystem weaponSystem = FindObjectOfType<PlayerWeaponSystem>();

        if (weaponSystem == null)
        {
            Debug.LogError("GameplayUIController no encontró un PlayerWeaponSystem " +
                           "en la escena.");
            return;
        }

        hudWeaponIcon.Initialize(weaponSystem);
        weaponWheelController.Initialize(weaponSystem);
    }
}