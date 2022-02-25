using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameUI : MonoBehaviour
{
    private static InGameUI instance;
    private Transform _transform;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Transform moneyImageTransform;
    [SerializeField] private Transform canvasTransform;

    public static InGameUI Instance { get => instance; }
    public Transform Transform { get => _transform; }
    public Transform CanvasTransform { get => canvasTransform; }
    public Canvas Canvas { get => canvas; }
    public Transform MoneyImageTransform { get => moneyImageTransform; }

    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        _transform = transform;
    }
    public void SetMoneyText(int money)
    {
        moneyText.text = money.ToString();
    }
}
