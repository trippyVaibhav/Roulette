using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    [SerializeField]
    internal Transform parent_Transform;
    [SerializeField]
    private UIManager uiManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.gameObject.transform.SetParent(parent_Transform);
        this.gameObject.transform.localPosition = new Vector2(0, 0);
        StartCoroutine(uiManager.StopAtNumber());
    }
}
