using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBarController : MonoBehaviour
{
    //[SerializeField] private TMP_Text barText;
    //[SerializeField] private Image bar;

    [SerializeField] private List<Transform> pointsGO;
    [SerializeField] private int startPointCount = 2;

    //[SerializeField] private int maxPointsCount;

    private int currentPointsCount;
    private int maxPointCount;

    public void InitializeProgressBar()
    {
        maxPointCount = pointsGO.Count;
        currentPointsCount = startPointCount;

        UpdatePointsBar();
    }

    public void AddPoint(int value)
    {
        currentPointsCount += value;

        if (currentPointsCount > maxPointCount)
        {
            currentPointsCount = maxPointCount;
            return;
        }
        if (currentPointsCount < 0)
        {
            currentPointsCount = 0;
            return;
        }

        UpdatePointsBar();
    }

    private void UpdatePointsBar()
    {
        for (int i = 0; i < pointsGO.Count; i++)
        {
            if (i < currentPointsCount)
                pointsGO[i].gameObject.SetActive(true);
            else
                pointsGO[i].gameObject.SetActive(false);
        }
    }
}
