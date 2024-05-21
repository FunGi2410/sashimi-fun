using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaitingQueue : MonoBehaviour
{
    public static Action MoveToSeatEvent;

    [SerializeField] List<Transform> waitingQueuePosList = new List<Transform>();
    [SerializeField] List<Customer> lineUpCustomers = new List<Customer>();
    SeatManager seatManager;
    public Transform lineUpPos;
    private void Start()
    {
        this.seatManager = GameObject.FindGameObjectWithTag("SeatManager").GetComponent<SeatManager>();
        WalkState.AddLineUpCustomerEvent += AddCustomerLineUp;
        //DeliveryDesk.MoveToSeatEvent += LeaveWaitingQueue;
        MoveToSeatEvent += LeaveWaitingQueue;
        foreach (Transform pos in this.lineUpPos)
            this.waitingQueuePosList.Add(pos);
    }

    public void AddCustomerLineUp(Customer customer)
    {
        this.lineUpCustomers.Add(customer);
        int indexLine = this.lineUpCustomers.Count - 1;
        customer.MoveTo(this.waitingQueuePosList[indexLine].position);
    }

    public void LeaveWaitingQueue()
    {
        print("Leave event");
        Invoke("Leave", 1f);
    }
    public void Leave()
    {
        Transform emptySeat = this.seatManager.GetEmptySeat();
        if (emptySeat == null) return;
        print("Invoke Leave");
        Customer headCustomer = GetHeadLineUpCustomer();
        if (headCustomer != null)
        {
            print(headCustomer.name + " Leave");
            emptySeat.gameObject.GetComponent<Seat>().IsEmpty = false;
            headCustomer.MoveTo(emptySeat.GetChild(0).position);
            headCustomer.Animator.SetBool("isWalking", true);
        }
    }

    public Customer GetHeadLineUpCustomer()
    {
        if (this.lineUpCustomers.Count == 0) return null;
        Customer customer = this.lineUpCustomers[0];
        if (customer.state != CustomerState.readyEat) return null;
        this.lineUpCustomers.RemoveAt(0);
        Invoke("ReArrangeAll", 1f);
        return customer;
    }

    void ReArrangeAll()
    {
        for(int i = 0; i < this.lineUpCustomers.Count; i++)
        {
            this.lineUpCustomers[i].MoveTo(this.waitingQueuePosList[i].position);
        }
    }
}
