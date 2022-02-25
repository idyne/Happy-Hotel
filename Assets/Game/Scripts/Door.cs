using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private List<string> reactToTags;
    [SerializeField] private Transform leftDoorTransform, rightDoorTransform;
    [SerializeField] private GameObject physicsColliderObject;
    private bool isOpen = false;
    private List<Transform> overlappedCharacters = new List<Transform>();
    private bool isRotating = false;
    private bool isLocked = false;

    private void Update()
    {
        if (overlappedCharacters.Count > 0)
            Open();
        else
            Close();
    }

    public void Open()
    {
        if (isLocked || isOpen || isRotating) return;
        isRotating = true;
        if (physicsColliderObject)
            physicsColliderObject.SetActive(false);
        if (leftDoorTransform)
            leftDoorTransform.LeanRotateAroundLocal(leftDoorTransform.up, 105, 0.3f);
        if (rightDoorTransform)
            rightDoorTransform.LeanRotateAroundLocal(rightDoorTransform.up, -105, 0.3f);
        LeanTween.delayedCall(0.35f, () =>
        {
            isRotating = false;
            
        });
        isOpen = true;
    }

    public void Close()
    {
        if (!isOpen || isRotating) return;
        print("Close");
        isRotating = true;
        if (leftDoorTransform)
            leftDoorTransform.LeanRotateAroundLocal(leftDoorTransform.up, -105, 0.3f);
        if (rightDoorTransform)
            rightDoorTransform.LeanRotateAroundLocal(rightDoorTransform.up, 105, 0.3f);
        LeanTween.delayedCall(0.35f, () =>
        {
            isRotating = false;
            if (physicsColliderObject)
                physicsColliderObject.SetActive(true);
        });
        isOpen = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (reactToTags.Contains(other.tag))
            overlappedCharacters.Add(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (reactToTags.Contains(other.tag))
            overlappedCharacters.Remove(other.transform);
    }

}
