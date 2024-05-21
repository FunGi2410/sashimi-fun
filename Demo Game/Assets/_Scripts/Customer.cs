using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using DG.Tweening;

public class Customer : MonoBehaviour
{
    NavMeshAgent agent;

    DeliveryDesk deliveryDesk;
    public CustomerState state;
    Animator animator;
    Transform entrancePos;

    [SerializeField] private int amountProductRequire;
    [SerializeField] private float timeEating;

    [SerializeField] private TextMeshProUGUI amountTxt;
    public GameObject AmountTable;

    [SerializeField] bool isSat = false;
    Seat mSeat;

    public Animator Animator { get => animator; set => animator = value; }
    public int AmountProductRequire { get => amountProductRequire; set => amountProductRequire = value; }

    private void Start()
    {
        this.agent = GetComponent<NavMeshAgent>();
        this.Animator = GetComponent<Animator>();

        this.entrancePos = GameObject.FindGameObjectWithTag("EntrancePos").transform;
        this.agent.SetDestination(this.entrancePos.position);

        this.deliveryDesk = GameObject.FindGameObjectWithTag("ProductPlace").GetComponent<DeliveryDesk>();
        this.state = CustomerState.walking;

        this.timeEating = Random.Range(5f, 8f);

        // set amount product require text
        this.AmountProductRequire = Random.Range(1, 6);
        this.amountTxt.text = this.AmountProductRequire.ToString();
        this.AmountTable.SetActive(false);
    }

    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PurchasePlace"))
        {
            //print(gameObject.name + " Collide with PurchasePlace");
            this.AmountTable.SetActive(true);
            this.state = CustomerState.readyBuy;
            Invoke("GiveProduct", 2);
        }

        // Arrive seat
        if (other.CompareTag("Seat") && !isSat && other.transform.GetChild(0).childCount == 0 && !other.gameObject.GetComponent<Seat>().IsEmpty)
        {
            Animator.SetBool("isWalking", false);
            Animator.SetBool("isSeating", true);

            print(gameObject.name + " Arrive Seat");
            isSat = true;

            agent.radius = 0;
            
            transform.parent = other.transform.GetChild(0);
            //transform.localPosition = new Vector3(other.transform.GetChild(0).position.x, other.transform.GetChild(0).position.y, other.transform.GetChild(0).position.z);

            transform.DOJump(new Vector3(other.transform.GetChild(0).position.x, other.transform.GetChild(0).position.y, other.transform.GetChild(0).position.z),
                2f, 1, 0.5f).SetEase(Ease.OutQuad);

            //transform.localPosition = new Vector3(0f, 0f, 0f);

            this.mSeat = other.gameObject.GetComponent<Seat>();
            this.Eating();
        }
    }

    public void MoveTo(Vector3 pos)
    {
        StartCoroutine(MoveToCoroutine(pos));
    }

    public IEnumerator MoveToCoroutine(Vector3 pos)
    {
        Animator.SetBool("isWalking", true);
        this.agent.SetDestination(pos);
        float dist;

        if (this.agent.pathPending)
            dist = Vector3.Distance(transform.position, pos);
        else
            dist = this.agent.remainingDistance;

        while (dist > this.agent.stoppingDistance)
        {
            yield return new WaitForSecondsRealtime(.5f);
        }
        Animator.SetBool("isWalking", false);
        StopCoroutine(MoveToCoroutine(pos));
    }

    void Eating()
    {
        // Finish eating after "timeEating"
        state = CustomerState.eating;
        //this.agent.enabled = true;
        Invoke("LeaveRestaurant", this.timeEating);
    }

    void LeaveRestaurant()
    {
        agent.radius = 0.5f;
        Animator.SetBool("isSeating", false);
        Animator.SetBool("isWalking", true);

        state = CustomerState.finish;
        this.mSeat.IsEmpty = true;
        transform.parent = null;

        this.agent.SetDestination(this.entrancePos.GetChild(1).position);
        foreach (Transform product in transform.GetChild(0))
        {
            Destroy(product.gameObject);
        }

        // Call customer move to empty seat event
        WaitingQueue.MoveToSeatEvent.Invoke();
    }

    void GiveProduct()
    {
        this.deliveryDesk.Deliver();
    }
}

public enum CustomerState
{
    walking,
    arrive,
    waiting,
    readyBuy,
    eating,
    readyEat,
    finish
}
