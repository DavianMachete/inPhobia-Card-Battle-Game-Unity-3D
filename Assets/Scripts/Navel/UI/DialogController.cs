using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogController : MonoBehaviour
{
    public TMP_Text persionTmp;
    public TMP_Text textTMP;
    public Button skipButton;
    public Button nextButton;
    public List<TMP_Text> questions;
    [Space(80)]
    [Header("DIOLOGS")]
    public UnityEvent OnDialogsStarts;
    public List<Dialog> dialog;
    public UnityEvent OnDialogsEnd;

    private int dialogIndex = 0;

    private void OnEnable()
    {
        dialogIndex = 0;
        PrepareNextDialog();
        OnDialogsStarts?.Invoke();
    }

    public void PrepareNextDialog()
    {
        if (dialog.Count <= dialogIndex)
        {
            OnDialogsEnd?.Invoke();
            return;
        }

        foreach (TMP_Text q in questions)
        {
            q.transform.parent.gameObject.SetActive(false);
        }
        skipButton.interactable = true;
        nextButton.interactable = true;

        persionTmp.text = dialog[dialogIndex].Persion;
        textTMP.text = dialog[dialogIndex].text;

        if (dialog[dialogIndex].hasQuestion)
        {
            skipButton.interactable = false;
            nextButton.interactable = false;

            for (int i = 0; i < dialog[dialogIndex].questions.Count; i++)
            {
                questions[i].transform.parent.gameObject.SetActive(true);
                questions[i].text = dialog[dialogIndex].questions[i].question;
                questions[i].transform.parent.GetComponent<Button>().onClick.RemoveAllListeners();
                questions[i].transform.parent.GetComponent<Button>().onClick.AddListener(dialog[dialogIndex].questions[i].onQuestion.Invoke);
                questions[i].transform.parent.GetComponent<Button>().onClick.AddListener(PrepareNextDialog);
            }
        }
        dialogIndex++;
    }

    public void PrepareNextDialogWithQuestion()
    {
        for (int i = dialogIndex; i < dialog.Count; i++)
        {
            if (dialog[i].hasQuestion)
            {
                dialogIndex = i;
                PrepareNextDialog();
                break;
            }
        }
        dialogIndex = dialog.Count;
        PrepareNextDialog();
    }

    public void AddBranch(DialogBranch dialogBrach)
    {
        dialog.AddRange(dialogBrach.dialog);
    }
}
