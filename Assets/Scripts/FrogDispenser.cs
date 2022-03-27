using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrogDispenser : MonoBehaviour
{
    public GameObject toDispense;

    public void DispenseFrog()
    {
        GameObject dispensed = Instantiate(toDispense);
        dispensed.transform.position = this.transform.position;
    }
}
