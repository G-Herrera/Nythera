using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class MainMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private RectTransform selector;
    [SerializeField] private RectTransform[] menuOptions;

    private int currentOptionIndex = 0;
    private PlayerInputActions inputActions;
    private Vector2 navigateInput;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Navigate.started += OnNavigate;
        inputActions.UI.Submit.performed += OnSubmit;
    }

    private void OnDisable()
    {
        inputActions.UI.Disable();
        inputActions.UI.Navigate.started -= OnNavigate;
        inputActions.UI.Submit.performed -= OnSubmit;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateSelector();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateSelector()
    {
        if ( selector != null && menuOptions.Length > 0)
        {
            selector.position = menuOptions[currentOptionIndex].position;
            Vector3 pos = menuOptions[currentOptionIndex].position;
            pos.x -= 290f;   // o el valor que visualmente se vea bien
            pos.y += 15f;    // o el valor que visualmente se vea bien
            selector.position = pos;
        }
    }

    private void OnNavigate(InputAction.CallbackContext context)
    {
        navigateInput = context.ReadValue<Vector2>();

        if (navigateInput.y > 0f)
        {
            // Move up in the menu
            MoveUp();
        } 
        else if (navigateInput.y < 0f)
        {
            // Move down in the menu
            MoveDown();
        }
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
        // Handle menu option selection based on currentOptionIndex
        switch (currentOptionIndex)
        {
            case 0:
                // Start Game
                StartNewGame();
                break;
            case 1:
                // Options
                OpenOptions();
                break;
            case 2:
                // Exit
                ExitGame();
                break;
            default:
                break;
        }
    }

    private void StartNewGame()
    {
        //Load the first level or main game scene
        SceneManager.LoadScene("scn_PlayerController");
    }

    private void OpenOptions()
    {
        //Load the options panel or scene
        //SceneManager.LoadScene("OptionsScene"); // Replace with your options scene name
        Debug.Log("Options selected. Implement options functionality here.");
    }

    private void ExitGame()
    {
        //Exit the game
        //Application.Quit();
        Debug.Log("Exiting game. Implement exit functionality here.");
    }



    private void MoveUp()
    {
        if (currentOptionIndex == 0) return;

            currentOptionIndex--;
            UpdateSelector();   
    }

    private void MoveDown()
    {
        if (currentOptionIndex == menuOptions.Length - 1) return;

        currentOptionIndex++;
        UpdateSelector();
    }
}
