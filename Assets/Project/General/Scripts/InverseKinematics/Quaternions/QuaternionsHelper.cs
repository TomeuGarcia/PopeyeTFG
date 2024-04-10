using UnityEngine;

namespace Project.Scripts.InverseKinematics.Quaternions
{
    public static class QuaternionsHelper
    {
        public static void ClampBoneRotation(Transform bone, Vector3 clampedAnglesMin, Vector3 clampedAnglesMax, Vector3 clampAxis)
        {
            Quaternion swingLocalRotation = GetSwing(bone.localRotation, clampAxis);

            Quaternion clampedLocalRotation = GetClampedQuaternion(swingLocalRotation, clampedAnglesMin, clampedAnglesMax);
            
            bone.localRotation = clampedLocalRotation;
        }
        
        public static Quaternion GetTwist(Quaternion rotation , Vector3 twistAxis)
        {
            return new Quaternion(rotation.x * twistAxis.x, rotation.y * twistAxis.y, rotation.z * twistAxis.z, rotation.w);
        }

        public static Quaternion GetSwing(Quaternion rotation, Vector3 twistAxis)
        {
            return rotation * Quaternion.Inverse(GetTwist(rotation, twistAxis));
        }

        public static Quaternion GetClampedQuaternion(Quaternion q, Vector3 minBounds, Vector3 maxBounds)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
            angleX = Mathf.Clamp(angleX, minBounds.x, maxBounds.x);
            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);
            angleY = Mathf.Clamp(angleY, minBounds.y, maxBounds.y);
            q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);

            float angleZ = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.z);
            angleZ = Mathf.Clamp(angleZ, minBounds.z, maxBounds.z);
            q.z = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleZ);
            
            return q;
        }
    }
}