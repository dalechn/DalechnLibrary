﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CJTools
{
    public static partial class GameUtils
    {

        public static T RandomListNotRepeat<T>(List<T> list, HashSet<int> set)
        {
            if (list.Count != 0)
            {
                int n = RandomIntNotRepeat(0, list.Count, set);

                T child = list[n];
                return child;
            }
            return default(T);
        }

        private static bool IsExit(HashSet<int> set, int value)
        {
            if (!set.Contains(value))
            {
                set.Add(value);
                return false;
            }

            return true;
        }

        private static int RandomIntNotRepeat(int min, int max, HashSet<int> set)
        {
            if (max - min == set.Count)
            {
                set.Clear();
            }

            int num = UnityEngine.Random.Range(min, max);
            while (IsExit(set, num))
            {
                num = UnityEngine.Random.Range(min, max);
            }
            return num;
        }

        public static Vector3 Range(Vector3 min, Vector3 max)
        {
            float x = UnityEngine.Random.Range(min.x, max.x);
            float y = UnityEngine.Random.Range(min.y, max.y);
            float z = UnityEngine.Random.Range(min.z, max.z);
            Vector3 temp = new Vector3(x, y, z);
            return temp;
        }


    }

    public class vFisherYatesRandom
    {
        private int[] randomIndices = null;
        private int randomIndex = 0;
        private int prevValue = -1;

        public int Next(int len)
        {
            if (len <= 1)
                return 0;

            if (randomIndices == null || randomIndices.Length != len)
            {
                randomIndices = new int[len];
                for (int i = 0; i < randomIndices.Length; i++)
                    randomIndices[i] = i;
            }

            if (randomIndex == 0)
            {
                int count = 0;
                do
                {
                    for (int i = 0; i < len - 1; i++)
                    {
                        int j = Random.Range(i, len);
                        if (j != i)
                        {
                            int tmp = randomIndices[i];
                            randomIndices[i] = randomIndices[j];
                            randomIndices[j] = tmp;
                        }
                    }
                } while (prevValue == randomIndices[0] && ++count < 10); // Make sure the new first element is different from the last one we played
            }

            int value = randomIndices[randomIndex];
            if (++randomIndex >= randomIndices.Length)
                randomIndex = 0;

            prevValue = value;
            return value;
        }

        public int Range(int min, int max)
        {
            var len = (max - min) + 1;
            if (len <= 1)
                return max;

            if (randomIndices == null || randomIndices.Length != len)
            {
                randomIndices = new int[len];
                for (int i = 0; i < randomIndices.Length; i++)
                    randomIndices[i] = min + i;
            }

            if (randomIndex == 0)
            {
                int count = 0;
                do
                {
                    for (int i = 0; i < len - 1; i++)
                    {
                        int j = Random.Range(i, len);
                        if (j != i)
                        {
                            int tmp = randomIndices[i];
                            randomIndices[i] = randomIndices[j];
                            randomIndices[j] = tmp;
                        }
                    }
                } while (prevValue == randomIndices[0] && ++count < 10); // Make sure the new first element is different from the last one we played
            }

            int value = randomIndices[randomIndex];
            if (++randomIndex >= randomIndices.Length)
                randomIndex = 0;

            prevValue = value;
            return value;
        }
    }

}