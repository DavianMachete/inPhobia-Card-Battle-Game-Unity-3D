using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private Phobia phobia;
    [SerializeField]
    private Patient patient;
    [SerializeField]
    private Therapist therapist;
    [SerializeField]
    private UIController _UIcontroller;




    public void QuitFromGame()
    {
        Application.Quit();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Break();
        }
#endif
    }

    public void InitialiseGame()
    {
        _UIcontroller.InitializeUIController();
        _UIcontroller.SetInteractable(true);

        patient.InitializePatient();
        phobia.InitializePhobia();

        therapist.InitializeTherapist();
    }

    public void PlayNextTurn()
    {
        patient.PrepareNewTurn();
        therapist.PrepareNewTurn();
    }

    public void LevelCompleted()
    {
        _UIcontroller.OpenEndGamePanel(true);
    }

    public void LevelFailed()
    {
        _UIcontroller.OpenEndGamePanel(false);
    }

    private void Awake()
    {
        InitialiseGame();
        MakeInstance();
    }

    private void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
