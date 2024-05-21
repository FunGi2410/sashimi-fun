using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class Player : MonoBehaviour
{
    public float DISTANCE_PRODUCT_Y;
    public float MAX_AMOUNT_PRODUCT_HOLD;
    public float DELAY_TRANSFER_TO_DESK;

    public static Player instance;
    [SerializeField] private float speedProduct;
    [SerializeField] private List<Transform> products = new List<Transform>();
    [SerializeField] Transform productPlace;
    Animator animator;
    float yDis;

    [SerializeField] private int curMoney;

    public static Action SetMoneyTextEvent;
    [SerializeField] private TextMeshProUGUI moneyTxt;

    public int CurMoney { get => curMoney; set => curMoney = value; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        this.products.Add(this.productPlace);
        this.animator = GetComponent<Animator>();

        // Set UI
        SetMoneyTextEvent += SetMoneyTxt;
        SetMoneyTxt();
    }

    private void Update()
    {
        this.Collection();
    }

    void SetMoneyTxt()
    {
        this.moneyTxt.text = this.curMoney.ToString() + " $";
    }

    void Collection()
    {
        // Move products intro player
        if(products.Count > 1)
        {
            for (int i = 1; i < products.Count; i++)
            {
                var firstProduct = products[i - 1];
                var secondProduct = products[i];
                secondProduct.position = new Vector3(Mathf.Lerp(secondProduct.position.x, firstProduct.position.x, Time.deltaTime * speedProduct),
                    Mathf.Lerp(secondProduct.position.y, firstProduct.position.y + DISTANCE_PRODUCT_Y, Time.deltaTime * speedProduct), firstProduct.position.z);
            }
        }

        if(Physics.Raycast(transform.position, transform.forward, out var hit, 1f))
        {
            Debug.DrawRay(transform.position, transform.forward * 1f, Color.green);
            if (hit.collider.CompareTag("ProductShelf") && products.Count < MAX_AMOUNT_PRODUCT_HOLD)
            {
                if(hit.collider.transform.childCount > 1)
                {
                    var product = hit.collider.transform.parent.GetComponent<Factory>().GetProduct();
                    products.Add(product);
                    product.parent = null;

                    if (hit.collider.transform.parent.GetComponent<Factory>().CountProduct > 1)
                        hit.collider.transform.parent.GetComponent<Factory>().CountProduct--;

                    if (hit.collider.transform.parent.GetComponent<Factory>().YDis > 0)
                        hit.collider.transform.parent.GetComponent<Factory>().YDis -= DISTANCE_PRODUCT_Y;

                    this.animator.SetBool("isCarry", true);
                }
            }

            // transfer product from player to delivery desk
            if (hit.collider.CompareTag("ProductPlace") && this.products.Count > 1)
            {
                // animation from carry to idle
                this.animator.SetBool("isCarry", false);
                
                // transfer
                var deliveryDesk = hit.collider.transform;
                if(deliveryDesk.childCount > 0)
                    this.yDis = deliveryDesk.GetChild(deliveryDesk.childCount - 1).position.y;
                else
                    this.yDis = deliveryDesk.position.y;

                for(var i = products.Count - 1; i >= 1; i--)
                {
                    this.products[i].DOJump(new Vector3(deliveryDesk.position.x, this.yDis, deliveryDesk.position.z), 2f, 1, 0.5f).SetDelay(DELAY_TRANSFER_TO_DESK).SetEase(Ease.Flash);
                    this.products[i].parent = deliveryDesk;
                    this.products.RemoveAt(i);
                    
                    this.yDis += DISTANCE_PRODUCT_Y;
                }
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 1f, Color.red);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ProductPlace"))
        {
            other.GetComponent<DeliveryDesk>().Deliver();
            // Call customer move to empty seat event
            WaitingQueue.MoveToSeatEvent.Invoke();
        }
    }
}
