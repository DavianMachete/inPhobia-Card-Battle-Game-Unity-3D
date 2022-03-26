using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGameObject : MonoBehaviour
{
    public string cardName;
    public CardTypes cardType;
    //public string affect;
    public int actionPoint;
    public Rarity rarity;


    public void ApplyToCardGameObject(Card card)
    {
        cardName = card.cardName;
        cardType = card.cardType;
        //affect = card.affect;
        actionPoint = card.actionPoint;
        rarity = card.rarity;

        if (card.cardType == CardTypes.Equipment)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text = $"{card.actionPoint}";
        }
        transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = card.cardName;
        //2
        transform.GetChild(3).GetChild(0).GetComponent<TMPro.TMP_Text>().text = card.cardType.ToString();
        //transform.GetChild(4).GetComponent<TMPro.TMP_Text>().text = card.affect;
    }

    public void DestroyCard()
    {
        Destroy(gameObject);
    }

}
