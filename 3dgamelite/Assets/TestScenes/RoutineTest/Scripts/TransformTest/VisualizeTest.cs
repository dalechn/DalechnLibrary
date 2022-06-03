using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dalechn
{
    public partial class TansformTest
    {
        private void Move(float step, float fractionOfJourney)
        {
            //-------------------------------直线运动

            tempPos = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
            //// 线性变化
            //tempPos = Vector3.MoveTowards(startMarker.position, endMarker.position, fractionOfJourney);
            //// 非匀减速曲线 无法使用
            //tempPos = Vector3.SmoothDamp(startMarker.position, endMarker.position, ref velocity, smoothDampTime);
            //// sininout
            //tempPos = new Vector3(Mathf.SmoothStep(startMarker.position.x, endMarker.position.x, fractionOfJourney), transform.position.y, transform.position.z);

            //// 非匀减速曲线 可以和lerp或者 movetoward同时用?
            //tempPos = Vector3.SmoothDamp(tempPos, endMarker.position, ref velocity, smoothDampTime);
            ////匀减速
            //tempPos = Vector3.Lerp(tempPos, endMarker.position, step);
            //tempPos = Vector3.Slerp(tempPos, endMarker.position, step);

            //// 线性变化 
            //tempPos = Vector3.MoveTowards(tempPos, endMarker.position, step);
            //// sininout 
            //tempPos = new Vector3(Mathf.SmoothStep(tempPos.x, endMarker.position.x, step), tempPos.y, tempPos.z);

            tempPos = new Vector3(Mathf.Repeat(fractionOfJourney, 3), tempPos.y, tempPos.z);
            //tempPos = new Vector3(Mathf.PingPong(fractionOfJourney, 3), tempPos.y, tempPos.z);

            transform.position = tempPos;

        }


        private void CurveMove(float step, float fractionOfJourney)
        {

            ////-----------------------------------ClampMagnitude
            //Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //Vector3 newPos = transform.position + movement;

            //Vector3 offset = newPos - polygonCenter;
            //transform.position = polygonCenter + Vector3.ClampMagnitude(offset, polygonRadius);

            // //----------------------------------圆周运动
            if (fractionOfJourney >= Mathf.PI * 2f) fractionOfJourney -= Mathf.PI * 2f;
            // 3个轴只有一个sin或者cos做来回运动 , 有至少一个sin和cos做圆周运动
            Vector3 startCorner = new Vector3(-polygonRadius, 0, -polygonRadius) + startMarker.position; // 把初始位置偏移,因为cos(0) = 1;
            transform.position = startCorner + new Vector3(
                 Mathf.Cos(fractionOfJourney) * polygonRadius,
                 Mathf.Sin(fractionOfJourney) * polygonRadius,
                 Mathf.Cos(fractionOfJourney) * polygonRadius);

            ////------------------------------------ 正弦/余弦运动
            //transform.position = startMarker.position + new Vector3(
            //    fractionOfJourney * polygonRadius,
            //    Mathf.Sin(fractionOfJourney * Mathf.PI) * polygonRadius,
            //    fractionOfJourney * polygonRadius);

            Vector3 previousCorner = startMarker.position;
            Vector3 currentCorner = startMarker.position;
            for (int i = 1; i < numberOfSides; i++)
            {
                float cornerAngle = 2f * Mathf.PI / (float)numberOfSides * i;

                currentCorner = startCorner + new Vector3(
                    Mathf.Cos(cornerAngle) * polygonRadius,
                    Mathf.Sin(cornerAngle) * polygonRadius,
                Mathf.Cos(cornerAngle) * polygonRadius);

                Debug.DrawLine(currentCorner, previousCorner);

                previousCorner = currentCorner;
            }

            Debug.DrawLine(startMarker.position, previousCorner);

        }

    }

}
