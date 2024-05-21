using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Factory : MonoBehaviour
{
    public float DISTANCE_PRODUCT_Y;

    [SerializeField] int MAX_AMOUNT_PRODUCT;

    [SerializeField] private GameObject productInstance;
    [SerializeField] private float productionTime;
    private float yDis;
    [SerializeField] Transform exportedPlaceProduct;

    [SerializeField] private int countProduct = 0;
    Stack<Transform> exportedProducts = new Stack<Transform>();

    float timer;

    public int CountProduct { get => countProduct; set => countProduct = value; }
    public float YDis { get => yDis; set => yDis = value; }

    private void Start()
    {
        this.exportedPlaceProduct = transform.GetChild(0);
    }

    private void Update()
    {
        this.timer += Time.deltaTime;
        if (this.timer >= this.productionTime && CountProduct < MAX_AMOUNT_PRODUCT)
        {
            Manufacture();
            this.timer = 0;
        }
    }

    public Transform GetProduct()
    {
        return this.exportedProducts.Pop();
    }

    public void Manufacture()
    {
        GameObject newProduct = Instantiate(this.productInstance, new Vector3(transform.position.x, -3f, transform.position.z), Quaternion.identity);
        newProduct.transform.parent = this.exportedPlaceProduct;
        this.exportedProducts.Push(newProduct.transform);

        // animation using dot tween
        newProduct.transform.DOJump(new Vector3(this.exportedPlaceProduct.position.x, this.exportedPlaceProduct.position.y + this.YDis, this.exportedPlaceProduct.position.z),
            2f, 1, 0.5f).SetEase(Ease.OutQuad);

        this.YDis += DISTANCE_PRODUCT_Y;
        this.CountProduct++;
    }
}
