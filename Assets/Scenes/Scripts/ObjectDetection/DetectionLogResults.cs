using Niantic.Lightship.AR.ObjectDetection;
using UnityEngine;

public class DetectionLogResults : MonoBehaviour
{

    [SerializeField] private ARObjectDetectionManager _objectDetectionManager;
    [SerializeField] private float _confidenceThreshold = .5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _objectDetectionManager.enabled = true;
        // To make sure than _objectDetectionManager is initialiazed when we try to use the function ObjectDetectionManagerMetadataInitialized : 
        _objectDetectionManager.MetadataInitialized += ObjectDetectionManagerMetadataInitialized;
    }

    private void OnDestroy()
    {
        _objectDetectionManager.MetadataInitialized -= ObjectDetectionManagerMetadataInitialized;
        _objectDetectionManager.ObjectDetectionsUpdated -= ObjectDetectionManagerObjectDetectionsUpdated;
    }

    private void ObjectDetectionManagerMetadataInitialized(ARObjectDetectionModelEventArgs obj)
    {
        _objectDetectionManager.ObjectDetectionsUpdated += ObjectDetectionManagerObjectDetectionsUpdated;
    }
    private void ObjectDetectionManagerObjectDetectionsUpdated(ARObjectDetectionsUpdatedEventArgs obj)
    {
        string resultName = "";
        var result = obj.Results;

        if (result == null) return;

        for (int i = 0; i < result.Count; i++)
        {
            var detection = result[i];
            var categories = detection.GetConfidentCategorizations();

            if (categories.Count <= 0) break;

            // on va déclarer une fonction lambda pour faire le tri des catégories :
            categories.Sort((a, b) => b.Confidence.CompareTo(a.Confidence));
            
            for (int j = 0; j < categories.Count; j++)
            {
                var categoryToDisplay = categories[j];
                resultName = $"Détection de {categoryToDisplay.CategoryName} avec une certitude de {categoryToDisplay.Confidence*100}% - ";
            }

            Debug.Log(resultName);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
