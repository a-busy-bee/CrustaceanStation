using System;
using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class TrainController : TransportController
{
    public Cart.Type GetRandomCartType() // get a random cart type when instantiating the ticket
    {
        int random = UnityEngine.Random.Range(0, 7);
        if (random <= 1) return Cart.Type.Deluxe;
        else return Cart.Type.Economy;
        
    }
}
