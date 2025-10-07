using System.Collections.Generic;
using UnityEngine;

 public class RandomUtils
{
     public List<int> generated = new List<int>();

     public int GenerateRandom(int min, int max)
    {
        if (generated.Count == 0)
        {
            for (int i = min; i <= max; i++)
            {
                generated.Add(i);
            }
        }
        if (generated.Count > 0)
        {

            int number_ = Random.Range(0, generated.Count);
            int r = generated[number_];

            generated.Remove(r);

            return r;

        }
        return 0;
    }

}

