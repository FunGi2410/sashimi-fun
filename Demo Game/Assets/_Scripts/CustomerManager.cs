using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> customerPrefabs;
    [SerializeField] private Transform spawnPlace;
    [SerializeField] private int limitCountCustomer;
    [SerializeField] private float timeSpawn;
    int countCustomer;

    [SerializeField] List<GameObject> customers = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(SpawnCoroutine(this.timeSpawn));
    }

    public GameObject GetHeadCustomer()
    {
        if(this.customers.Count > 0)
            return this.customers[0];
        return null;
    }

    public void RemoveHeadCustomer()
    {
        this.customers.RemoveAt(0);
        StartCoroutine(SpawnCoroutine(this.timeSpawn));
    }

    public IEnumerator SpawnCoroutine(float time)
    {
        while (this.customers.Count < limitCountCustomer)
        {
            int indexCustomer = Random.Range(0, this.customerPrefabs.Count);
            GameObject newCustomer = Instantiate(this.customerPrefabs[indexCustomer], this.spawnPlace.position, Quaternion.identity);
            newCustomer.transform.parent = transform;
            this.customers.Add(newCustomer);

            
            this.countCustomer++;

            yield return new WaitForSecondsRealtime(time);
        }
    }
}
