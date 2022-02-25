using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(BoxCollider))]
public class ExpansionSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private GameObject canvasObject;
    public int currentPrice = 100;
    private Transform _transform;
    private BoxCollider boxCollider;


    public int CurrentPrice { get => currentPrice; }
    public Transform Transform { get => _transform; }

    private void Awake()
    {
        _transform = transform;
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        SetMoneyText();
    }

    private void SetMoneyText()
    {
        moneyText.text = currentPrice.ToString();
    }

    public void AddMoney(int money)
    {
        currentPrice -= money;
        SetMoneyText();
        SaveSystem.PlayerData.expansionSlotCurrentPrice = currentPrice;
        if (currentPrice <= 0)
            Expand();
    }

    public void Expand()
    {
        Hotel.Instance.Expand();
        Disable();
    }
    private void Disable()
    {
        boxCollider.enabled = false;
        canvasObject.SetActive(false);
        enabled = false;
    }

}
