using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Floor2Script : MonoBehaviour
{
    public GameObject Floor2;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bool isActive = Floor2.activeSelf;
            Floor2.SetActive(!isActive);
        }
    }
}
