using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Interactable : MonoBehaviour
{
    public string text;

    public int price;

    public enum ActionType { Door };

    public ActionType actionType = ActionType.Door;

    public void OnActionAccepted()
    {
        switch(actionType)
        {
            case ActionType.Door:
                if (GameManager.instance.UsePlayerMoney(price))
                {
                    Destroy(this.gameObject);
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            InteractManager.instance.StartInteractWithUser(this, text, price);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            InteractManager.instance.EndInteractWithUser(this);
        }
    }
}
