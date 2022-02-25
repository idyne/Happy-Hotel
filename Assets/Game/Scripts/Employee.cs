using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public abstract class Employee : MonoBehaviour
{
    [SerializeField] private float castDuration = 2;
    [SerializeField] private string relatedInteractionZoneTag;
    [SerializeField] private Animator animator;
    [SerializeField] private bool hasCastAnimation;
    [SerializeField] private Image castImage;
    protected Action Callback = () => { };
    protected InteractionZone currentInteractionZone;
    private IEnumerator currentCastCoroutine;
    private bool isCasting = false;
    private LTDescr castImageTween = null;
    public float Cooldown { get => castDuration; }

    public abstract void Interact();
    public abstract bool CanInteract();

    private void Cast()
    {
        if (!CanInteract()) return;
        currentCastCoroutine = CastCoroutine(Callback);
        StartCoroutine(currentCastCoroutine);
    }


    private void Update()
    {
        if (currentInteractionZone)
        {
            if (!currentInteractionZone.gameObject.activeSelf)
            {
                ExitInteractionZone();
                return;
            }
            if (!isCasting)
                Cast();
        }
    }

    private IEnumerator CastCoroutine(Action Callback)
    {
        isCasting = true;
        if (castImage)
        {
            if (castImageTween != null)
                LeanTween.cancel(castImageTween.id);
            castImage.fillAmount = 0;
            castImage.gameObject.SetActive(true);
            castImageTween = LeanTween.value(gameObject, (val) =>
            {
                castImage.fillAmount = val;
            }, 0, 1, castDuration).setOnComplete(() =>
            {
                castImage.gameObject.SetActive(false);
                castImageTween = null;
            });
        }
        if (hasCastAnimation)
        {
            animator.ResetTrigger("CAST");
            animator.SetTrigger("CAST");
        }
        yield return new WaitForSeconds(castDuration);
        Interact();
        isCasting = false;
        currentCastCoroutine = null;
        Callback();
    }

    private void EnterInteractionZone(InteractionZone zone)
    {
        currentInteractionZone = zone;
        CancelCasting();
        isCasting = false;
    }

    private void ExitInteractionZone()
    {
        currentInteractionZone = null;
        CancelCasting();
        isCasting = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(relatedInteractionZoneTag))
        {
            EnterInteractionZone(other.GetComponent<InteractionZone>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(relatedInteractionZoneTag) && currentInteractionZone == other.GetComponent<InteractionZone>())
            ExitInteractionZone();
    }

    public void SetCallback(Action Callback)
    {
        this.Callback = Callback;
    }

    public void CancelCasting()
    {
        if (currentCastCoroutine != null)
        {
            StopCoroutine(currentCastCoroutine);
            currentCastCoroutine = null;
        }
    }


}
