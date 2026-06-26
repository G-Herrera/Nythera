using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private PlayerController player;
    [SerializeField] private float lookDistance = 2.0f;
    [SerializeField] private float smoothSpeed = 5.0f;

    private CinemachineFramingTransposer framingTransposer;

    private void Awake()
    {
        framingTransposer = GetComponent<CinemachineVirtualCamera>()
        .GetCinemachineComponent<CinemachineFramingTransposer>();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetOffset = player.LookInput * lookDistance;

        if (framingTransposer != null)
        {
            framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(
                framingTransposer.m_TrackedObjectOffset,
                targetOffset,
                smoothSpeed * Time.deltaTime
                    );
        }
        
    }
}
