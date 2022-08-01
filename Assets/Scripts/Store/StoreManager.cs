using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public List<Item> sellingItems = new() {new Item("테스트 아이템 1", 10, "", 2), new Item("테스트 아이템 2", 20, "", 5), new Item("테스트 아이템 3", 50)};
}
