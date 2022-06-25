using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Public Fields

    public static UIManager instance;

    #endregion

    #region Serialized Fields

    [Header("Main scenes")]

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject cutscene;
    [SerializeField] private GameObject desctop;
    [SerializeField] private GameObject novel;
    [SerializeField] private GameObject cardCollecter;
    [SerializeField] private GameObject fight;
    [SerializeField] private GameObject cardsCanvas;
    [SerializeField] private GameObject gameEnd;
    [SerializeField] private GameObject Bar;

    [Header("Main Menu")]
    [SerializeField] private GameObject continueButton;

    [Header("Desctop")]
    [SerializeField] private DesctopController desctopController;

    [Header("Dialogs")]
    [SerializeField] private DialogController dialogController;

    [Header("Progress Bar")]
    [SerializeField] private ProgressBarController progressBarController;

    [Header("Therapist Deck Collecter")]
    [SerializeField] private TherapistDeckCollecter therapistDeckCollecter;

    [Header("Canvas Settings")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Fight Scene")]
    [SerializeField] private RectTransform card_sCanvasRT;

    [Header("Test settings")]
    [SerializeField] private IdeaController ideaController;

    #endregion

    #region Unity Behaviour

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        
    }

    #endregion

    #region Public Methods

    public RectTransform GetCard_sCanvas()
    {
        return card_sCanvasRT;
    }

    public void Initialize()
    {
        OpenMainMenu(false);
    }

    public void OpenMainMenu(bool withPause)
    {
        mainMenu.SetActive(true);
        if (withPause)
        {
            continueButton.SetActive(true);
        }
        else
        {
            cutscene.SetActive(false);
            desctop.SetActive(false);
            novel.SetActive(false);
            cardCollecter.SetActive(false);
            fight.SetActive(false);
            cardsCanvas.SetActive(false);
            gameEnd.SetActive(false);
            Bar.SetActive(false);
        }
    }

    public void ContinueGame()
    {
        continueButton.SetActive(false);
        mainMenu.SetActive(false);
    }

    public void OpenCutScene()
    {
        //mainMenu.SetActive(false);
        cutscene.SetActive(true);
        desctop.SetActive(false);
        novel.SetActive(false);
        cardCollecter.SetActive(false);
        fight.SetActive(false);
        cardsCanvas.SetActive(false);
        gameEnd.SetActive(false);
        Bar.SetActive(false);
    }

    public void OpenDesctop()
    {
        desctopController.ShowNextPatient(true);

        mainMenu.SetActive(false);
        cutscene.SetActive(false);
        desctop.SetActive(true);
        novel.SetActive(false);
        cardCollecter.SetActive(false);
        fight.SetActive(false);
        cardsCanvas.SetActive(false);
        gameEnd.SetActive(false);
        Bar.SetActive(false);
    }

    public void OpenNovel()
    {
        progressBarController.InitializeProgressBar();
        dialogController.InitializeDiolog();

        mainMenu.SetActive(false);
        cutscene.SetActive(false);
        desctop.SetActive(false);
        novel.SetActive(true);
        cardCollecter.SetActive(false);
        fight.SetActive(false);
        cardsCanvas.SetActive(false);
        gameEnd.SetActive(false);
        Bar.SetActive(true);
    }

    [ContextMenu("Open Card collector (5trust, 9 idea)")]
    public void OpenCardCollecterImmidiatly()
    {
        progressBarController.InitializeProgressBar();
        ideaController.Initialize();

        progressBarController.AddPoint(5);
        ideaController.AddIdea(9);

        OpenCardCollecter();
    }

    public void OpenCardCollecter()
    {
        therapistDeckCollecter.InitializeCollecter();

        mainMenu.SetActive(false);
        cutscene.SetActive(false);
        desctop.SetActive(false);
        novel.SetActive(false);
        cardCollecter.SetActive(true);
        fight.SetActive(false);
        cardsCanvas.SetActive(false);
        gameEnd.SetActive(false);
        Bar.SetActive(true);
    }

    public void OpenFightScene()
    {
        mainMenu.SetActive(false);
        cutscene.SetActive(false);
        desctop.SetActive(false);
        novel.SetActive(false);
        cardCollecter.SetActive(false);
        fight.SetActive(true);
        cardsCanvas.SetActive(true);
        gameEnd.SetActive(false);
        Bar.SetActive(false);
    }

    public void OpenGameEndScene()
    {
        mainMenu.SetActive(false);
        cutscene.SetActive(false);
        desctop.SetActive(false);
        novel.SetActive(false);
        cardCollecter.SetActive(false);
        fight.SetActive(false);
        cardsCanvas.SetActive(false);
        gameEnd.SetActive(false);
        Bar.SetActive(false);
    }

    public void SetCanvasGroupActive(bool value)
    {
        canvasGroup.interactable = value;
        canvasGroup.interactable = value;
    }

    #endregion

    #region Private Fields

    #endregion

    #region Private Methods

    #endregion

    #region Coroutines
    #endregion
}
