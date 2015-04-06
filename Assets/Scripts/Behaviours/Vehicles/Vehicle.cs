﻿using UnityEngine;
using VehicleDef = SanAndreasUnity.Importing.Items.Definitions.Vehicle;

namespace SanAndreasUnity.Behaviours.Vehicles
{
    public partial class Vehicle : MonoBehaviour
    {
        public VehicleDef Definition { get; private set; }

        public Transform DriverTransform
        {
            get { return GetPart("ped_frontseat"); }
        }

        private void Update()
        {
            foreach (var wheel in _wheels)
            {
                Vector3 position = wheel.Collider.transform.position;

                WheelHit wheelHit;

                if (wheel.Collider.GetGroundHit(out wheelHit))
                {
                    position.y = wheelHit.point.y + wheel.Collider.radius;
                }
                else
                {
                    position.y -= wheel.Collider.suspensionDistance;
                }

                wheel.Child.transform.position = position;

                // reset the yaw
                wheel.Child.localRotation = wheel.Roll;

                // calculate new roll
                wheel.Child.Rotate(Vector3.right, wheel.Collider.rpm / 60.0f * 360.0f * Time.deltaTime);
                wheel.Roll = wheel.Child.localRotation;

                // apply yaw
                wheel.Child.localRotation = Quaternion.AngleAxis(wheel.Collider.steerAngle, Vector3.up) * wheel.Roll;
            }
        }
    }
}
