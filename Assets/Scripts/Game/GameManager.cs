using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SetPause();
        }
#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Break();
        }
#endif
    }

    private void Start()
    {
        instance = this;
        UIManager.instance.Initialize();
    }

    public void QuitFromGame()
    {
        Application.Quit();
    }

    public void InitialiseGame()
    {
        UIManager.instance.OpenCutSceneOne();
        PatientManager.instance.InitializePatient();
        TherapistManager.instance.InitializeTherapist();
        PhobiaManager.instance.InitializePhobia();
    }

    public void SetPause()
    {
        UIManager.instance.OpenMainMenu(true);
    }

    public void PlayNextTurn()
    {
        PatientManager.instance.PrepareNewTurn();
        TherapistManager.instance.PrepareNewTurn();
    }

    public void LevelCompleted()
    {
        //_UIcontroller.OpenEndGamePanel(true);
    }

    public void LevelFailed()
    {
        //_UIcontroller.OpenEndGamePanel(false);
    }
}
