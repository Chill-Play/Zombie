using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class UIScreen : MonoBehaviour
{
    [SerializeField] SubjectId id;

    public SubjectId Id => id;
}
