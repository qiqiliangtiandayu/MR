using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public enum ItemType
{
    Tool,
    Consumable,
    Environment
}
// 物品数据类示例
[System.Serializable]
public class Item
{


    public ItemType type;
    public bool CanPickUp => type != ItemType.Environment;
    public bool CanEquip => type != ItemType.Consumable;
    public bool CanBeStoredInBag => type == ItemType.Consumable;
}
