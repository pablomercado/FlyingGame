using UnityEngine;
using System.Collections;

public class CoinSpawner : MonoBehaviour {
    public GameObject coinGO;
    // Use this for initialization

    private float multiplier = 0;

	void Start () {

        StartCoroutine(instantiateCoins(0.1f));
	}
	
    IEnumerator instantiateCoins(float wait)
    {
        multiplier++;
        yield return new WaitForSeconds(wait);
        Vector3 position = new Vector3(Time.time, Mathf.Sin(Time.time));
        Instantiate(coinGO, position, Quaternion.identity);
        coinGO.GetComponent<CoinMotion>().multiplier = multiplier;
        StartCoroutine(instantiateCoins(0.3f));
    }

	// Update is called once per frame
	void Update () {
	    
	}
}
