using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;
using UnityEngine.Events;
using UnityEngine.AI;

public class Hotel : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMeshSurface;
    [SerializeField] private Transform meshTransform;
    [SerializeField] private MoneyPile moneyPile;
    [SerializeField] private CleanSheetStation cleanSheetStation;
    [SerializeField] private List<Room> rooms = new List<Room>();
    [SerializeField] private List<RoomSlot> roomSlots = new List<RoomSlot>();
    [SerializeField] private List<MaidAI> maids = new List<MaidAI>();
    [SerializeField] private InteractionZone receptionInteractionZone, checkoutInteractionZone;
    [SerializeField] private ExpandData[] expandData;
    [Header("Point Transforms")]
    [SerializeField] private Transform customerSpawnPointTransform;
    [SerializeField] private Transform customerLeavePointTransform;
    [SerializeField] private Transform maid1BasePointTransform;
    [SerializeField] private Transform maid2BasePointTransform;
    [SerializeField] private Transform maid3BasePointTransform;
    [SerializeField] private Transform[] receptionLinePointTransforms;
    [SerializeField] private Transform[] checkoutLinePointTransforms;
    [Header("Prefabs")]
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject roomSlotPrefab;
    [SerializeField] private GameObject expansionSlotPrefab;
    private static Hotel instance = null;
    private Player player;
    private List<Customer> customersInReception = new List<Customer>();
    private List<Customer> customersInCheckout = new List<Customer>();
    private List<Room> roomsToBeCleaned = new List<Room>();
    private float maidSpeed = 3.5f;
    private int maidCapacity = 1;


    public int NumberOfCustomersInHotel
    {
        get
        {
            int numberOfCustomersInRooms = 0;
            for (int i = 0; i < rooms.Count; i++)
                if (!rooms[i].IsEmpty) numberOfCustomersInRooms++;
            return numberOfCustomersInRooms + customersInReception.Count + customersInCheckout.Count;
        }
    }

    public bool ThereAreRoomsToBeCleaned { get => roomsToBeCleaned.Count > 0; }

    public static Hotel Instance { get => instance; }
    public Transform CustomerSpawnPointTransform { get => customerSpawnPointTransform; }
    public Transform[] ReceptionLinePointTransforms { get => receptionLinePointTransforms; }
    public Transform[] CheckoutLinePointTransforms { get => checkoutLinePointTransforms; }
    public Transform CustomerLeavePointTransform { get => customerLeavePointTransform; }
    public CleanSheetStation CleanSheetStation { get => cleanSheetStation; }
    public MoneyPile MoneyPile { get => moneyPile; }
    public Player Player { get => player; }
    public List<RoomSlot> RoomSlots { get => roomSlots; }
    public List<Room> Rooms { get => rooms; }


    private void Awake()
    {
        if (instance) { DestroyImmediate(gameObject); return; }
        instance = this;
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        StartCoroutine(SpawnCustomerLoop());
        //LeanTween.delayedCall(10, Expand);
    }

    public void Spawn()
    {
        if (SaveSystem.PlayerData.IsReceptionistHired) SpawnReceptionist();
        if (SaveSystem.PlayerData.IsCashierHired) SpawnCashier();
        if (SaveSystem.PlayerData.IsMaid1Hired) SpawnMaid();
        if (SaveSystem.PlayerData.IsMaid2Hired) SpawnMaid();
        if (SaveSystem.PlayerData.IsMaid3Hired) SpawnMaid();
        if (SaveSystem.PlayerData.expansionLevel < 0)
            Expand();
        else
        {
            if (meshTransform)
                Destroy(meshTransform.gameObject);
            ExpandData expandData = this.expandData[SaveSystem.PlayerData.expansionLevel];
            if (SaveSystem.PlayerData.expansionSlotCurrentPrice > 0)
            {
                ExpansionSlot expansionSlot = Instantiate(expansionSlotPrefab, expandData.ExpansionSlotPosition, Quaternion.Euler(expandData.ExpansionSlotRotation)).GetComponent<ExpansionSlot>();
                expansionSlot.currentPrice = SaveSystem.PlayerData.expansionSlotCurrentPrice;
            }
            meshTransform = Instantiate(expandData.MeshPrefab, transform).transform;
            navMeshSurface.BuildNavMesh();
            for (int i = 0; i < SaveSystem.PlayerData.roomSlotData.Count; i++)
                SpawnRoomSlot(SaveSystem.PlayerData.roomSlotData[i]);
        }
        player.AdjustSpeed();
        player.AdjustCapacity();
        AdjustMaidSpeed();
        AdjustMaidCapacity();
    }

    public void AdjustMaidSpeed()
    {
        maidSpeed = 3.5f + SaveSystem.PlayerData.maidSpeedLevel;
        foreach (MaidAI maid in maids)
            maid.Agent.speed = maidSpeed;
    }

    public void AdjustMaidCapacity()
    {
        maidCapacity = 1 + SaveSystem.PlayerData.maidCapacityLevel;
        foreach (MaidAI maid in maids)
            maid.Maid.cleanSheetLimit = maidCapacity;
    }

    public void SpawnRoomSlot(PlayerData.RoomSlotData data)
    {
        RoomSlot roomSlot = Instantiate(roomSlotPrefab, new Vector3(data.posX, data.posY, data.posZ), Quaternion.Euler(data.rotX, data.rotY, data.rotZ)).GetComponent<RoomSlot>();
        roomSlot.SetSpawnPositionAndRotation(new Vector3(data.spawnPosX, data.spawnPosY, data.spawnPosZ), Quaternion.Euler(data.spawnRotX, data.spawnRotY, data.spawnRotZ));
        roomSlot.data = data;
        roomSlot.currentPrice = data.currentPrice;
        if (data.currentPrice <= 0)
            roomSlot.BuyRoom();
    }

    private IEnumerator SpawnCustomerLoop()
    {
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        SpawnCustomer();
        StartCoroutine(SpawnCustomerLoop());
    }

    private void SpawnCustomer()
    {
        if (NumberOfCustomersInHotel >= rooms.Count) return;
        Customer customer = ObjectPooler.Instance.SpawnFromPool("Customer", customerSpawnPointTransform.position, customerSpawnPointTransform.rotation).GetComponent<Customer>();
        customer.ChangeState(customer.IN_RECEPTION);
        customer.IN_RECEPTION.SetPlaceInLine(EnqueueToReceptionLine(customer));

    }

    public Room GetEmptyRoom()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            Room room = rooms[i];
            if (room.IsAvaliable) return room;
        }
        return null;
    }

    public bool CanGiveCustomerRoom()
    {
        Customer customer = PeekReceptionLine();
        if (!customer || !customer.IN_RECEPTION.HasReachedPlaceInLine) return false;
        Room room = GetEmptyRoom();
        if (!room) return false;
        return true;
    }

    public void GiveCustomerRoom()
    {
        Customer customer = PeekReceptionLine();
        Room room = GetEmptyRoom();
        DequeueFromReceptionLine();
        room.SetCustomer(customer);
        customer.GOING_TO_ROOM.SetRoom(room);
        customer.ChangeState(customer.GOING_TO_ROOM);
        RearrangeReceptionLine();
    }

    public bool CanCustomerCheckout()
    {
        Customer customer = PeekCheckoutLine();
        if (!customer || !customer.IN_CHECKOUT.HasReachedPlaceInLine) return false;
        return true;
    }

    public void CheckoutCustomer()
    {
        Customer customer = PeekCheckoutLine();
        DequeueFromCheckoutLine();
        customer.IN_CHECKOUT.PayForRoom();
        customer.ChangeState(customer.LEAVING);
        RearrangeCheckoutLine();
    }

    private void RearrangeReceptionLine()
    {
        for (int i = 0; i < customersInReception.Count; i++)
        {
            Customer customer = customersInReception[i];
            customer.IN_RECEPTION.SetPlaceInLine(receptionLinePointTransforms[i]);
        }
    }

    private void RearrangeCheckoutLine()
    {
        for (int i = 0; i < customersInCheckout.Count; i++)
        {
            Customer customer = customersInCheckout[i];
            customer.IN_CHECKOUT.SetPlaceInLine(checkoutLinePointTransforms[i]);
        }
    }

    public Transform EnqueueToReceptionLine(Customer customer)
    {
        Transform result = receptionLinePointTransforms[customersInReception.Count];
        customersInReception.Add(customer);
        return result;
    }

    public void DequeueFromReceptionLine()
    {
        if (customersInReception.Count > 0)
            customersInReception.RemoveAt(0);
    }

    public Customer PeekReceptionLine()
    {
        if (customersInReception.Count > 0)
            return customersInReception[0];
        return null;
    }

    public Transform EnqueueToCheckoutLine(Customer customer)
    {
        Transform result = checkoutLinePointTransforms[customersInCheckout.Count];
        customersInCheckout.Add(customer);
        return result;
    }

    public void DequeueFromCheckoutLine()
    {
        if (customersInCheckout.Count > 0)
            customersInCheckout.RemoveAt(0);
    }

    public Customer PeekCheckoutLine()
    {
        if (customersInCheckout.Count > 0)
            return customersInCheckout[0];
        return null;
    }

    public Room GetRoomToBeCleaned()
    {
        Room result = null;
        if (ThereAreRoomsToBeCleaned)
        {
            result = roomsToBeCleaned[0];
            roomsToBeCleaned.RemoveAt(0);
        }
        return result;
    }

    public void AssignMaidToRoom(Room room)
    {
        bool isAssigned = false;
        for (int i = 0; i < maids.Count; i++)
        {
            MaidAI maid = maids[i];
            if (maid.State == maid.WAITING_SIGNAL)
            {
                maid.WAITING_SIGNAL.CleanRoom(room);
                isAssigned = true;
                break;
            }
        }
        if (!isAssigned) roomsToBeCleaned.Add(room);
    }

    public void RemoveRoomToBeCleaned(Room room)
    {
        roomsToBeCleaned.Remove(room);
    }

    public void SpawnReceptionist()
    {
        ObjectPooler.Instance.SpawnFromPool("Receptionist", receptionInteractionZone.Transform.position, receptionInteractionZone.Transform.rotation);
        receptionInteractionZone.DisableEffect();
        player.Receptionist.enabled = false;
    }

    public void SpawnCashier()
    {
        ObjectPooler.Instance.SpawnFromPool("Cashier", checkoutInteractionZone.Transform.position, checkoutInteractionZone.Transform.rotation);
        checkoutInteractionZone.DisableEffect();
        player.SalesWoman.enabled = false;
    }

    public void SpawnMaid()
    {
        Transform basePointTransform;
        if (maids.Count == 0) basePointTransform = maid1BasePointTransform;
        else if (maids.Count == 1) basePointTransform = maid2BasePointTransform;
        else basePointTransform = maid3BasePointTransform;
        MaidAI maid = ObjectPooler.Instance.SpawnFromPool("Maid", basePointTransform.position, basePointTransform.rotation).GetComponent<MaidAI>();
        maid.basePointTransform = basePointTransform;
        maids.Add(maid);
        Room room = GetRoomToBeCleaned();
        if (room)
            AssignMaidToRoom(room);
        AdjustMaidSpeed();
        AdjustMaidCapacity();
    }

    public Room SpawnRoom(Transform pointTransform)
    {
        print("Room spawned.");
        Room room = Instantiate(roomPrefab, pointTransform.position, pointTransform.rotation).GetComponent<Room>();
        rooms.Add(room);
        navMeshSurface.BuildNavMesh();
        return room;
    }

    public void Expand()
    {
        SaveSystem.PlayerData.expansionLevel++;
        ExpandData expandData = this.expandData[SaveSystem.PlayerData.expansionLevel];
        SaveSystem.PlayerData.expansionSlotCurrentPrice = expandData.NextExpansionPrice;
        if (meshTransform)
            Destroy(meshTransform.gameObject);
        meshTransform = Instantiate(expandData.MeshPrefab, transform).transform;
        navMeshSurface.BuildNavMesh();
        if (expandData.NextExpansionPrice > 0)
        {
            ExpansionSlot expansionSlot = Instantiate(expansionSlotPrefab, expandData.ExpansionSlotPosition, Quaternion.Euler(expandData.ExpansionSlotRotation)).GetComponent<ExpansionSlot>();
            expansionSlot.currentPrice = expandData.NextExpansionPrice;
        }
        for (int i = 0; i < expandData.Slots.Length; i++)
        {
            PlayerData.RoomSlotData slot = expandData.Slots[i];
            SaveSystem.PlayerData.roomSlotData.Add(slot);
            SpawnRoomSlot(slot);
        }
    }

    [System.Serializable]
    public class ExpandData
    {
        [SerializeField] private GameObject meshPrefab;
        [SerializeField] private int nextExpansionPrice = 100;
        [SerializeField] private Vector3 expansionSlotPosition;
        [SerializeField] private Vector3 expansionSlotRotation;
        [SerializeField] private PlayerData.RoomSlotData[] slots;

        public GameObject MeshPrefab { get => meshPrefab; }
        public PlayerData.RoomSlotData[] Slots { get => slots; }
        public int NextExpansionPrice { get => nextExpansionPrice; }
        public Vector3 ExpansionSlotPosition { get => expansionSlotPosition; set => expansionSlotPosition = value; }
        public Vector3 ExpansionSlotRotation { get => expansionSlotRotation; set => expansionSlotRotation = value; }
    }

}
