using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerupShop : MonoBehaviour
{
    [SerializeField] private LayoutGroup powerUpLayout = null;
    [SerializeField] private PowerupShopItem shopItemPrefab = null;

    [SerializeField] private TextMeshProUGUI selectedItemTitle = null;
    [SerializeField] private TextMeshProUGUI selectedItemDescription = null;
    [SerializeField] private Button purchaseButton = null;
    [SerializeField] private TextMeshProUGUI purchaseButtonText = null;

    private Dictionary<PowerupBase.PowerupType, PowerupShopItem> shopItemDictionary = new Dictionary<PowerupBase.PowerupType, PowerupShopItem>();

    private PowerupBase.PowerupType _selectedItem = PowerupBase.PowerupType.NONE;
    public PowerupBase.PowerupType selectedItem
    {
        get
        {
            return _selectedItem;
        }
        set
        {
            SetItemTexts(value);

            _selectedItem = value;
        }
    }

    private void Awake()
    {
        InitializeShop();
    }

    public void InitializeShop()
    {
        List<PowerupCostList.PowerUpCostPair> powerUpCosts = GameManager.Instance.powerUpCosts.powerUpCostList;

        foreach (PowerupCostList.PowerUpCostPair powerUpCost in powerUpCosts)
        {
            PowerupShopItem shopItem = Instantiate(shopItemPrefab, powerUpLayout.transform);
            shopItem.InitializeItem(powerUpCost, this);
            shopItemDictionary.Add(powerUpCost.type, shopItem);
        }

        shopItemDictionary[PowerupBase.PowerupType.DOUBLE_JUMP].OnItemClicked();
    }

    private void SetItemTexts(PowerupBase.PowerupType type)
    {
        selectedItemTitle.text = GameManager.Instance.powerUpCosts.GetTitleText(type);
        selectedItemDescription.text = GameManager.Instance.powerUpCosts.GetDescriptionText(type);
        bool owned = PlayerInventory.Instance.playerPowerups.ContainsKey(type);
        purchaseButtonText.text = owned ? "Owned" : "Cost: " + GameManager.Instance.powerUpCosts.GetCostForPowerup(type);
        purchaseButton.image.color = owned ? Color.red : new Color(1, 1, 1, 0.5f);
    }

    public void PurchaseSelectedItem()
    {
        PlayerInventory.Instance.PurchasePowerup(selectedItem);
        shopItemDictionary[selectedItem].InitializeItem(selectedItem, this);
        SetItemTexts(selectedItem);
    }

    public void CloseShop()
    {
        Time.timeScale = 1;
        Destroy(this.gameObject);
    }
}
