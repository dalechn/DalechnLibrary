using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dalechn
{
    public class RandomTest : MonoBehaviour
    {
        public void Start()
        {
            System. DateTime date1 = System.DateTime.Now;        //localtime              
            System.DateTime date2 = System.DateTime.UtcNow;     //utc time             
            System.DateTime date3 = System.DateTime.Today;
            Debug.Log(date1.ToString("yyyyMMddHH:mm:ss"));          //    2008060606:06:06 
            Debug.Log(date2.ToString("yyyy-MM-dd★HH→mm☆ss"));     //    2008-06-06★06→06☆06 

            //只要确定了seed 每次随机出来的都是一样的(以程序运行一次为单位)
            System.Random random = new System.Random(-20);
            Debug.Log(random.Next(0, 1)); //[m,n)
            Debug.Log(random.Next(0, 100)); //[m,n)

            Random.InitState(-20);
            Debug.Log(Random.Range(0, 100)); //[m,n]
            Debug.Log(Random.Range(0, 10.0f)); //[m,n]
            Debug.Log(Random.value); //[0,1]
            Debug.Log(Random.value); //[0,1]
            Debug.Log(Random.insideUnitSphere); //返回半径为1的球体内的一个随机点。
            Debug.Log(Random.insideUnitCircle); //返回半径为1的圆内的一个随机点。
            Debug.Log(Random.onUnitSphere); //返回半径为1的球体在表面上的一个随机点。
            Debug.Log(Random.rotation);//返回一个随机旋转角度。

        }

        public  void Update()
        {
            HashSet<int> hashSet = new HashSet<int>();
            vFisherYatesRandom vFisher = new vFisherYatesRandom();
            for (int i = 0; i < 20; i++)
            {
                Debug.Log(GameUtils.RandomIntNotRepeat(0, 20, ref hashSet) + " " + vFisher.Next(20));
            }

            Debug.Log("----------------------------------------------");

            // A % B = A - A / B * B =>[0,B)
            // repeat(A,B)  =>[0,B)
            // Repeat(-160,360)  =>-160 - Mathf.Floor(-0.44f)(-1) * 360 =>200
            Debug.Log((160) % -360 + " " + Mathf.Repeat(160, -360)); // 160, 0
            Debug.Log((-160) % 360 + " " + Mathf.Repeat(-160, 360)); // -160,200
            Debug.Log((Time.time - 1000) % 10 + " " + Mathf.Repeat((Time.time - 1000), 10));
        }
    }

    public class UnityCSharpTest : MonoBehaviour
    {
        CSharpTest cSharpTest;

        public void Start()
        {
            cSharpTest = new CSharpTest();
            cSharpTest.Init();
        }

        public void Update()
        {
            cSharpTest.Update();
        }
    }

    public class FloatTest : MonoBehaviour
    {
        public void Start()
        {
            // Mathf.Epsilon = float.Epsilon = 1.401298E-45f
            // Quaternion.kEpsilon = 1E-06F;(10^-6, 0.000006)
            // Vector3.kEpsilon=1E - 05F;
            // Vector3.kEpsilonNormalSqrt = 1E-15F;

            Debug.Log(transform.position + " " + transform.rotation);

            // 低精度下Mathf.Approximately ,和0的判断,== ,<=, >= 都可以使用
            // 高精度下一般使用 Mathf.Abs(a - b) < 1E - 05F)
            Debug.Log(Mathf.Approximately(0, float.Epsilon)); //true

            float a = 0.01f, b = 0.01f;
            Debug.Log(Mathf.Abs(a - b) < 0.001f); //true
            Debug.Log(Mathf.Approximately(a, b)); //true

            a = 6.000025415f; b = 6.000004545f;
            Debug.Log(Mathf.Abs(a - b) < 0.001f); //true
            Debug.Log(Mathf.Approximately(a, b)); //false
            Debug.Log(Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a), Mathf.Abs(b)))); //6.000025E-06

            a = 8f; b = 8.0000004545f;
            Debug.Log(Mathf.Abs(a - b) < 0.001f); //true
            Debug.Log(Mathf.Approximately(a, b)); //true
            Debug.Log(Mathf.Max(Mathf.Abs(a), Mathf.Abs(b))); // 结果是8...离谱
            Debug.Log(Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a), Mathf.Abs(b)))); //8E-06

            a = 0.000001121f; b = 0;
            Debug.Log(Mathf.Abs(a) < 0.001f); //true
            Debug.Log(Mathf.Approximately(a, b)); //false
            Debug.Log(Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a), Mathf.Abs(b)))); //1.121E-12

            a = 0.00003f; b = 0;
            Debug.Log(Mathf.Abs(a) < 0.001f); //true
            Debug.Log(Mathf.Approximately(a, b)); //false
            Debug.Log(Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a), Mathf.Abs(b)))); //3E-11

        }

    }

    public class UnityCoroutineTest : MonoBehaviour
    {
        private float duration = 0;
        private float time = 1f;

        public void Start()
        {
            StartCoroutine(CoroutineTest());
        }

        private IEnumerator CoroutineTest()
        {
            yield return new WaitForSeconds(1f);

            while (duration <= time)
            {
                duration += Time.deltaTime;

                Debug.Log(duration);

                yield return null;
            }
            //while (duration <= 1)
            //{
            //    duration += Time.deltaTime*1/time;

            //    Debug.Log(duration);

            //    yield return null;
            //}
            Debug.Log("over");

            int f = 0;
            while (true)
            {
                // null 或者任意数字会让后续的代码在下一帧执行
                Debug.Log(Time.time);
                yield return null;
                //yield return 0;

                //   return WaitForEndOfFrame 会让后续的代码在 lateUpdtae  以后执行,不用等到下一帧
                Debug.Log(Time.time);
                yield return new WaitForEndOfFrame();

                //   return WaitForEndOfFrame 会让后续的代码在 fixedUpdate  以后执行,下一帧
                Debug.Log(Time.time);
                yield return new WaitForFixedUpdate();

                //   实时挂起时间等于给定时间除以 Time.timeScale
                Debug.Log(Time.time);
                yield return new WaitForSeconds(1f);

                //   无视 Time.timeScale
                Debug.Log(Time.time);
                yield return new WaitForSecondsRealtime(1f);

                f++;
                Debug.Log("【CorTest】Normal 运行一次协程，f值：" + f);
            }
        }
    }

    public class GimbalLockTest: MonoBehaviour
    {
        public void Start()
        {
            // // 直接设定欧拉角(unity旋转顺序world: zxy  local: yxz)
            transform.eulerAngles = new Vector3(0, 90, 0); // 未知: y或者z旋转90度都会触发gimbal lock?
                                                           //transform.localEulerAngles = new Vector3(90f, 180f, -90f);
                                                           //Log: (90,270,0),Inspector:(90,0,90)
                                                           //Debug.Log(transform.localEulerAngles);

            StartCoroutine(ChangeRotation());
        }

        private IEnumerator ChangeRotation()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                var original = transform.eulerAngles;
                original.x += 5f;
                transform.eulerAngles = original;
            }
        }

    }
}

