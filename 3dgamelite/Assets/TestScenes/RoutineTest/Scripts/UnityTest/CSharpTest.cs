using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices; //dll
using UnityEngine;
using UnityEngine.Events;

namespace Dalechn
{

    public interface IInterfaceTest
    {
        //object Current { get; }
        //bool MoveNext();
        //void Reset();

        event EventHandler OnDataUpdate;
        event EventHandler InternalOnDataUpdate;
    }

    internal static class StaticClass<T>{}

    public partial class CSharpTest{}

    public abstract class CSharpTestBase
    {
        abstract public void Init();
    }

    public enum EEnumTest { ENUMTEST}

    public struct SStructTest
    {
        int structTest;
    }

    // 命名空间下的class/struct/interface前面只能加public或者不加,类内定义的class /struct/interface无限制
    // 结构体(struct)是数值型,在迭代器里会有各种限制,无法初始化
    // 类是(class)引用型
    // 接口(interface)无法创建成员变量,不需要加abstract/virtual,不可以加static,默认public(无法修改访问限制)
    // 抽象类(abstract class)抽象成员必须加abstract,static不可以加abstract/virtual
    public sealed partial class CSharpTest : CSharpTestBase, IInterfaceTest
    {
        //接口带event的实现方式
        public event EventHandler OnDataUpdate;

        event EventHandler IInterfaceTest.InternalOnDataUpdate
        {
            add
            {
                OnDataUpdate += value;
            }
            remove
            {
                OnDataUpdate -= value;
            }
        }

        //1. public 访问不受限制
        //2. protected 类内和子类访问
        //3. internal 当前程序集/命名空间内访问
        //4. protected internal 当前程序集/命名空间 类内和子类访问
        //5. private 类内访问
        public readonly RangeFloat rangeTest;
        protected internal static readonly RangeFloat3 range3Test; // 修饰任意类型,运行时确定值,无法在inspector显示 //未知:不知道存在哪?
        internal const int testConst = 0; // 只能修饰基本类型 ,编译时确定值,无法在inspector显示

        //Expression-bodied members
        public void Command() => CPPTest();
        public void Command1(int a) => Debug.Log(a);
        public string ID => "man";
        // 和上面写法都相当于只读属性
        //public string ID { get; } = "man";


        [DllImport("wintest", EntryPoint = "Prime")]
        public static extern bool Prime(int num);

        //参数类型和参数数量都可以不匹配
        [DllImport("wintest", EntryPoint = "RefTest")]
        unsafe public static extern void RefTest(float val1, int* val2);

        unsafe private void CPPTest()
        {
            Debug.Log(Prime(2));
            float val1 = 1;
            int val2 = 2;
            RefTest(val1, &val2);
            Debug.Log(val1);
        }

        private  void InOutRefTest(in int test1, out int test2, ref int test3)
        {
            //test1 = 3;
            test2 = 4;
            test3 = 5;
        }

