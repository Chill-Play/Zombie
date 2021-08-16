using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        //FindObjectOfType<UIEnemyHealthBars>().CreateHealthBar(this);
    }

    // Update is called once per frame
    void OnDisable()
    {
        //UIEnemyHealthBars bars = FindObjectOfType<UIEnemyHealthBars>();
        //if (bars != null)
        //{
        //    bars.RemoveHealthBar(this);
        //}
    }
}
