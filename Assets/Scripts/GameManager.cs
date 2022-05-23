using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

        patient.InitializePatient();
        phobia.InitializePhobia();
        therapist.InitializeTherapist();
    }

    private void Awake()
    {
        InitialiseGame();
    }
}
