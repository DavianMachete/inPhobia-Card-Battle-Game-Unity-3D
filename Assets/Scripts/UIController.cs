using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class UIController : MonoBehaviour
{
    public static UIController instance;

    public List<CardController> therapistCardsInHand;
    public List<CardController> patientCardsInHand;

    public CardController firstSelectedCard;
    public CardController secondSelectedCard;

    public GameObject bigCardUI;

    #region Serialized Fields 

    [Header("                Before Main Game Game UIs            ")]
    [SerializeField]
    private GameObject backGroundUI;
    [SerializeField]
    private GameObject beforStartUI;
    [SerializeField]
    private GameObject mainGameUI;


    [Header("                The Main Game UIs            ")]
    [SerializeField]
    private GameObject cardPrefab;
    [SerializeField]
    private RectTransform therapistCardsParent;
    [SerializeField]
    private RectTransform patientCardsParent;
    [SerializeField]
    private Vector2 patientLowestPosOfCard;
    [SerializeField]
    private Vector2 patientHighestPosOfCard;
    [SerializeField]
    private Vector2 therapistLowestPosOfCard;
    [SerializeField]
    private Vector2 therapistHighestPosOfCard;

    [SerializeField]
    private Vector2 therapistCenteredCardPos;
    [SerializeField]
    private Vector2 patientCenteredCardPos;

    [Space(40f)]
    [Header("            Card Movement settings        ")]
    [SerializeField]
    private List<float> screenPartBoundsOnX;
    [SerializeField]
    private float patientAnimationDuration = 1f;


    [Space(40f)]
    [Header("            Game Endings        ")]
    [SerializeField] GameObject endPanel;
    [SerializeField] TMPro.TMP_Text levelinfoTxt;
    #endregion

    #region Private Fields


    #endregion


    #region Public Methods

    public void InitializeUIController()
    {
        MakeInstance();

        //UpdateCards(false);


        if (IFadeMainGameUIHelper != null)
            StopCoroutine(IFadeMainGameUIHelper);

        CanvasGroup cg = mainGameUI.GetComponent<CanvasGroup>();
        cg.alpha = 1f;

        endPanel.gameObject.SetActive(false);



        if (therapistCardsInHand == null)
            therapistCardsInHand = new List<CardController>();
        if (patientCardsInHand == null)
            patientCardsInHand = new List<CardController>();

        foreach (CardController card in therapistCardsInHand)
        {
            card.DestroyCard();
        }
        therapistCardsInHand.Clear();

        foreach (CardController card in patientCardsInHand)
        {
            card.DestroyCard();
        }
        patientCardsInHand.Clear();

        mainGameUI.SetActive(false);
        beforStartUI.SetActive(true);
        backGroundUI.SetActive(true);
    }

    public ScreenPart GetScreenPart(Vector2 mousePosition)
    {
        float leftBound, rightBound;

        float mouseX = mousePosition.x * 1920f / (float)Screen.width;
        mouseX -= 1920f / 2f;

        int index = 0; 
        for (int i = 0; i < screenPartBoundsOnX.Count+1; i++)
        {
            if (i == 0)
                leftBound = -1920f / 2f; 
            else
                leftBound = screenPartBoundsOnX[i - 1];

            if (i == screenPartBoundsOnX.Count)
                rightBound = 1920f / 2f;
            else
                rightBound = screenPartBoundsOnX[i];

            //Debug.Log($"leftBound= {leftBound}, rightBound = {rightBound}, Mouse Posotion.x = {mouseX}");

            if (mouseX >= leftBound && mouseX < rightBound)
            {
                index = i;
                switch (index)
                {
                    case 0:
                        return ScreenPart.PatientHand;
                    case 1:
                        return ScreenPart.Middle;
                    case 2:
                        return ScreenPart.Therapist;
                    default:
                        return ScreenPart.Therapist;
                }
            }
        }
        return ScreenPart.Therapist;
    }

    public void PlayPatientTopCard(UnityAction onDone)
    {
        if (IPlayPatientTopCardHelper == null)
            IPlayPatientTopCardHelper = StartCoroutine(IPlayPatientTopCard(onDone));
    }

    public void UpdateCards(bool smoothly)
    {
        List<CardController> sortedPatientCardsList = patientCardsInHand.OrderBy(o => o.index).ToList();
        patientCardsInHand = sortedPatientCardsList;

        List<CardController> sortedTherapistCardsList = therapistCardsInHand.OrderBy(o => o.index).ToList();
        therapistCardsInHand = sortedTherapistCardsList;

        foreach (CardController cardT in therapistCardsInHand)
        {
            cardT.UpdateCard(smoothly);
        }
        foreach (CardController cardP in patientCardsInHand)
        {
            cardP.UpdateCard(smoothly);
        }
    }

    public void UpdateCardsPosition(bool smoothly)
    {
        foreach (CardController cardT in therapistCardsInHand)
        {
            cardT.UpdateCardPosition(smoothly);
        }
        foreach (CardController cardP in patientCardsInHand)
        {
            cardP.UpdateCardPosition(smoothly);
        }
    }

    public void UpdateCardsScale(bool smoothly)
    {
        foreach (CardController cardT in therapistCardsInHand)
        {
            cardT.UpdateCardScale(smoothly);
        }
        foreach (CardController cardP in patientCardsInHand)
        {
            cardP.UpdateCardScale(smoothly);
        }
    }

    public void PullCardForTherapist(Card card)
    {
        // int rIndex = Random.Range(0, therapistCardsInHand.Count + 1);
        int rIndex = therapistCardsInHand.Count;

        GameObject newCard = Instantiate(cardPrefab, therapistCardsParent);
        CardController newCardController = newCard.GetComponent<CardController>();


        foreach (CardController cardT in therapistCardsInHand)
        {
            if (cardT.index >= rIndex)
                cardT.index++;
        }

        //therapistCardsInHand.Insert(rIndex, newCardController);
        therapistCardsInHand.Add(newCardController);


        newCardController.SetCardParametrsToGameObject(card);
        newCardController.SetCardCurrentType(CardUIType.TherapistCard);
        newCardController.SetCardMetrics(rIndex, therapistCenteredCardPos, therapistHighestPosOfCard, therapistLowestPosOfCard);
        Therapist.instance.AddCardToHand(rIndex, newCardController.card);

        //UpdateCards(true);
    }

    public void PullCardForPatient(Card card)
    {
        //int rIndex = Random.Range(0, patientCardsInHand.Count + 1);
        int rIndex = patientCardsInHand.Count;

        GameObject newCard = Instantiate(cardPrefab, patientCardsParent);
        CardController newCardController = newCard.GetComponent<CardController>();


        foreach (CardController cardP in patientCardsInHand)
        {
            if (cardP.index >= rIndex)
                cardP.index++;
        }

        // patientCardsInHand.Insert(rIndex, newCardController);
        patientCardsInHand.Add(newCardController);

        newCardController.SetCardParametrsToGameObject(card);
        newCardController.SetCardCurrentType(CardUIType.PatientCard);
        newCardController.SetCardMetrics(rIndex, patientCenteredCardPos, patientHighestPosOfCard, patientLowestPosOfCard);
        Patient.instance.AddCardToHand(rIndex, newCardController.card);

        //UpdateCards(true);
    }

    //Поставить свою карту в случайное место в руке пациента
    public void PutCardInRandomPlace(CardController cardController)
    {
        if (Therapist.instance.therapistCurrentAP - 1 < 0)
        {
            cardController.UpdateCard(true);
            return;
        }

        int tookCardIndex = cardController.index;

        therapistCardsInHand.Remove(cardController);
        Therapist.instance.RemoveCardFromHand(cardController.card);

        foreach (CardController cardT in therapistCardsInHand)
        {
            if (cardT.index > tookCardIndex)
                cardT.index--;
        }

        int rIndex = Random.Range(0, patientCardsInHand.Count + 1);

        foreach (CardController cardP in patientCardsInHand)
        {
            if (cardP.index >= rIndex)
                cardP.index++;
        }

        patientCardsInHand.Insert(rIndex, cardController);

        cardController.transform.SetParent(patientCardsParent);
        cardController.SetCardCurrentType(CardUIType.PatientCard);
        cardController.SetCardMetrics(rIndex, patientCenteredCardPos, patientHighestPosOfCard, patientLowestPosOfCard);
        Patient.instance.AddCardToHand(rIndex, cardController.card);

        UpdateCards(true);

        //Update TherapistActionPoints
        if (cardController.card.cardType == CardTypes.Equipment)
        {
            Therapist.instance.SetActionPoint(Therapist.instance.therapistCurrentAP - 1, Therapist.instance.therapistMaxAP - 1);
        }
        else
        {
            Therapist.instance.SetActionPoint(Therapist.instance.therapistCurrentAP - 1, Therapist.instance.therapistMaxAP);
        }
    }

    //Поменять местами 2 карты (в руке пациента или один из руки пациентаа другой из руки игрока,
    //firstSelectedCard всегда находится в руке пациента, secondSelectedCard может быть или из руки пацента, или из руки теропевта)
    public void CheckSelectedCardUIs()
    {
        if (firstSelectedCard != null && secondSelectedCard != null)
        {
            if (firstSelectedCard.cardCurrentType != secondSelectedCard.cardCurrentType)
            {
                if (Therapist.instance.therapistCurrentAP - 3 < 0)
                {
                    ResetSelectedes();
                    return;
                }
            }
            else
            {
                if (Therapist.instance.therapistCurrentAP - 1 < 0)
                {
                    ResetSelectedes();
                    return;
                }
            }

            patientCardsInHand.Remove(firstSelectedCard);
            Patient.instance.RemoveCardFromHand(firstSelectedCard.card);

            if (secondSelectedCard.cardCurrentType == CardUIType.PatientCard)
            {
                patientCardsInHand.Insert(secondSelectedCard.index, firstSelectedCard);
                Patient.instance.AddCardToHand(secondSelectedCard.index, firstSelectedCard.card);

                patientCardsInHand.Remove(secondSelectedCard);
                Patient.instance.RemoveCardFromHand(secondSelectedCard.card);
            }
            else
            {
                therapistCardsInHand.Insert(secondSelectedCard.index, firstSelectedCard);
                Therapist.instance.AddCardToHand(secondSelectedCard.index, firstSelectedCard.card);

                therapistCardsInHand.Remove(secondSelectedCard);
                Therapist.instance.RemoveCardFromHand(secondSelectedCard.card);
            }

            patientCardsInHand.Insert(firstSelectedCard.index, secondSelectedCard);
            Patient.instance.AddCardToHand(firstSelectedCard.index, secondSelectedCard.card);

            int indexHolder = firstSelectedCard.index;
            Vector2 centerPosHolder = firstSelectedCard.centeredCardPos;
            Vector2 lowestPosHolder = firstSelectedCard.lowestPosOfCard;
            Vector2 highestPosHolder = firstSelectedCard.highestPosOfCard;

            /////Set Patient card settings as Therapist
            firstSelectedCard.SetCardParametrsToGameObject(firstSelectedCard.card);
            firstSelectedCard.SetCardCurrentType(secondSelectedCard.cardCurrentType);
            firstSelectedCard.SetCardMetrics(secondSelectedCard.index, secondSelectedCard.centeredCardPos, secondSelectedCard.highestPosOfCard, secondSelectedCard.lowestPosOfCard);

            //Debug.Log($"secondSelectedCard.transform.parent name is {transformHolder.parent.gameObject.name}");
            firstSelectedCard.transform.SetParent(secondSelectedCard.transform.parent);


            /////Set Therapist card settings as Patient
            secondSelectedCard.SetCardParametrsToGameObject(secondSelectedCard.card);
            secondSelectedCard.SetCardCurrentType(CardUIType.PatientCard);
            secondSelectedCard.SetCardMetrics(indexHolder, centerPosHolder, highestPosHolder, lowestPosHolder);

            //Debug.Log($"transformHolder.parent name is {transformHolder.parent.gameObject.name}");
            secondSelectedCard.transform.SetParent(patientCardsParent);

            if (firstSelectedCard.cardCurrentType != CardUIType.PatientCard)
            {
                Therapist.instance.SetActionPoint(Therapist.instance.therapistCurrentAP - 3, Therapist.instance.therapistMaxAP);
                //NEED TO change deck belonging
            }
            else
            {
                Therapist.instance.SetActionPoint(Therapist.instance.therapistCurrentAP - 1, Therapist.instance.therapistMaxAP);
            }

            UpdateCards(true);
            //reset selectedes
            ResetSelectedes();
        }
    }

    public void AnimatePatientCardsBeforeDrop(float mouseY) 
    {
        float t = Mathf.InverseLerp(patientHighestPosOfCard.y, patientLowestPosOfCard.y, mouseY);
        //float indexByT = t * patientCards.Count;

        float step = 1f / patientCardsInHand.Count;

        for (int i = 0; i < patientCardsInHand.Count; i++)
        {
            float tForCard = step * i;
            if (tForCard > t)
                tForCard += step;
            patientCardsInHand[i].MoveCardToPlace(0.5f, tForCard, () => { });
        }
    }

    //Добавить свою карту в руку пациента в любое место, по желанию игрока
    public void DropCardToPatientHand(CardController cardController)
    {
        if (Therapist.instance.therapistCurrentAP - 2 < 0)
        {
            cardController.UpdateCard(true); 
            return;
        }

        therapistCardsInHand.Remove(cardController);
        Therapist.instance.RemoveCardFromHand(cardController.card);

        //detect index
        float t = Mathf.InverseLerp(patientHighestPosOfCard.y, patientLowestPosOfCard.y, cardController.GetComponent<RectTransform>().anchoredPosition.y);
        float step = 1f / patientCardsInHand.Count;
        int index = 0;
        for (int i = 0; i < patientCardsInHand.Count; i++)
        {
            float tForCard = step * i;
            if (t >= tForCard && t < tForCard + step)
                index = i;
        }
        cardController.SetCardParametrsToGameObject(cardController.card);
        cardController.SetCardCurrentType(CardUIType.PatientCard);
        cardController.SetCardMetrics(index, patientCenteredCardPos, patientHighestPosOfCard, patientLowestPosOfCard);

        //

        foreach (CardController cardP in patientCardsInHand)
        {
            if (cardP.index >= index)
                cardP.index++;
        }

        patientCardsInHand.Insert(index, cardController);
        Patient.instance.AddCardToHand(index, cardController.card);

        cardController.transform.SetParent(patientCardsParent);

        UpdateCards(true);

        Therapist.instance.SetActionPoint(Therapist.instance.therapistCurrentAP - 2, Therapist.instance.therapistMaxAP);
    }

    public void AddPsychosisToPatient()
    {
        //UpdateCardsUI(false);

        int index = Random.Range(0, patientCardsInHand.Count + 1);

        GameObject newCard = Instantiate(cardPrefab, patientCardsParent);
        newCard.GetComponent<RectTransform>().localScale = 0.4f * Vector3.one;
        CardController newCardController = newCard.GetComponent<CardController>();
        newCardController.SetCardParametrsToGameObject(Cards.Psychosis);
        newCardController.SetCardCurrentType(CardUIType.PatientCard);
        newCardController.SetCardMetrics(index, patientCenteredCardPos, patientHighestPosOfCard, patientLowestPosOfCard);

        //Debug.Log($"index = {index}");
        foreach (CardController cardP in patientCardsInHand)
        {
            if (cardP.index >= index)
                cardP.index++;
        }

        patientCardsInHand.Insert(index, newCardController);
        Patient.instance.AddCardToHand(index, newCardController.card);

        //Need to Insert and remove card in nessary abstract holders

        UpdateCards(true);
    }

    public void Discard(CardUIType cardUIType)
    {
        if(cardUIType == CardUIType.PatientCard)
        {
            foreach (CardController cardC in patientCardsInHand)
            {
                cardC.MoveCardToDiscard(new Vector2(80, -580));
            }
            patientCardsInHand.Clear();
        }
        if(cardUIType == CardUIType.TherapistCard)
        {
            foreach (CardController cardC in therapistCardsInHand)
            {
                cardC.MoveCardToDiscard(new Vector2(-80, -580));
            }
            therapistCardsInHand.Clear();
        }
    }

    public void OpenEndGamePanel(bool levelCompleted)
    {
        if (IFadeMainGameUIHelper != null)
            StopCoroutine(IFadeMainGameUIHelper);

        IFadeMainGameUIHelper = StartCoroutine(IFadeMainGameUI());

        levelinfoTxt.text = levelCompleted ? "Level Completed!!!" : "Level Faild(((";
        endPanel.gameObject.SetActive(true);
    }

    #endregion


    #region Private Methods

    private void ResetSelectedes() 
    {
        foreach (CardController card in patientCardsInHand)
        {
            card.transform.GetChild(0).gameObject.SetActive(false);
        }
        foreach (CardController card in therapistCardsInHand)
        {
            card.transform.GetChild(0).gameObject.SetActive(false);
        }

        secondSelectedCard = null;
        firstSelectedCard = null;
    }

    private Coroutine IPlayPatientTopCardHelper;
    private IEnumerator IPlayPatientTopCard(UnityAction onDone)
    {
        if (patientCardsInHand.Count <= 0)
            yield break;
        CardController patientTopCard = patientCardsInHand[0];
        patientTopCard.transform.SetParent(patientCardsParent.parent);
        patientCardsInHand.RemoveAt(0);

        foreach (CardController card in patientCardsInHand)
        {
            card.index--;
        }

        UpdateCards(true);

        patientTopCard.MoveCardToCenter(() => 
        {
            onDone();
            IPlayPatientTopCardHelper = null;
        });
    }

    private Coroutine IFadeMainGameUIHelper;
    private IEnumerator IFadeMainGameUI()
    {
        CanvasGroup cg = mainGameUI.GetComponent<CanvasGroup>();
        cg.alpha = 1f;

        float t = 0;
        float duration = 0.5f;

        while (t / duration < 1f)
        {
            cg.alpha = Mathf.Lerp(1f, 0f, t / duration);
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        cg.alpha = 0f;
        IFadeMainGameUIHelper = null;
    }

    #endregion


    private void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
