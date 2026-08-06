using UnityEngine;
using UnityEngine.UI;

public class HUDWeaponIcon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image weaponIconImage;
    [SerializeField] private PlayerWeaponSystem playerWeaponSystem;

    private void Awake()
    {
        if (weaponIconImage == null)
            weaponIconImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        if (playerWeaponSystem == null)
        {
            Debug.LogError(
                "HUDWeaponIcon: falta asignar PlayerWeaponSystem.",
                this
            );

            return;
        }

        playerWeaponSystem.OnWeaponChanged += UpdateWeaponIcon;
    }

    private void Start()
    {
        RefreshCurrentWeapon();
    }

    private void OnDisable()
    {
        if (playerWeaponSystem != null)
            playerWeaponSystem.OnWeaponChanged -= UpdateWeaponIcon;
    }

    private void RefreshCurrentWeapon()
    {
        if (playerWeaponSystem == null)
            return;

        if (playerWeaponSystem.CurrentWeapon == null)
        {
            Debug.LogWarning(
                "HUDWeaponIcon: todavía no existe un arma equipada.",
                this
            );

            return;
        }

        UpdateWeaponIcon(playerWeaponSystem.CurrentWeapon);
    }

    private void UpdateWeaponIcon(WeaponSlotData newWeapon)
    {
        if (newWeapon == null)
        {
            Debug.LogError("HUDWeaponIcon recibió un arma nula.", this);
            return;
        }

        if (weaponIconImage == null)
        {
            Debug.LogError(
                "HUDWeaponIcon: falta asignar la Image del icono.",
                this
            );

            return;
        }

        Sprite newIcon = newWeapon.WeaponIcon;

        Debug.Log(
            $"HUD actualizando arma: {newWeapon.WeaponName}. " +
            $"Icono: {(newIcon != null ? newIcon.name : "NULL")}",
            this
        );

        weaponIconImage.sprite = newIcon;
        weaponIconImage.enabled = newIcon != null;
        weaponIconImage.preserveAspect = true;

        weaponIconImage.SetAllDirty();
    }
}