using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FateGames;

public class MoneyPile : MonoBehaviour
{
    private List<Transform> moneyPointTransforms = new List<Transform>();
    private Transform _transform;
    private int moneyTransformCount = 0;
    private int totalMoney = 0;
    private Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
        _transform = transform;
        for (int i = 0; i < _transform.childCount; i++)
        {
            Transform child = _transform.GetChild(i);
            moneyPointTransforms.Add(child);
            for (int j = 0; j < child.childCount; j++)
                Destroy(child.GetChild(j).gameObject);
        }
        moneyPointTransforms = moneyPointTransforms.OrderBy(o => o.position.y).ToList();
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        totalMoney = SaveSystem.PlayerData.MoneyInPile;
        moneyTransformCount = Mathf.Clamp(totalMoney / 10, 0, moneyPointTransforms.Count);
        for (int i = 0; i < moneyTransformCount; i++)
        {
            Transform parent = moneyPointTransforms[i];
            Transform moneyTransform = ObjectPooler.Instance.SpawnFromPool("Money", parent.position, Quaternion.identity).transform;
            moneyTransform.parent = parent;
            moneyTransform.localRotation = Quaternion.identity;
        }
    }

    public void AddMoney(Vector3 from)
    {
        Transform moneyTransform = ObjectPooler.Instance.SpawnFromPool("Money", from, Quaternion.identity).transform;
        int index = Mathf.Clamp(++moneyTransformCount, 0, moneyPointTransforms.Count) - 1;
        moneyTransform.parent = moneyPointTransforms[index];
        moneyTransform.localRotation = Quaternion.identity;
        if (moneyTransformCount > moneyPointTransforms.Count)
        {
            moneyTransform.parent = null;
            moneyTransform.gameObject.SetActive(false);
            moneyTransformCount = moneyPointTransforms.Count;
        }
        moneyTransform.LeanMove(moneyPointTransforms[index].position, 0.2f).setOnComplete(() =>
        {
            if (moneyTransformCount > moneyPointTransforms.Count)
            {
                moneyTransform.gameObject.SetActive(false);
            }
        });
        totalMoney += 10;
        SaveSystem.PlayerData.MoneyInPile = totalMoney;
    }

    public int CollectMoney()
    {
        int result = totalMoney;
        if (moneyTransformCount > 0)
        {
            List<Transform> moneyTransformsToDisappear = new List<Transform>();
            for (int i = moneyTransformCount - 1; i >= 0; i--)
            {
                Transform moneyTransform = moneyPointTransforms[i].GetChild(0);
                moneyTransform.parent = null;
                moneyTransformsToDisappear.Add(moneyTransform);
            }
            StartCoroutine(CollectMoneyCoroutine(moneyTransformsToDisappear));
            moneyTransformCount = 0;
            totalMoney = 0;
        }
        SaveSystem.PlayerData.MoneyInPile = totalMoney;
        return result;
    }

    private IEnumerator CollectMoneyCoroutine(List<Transform> moneyTransformsToDisappear)
    {
        Vector2 position = mainCamera.WorldToScreenPoint(moneyTransformsToDisappear[0].position);
        moneyTransformsToDisappear[0].gameObject.SetActive(false);
        moneyTransformsToDisappear.RemoveAt(0);
        Transform moneyImage = ObjectPooler.Instance.SpawnFromPool("Money Image", position, Quaternion.identity).transform;
        moneyImage.parent = InGameUI.Instance.CanvasTransform;
        moneyImage.position = position;
        moneyImage.localScale = Vector3.one * 0.7f;
        moneyImage.LeanScale(Vector3.one, 0.7f).setEaseInQuint();
        moneyImage.LeanMove(InGameUI.Instance.MoneyImageTransform.position, 0.7f).setEaseInQuint().setOnComplete(() =>
        {
            moneyImage.gameObject.SetActive(false);
        });
        yield return new WaitForSeconds(0.005f);
        if (moneyTransformsToDisappear.Count > 0)
            StartCoroutine(CollectMoneyCoroutine(moneyTransformsToDisappear));
    }
}
