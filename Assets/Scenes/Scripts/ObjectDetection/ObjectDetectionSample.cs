using System;
using System.Collections;
using System.Collections.Generic;
using Niantic.Lightship.AR.ObjectDetection;
using UnityEngine;

public class ObjectDetectionSample : MonoBehaviour
{
    [SerializeField] private float _probabilityThreshold = .5f;

    [SerializeField] private ARObjectDetectionManager _objectDetectionManager;

    private Color[] colors = new[]
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.magenta,
        Color.cyan,
        Color.white,
        Color.black
    };


    [SerializeField] private DrawRect _drawRect;

    private Canvas _canvas;


    private void Awake()
    {
        _canvas = FindFirstObjectByType<Canvas>();
    }

    void Start()
    {
        _objectDetectionManager.enabled = true;
        _objectDetectionManager.MetadataInitialized += ObjectDetectionManagerOnMetadataInitialized;
    }

    private void OnDestroy()
    {
        _objectDetectionManager.MetadataInitialized -= ObjectDetectionManagerOnMetadataInitialized;
        _objectDetectionManager.ObjectDetectionsUpdated -= ObjectDetectionManagerOnObjectDetectionsUpdated;
    }

    private void ObjectDetectionManagerOnMetadataInitialized(ARObjectDetectionModelEventArgs obj)
    {
        _objectDetectionManager.ObjectDetectionsUpdated += ObjectDetectionManagerOnObjectDetectionsUpdated;
    }

    private void ObjectDetectionManagerOnObjectDetectionsUpdated(ARObjectDetectionsUpdatedEventArgs obj)
    {
        string resultString = "";
        float confidence = 0;
        string _name = "";
        var result = obj.Results;

        if (result == null)
            return;

        _drawRect.ClearRects();

        for (int i = 0; i < result.Count; i++)
        {
            var detection = result[i];
            var categorization = detection.GetConfidentCategorizations(_probabilityThreshold);


            if (categorization.Count <= 0)
            {
                break;
            }

            categorization.Sort((a, b) => b.Confidence.CompareTo(a.Confidence));
            var categoryToDisplay = categorization[0];

            confidence = categoryToDisplay.Confidence;
            name = categoryToDisplay.CategoryName;

            int h = Mathf.FloorToInt(_canvas.GetComponent<RectTransform>().rect.height);
            int w = Mathf.FloorToInt(_canvas.GetComponent<RectTransform>().rect.width);



            var rect = result[i].CalculateRect(w, h, Screen.orientation);

            resultString = $"{name}: {confidence}\n";

            _drawRect.CreateRect(rect, colors[i % colors.Length], resultString);


        }
    }


}