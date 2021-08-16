//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Formation : MonoBehaviour
//{
//    [SerializeField] float unitRadius;
//    [SerializeField] int count;
//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
//    }


//    private void OnDrawGizmosSelected()
//    {
//        for (int i = 0; i < count; i++)
//        {
//            Vector3 pos = GetPosition(i, unitRadius);
//            pos += transform.position;
//            Gizmos.DrawWireSphere(pos, unitRadius);
//        }
//    }
//}
