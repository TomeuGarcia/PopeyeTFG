using DG.Tweening;
using UnityEngine;

namespace Popeye.Scripts.TransformUtilities
{
    public interface IMotionBehaviour
    {
        Vector3 Position { get; }
        Vector3 Forward { get; }
        Quaternion Rotation { get; }


        void SetPosition(Vector3 position);
        void SetLocalPosition(Vector3 position);

        public void SetRotation(Quaternion rotation);

        void MoveByDisplacement(Vector3 displacement, float duration, Ease ease = Ease.Linear);

        void MoveToPosition(Vector3 position, float duration, Ease ease = Ease.Linear);
        void MoveToPosition(Vector3 position, float duration, AnimationCurve ease);

        public void MoveAlongPath(Vector3[] path, float duration, Ease ease = Ease.Linear);
        public void MoveAlongPath(Vector3[] path, float duration, AnimationCurve ease);

        public void MoveAndRotateAlongPath(Vector3[] positionPath, Quaternion[] rotationPath, float duration, AnimationCurve ease);


        public void Rotate(Quaternion endRotation, float duration, Ease ease = Ease.Linear);

        public void RotateStartToEnd(Quaternion startRotation, Quaternion endRotation, float duration,
            AnimationCurve ease);

        public void RotateStartToEnd(Quaternion startRotation, Quaternion endRotation, float duration,
            Ease ease = Ease.Linear);


        public void CancelMovement();

        public void Parent(Transform parent);
        public void Unparent();
        public void ParentAndReset(Transform parent, float duration, Ease ease = Ease.Linear);
        public void ParentAndResetInstantly(Transform parent);

        public void ParentAndUpdate(Transform parent, Vector3 localPosition, Quaternion localRotation,
            float duration, Ease ease = Ease.Linear);

        public void SetLocalScale(Vector3 localScale);
        public void ResetScale();
    }
}