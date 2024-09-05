using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    private CurrencyTracker currencyTracker;
    private int currencyTotal;

    private void Awake()
    {
        currencyTracker = GetComponent<CurrencyTracker>();
        currencyTotal = PlayerPrefs.GetInt("currencyTotal");
    }

    //Adds currency count to the internal tracker and to the visual tracker
    private void GainCurrency(int enemyValue)
    {
        currencyTracker.AddCurrency(enemyValue);
        currencyTotal += enemyValue;
        PlayerPrefs.SetInt("currencyTotal", currencyTotal);
    }
}
