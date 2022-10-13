using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyShop
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        protected virtual void Awake()
        {
            Instance = this;
        }

        void Start()
        {

        }

        void Update()
        {

        }
    }
}