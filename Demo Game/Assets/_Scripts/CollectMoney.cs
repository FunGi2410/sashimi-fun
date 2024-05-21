using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectMoney : MonoBehaviour
{
    //Stack<Transform> moneyCollecteds = new Stack<Transform>();
    Player player;
    DeliveryDesk delivery;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        delivery = GameObject.FindGameObjectWithTag("ProductPlace").GetComponent<DeliveryDesk>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Collect());
        }
    }

    IEnumerator Collect()
    {
        foreach (Transform money in transform)
        {
            player.CurMoney += 5;
            Player.SetMoneyTextEvent.Invoke();
            money.DOJump(new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z), 1f, 1, 0.1f).SetEase(Ease.Flash);
            Destroy(money.gameObject, 0.3f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        delivery.YDisMoney = 0;
        StopCoroutine(Collect());
    }
}
