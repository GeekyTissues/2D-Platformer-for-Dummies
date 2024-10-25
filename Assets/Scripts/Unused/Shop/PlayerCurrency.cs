using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    [SerializeField] CurrencyTracker currencyTracker;
    public int currencyTotal {  get; private set; }

    private void Awake()
    {
        currencyTotal = PlayerPrefs.GetInt("currencyTotal");
    }

    //Adds currency count to the internal tracker and to the visual tracker
    public void GainCurrency(int enemyValue)
    {
        currencyTracker.AddCurrency(enemyValue);
        currencyTotal += enemyValue;
        PlayerPrefs.SetInt("currencyTotal", currencyTotal);
    }
}
