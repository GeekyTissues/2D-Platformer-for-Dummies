using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyTracker : MonoBehaviour
{
    [SerializeField] private Text currencyTotal;

    private int currencyCount;

    //Updates the tracker in the UI
    public void AddCurrency(int _amount)
    {
        currencyCount += _amount;
        currencyTotal.text = currencyCount.ToString();
    }
}
