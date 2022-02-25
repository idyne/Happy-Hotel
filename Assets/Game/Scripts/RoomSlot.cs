using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(BoxCollider))]
public class RoomSlot : MonoBehaviour
{
    [SerializeField] private int price = 100;
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private GameObject canvasObject;
    public int currentPrice = 100;
    private Transform _transform;
    private BoxCollider boxCollider;
    public PlayerData.RoomSlotData data;


    public int CurrentPrice { get => currentPrice; }
    public Transform Transform { get => _transform; }
    public Transform SpawnTransform { get => spawnTransform; }

    private void Awake()
    {
        currentPrice = price;
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
        data.currentPrice = currentPrice;
        if (currentPrice <= 0)
            BuyRoom();
    }

    public void BuyRoom()
    {
        Hotel.Instance.SpawnRoom(spawnTransform);
        Disable();
    }

    private void Disable()
    {
        boxCollider.enabled = false;
        canvasObject.SetActive(false);
        enabled = false;
    }

    public void SetSpawnPositionAndRotation(Vector3 position, Quaternion rotation)
    {
        spawnTransform.SetPositionAndRotation(position, rotation);
    }

}
