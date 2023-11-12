using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KosciachTools.TransformUtility
{
    public static class KosciachTransformUtility
    {
        //Pos
        public static void SetPosX(Transform transform, float posX)
        {
            Vector3 pos = transform.position;
            pos.x = posX;
            transform.position = pos;
        }
        public static void SetPosY(Transform transform, float posY)
        {
            Vector3 pos = transform.position;
            pos.y = posY;
            transform.position = pos;
        }
        public static void SetPosZ(Transform transform, float posZ)
        {
            Vector3 pos = transform.position;
            pos.z = posZ;
            transform.position = pos;
        }
        public static void SetPosXLocal(Transform transform, float posX)
        {
            Vector3 pos = transform.localPosition;
            pos.x = posX;
            transform.localPosition = pos;
        }
        public static void SetPosYLocal(Transform transform, float posY)
        {
            Vector3 pos = transform.localPosition;
            pos.y = posY;
            transform.localPosition = pos;
        }
        public static void SetPosZLocal(Transform transform, float posZ)
        {
            Vector3 pos = transform.localPosition;
            pos.z = posZ;
            transform.localPosition = pos;
        }


        //Scale
        public static void SetScaleXLocal(Transform transform, float scaleX)
        {
            Vector3 scale = transform.localScale;
            scale.x = scaleX;
            transform.localScale = scale;
        }
        public static void SetScaleYLocal(Transform transform, float scaleY)
        {
            Vector3 scale = transform.localScale;
            scale.y = scaleY;
            transform.localScale = scale;
        }
        public static void SetScaleZLocal(Transform transform, float scaleZ)
        {
            Vector3 scale = transform.localScale;
            scale.z = scaleZ;
            transform.localScale = scale;
        }


        //Rotation
        public static void SetRotX(Transform transform, float rotX)
        {
            Vector3 rot = transform.eulerAngles;
            rot.x = rotX;
            transform.eulerAngles = rot;
        }
        public static void SetRotY(Transform transform, float rotY)
        {
            Vector3 rot = transform.eulerAngles;
            rot.y = rotY;
            transform.eulerAngles = rot;
        }
        public static void SetRotZ(Transform transform, float rotZ)
        {
            Vector3 rot = transform.eulerAngles;
            rot.z = rotZ;
            transform.eulerAngles = rot;
        }
        public static void SetRotXLocal(Transform transform, float rotX)
        {
            Vector3 rot = transform.localEulerAngles;
            rot.x = rotX;
            transform.localEulerAngles = rot;
        }
        public static void SetRotYLocal(Transform transform, float rotY)
        {
            Vector3 rot = transform.localEulerAngles;
            rot.y = rotY;
            transform.localEulerAngles = rot;
        }
        public static void SetRotZLocal(Transform transform, float rotZ)
        {
            Vector3 rot = transform.localEulerAngles;
            rot.z = rotZ;
            transform.localEulerAngles = rot;
        }
    }
}