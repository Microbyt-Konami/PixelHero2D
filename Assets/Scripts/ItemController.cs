using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    // fields
    [SerializeField] private float waitForHide;

    // Components
    private SpriteRenderer sr;
    private Transform transformItem;
    private ItemsManager itemsManager;
    private SaveDataGame saveDataGame;

    // Variables
    private bool isHiding;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        transformItem = GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        itemsManager = FindAnyObjectByType<ItemsManager>();
        saveDataGame = FindAnyObjectByType<SaveDataGame>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isHiding)
            StartCoroutine(CatchIt());
    }

    private IEnumerator CatchIt()
    {
        if (!itemsManager.CatchIt(this))
            yield break;

        saveDataGame.ItemsGOCatched.Add(gameObject.name);
        isHiding = true;

        var position = transformItem.position;
        var color = sr.color;
        var _waitForHide = waitForHide;

        while (waitForHide > 0)
        {
            waitForHide -= Time.deltaTime;
            transformItem.position = position + (2 - 2 * Mathf.Lerp(0, _waitForHide, waitForHide)) * Vector3.up;
            sr.color = new Color(color.r, color.g, color.b, Mathf.Lerp(0, _waitForHide, waitForHide));

            yield return null;
        }
        Destroy(gameObject);
    }
}
