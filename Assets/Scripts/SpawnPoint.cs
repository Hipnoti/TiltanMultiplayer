﻿using UnityEngine;

    public class SpawnPoint : MonoBehaviour
    {
        public int ID = 0;
        private bool isTaken = false;
    
        public bool IsTaken 
        {
            get { return isTaken; }
        }

        public void Take()
        {
            isTaken = true;
        }
        
        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 1.0f);
        }
    }
    