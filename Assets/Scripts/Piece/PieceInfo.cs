using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PieceInfo : MonoBehaviour
{
    [SerializeField] PieceColor myColor;
    public PieceColor MyColor { get { return myColor; } }
    public PieceType MyType { get; set; } = PieceType.Man;
}
