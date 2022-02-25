using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrader : MonoBehaviour
{
    [SerializeField] private int receptionistCost = 100;
    [SerializeField] private int cashierCost = 150;
    [SerializeField] private int maid1Cost = 500;
    [SerializeField] private int maid2Cost = 1200;
    [SerializeField] private int maid3Cost = 3000;
    [SerializeField] private int[] playerCapacityCosts;
    [SerializeField] private int[] playerSpeedCosts;
    [SerializeField] private int[] maidCapacityCosts;
    [SerializeField] private int[] maidSpeedCosts;
    [SerializeField] private UpgradeScreen upgradeScreen;

    public int ReceptionistCost { get => receptionistCost; }
    public int CashierCost { get => cashierCost; }
    public int Maid1Cost { get => maid1Cost; }
    public int Maid2Cost { get => maid2Cost; }
    public int Maid3Cost { get => maid3Cost; }
    public int[] PlayerCapacityCost { get => playerCapacityCosts; }
    public int[] PlayerSpeedCosts { get => playerSpeedCosts; }
    public int[] MaidCapacityCosts { get => maidCapacityCosts; }
    public int[] MaidSpeedCosts { get => maidSpeedCosts; }

    private void Awake()
    {
        upgradeScreen.CloseButton.onClick.AddListener(CloseUpgradeScreen);
        upgradeScreen.HireReceptionistButton.onClick.AddListener(HireReceptionist);
        upgradeScreen.HireCashierButton.onClick.AddListener(HireCashier);
        upgradeScreen.HireMaid1Button.onClick.AddListener(HireMaid1);
        upgradeScreen.HireMaid2Button.onClick.AddListener(HireMaid2);
        upgradeScreen.HireMaid3Button.onClick.AddListener(HireMaid3);
        upgradeScreen.YourSpeedButton.onClick.AddListener(IncreasePlayerSpeed);
        upgradeScreen.YourCapacityButton.onClick.AddListener(IncreasePlayerCapacity);
        upgradeScreen.MaidSpeedButton.onClick.AddListener(IncreaseMaidSpeed);
        upgradeScreen.MaidCapacityButton.onClick.AddListener(IncreaseMaidCapacity);
    }

    public void OpenUpgradeScreen()
    {
        if (Hotel.Instance.Player.Money < receptionistCost) upgradeScreen.DisableButton(upgradeScreen.HireReceptionistButton);
        else upgradeScreen.EnableButton(upgradeScreen.HireReceptionistButton);
        if (Hotel.Instance.Player.Money < cashierCost) upgradeScreen.DisableButton(upgradeScreen.HireCashierButton);
        else upgradeScreen.EnableButton(upgradeScreen.HireCashierButton);
        if (Hotel.Instance.Player.Money < maid1Cost) upgradeScreen.DisableButton(upgradeScreen.HireMaid1Button);
        else upgradeScreen.EnableButton(upgradeScreen.HireMaid1Button);
        if (Hotel.Instance.Player.Money < maid2Cost) upgradeScreen.DisableButton(upgradeScreen.HireMaid2Button);
        else upgradeScreen.EnableButton(upgradeScreen.HireMaid2Button);
        if (Hotel.Instance.Player.Money < maid3Cost) upgradeScreen.DisableButton(upgradeScreen.HireMaid3Button);
        else upgradeScreen.EnableButton(upgradeScreen.HireMaid3Button);
        if (Hotel.Instance.Player.Money < playerSpeedCosts[SaveSystem.PlayerData.playerSpeedLevel]) upgradeScreen.DisableButton(upgradeScreen.YourSpeedButton);
        else upgradeScreen.EnableButton(upgradeScreen.YourSpeedButton);
        if (Hotel.Instance.Player.Money < playerCapacityCosts[SaveSystem.PlayerData.playerCapacityLevel]) upgradeScreen.DisableButton(upgradeScreen.YourCapacityButton);
        else upgradeScreen.EnableButton(upgradeScreen.YourCapacityButton);
        if (Hotel.Instance.Player.Money < maidSpeedCosts[SaveSystem.PlayerData.maidSpeedLevel]) upgradeScreen.DisableButton(upgradeScreen.MaidSpeedButton);
        else upgradeScreen.EnableButton(upgradeScreen.MaidSpeedButton);
        if (Hotel.Instance.Player.Money < maidCapacityCosts[SaveSystem.PlayerData.maidCapacityLevel]) upgradeScreen.DisableButton(upgradeScreen.MaidCapacityButton);
        else upgradeScreen.EnableButton(upgradeScreen.MaidCapacityButton);
        upgradeScreen.Open();
    }
    public void CloseUpgradeScreen()
    {
        upgradeScreen.Close();
    }
    public void HireReceptionist()
    {
        if (Hotel.Instance.Player.Money < receptionistCost) return;
        Hotel.Instance.Player.AddMoney(-receptionistCost);
        Hotel.Instance.SpawnReceptionist();
        SaveSystem.PlayerData.IsReceptionistHired = true;
        upgradeScreen.Open();
    }

    public void HireCashier()
    {
        if (Hotel.Instance.Player.Money < cashierCost) return;
        Hotel.Instance.Player.AddMoney(-cashierCost);
        Hotel.Instance.SpawnCashier();
        SaveSystem.PlayerData.IsCashierHired = true;
        upgradeScreen.Open();
    }

    public void HireMaid1()
    {
        if (Hotel.Instance.Player.Money < maid1Cost) return;
        Hotel.Instance.Player.AddMoney(-maid1Cost);
        Hotel.Instance.SpawnMaid();
        SaveSystem.PlayerData.IsMaid1Hired = true;
        upgradeScreen.Open();
    }

    public void HireMaid2()
    {
        if (Hotel.Instance.Player.Money < maid2Cost) return;
        Hotel.Instance.Player.AddMoney(-maid2Cost);
        Hotel.Instance.SpawnMaid();
        SaveSystem.PlayerData.IsMaid2Hired = true;
        upgradeScreen.Open();
    }

    public void HireMaid3()
    {
        if (Hotel.Instance.Player.Money < maid3Cost) return;
        Hotel.Instance.Player.AddMoney(-maid3Cost);
        Hotel.Instance.SpawnMaid();
        SaveSystem.PlayerData.IsMaid3Hired = true;
        upgradeScreen.Open();
    }
    public void IncreasePlayerSpeed()
    {
        int cost = playerSpeedCosts[SaveSystem.PlayerData.playerSpeedLevel];
        if (Hotel.Instance.Player.Money < cost) return;
        Hotel.Instance.Player.AddMoney(-cost);
        SaveSystem.PlayerData.playerSpeedLevel++;
        Hotel.Instance.Player.AdjustSpeed();
        upgradeScreen.Open();
    }

    public void IncreasePlayerCapacity()
    {
        int cost = playerCapacityCosts[SaveSystem.PlayerData.playerCapacityLevel];
        if (Hotel.Instance.Player.Money < cost) return;
        Hotel.Instance.Player.AddMoney(-cost);
        SaveSystem.PlayerData.playerCapacityLevel++;
        Hotel.Instance.Player.AdjustCapacity();
        upgradeScreen.Open();
    }

    public void IncreaseMaidSpeed()
    {
        int cost = maidSpeedCosts[SaveSystem.PlayerData.maidSpeedLevel];
        if (Hotel.Instance.Player.Money < cost) return;
        Hotel.Instance.Player.AddMoney(-cost);
        SaveSystem.PlayerData.maidSpeedLevel++;
        Hotel.Instance.AdjustMaidSpeed();
        upgradeScreen.Open();
    }

    public void IncreaseMaidCapacity()
    {
        int cost = maidCapacityCosts[SaveSystem.PlayerData.maidCapacityLevel];
        if (Hotel.Instance.Player.Money < cost) return;
        Hotel.Instance.Player.AddMoney(-cost);
        SaveSystem.PlayerData.maidCapacityLevel++;
        Hotel.Instance.AdjustMaidCapacity();
        upgradeScreen.Open();
    }
}
