using System.Collections.Generic;

public class ComposeSystem
{
    private static bool isInitialized = false;
    private static PlayerInventory inventory;

    private static void Initialize()
    {
        if (inventory == null)
            inventory = GameManager.instance.player.GetComponent<PlayerInventory>();
    }
    
    public static void Compose(ItemTypes type, ComposeData data, int[] indexOfInventory)
    {
        if (!isInitialized)
            Initialize();

        if (type == ItemTypes.Consumable)
            return;

        inventory.Compose(type, data, indexOfInventory);
    }

    public static bool CheckComposable(string materialId)
    {
        if (!isInitialized)
            Initialize();
        if (GetData(materialId) == null)
            return false;
        return true;
    }

    public static bool CheckMaterials(ItemTypes type, string id)
    {
        if (!isInitialized)
            Initialize();
        var data = GetData(id);
        if (data != null)
            return CheckMaterials(type, data);
        return false;
    }

    private static bool CheckMaterials(ItemTypes type, ComposeData data)
    {
        if (!isInitialized)
            Initialize();

        List<string> ids = null;
        switch (type)
        {
            case ItemTypes.Weapon:
                ids = inventory.Weapons;
                break;
            case ItemTypes.Armor:
                ids = inventory.Armors;
                break;
            default:
                return false;
        }

        var len = ids.Count;
        bool[] materials = new bool[2];
        for (int i = 0; i < len; ++i)
        {
            if (!materials[0] && string.Equals(ids[i], data.material1Id))
            {
                materials[0] = true;
                continue;
            }
            if (!materials[1] && string.Equals(ids[i], data.material2Id))
            {
                materials[1] = true;
                continue;
            }
            if (materials[0] && materials[1])
                return true;
        }
        return false;
    }

    public static string Get2ndMaterial(string id)
    {
        if (!isInitialized)
            Initialize();
        var data = GetData(id);
        if (data != null)
            return data.material2Id;
        return null;
    }

    public static ComposeData GetData(string toSynthesisId)
    {
        if (!isInitialized)
            Initialize();
        var table = DataTableMgr.GetTable<ComposeData>().GetTable();
        foreach (var data in table)
        {
            if (string.Equals(data.Value.material1Id, toSynthesisId))
                return data.Value;
        }
        return null;
    }
}
