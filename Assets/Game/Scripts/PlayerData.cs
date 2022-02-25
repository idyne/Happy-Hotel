using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int Money = 5000;
    public int MoneyInPile = 0;
    public bool IsReceptionistHired = false;
    public bool IsCashierHired = false;
    public bool IsMaid1Hired = false;
    public bool IsMaid2Hired = false;
    public bool IsMaid3Hired = false;
    public int playerCapacityLevel = 0;
    public int playerSpeedLevel = 0;
    public int maidCapacityLevel = 0;
    public int maidSpeedLevel = 0;
    public List<RoomSlotData> roomSlotData = new List<RoomSlotData>();
    public int expansionLevel = -1;
    public int expansionSlotCurrentPrice = 100;

    public PlayerData()
    {
        /*roomSlotData.Add(new RoomSlotData(
            Vector3.left * 10,
            Quaternion.identity,
            Vector3.left * 10,
            Quaternion.identity,
            0));*/
    }

    [System.Serializable]
    public class RoomSlotData
    {
        public float posX, posY, posZ;
        public float rotX, rotY, rotZ;
        public float spawnPosX, spawnPosY, spawnPosZ;
        public float spawnRotX, spawnRotY, spawnRotZ;
        public int currentPrice;

        public RoomSlotData(Vector3 position, Quaternion rotation, Vector3 spawnPosition, Quaternion spawnRotation, int currentPrice)
        {
            this.posX = position.x;
            this.posY = position.y;
            this.posZ = position.z;
            this.rotX = rotation.eulerAngles.x;
            this.rotY = rotation.eulerAngles.y;
            this.rotZ = rotation.eulerAngles.z;
            this.spawnPosX = spawnPosition.x;
            this.spawnPosY = spawnPosition.y;
            this.spawnPosZ = spawnPosition.z;
            this.spawnRotX = spawnRotation.eulerAngles.x;
            this.spawnRotY = spawnRotation.eulerAngles.y;
            this.spawnRotZ = spawnRotation.eulerAngles.z;
            this.currentPrice = currentPrice;
        }
    }

}
