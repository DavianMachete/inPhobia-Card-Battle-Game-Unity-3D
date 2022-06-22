using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBarController : MonoBehaviour
{
    [SerializeField] private IdeaController ideaController;
    [SerializeField] private List<Transform> pointsGO;
    [SerializeField] private int startPointCount = 2;
    [SerializeField] private int pointsMinimumLimit = 1;

    private int currentPointsCount;
    private int maxPointCount;

    public void InitializeProgressBar()
    {
        ideaController.Initialize();

        pointsMinimumLimit = 1;

        maxPointCount = pointsGO.Count;
        currentPointsCount = startPointCount;

        UpdatePointsBar();
    }

    public void SetPointsMinimumLimit(int limit)
    {
        pointsMinimumLimit = limit;
    }

    public bool HasNeededAmoutOfPointsToUse(int amountNeeded)
    {
        if (currentPointsCount < amountNeeded)
            return false;
        else
            return true;
    }

    public bool PointsHavePassedTheLimit()
    {
        if (currentPointsCount <= pointsMinimumLimit)
            return true;
        else
            return false;
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
