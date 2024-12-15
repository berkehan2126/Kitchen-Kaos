using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private GameObject clearCounter;
    private void Start()
    {
        Instantiate(clearCounter).GetComponent<ClearCounter>().TestFunc();
    }





}
