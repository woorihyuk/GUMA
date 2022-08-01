using System;

[Serializable]
public class ItemData
{
    public string name; // 이름
    public int cost; // 비용
    public string description; // 설명
    public int maxQuantity; // 최대 구매가능한 수량
    public int currentQuantity; // 현재 구매가능한 수량
}

public class Item
{
    public string name; // 이름
    public int cost; // 비용
    public string description; // 설명
    public int currentQuantity; // 현재 구매가능한 수량

    public Item(string name, int cost = 0, string description = "", int currentQuantity = 1)
    {
        this.name = name;
        this.cost = cost;
        this.description = description;
        this.currentQuantity = currentQuantity;
    }
}