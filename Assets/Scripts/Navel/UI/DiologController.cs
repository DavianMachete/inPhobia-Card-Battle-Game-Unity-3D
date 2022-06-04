using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class DiologController : MonoBehaviour
{
    public TMP_Text persionTmp;
    public TMP_Text textTMP;
    public Button skipButton;
    public Button nextButton;
    public List<TMP_Text> questions;
    [Space(80)]
    [Header("DIOLOGS")]
    public List<Diolog> diolog;
    public UnityEvent OnDiologsEnd;

    private int diologIndex = 0;

    private void OnEnable()
    {
        diologIndex = 0;
        PrepareNextDiolog();
    }

    public void PrepareNextDiolog()
    {
        if (diolog.Count <= diologIndex)
        {
            OnDiologsEnd?.Invoke();
            return;
        }

        foreach (TMP_Text q in questions)
        {
            q.transform.parent.gameObject.SetActive(false);
        }
        skipButton.interactable = true;
        nextButton.interactable = true;

        persionTmp.text = diolog[diologIndex].Persion;
        textTMP.text = diolog[diologIndex].text;

        if (diolog[diologIndex].hasQuestion)
        {
            skipButton.interactable = false;
            nextButton.interactable = false;

            for (int i = 0; i < diolog[diologIndex].questions.Count; i++)
            {
                questions[i].transform.parent.gameObject.SetActive(true);
                questions[i].text = diolog[diologIndex].questions[i].question;
                questions[i].transform.parent.GetComponent<Button>().onClick.RemoveAllListeners();
                questions[i].transform.parent.GetComponent<Button>().onClick.AddListener(diolog[diologIndex].questions[i].onQuestion.Invoke);
                questions[i].transform.parent.GetComponent<Button>().onClick.AddListener(PrepareNextDiolog);
            }
        }
        diologIndex++;
    }

    public void PrepareNextDiologWithQuestion()
    {
        for (int i = diologIndex; i < diolog.Count; i++)
        {
            if (diolog[i].hasQuestion)
            {
                diologIndex = i;
                PrepareNextDiolog();
                break;
            }
        }
        diologIndex = diolog.Count;
        PrepareNextDiolog();
    }
}
