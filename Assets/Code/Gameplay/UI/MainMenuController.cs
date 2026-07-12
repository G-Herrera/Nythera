using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private RectTransform selector;
    [SerializeField] private RectTransform[] menuOptions;

    private int currentOptionIndex = 0;

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
        }
    }
}
