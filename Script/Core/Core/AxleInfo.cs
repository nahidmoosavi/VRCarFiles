using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core
{
    /// <summary>
    /// This class holds information about a car's axle
    /// </summary>
    [System.Serializable]
    public class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;

        public bool canSteer;
        public bool canAccelerate;
    }
}
