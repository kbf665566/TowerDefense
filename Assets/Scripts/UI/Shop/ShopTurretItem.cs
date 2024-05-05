using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShopTurretItem : MonoBehaviour
{
    [SerializeField] private Image turretImage;
    [SerializeField] private TextMeshProUGUI costText;

    public void SetCost(int cost)
    {
        costText.text = "$" + cost;
    }
}
