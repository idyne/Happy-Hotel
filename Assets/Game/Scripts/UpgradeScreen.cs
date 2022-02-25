using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeScreen : MonoBehaviour
{
    [SerializeField] private GameObject staffPanel, upgradesPanel;
    [SerializeField] private Upgrader upgrader;
    [SerializeField]
    private Button
        closeButton,
        hireReceptionistButton,
        hireCashierButton,
        hireMaid1Button,
        hireMaid2Button,
        hireMaid3Button,
        yourSpeedButton,
        yourCapacityButton,
        maidSpeedButton,
        maidCapacityButton;
    [SerializeField]
    private Image
        hireReceptionistCheckMark,
        hireCashierCheckMark,
        hireMaid1CheckMark,
        hireMaid2CheckMark,
        hireMaid3CheckMark,
        yourSpeedCheckMark,
        yourCapacityCheckMark,
        maidSpeedCheckMark,
        maidCapacityCheckMark;
    [SerializeField]
    private TextMeshProUGUI
        hireReceptionistPrice,
        hireCashierPrice,
        hireMaid1Price,
        hireMaid2Price,
        hireMaid3Price,
        yourSpeedPrice,
        yourCapacityPrice,
        maidSpeedPrice,
        maidCapacityPrice;

    public Button HireReceptionistButton { get => hireReceptionistButton; }
    public Button HireCashierButton { get => hireCashierButton; }
    public Button HireMaid1Button { get => hireMaid1Button; }
    public Button HireMaid2Button { get => hireMaid2Button; }
    public Button HireMaid3Button { get => hireMaid3Button; }
    public Button CloseButton { get => closeButton; }
    public Button YourSpeedButton { get => yourSpeedButton; }
    public Button YourCapacityButton { get => yourCapacityButton; }
    public Button MaidSpeedButton { get => maidSpeedButton; }
    public Button MaidCapacityButton { get => maidCapacityButton; }

    public void ShowUpgradesPanel()
    {
        upgradesPanel.SetActive(true);
        staffPanel.SetActive(false);
        Open();
    }

    public void ShowStaffPanel()
    {
        upgradesPanel.SetActive(false);
        staffPanel.SetActive(true);
        Open();
    }

    public void Open()
    {
        hireReceptionistPrice.text = "$ " + upgrader.ReceptionistCost;
        hireCashierPrice.text = "$ " + upgrader.CashierCost;
        hireMaid1Price.text = "$ " + upgrader.Maid1Cost;
        hireMaid2Price.text = "$ " + upgrader.Maid2Cost;
        hireMaid3Price.text = "$ " + upgrader.Maid3Cost;
        yourSpeedPrice.text = "$ " + upgrader.PlayerSpeedCosts[SaveSystem.PlayerData.playerSpeedLevel];
        yourCapacityPrice.text = "$ " + upgrader.PlayerSpeedCosts[SaveSystem.PlayerData.playerCapacityLevel];
        maidSpeedPrice.text = "$ " + upgrader.PlayerSpeedCosts[SaveSystem.PlayerData.maidSpeedLevel];
        maidCapacityPrice.text = "$ " + upgrader.PlayerSpeedCosts[SaveSystem.PlayerData.maidCapacityLevel];
        SetButtonState(SaveSystem.PlayerData.IsReceptionistHired, hireReceptionistButton, hireReceptionistCheckMark);
        SetButtonState(SaveSystem.PlayerData.IsCashierHired, HireCashierButton, hireCashierCheckMark);
        SetButtonState(SaveSystem.PlayerData.IsMaid1Hired, HireMaid1Button, hireMaid1CheckMark);
        SetButtonState(SaveSystem.PlayerData.IsMaid2Hired, HireMaid2Button, hireMaid2CheckMark);
        SetButtonState(SaveSystem.PlayerData.IsMaid3Hired, HireMaid3Button, hireMaid3CheckMark);
        SetButtonState(SaveSystem.PlayerData.playerSpeedLevel > 2, YourSpeedButton, yourSpeedCheckMark);
        SetButtonState(SaveSystem.PlayerData.playerCapacityLevel > 2, yourCapacityButton, yourCapacityCheckMark);
        SetButtonState(SaveSystem.PlayerData.maidSpeedLevel > 2, MaidSpeedButton, maidSpeedCheckMark);
        SetButtonState(SaveSystem.PlayerData.maidCapacityLevel > 2, MaidCapacityButton, maidCapacityCheckMark);
        gameObject.SetActive(true);
    }

    private void SetButtonState(bool condition, Button button, Image image)
    {
        button.gameObject.SetActive(!condition);
        image.gameObject.SetActive(condition);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void DisableButton(Button button)
    {
        button.interactable = false;
    }

    public void EnableButton(Button button)
    {
        button.interactable = true;
    }
}
