using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatManager : MonoBehaviour
{
    public Transform GetEmptySeat()
    {
        foreach (Transform seat in transform)
        {
            if (seat.GetComponent<Seat>().IsEmpty)
            { 
                //seat.GetComponent<Seat>().IsEmpty = false;
                return seat;
            }
        }  
        return null;
    }
}
