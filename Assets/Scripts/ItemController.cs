using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    // fields
    [SerializeField] private float waitForHide;

    // Components
    private ItemsManager itemsManager;

    // Start is called before the first frame update
    void Start()
    {
        itemsManager = FindAnyObjectByType<ItemsManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            itemsManager.CatchIt(this);
    }
}