        private void DataTypeTest()
        {
            //可空类型(nullable)
            int? num1 = null;
            int? num2 = 45;
            int itest = new int();//初始化为0
            double? num3 = new double?();
            double? num4 = 3.14157;

            bool? boolval = new bool?();

            // 值类型(value type)
            bool aBool = true; //8位无符号      //Boolean
            char aChar = 'a'; //16位                 //Char

            int aInt = 0; // 32位                        //Int32
            long aLong = 0L; // 64位               //Int64
            short aShort = 0; //16位                 //Int16
            sbyte aSbyte = 0; //8位有符号                  //SByte

            uint aUint = 1;   //32 位无符号         //UInt32
            ulong aUlong = 1L; //64 位无符号    //UInt64
            ushort aUshort = 1; //16 位无符号   //UInt16
            byte aByte = 0;// 8位无符号         //Byte

            float aFloat = 0f;// 32位                    //Single
            double aDouble = 0d; // 64位             //Double
            decimal aDecimal = 0m; // 128位      //Decimal

            //引用类型(reference type)

            object obj = 0;//装箱                         //Object
            object obj3 = aInt; //隐式装箱              
            int int1 = (int)obj; //拆箱
            object obj4 = 1;
            object obj2 = obj4 ?? obj;
            //COALESCE 表达式相当于
            //obj2 = obj4 != null ? obj4 : obj;
            //  空合并运算符为右结合运算符，即操作时从右向左进行组合的。如，“a ?? b ?? c”的形式按“a ?? (b ?? c)”
            Debug.Log(obj3 + " " + obj2);

            //会查询暂存池(托管堆中)
            //1.利用字面量值创建string对象:
            string str1 = @"999";                        //String
            string str2 = str1 + str1;

            //2.利用string.Intern()创建string对象:
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder("777");
            string str3 = string.Intern(ToString());
            string str4 = string.Intern(sb1.ToString());

            //猜测:只要是使用了toString()就不会查询暂存池
            string str5 ="888"+ str3.ToString();

            //@代表不解释转义字符
            //  \' 单引号 \f 换页 \" 双引号 \n 新行 \\ 反斜杠 \r 回车 \0 空字符 
            //  \t 水平制表符 \a 警告(产生蜂鸣) \v垂直制表符 \b退格

            //猜测:数组为Array类型?
            int[] myArray1 = new int[4];
            int[] myArray2 = new int[4] { 1, 3, 5, 7 };
            int[] myArray3 = new int[] { 1, 3, 5, 7 };
            int[] myArray4 = { 1, 3, 5, 7 };

            int[,] myArray2d1 = new int[5,3];
            int[,] myArray2d2 = new int[,] { { 1, 2 }, { 3, 4}, { 5 ,6}, { 7 ,8} };
            int[][] myArray2d4 = new int[4][];

            dynamic dy; //c# 4.0(动态编译,不需要初始化)
            var val = 0;        //c#3.0(静态编译,需要初始化)

            int intSize = sizeof(int);
            string typeName = nameof(dy);
        }

        private void DataStructTest()
        {
            //未知:暂时没用过?
            BitArray ba1 = new BitArray(8);
            BitArray ba2 = new BitArray(8);
     

            // 有装箱
            ArrayList arrayList = new ArrayList() { 1, 2, 3, 4 };

            {
                // 无装箱
                List<Person> intList = new List<Person>();

                var fa = new Person("Fa", 45, "man");
                intList.Add(fa);
                Debug.Log("capacity:" + intList.Capacity);
                intList.Add(new Person("I", 26, "man"));
                intList.Add(new Person("Ma", 46, "woman"));
                intList.Add(new Person("Sis", 12, "woman"));
                intList.Add(new Person("Bro", 9, "man"));
                Debug.Log("capacity:" + intList.Capacity);

                intList[1].age = 100;

                for (int i = intList.Count - 1; i >= 0; i--)
                {
                    if (intList[i].age == 26)
                        intList.Remove(intList[i]);
                }

                var other = new Person("Other", 45, "woman");
                intList.Insert(0, other);

                //性能:Contains > Exists > Where > Any
                Debug.Log(intList.Contains(fa));
                Debug.Log(intList.Exists(e => e.age == 45));
                Debug.Log(intList.Where(e => e.age == 45).Any());
                Debug.Log(intList.Any(e => e.age == 45));

                Person findVal = intList.Find(e => e.age == 45);
                findVal = intList.FindLast(e => e.age == 45); // 找到最后一个
                                                              //intList.Remove(findVal); //第二种删除方式


                List<Person> findVals = intList.FindAll(e => e.age == 45);
                int findIndex = intList.FindIndex(e => e.age == 45);
                findIndex = intList.FindLastIndex(e => e.age == 45);

                //intList.Sort(new People.TestComparer());
                //intList.Sort(delegate (People a, People b) { return a.age.CompareTo(b.age); });
                intList.Sort((a, b) => a.age.CompareTo(b.age));

                foreach (var val in intList)
                {
                    Debug.Log(val.pName + " " + val.age);
                }

            }

            LinkedList<int> linkedList = new LinkedList<int>();

            //先进先出
            Queue<string> queue = new Queue<string>();
            //先进后出
            Stack<string> stack = new Stack<string>();

            //不允许重复
            HashSet<string> hashSet = new HashSet<string>();
            hashSet.Add("123");

            //有装箱操作
            Hashtable table = new Hashtable();
            table.Add(1, "table");
            Hashtable.Synchronized(table);//线程安全,只有一个线程写  多个线程读

            //字典不是线程安全 
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(1, "dic");

            //  不允许重复
            SortedSet<string> sortedSet = new SortedSet<string>();
            sortedSet.Add("123");

            // 有装箱操作 不允许重复
            SortedList sortedList = new SortedList();//IComparer
            sortedList.Add(1, "sortedList");
            sortedList.Add(0, "sortedList");

            // 不允许重复
            SortedDictionary<int, string> sortedDic = new SortedDictionary<int, string>();
            sortedDic.Add(1, "sortedDic");
            sortedDic.Add(0, "sortedDic");

            //foreach (DictionaryEntry val in sortedList)
            //{
            //    Debug.Log(val.Key + " " + val.Value);
            //}
            //ConcurrentQueue 线程安全版本的Queue
            //ConcurrentStack 线程安全版本的Stack
            //ConcurrentDictionary 线程安全版本的Dictionary
        }


