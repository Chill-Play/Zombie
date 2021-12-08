using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProbabilityHelper 
{
    public static int Choose(IEnumerable<IProbability> probs)
    {
        float total = 0;

        foreach (IProbability elem in probs)
        {
            total += elem.Probability;
        }

        float randomPoint = Random.value * total;
        int i = 0;
        foreach (IProbability elem in probs)
        {           
            if (randomPoint < elem.Probability)
            {
                return i;
            }
            else
            {
                randomPoint -= elem.Probability;
                i++;
            }
        }       
        return i;
    }
}
