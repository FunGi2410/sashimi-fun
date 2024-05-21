using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UnlockObject : MonoBehaviour
{
    [SerializeField] private GameObject unlockObejctPref;
    //[SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI moneyTxt;
    [SerializeField] private int price;
    [SerializeField] private Transform objectParrent;
    //[SerializeField] private NavMeshSurface navSurface;

    private void Start()
    {
        this.moneyTxt.text = this.price.ToString() + " $";
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Trigger New desk");
        if(other.CompareTag("Player") && Player.instance.CurMoney >= this.price)
        {
            Player.instance.CurMoney -= this.price;
            Player.SetMoneyTextEvent.Invoke();

            GameObject newUnlockObject = Instantiate(this.unlockObejctPref, new Vector3(transform.position.x, 0f, transform.position.z), Quaternion.identity);
            newUnlockObject.transform.DOScale(1f, 1f).SetEase(Ease.OutElastic);
            newUnlockObject.transform.parent = this.objectParrent;
            transform.parent.gameObject.SetActive(false);

            //Invoke("BuildNavMesh", 1f);

            // Call customer move to empty seat event
            WaitingQueue.MoveToSeatEvent.Invoke();
        }
    }

    /*void BuildNavMesh()
    {
        navSurface.BuildNavMesh();
    }*/
}