        [System.Serializable]
        public class UnityEventInt : UnityEvent<int> { }
        public UnityEventInt unityEventInt { get; set; }
        public UnityEvent unityEvent { get; set; }


        public delegate void TestDelegate();

        public TestDelegate delTest;
        public event TestDelegate eventTest;

        public Func<int, int> funcTest; //最后一个参数为out
        public Action<int, int> actTest;
        public UnityAction<int> unityAction;

        private void TestDelgate()
        {
            //在类的外部，事件只能用“+=”和“-=”去订阅/取消订阅，如果是委托的话还可以使用“=”
            eventTest = () => { Debug.Log("testEvent1"); };
            eventTest += delegate () { Debug.Log("testEvent2"); };

            delTest = () => { Debug.Log("delTest1"); };
            delTest += delegate () { Debug.Log("delTest2"); };

            unityAction = (a) => { Debug.Log("unityAction1"); };
            //unityAction = a => { Debug.Log("unityAction1"); };
            unityAction += delegate (int a) { Debug.Log("unityAction2"); };

            unityEvent?.AddListener(() => { Debug.Log("unityEvent1"); });
            unityEvent?.AddListener(delegate () { Debug.Log("unityEvent2"); });

            unityEventInt?.AddListener(unityAction);

            actTest += (int a, int b) => { Debug.Log("actTest"); };
            funcTest += new Func<int, int>(FuncTest);

            delTest?.Invoke();
            eventTest?.Invoke();
            unityAction?.Invoke(1);
            unityEvent?.Invoke();
            unityEventInt?.Invoke(1);

            actTest.Invoke(0, 0);
            funcTest.Invoke(0);
        }

        private int FuncTest(int inVal)
        {
            Debug.Log("funcTest");

            return 0;
        }


        private void IEnumeratorTest()
        {
            PeopleList<int> peopleList = new PeopleList<int>();
            
            for (int i = 0; i < 10; i++)
            {
                peopleList.AddPeople(i);
            }

            //IEnumerator iEnum = peopleList.GetEnumerator();
            IEnumerator iEnum = IEnumerableTest().GetEnumerator();
            while (iEnum.MoveNext())
            {
                Debug.Log(iEnum.Current);
            }

            for (int i = 0; i < peopleList.peopleList.Count; i++)
            {
                Debug.Log(peopleList[i]);
            }

            foreach (var val in peopleList)
            {
                Debug.Log(val);
            }

        }

        //猜测:返回IEnumerable或者IEnumerator 取决于上层是否使用foreach
        //IEnumerator<Vector3> IEnumeratorTest()
        //IEnumerable<Vector3> IEnumerableTest()
        // IEnumerator IEnumeratorTest()
        private IEnumerable IEnumerableTest()
        {
            for (var i = 0; i < 100; i++)
            {
                if(i>30)
                {
                    yield break;
                }
                yield return i;
            }
        }


        public CSharpTest() { }


