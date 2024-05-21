using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class DeliveryDesk : MonoBehaviour
{
    CustomerManager customerManager;
    float yDisProduct;
    [SerializeField] int priceProduct;

    // money
    [SerializeField] float DISTANCE_MONEY_Y;
    [SerializeField] GameObject moneyPref;
    [SerializeField] Transform moneyPlace;
    [SerializeField] Transform moneyParrent;
    float yDisMoney = 0.2f;
    int moneyPlaceIndex = 0;

    public float YDisMoney { get => yDisMoney; set => yDisMoney = value; }

    private void Start()
    {
        this.customerManager = GameObject.FindGameObjectWithTag("CustomerManager").GetComponent<CustomerManager>();
    }

    IEnumerator MakeMoney(int amountMoney)
    {
        var counter = 0;
        yield return new WaitForSecondsRealtime(1);

        while(counter < amountMoney)
        {
            /*GameObject newMoney = Instantiate(this.moneyPref, new Vector3(this.moneyPlace.GetChild(moneyPlaceIndex).position.x, this.yDisMoney, this.moneyPlace.GetChild(moneyPlaceIndex).position.z),
                this.moneyPlace.GetChild(moneyPlaceIndex).rotation);*/
            GameObject newMoney = Instantiate(this.moneyPref, new Vector3(transform.position.x, 0f, transform.position.z),
                this.moneyPlace.GetChild(moneyPlaceIndex).rotation);
            //newMoney.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.OutElastic);
            newMoney.transform.DOJump(new Vector3(this.moneyPlace.GetChild(moneyPlaceIndex).position.x, this.YDisMoney, this.moneyPlace.GetChild(moneyPlaceIndex).position.z), 1f, 1, 0.3f).SetEase(Ease.Flash);
            newMoney.transform.parent = moneyParrent;

            if (moneyPlaceIndex < 8) moneyPlaceIndex++;
            else
            {
                moneyPlaceIndex = 0;
                this.YDisMoney += DISTANCE_MONEY_Y;
            }

            counter++;

            yield return new WaitForSecondsRealtime(0.1f);
        }
        StopCoroutine(MakeMoney(0));
    }

    public void Deliver()
    {
        if(transform.childCount > 0)
        {
            //Destroy(transform.GetChild(transform.childCount - 1).gameObject, 1f);
            //print("Delivery for customer");

            // transfer
            GameObject headCustomer = this.customerManager.GetHeadCustomer();
            if (headCustomer.GetComponent<Customer>().state != CustomerState.readyBuy) return;
            
            //if (headCustomer.transform.GetChild(0).childCount == 0) return;
            /*if (headCustomer.transform.GetChild(0).childCount > 0)
            {
                this.yDisProduct = headCustomer.transform.GetChild(0).GetChild(headCustomer.transform.GetChild(0).childCount - 1).position.y;
            }
            else
            {
                this.yDisProduct = headCustomer.transform.GetChild(0).position.y;
            }*/

            this.yDisProduct = headCustomer.transform.GetChild(0).position.y;

            int amountProductRequire = headCustomer.GetComponent<Customer>().AmountProductRequire;
            if (transform.childCount < amountProductRequire)
            {
                StopCoroutine(MakeMoney(amountProductRequire));
                return;
            }
            int count = 0;
            for (var i = transform.childCount - 1; i >= 1; i--)
            {
                var product = transform.GetChild(i);
                product.DOJump(new Vector3(headCustomer.transform.GetChild(0).position.x, this.yDisProduct, headCustomer.transform.GetChild(0).position.z), 1f, 1, 0.5f).SetEase(Ease.Flash);
                product.parent = headCustomer.transform.GetChild(0);
                
                this.yDisProduct += 0.18f;
                if (++count >= amountProductRequire)
                {
                    StartCoroutine(MakeMoney(amountProductRequire * this.priceProduct));
                    headCustomer.GetComponent<Customer>().state = CustomerState.readyEat;
                    headCustomer.GetComponent<Customer>().AmountTable.SetActive(false);
                    WaitingQueue.MoveToSeatEvent.Invoke();
                    this.customerManager.RemoveHeadCustomer();
                    AudioManager.Instance.PlayerSFX(AudioManager.Instance.cashRegister);
                    break;
                }
            }
        }
    }
}
