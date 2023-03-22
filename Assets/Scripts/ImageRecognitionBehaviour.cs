using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageRecognitionBehaviour : MonoBehaviour
{
    private ARTrackedImageManager _arTracableImageManager;


    [SerializeField]
    private GameObject[] arPrefabs;

    private readonly Dictionary<string, GameObject> _instantiatePrefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        _arTracableImageManager = FindObjectOfType<ARTrackedImageManager>();
    }

    public void OnEnable()
    {
        _arTracableImageManager.trackedImagesChanged += OnImageChanged;
    }

    public void OnDisable()
    {
        _arTracableImageManager.trackedImagesChanged -= OnImageChanged;
    }

    public void OnImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(var trackedImage in eventArgs.added)
        {
            var imageName = trackedImage.referenceImage.name;
            Debug.Log(imageName + "Found");

            foreach(var curPrefab in arPrefabs)
            {
                if(string.Compare(curPrefab.name, imageName, System.StringComparison.OrdinalIgnoreCase) == 0
                    && !_instantiatePrefabs.ContainsKey(imageName))
                {
                    var newPrefab = Instantiate(curPrefab, trackedImage.transform);
                    _instantiatePrefabs[imageName] = newPrefab;
                }
            }
        }

        foreach(var trackedImage in eventArgs.updated)
        {
            _instantiatePrefabs[trackedImage.referenceImage.name].SetActive(trackedImage.trackingState == TrackingState.Tracking);
        }

        foreach(var trackedImage in eventArgs.removed)
        {
            var imageName = trackedImage.referenceImage.name;
            Debug.Log(imageName + "Lost");
            Destroy(_instantiatePrefabs[trackedImage.referenceImage.name]);
            _instantiatePrefabs.Remove(trackedImage.referenceImage.name);
        }
    }
}
