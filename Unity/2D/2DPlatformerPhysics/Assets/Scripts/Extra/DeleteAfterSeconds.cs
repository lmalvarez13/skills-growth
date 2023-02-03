using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAfterSeconds : MonoBehaviour
{
    [SerializeField] float seconds = -1f;

    private void Awake() {
        Invoke(nameof(Delete), seconds);
    }

    private void Delete()
    {
        Destroy(gameObject);
    }
}
