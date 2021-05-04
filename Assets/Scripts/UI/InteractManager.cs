using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractManager : MonoBehaviour
{
    public static InteractManager instance = null;

    public UIPage actionPage;

    public Text actionUIText;

    public Text actionUIPrice;

    private Interactable current;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    public void StartInteractWithUser(Interactable interactable, string text, int price)
    {
        current = interactable;

        actionUIText.text = text;
        actionUIPrice.text = price.ToString() + "$";

        actionPage.gameObject.SetActive(true);
    }

    public void EndInteractWithUser(Interactable interactable)
    {
        current = null;

        actionPage.gameObject.SetActive(false);
    }

    public void ActionConfirmed()
    {
        if (current != null)
        {
            current.OnActionAccepted();
        }
    }
}
