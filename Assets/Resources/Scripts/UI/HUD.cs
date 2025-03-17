using UnityEngine;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    private UIDocument _root;
    private Label _count;

    private Button _sheepButton;
    private Button _upgradeButton;
    private Button _sleepButton;

    private void Start()
    {
        _root = GetComponent<UIDocument>();
        
        // get the label
        _count = _root.rootVisualElement.Q<Label>("count");
        _count.text = "0";

        // get the buttons
        _sheepButton = _root.rootVisualElement.Q<Button>("sheep");
        _upgradeButton = _root.rootVisualElement.Q<Button>("upgrade");
        _sleepButton = _root.rootVisualElement.Q<Button>("sleep");

        // assign events
        GameManager.OnJumpCounted.AddListener(UpdateCount);
        GameManager.OnThresholdReached.AddListener(ActivateButtons);

        SheepHandler.OnCostChanged.AddListener(UpdateSpawnButton);
        SheepHandler.OnLevelChanged.AddListener(UpdateUpgradeButton);

        _sheepButton.clicked += () => SpawnClicked();
        _upgradeButton.clicked += () => UpgradeClicked();
    }

    private void ActivateButtons(int jumps)
    {
        if (jumps == GameManager.thresholds[0])
        {
            _sheepButton.RemoveFromClassList("button-inactive");
        }

        if (jumps == GameManager.thresholds[1])
        {
            _upgradeButton.RemoveFromClassList("button-inactive");
        }

        if (jumps == GameManager.thresholds[2])
        {
            _sleepButton.RemoveFromClassList("button-inactive");
        }
    }

    private void SpawnClicked()
    {
        SheepHandler.OnSpawnClicked.Invoke();
    }

    private void UpgradeClicked()
    {
        SheepHandler.OnUpgradeClicked.Invoke();
    }
    

    private void UpdateSpawnButton(int newCost)
    {
        _sheepButton.text = $"Buy 1 sheep\n\nCOST = {newCost} jumps";
    }

    private void UpdateUpgradeButton(int level, int newCost)
    {
        switch (level) 
        {
            case 2:
                _upgradeButton.text = $"GOLDEN SHEEP\n(worth 10)\n\nCOST = {newCost}"; break;
            case 3:
                _upgradeButton.text = $"CANDY SHEEP\n(worth 100)\n\nCOST = {newCost}"; break;
            case 4:
                _upgradeButton.text = $"MAX LEVEL";
                _upgradeButton.focusable = false; break;
        }

    }

    private void UpdateCount()
    {
        _count.text = GameManager.jumpsCounted.ToString();
    }
}
