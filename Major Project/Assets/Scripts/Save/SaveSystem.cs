using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using NaughtyAttributes;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private SaveData saveData = new SaveData();

    [SerializeField] private Transform playerTransform;

    [SerializeField] private DialogueController cedric;
    [SerializeField] private DialogueController isabella;
    [SerializeField] private DialogueController simon;
    [SerializeField] private DialogueController edith;
    [SerializeField] private DialogueController marcus;
    [SerializeField] private DialogueController tobias;

    void Start()
    {
        Load();
    }


    public void UpdateAll()
    {
        UpdateInventory();
        UpdateCollectibles();
        UpdateTools();
        UpdatePlayer();
    }

    public void UpdateInventory()
    {
        saveData.InventoryItems = new List<ItemData>(Inventory.Instance.Items.Keys);
        saveData.InventoryItemAmounts = new List<int>(Inventory.Instance.Items.Values);
    }

    public void UpdateCollectibles()
    {
        saveData.CollectibleItems = new List<ItemData>(CollectibleStorage.Instance.Items.Keys);
    }

    public void UpdateTools()
    {
        saveData.ToolItems = new List<ItemData>(ToolStorage.Instance.Items.Keys);
    }

    public void UpdatePlayer()
    {
        saveData.PlayerPosition = GRefs.Instance.PlayerTransform.position;
        saveData.PlayerRotation = GRefs.Instance.PlayerTransform.rotation;
    }

    public void UpdateNPCs()
    {
        saveData.SpokenToCedric = cedric.SpokenToPlayer;
        saveData.SpokenToIsabella = isabella.SpokenToPlayer;
        saveData.SpokenToSimon = simon.SpokenToPlayer;
        saveData.SpokenToEdith = edith.SpokenToPlayer;
        saveData.SpokenToMarcus = marcus.SpokenToPlayer;
        saveData.SpokenToTobias = tobias.SpokenToPlayer;
    }

    [Button]
    public void Save()
    {
        UpdateAll();

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    [Button]
    public void Load()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/savefile.json");
        saveData = JsonUtility.FromJson<SaveData>(json);

        Inventory.Instance.Items.Clear();
        Inventory.Instance.AddItems(saveData.InventoryItems.ToArray(), saveData.InventoryItemAmounts.ToArray());

        CollectibleStorage.Instance.Items.Clear();
        CollectibleStorage.Instance.AddItems(saveData.CollectibleItems.ToArray());

        ToolStorage.Instance.Items.Clear();
        ToolStorage.Instance.AddItems(saveData.ToolItems.ToArray());

        playerTransform.position = saveData.PlayerPosition;
        playerTransform.rotation = saveData.PlayerRotation;

        cedric.SpokenToPlayer = saveData.SpokenToCedric;
        isabella.SpokenToPlayer = saveData.SpokenToIsabella;
        simon.SpokenToPlayer = saveData.SpokenToSimon;
        edith.SpokenToPlayer = saveData.SpokenToEdith;
        marcus.SpokenToPlayer = saveData.SpokenToMarcus;
        tobias.SpokenToPlayer = saveData.SpokenToTobias;
    }

    [Button]
    public void DeleteSave()
    {
        File.Delete(Application.persistentDataPath + "/savefile.json");
    }

    [Button]
    public void PrintDataPath()
    {
        Debug.Log(Application.persistentDataPath);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        Save();
    }

    private void OnApplicationFocus(bool focusStatus)
    {
        Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}