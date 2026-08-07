using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class WeaponWheelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject weaponWheel;

    [Header("Time Settings")]
    [SerializeField, Range(0f, 1f)] private float wheelTimeScale = 0.1f;

    [Header("Weapon System")]
    [SerializeField] private PlayerWeaponSystem playerWeaponSystem;

    [Header("Selection")]
    [SerializeField] private RectTransform selectionArrow;
    [SerializeField] private RectTransform wheelCenter;
    [SerializeField] private Image[] weaponIcons; // Array of weapon icons in the wheel

    [SerializeField] private float deadZoneRadius = 50f; // Dead zone radius in pixels

    private PlayerInputActions controls;
    private bool isOpen = false;
    private int selectedWeaponIndex = -1; // -1 means no weapon selected

    private void Awake()
    {
        controls = new PlayerInputActions();
        if(weaponWheel !=null) weaponWheel.SetActive(false);
    }

    public void Initialize(PlayerWeaponSystem weaponSystem)
    {
        playerWeaponSystem = weaponSystem;

        if (playerWeaponSystem == null)
            Debug.LogError("WeaponWheelController recibió un PlayerWeaponSystem nulo.");
    }
    private void OnEnable()
    {
        // Enable the input actions
        controls.Player.Enable();

        controls.Player.WeaponWheel.started += OpenWheel;
        controls.Player.WeaponWheel.canceled += CloseWheel;
    }

    private void OnDisable()
    {
        // Disable the input actions
        controls.Player.WeaponWheel.started -= OpenWheel;
        controls.Player.WeaponWheel.canceled -= CloseWheel;

        controls.Player.Disable();
        Time.timeScale = 1f; // Ensure time scale is reset when the script is disabled
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpen) return;
        
        UpdateSelection();
        UpdateSlotVisuals();
    }

    private void OpenWheel(InputAction.CallbackContext context)
    {
        if (isOpen) return;

        isOpen = true;
        weaponWheel.SetActive(true);

        selectedWeaponIndex = playerWeaponSystem.CurrentWeaponIndex; // Start with the currently equipped weapon selected
        UpdateSlotVisuals();

        Time.timeScale = wheelTimeScale; // Slow down time
    }


    void UpdateSelection()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Vector2 centerPosition = RectTransformUtility.WorldToScreenPoint(null, wheelCenter.position);

        Vector2 direction = mousePosition - centerPosition;

        if (direction.magnitude < deadZoneRadius)
        {
            selectedWeaponIndex = -1; // No weapon selected
            UpdateSlotVisuals();
            return;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        selectionArrow.rotation = Quaternion.Euler(0, 0, angle - 90f); // Adjust for arrow orientation

        float angleFromTop = 90f - angle;

        if (angleFromTop < 0f)
            angleFromTop += 360f;

        float sectionSize = 360f / weaponIcons.Length;

        selectedWeaponIndex =
            Mathf.FloorToInt((angleFromTop + sectionSize / 2f) / sectionSize);

        selectedWeaponIndex %= weaponIcons.Length;
    }

    private void UpdateSlotVisuals()
    {
        for (int i = 0; i < weaponIcons.Length; i++)
        {
            bool isSelected = i == selectedWeaponIndex;

            weaponIcons[i].rectTransform.localScale =
                isSelected ? Vector3.one * 1.25f : Vector3.one;
        }
    }
    private void CloseWheel(InputAction.CallbackContext context)
    {
        if (!isOpen) return;

        if(selectedWeaponIndex >= 0)
        {
            playerWeaponSystem.EquipWeapon(selectedWeaponIndex);
        }
        
        isOpen = false;
        weaponWheel.SetActive(false);

        Time.timeScale = 1f; // Resume normal time
    }
}