        // overload(重载)是不同参数的同名func
        // overwrite(重写,覆盖,隐藏), 使用new 关键字或者不写 当父类引用调用此方法时还是调用的父类,父类不一定需要virtual
        // override(重写,覆盖),父类需要virtual
        // sealed密封类
        public sealed override void Init()
        {
            //DataTypeTest();

            //DataStructTest();

            //TestDel();

            //int test1 = 0; // in 必须初始化,方法内不可修改
            //int test2; // out 无需初始化,方法内必须修改
            //int test3 =2; // ref 必须初始化,无限制
            //InOutRefTest(test1,out test2,ref test3);
            //Debug.Log(test1+" "+test2+" "+test3);

            //CPPTest();

            IEnumeratorTest();
        }

        private void TimeTest()
        {

        }

        public void Update()
        {
            //TimeTest();
        }

        //public static void Main(string[] args)
        //{
        //    CSharpTest cSharpTest = new CSharpTest();
        //    cSharpTest.Init();

        //    Console.WriteLine("Hello CSharp");
        //}

    }


    public class Person
    {
        public string pName { get; protected set; }
        public string sex { get; set; }

        public int age   // 属性:attribute (带get或set的field)
        {
            //get { return m_age; }
            //set { m_age = value; }

            get => m_age;
            set => age = value;
        }

        private int m_age; // 字段:field 最好不为public


        public Person(string name, int age, string sex)
        {
            this.pName = name;
            this.age = age;
            this.sex = sex;
        }

        public class TestComparer : IComparer<Person>
        {
            public int Compare(Person p1, Person p2)
            {
                return p1.age.CompareTo(p2.age);

                // 默认升序排列, CompareTo原理:
                //当p1>p2时，return 1
                //当p1 = p2时，return 0
                //当p1 < p2时，return -1
            }
        }

    }

    //未知:泛型问题很大?
    //泛型化接口,避免装箱
    public class PeopleList<T> : IEnumerable<T>, IEnumerable //枚举器
    {
        public List<T> peopleList = new List<T>();

        public T this[int index] => peopleList[index];

        public void AddPeople(T stu)
        {
            peopleList.Add(stu);
        }

         IEnumerator<T> IEnumerable<T>.GetEnumerator()//显式接口实现,只能由接口访问 
        {
            //foreach (var item in peopleList)
            //{
            //    yield return item;
            //}

            return new PeopleEnumerator(peopleList);
        }

        public IEnumerator GetEnumerator() //迭代器,隐式接口实现
        {
            return GetEnumerator();
        }

        //也可以由PeopleList实现IEnumerator
        public class PeopleEnumerator : IEnumerator<T>, IEnumerator, IDisposable
        {
            private List<T> peopleList;
            private int _position = -1;
            public PeopleEnumerator(List<T> peopleList)
            {
                this.peopleList = peopleList;
            }

            ~PeopleEnumerator()
            {
                Dispose(disposing: false);
            }

            public void Dispose()
            {
                Dispose(disposing: true);
                GC.SuppressFinalize(this); // 请求公共语言运行时不要调用该对象上的终结器（在C#中就是指不要调用析构函数）
            }

            private bool disposedValue = false;
            protected virtual void Dispose(bool disposing)
            {
                if (!this.disposedValue)
                {
                    if (disposing)
                    {
                        // Release managed resources
                    }

                    // Release unmanaged resources
                }

                this.disposedValue = true;
            }


            public T Current
            {
                get
                {
                    if (_position == -1|| _position >= peopleList.Count)
                    {
                        throw new InvalidOperationException();
                    }
                   
                    return peopleList[_position];
                }
            }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (_position < peopleList.Count - 1)
                {
                    _position++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void Reset()
            {
                _position = -1;
            }
        }

    }

}

//public class Singleton<T> : MonoBehaviour where T : Singleton<T>
//{
//    public static T Instance { get; private set; }

//    protected virtual void Awake()
//    {
//        Instance = (T)this;

//        //if (Instance == null)
//        //{
//        //    Instance = (T)this;
//        //}
//        //else
//        //{
//        //    Destroy(gameObject);
//        //}
//    }
//}

