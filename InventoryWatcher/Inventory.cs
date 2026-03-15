using System;
using System.Collections.Generic;
using System.Text;

class Inventory
{
    public event Action<string, int, int> ItemChanged;

    private Dictionary<string, int> items = new Dictionary<string, int>();

    public void AddItem(string name, int count)
    {
        if (count < 0)
        {
            return;
        }

        int temp = 0;

        if (items.TryGetValue(name, out int curr))
        {
            temp = curr;
        }

        int add = temp + count;

        items[name] = add;
        ItemChanged?.Invoke(name, temp, add);
    }

    public void RemoveItem(string name, int count)
    {
        if (count < 0)
        {
            return;
        }

        if (!items.TryGetValue(name, out int currentCount))
        {
            return;
        }

        int temp = currentCount;
        int remove = temp - count;

        if (remove <= 0)
        {
            remove = 0;
            items.Remove(name);
        }
        else
        {
            items[name] = remove;
        }

        ItemChanged?.Invoke(name, temp, remove);
    }
}

class InventoryUI
{
    public void HandleInventoryUI(string name, int before, int changed)
    {
        Console.WriteLine($"[UI] {name}: {before} → {changed}");
    }
}

class AutoBuyer
{
    public void AutoBuy(string name, int before, int changed)
    {
        if (changed == 0)
        {
            Console.WriteLine($"[자동구매] {name} 재고 소진! 자동 구매 요청");
        }
    }
}
