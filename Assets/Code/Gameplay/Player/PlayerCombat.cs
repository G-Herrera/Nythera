using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Initial Settings")]
    [SerializeField] private GameObject sword;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private float attackDuration = 0.2f;
    [SerializeField] private float attackAngle = 90f;
    [SerializeField] private float attackSpeed = 450f;
    [SerializeField] private float attackCooldown = 0.5f;

    private PlayerInputActions playerControls;
    private bool canAttack = true;
    private Quaternion initialRotation;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerControls.Enable();

        playerControls.Player.Attack.performed += Attack;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (sword != null && sword.activeInHierarchy) sword.SetActive(false);

        initialRotation = weaponPivot.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Attack(InputAction.CallbackContext context)
    {

        PerfomAttack();
    }

    private void PerfomAttack()
    {
        if (!canAttack) return;

        canAttack = false;

        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        if (sword != null) sword.SetActive(true);

        yield return StartCoroutine(SwingWeapon());

        sword.SetActive(false);

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    private IEnumerator SwingWeapon()
    {
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0f, 0f, -attackAngle);

        yield return RotateWeapon(initialRotation, targetRotation, attackDuration);

        yield return RotateWeapon(targetRotation, initialRotation, attackDuration);
    }

    private IEnumerator RotateWeapon(Quaternion from, Quaternion to, float duration)
    {

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            weaponPivot.localRotation = Quaternion.Lerp(from, to, t);

            yield return null;
        }

        weaponPivot.localRotation = to;
    }
    private void OnDisable()
    {
        playerControls.Player.Attack.performed -= Attack;

        playerControls.Disable();  
    }
}
