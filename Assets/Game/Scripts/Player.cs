using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

[RequireComponent(typeof(Swerve))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Maid))]
[RequireComponent(typeof(Receptionist))]
[RequireComponent(typeof(SalesWoman))]
public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform canvasTransform;
    private Camera mainCamera;
    private Transform mainCameraTransform;
    private MoneyPile moneyPile;
    private Swerve swerve;
    private Transform _transform;
    private Rigidbody rb;
    private Vector3 previousPosition;
    public int Money = 0;
    private Receptionist receptionist;
    private SalesWoman salesWoman;
    private RoomSlot roomSlot;
    private ExpansionSlot expansionSlot;
    private Maid maid;
    private float velocity { get => Vector3.Distance(previousPosition, _transform.position) / Time.deltaTime; }
    public Receptionist Receptionist { get => receptionist; }
    public SalesWoman SalesWoman { get => salesWoman; }

    private void Awake()
    {
        mainCamera = Camera.main;
        mainCameraTransform = mainCamera.transform;
        maid = GetComponent<Maid>();
        swerve = GetComponent<Swerve>();
        rb = GetComponent<Rigidbody>();
        receptionist = GetComponent<Receptionist>();
        salesWoman = GetComponent<SalesWoman>();
        _transform = transform;
        previousPosition = _transform.position;

    }

    private void Start()
    {
        Money = SaveSystem.PlayerData.Money;
        InGameUI.Instance.SetMoneyText(Money);
    }

    private void Update()
    {
        animator.SetBool("CARRYING", maid.HasCleanSheets);

        canvasTransform.LookAt(mainCameraTransform);
        if (moneyPile)
        {
            AddMoney(moneyPile.CollectMoney());
        }
        if (roomSlot && (swerve.Rate == 0))
        {
            if (roomSlot.enabled)
            {
                int deltaPrice = Mathf.Clamp(Mathf.Clamp(Mathf.CeilToInt(Time.deltaTime * 120), 0, roomSlot.CurrentPrice), 0, Money);
                if (deltaPrice > 0)
                {
                    AddMoney(-deltaPrice);
                    roomSlot.AddMoney(deltaPrice);
                }
            }
            else roomSlot = null;
        }
        if (expansionSlot && (swerve.Rate == 0))
        {
            if (expansionSlot.enabled)
            {
                int deltaPrice = Mathf.Clamp(Mathf.Clamp(Mathf.CeilToInt(Time.deltaTime * 150), 0, expansionSlot.CurrentPrice), 0, Money);
                if (deltaPrice > 0)
                {
                    AddMoney(-deltaPrice);
                    expansionSlot.AddMoney(deltaPrice);
                }
            }
            else expansionSlot = null;
        }
        rb.velocity = Vector3.zero;
        _transform.position = new Vector3(_transform.position.x, 0, _transform.position.z);
        SetAnimatorVelocity();
        SetPreviousPosition();
    }

    public void AdjustSpeed()
    {
        speed = 3.5f + SaveSystem.PlayerData.playerSpeedLevel / 2f;
    }

    public void AdjustCapacity()
    {
        maid.cleanSheetLimit = 2 + SaveSystem.PlayerData.playerCapacityLevel;
    }

    public void AddMoney(int money)
    {
        Money += money;
        InGameUI.Instance.SetMoneyText(Money);
        SaveSystem.PlayerData.Money = Money;
    }

    public void Move()
    {
        Vector3 direction = new Vector3(swerve.Difference.x, 0, swerve.Difference.y);
        float sqrMagnitude = direction.sqrMagnitude;
        if (sqrMagnitude > 0.1f)
        {
            _transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
            _transform.position = (Vector3.MoveTowards(
                _transform.position,
                _transform.position + direction,
                Time.deltaTime * swerve.Rate * speed));
        }
    }

    private void SetAnimatorVelocity()
    {
        animator.SetFloat("velocity", velocity);
    }


    private void SetPreviousPosition()
    {
        previousPosition = _transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Money Pile"))
            moneyPile = other.GetComponent<MoneyPile>();
        else if (other.CompareTag("Upgrader Interaction Zone"))
            other.GetComponentInParent<Upgrader>().OpenUpgradeScreen();
        else if (other.CompareTag("Room Slot"))
            roomSlot = other.GetComponent<RoomSlot>();
        else if (other.CompareTag("Expansion Slot"))
            expansionSlot = other.GetComponent<ExpansionSlot>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Money Pile"))
            moneyPile = null;
        else if (other.CompareTag("Upgrader Interaction Zone"))
            other.GetComponentInParent<Upgrader>().CloseUpgradeScreen();
        else if (other.CompareTag("Room Slot"))
            roomSlot = null;
        else if (other.CompareTag("Expansion Slot"))
            expansionSlot = null;
    }

}
