using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogController : MonoBehaviour
{
    [SerializeField] private ProgressBarController progressBarController;

    public TMP_Text persionTmp;
    public TMP_Text textTMP;
    public Button skipButton;
    public Button nextButton;
    public List<TMP_Text> questions;
    [Header("DIOLOGS")]
    public UnityEvent OnDialogsStarts;
    public UnityEvent OnDialogsEnd;


    [SerializeField]private List<Dialog> dialog;

    private int dialogIndex = 0;

    public void InitializeDiolog()
    {
        if (dialog == null)
            dialog = new List<Dialog>();
        dialog.Clear();
        dialogIndex = 0;
        OnDialogsStarts?.Invoke();
        PrepareNextDialog();
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

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(dialog[dialogIndex].onNext.Invoke);
        nextButton.onClick.AddListener(PrepareNextDialog);

        if (dialog[dialogIndex].questions.Count > 0)
        {
            skipButton.interactable = false;
            nextButton.interactable = false;

            for (int i = 0; i < dialog[dialogIndex].questions.Count; i++)
            {
                questions[i].transform.parent.gameObject.SetActive(true);
                questions[i].text = dialog[dialogIndex].questions[i].question;

                Button questionButton = questions[i].transform.parent.GetComponent<Button>();

                int index = i;
                questionButton.onClick.RemoveAllListeners();
                questionButton.onClick.AddListener(()=> 
                {
                    dialog[dialogIndex - 1].questions[index].onQuestion.Invoke();
                    if (progressBarController.PointsHavePassedTheLimit())
                    {
                        dialog[dialogIndex - 1].onPointsHavePassedTheLimit?.Invoke();
                        int subscribersCount = dialog[dialogIndex - 1].onPointsHavePassedTheLimit.GetPersistentEventCount();
                        Debug.Log($"<color=white>DiologController:</color> onPointsHavePassedTheLimit subscribersCount = {subscribersCount}");
                        
                        if (subscribersCount > 0)
                            PrepareNextDialogWithQuestion();
                        else
                            PrepareNextDialog();
                    }
                    else
                    {
                        PrepareNextDialog();
                    }
                });

            }
        }
        dialogIndex++;
    }

    public void PrepareNextDialogWithQuestion()
    {
        for (int i = dialogIndex; i < dialog.Count; i++)
        {
            if (dialog[i].questions.Count > 0)
            {
                dialogIndex = i;
                PrepareNextDialog();
                break;
            }
        }
    }

    public void AddBlock(DialogBlock dialogBlock)
    {
        dialog.AddRange(dialogBlock.dialogBlock);
    }

    public void ChangeBlock(DialogBlock dialogBlock)
    {
        dialogIndex = 0;
        dialog = new List<Dialog>(dialogBlock.dialogBlock);
    }
}
