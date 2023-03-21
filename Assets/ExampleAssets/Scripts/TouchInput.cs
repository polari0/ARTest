using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TouchInput : MonoBehaviour
{
    [SerializeField]
    private TMP_Text DebugText;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private ARRaycastManager arcm;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private TrackableType trackableTypes = TrackableType.PlaneWithinPolygon;

    private void Awake()
    {
        arcm = GetComponent<ARRaycastManager>();
    }

    public void SingleTap(InputAction.CallbackContext ctx)
    {
        if(ctx.phase == InputActionPhase.Performed)
        {
            var touchPos = ctx.ReadValue<Vector2>();
            DebugText.text = touchPos.ToString();

            if (arcm.Raycast(touchPos, hits, trackableTypes))
            {
                var cube = Instantiate(prefab, hits[0].pose.position, new Quaternion());
            }
        }

        
    }

}
