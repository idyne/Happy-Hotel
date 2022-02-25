using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

[RequireComponent(typeof(Maid))]
public class SheetTaker : Employee
{
    private Maid maid;

    private void Awake()
    {
        maid = GetComponent<Maid>();
    }
    public override void Interact()
    {
        CleanSheet cleanSheet = ObjectPooler.Instance.SpawnFromPool("Clean Sheet", CleanSheetStation.Instance.CleanSheetSpawnPointTransform.position, Quaternion.identity).GetComponent<CleanSheet>();
        maid.PushCleanSheet(cleanSheet);
    }

    public override bool CanInteract()
    {
        return maid.CanTakeCleanSheet;
    }
}
