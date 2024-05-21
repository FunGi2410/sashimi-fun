using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seat : MonoBehaviour
{
    [SerializeField] bool isEmpty;

    private void Start()
    {
        this.isEmpty = true;
    }

    public bool IsEmpty { get => isEmpty; set => isEmpty = value; }
}
