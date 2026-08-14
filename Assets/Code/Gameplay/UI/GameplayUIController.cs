using UnityEngine;

public class GameplayUIController : MonoBehaviour
{
    [Header("UI Modules")]
    [SerializeField] private HUDWeaponIcon hudWeaponIcon;
    [SerializeField] private WeaponWheelController weaponWheelController;
    [SerializeField] private HUDKeys hudKeys;
    [SerializeField] private HUDHealthBar hudHealthBar;

    private void Start()
    {
        InitializeHealth();
        InitializeWeapons();
        InitializeKeys();
    }

    private void InitializeHealth()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning(
                "GameplayUIController no encontró al Player."
            );

            return;
        }

        SistemaVida playerHealth =
            player.GetComponent<SistemaVida>();

        if (playerHealth == null)
        {
            Debug.LogWarning(
                "El Player no tiene SistemaVida."
            );

            return;
        }

        if (hudHealthBar != null)
            hudHealthBar.Initialize(playerHealth);
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