using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SheetTaker))]
[RequireComponent(typeof(Cleaner))]
public class Maid : MonoBehaviour
{
    [SerializeField] private Transform[] cleanSheetPointTransforms;
    [SerializeField] public int cleanSheetLimit = 1;
    private List<CleanSheet> cleanSheets = new List<CleanSheet>();
    private SheetTaker sheetTaker;
    private Cleaner cleaner;
    [SerializeField] private MaidAI maidAI;

    public bool HasCleanSheets { get => cleanSheets.Count > 0; }
    public bool CanTakeCleanSheet { get => cleanSheets.Count < cleanSheetLimit; }

    private void Awake()
    {
        sheetTaker = GetComponent<SheetTaker>();
        cleaner = GetComponent<Cleaner>();
    }

    public CleanSheet PopCleanSheet()
    {
        if (!HasCleanSheets) return null;
        CleanSheet result = cleanSheets[cleanSheets.Count - 1];
        cleanSheets.RemoveAt(cleanSheets.Count - 1);
        result.Transform.parent = null;
        return result;
    }

    public void PushCleanSheet(CleanSheet cleanSheet)
    {
        Transform cleanSheetPointTransform = cleanSheetPointTransforms[cleanSheets.Count];
        cleanSheet.Transform.parent = cleanSheetPointTransform;
        cleanSheets.Add(cleanSheet);
        cleanSheet.Transform.LeanMoveLocal(Vector3.zero, 0.2f);
        cleanSheet.Transform.localRotation = Quaternion.identity;
    }

    public Transform[] CleanSheetPointTransforms { get => cleanSheetPointTransforms; }
    public List<CleanSheet> CleanSheets { get => cleanSheets; }
    public SheetTaker SheetTaker { get => sheetTaker; }
    public Cleaner Cleaner { get => cleaner; }
    public bool IsLocked { get { if (!maidAI) return false; return maidAI.State.GetType() == typeof(States.MaidState.CLEANING_ROOM); } }
}
