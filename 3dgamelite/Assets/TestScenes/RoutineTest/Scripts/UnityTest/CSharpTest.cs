using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices; //dll
using UnityEngine;
using UnityEngine.Events;

namespace Dalechn
{
    public class CSharpTest
    {
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

        //函数默认参数,可变参数
        //关键字测试
        private void FunctionTest(in int test1, out int test2, ref int test3, int a = 0, params int[] para)
        {
            //test1 = 3;
            test2 = 4;
            test3 = 5;

            Debug.Log(this is CSharpTest);
            CSharpTest cls2 = this as CSharpTest;
            Debug.Log(typeof(CSharpTest));
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
            Nullable<int> num5 = new Nullable<int>(3);

            // 值类型(value type)
            bool aBool = true; //8位无符号      //Boolean
            char aChar = 'a'; //16位                 //Char

            int aInt = 0; // 32位                        //Int32
            int aIntb = 0b01; // 2进制                     
            int aInto = 016; // 8进制                      
            int aIntx = 0xf5; // 16进制                       
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
            // - 未知:左边的变量好像不能是自身?,,不是,是因为没声明,这样写就行:
            //object obj2 = null;
            //obj2 = obj2 ?? obj4;

            // - 空合并运算符为右结合运算符: a ?? b ?? c”=“a ?? (b ?? c)
            // - COALESCE 表达式相当于:   obj2 = obj4 != null ? obj4 : obj;
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
            string str5 = "888" + str3.ToString();

            //@代表不解释转义字符
            //  \' 单引号 \f 换页 \" 双引号 \n 新行 \\ 反斜杠 \r 回车 \0 空字符 
            //  \t 水平制表符 \a 警告(产生蜂鸣) \v垂直制表符 \b退格

            //猜测:数组为Array类型?
            int[] arr1 = new int[4];
            int[] arr2 = new int[4] { 1, 3, 5, 7 };
            int[] arr3 = new int[] { 1, 3, 5, 7 };
            int[] arr4 = { 1, 3, 5, 7 };

            int[,] arr2d1 = new int[5, 3];
            int[,] arr2d2 = new int[,] { { 1, 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 } };
            int[][] arr2d3 = new int[4][]; //这样写只能手动创建
            for (int i = 0; i < arr2d3.Length; i++)
            {
                arr2d3[i] = new int[5];
            }
            //int[][] arr2d4= new int[4][5];//报错

            arr2d1[1, 2] = 10;
            arr2d3[2][2] = 10;

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

            //有装箱操作,,key不可重复,不可为null
            Hashtable table = new Hashtable();
            table.Add(1, "table");
            //table.Add(null, "table");
            Hashtable.Synchronized(table);//线程安全,只有一个线程写  多个线程读

            //字典不是线程安全 ,key不可重复,不可为null
            Dictionary<object, string> dic = new Dictionary<object, string>();
            dic.Add(1, "dic");
            //dic.Add(null, "dic");

            // 排序, 不允许重复
            SortedSet<string> sortedSet = new SortedSet<string>();
            sortedSet.Add("123");

            // 排序,不允许重复
            SortedDictionary<int, string> sortedDic = new SortedDictionary<int, string>();
            sortedDic.Add(1, "sortedDic");
            sortedDic.Add(0, "sortedDic");

            // 排序,有装箱操作 不允许重复
            SortedList sortedList = new SortedList();//IComparer
            sortedList.Add(1, "sortedList");
            sortedList.Add(0, "sortedList");

            //foreach (DictionaryEntry val in sortedList)
            //{
            //    Debug.Log(val.Key + " " + val.Value);
            //}
            //ConcurrentBag 线程安全版本的List?
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

            delTest = new TestDelegate(TestDelgate);
            actTest = new Action<int, int>((a, b) => { });

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
            PersonList<int> peopleList = new PersonList<int>();

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

            foreach (var val in peopleList)
            {
                Debug.Log(val);
            }

            for (int i = 0; i < peopleList.peopleList.Count; i++)
            {
                Debug.Log(peopleList[i]);
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
                if (i > 30)
                {
                    yield break;
                }
                yield return i;
            }
        }

        public void Init()
        {
            //DataTypeTest();

            //DataStructTest();

            //TestDel();

            int test1 = 0; // in 必须初始化,方法内不可修改
            int test2; // out 无需初始化,方法内必须修改
            int test3 = 2; // ref 必须初始化,无限制
            FunctionTest(test1, out test2, ref test3);
            //Debug.Log(test1+" "+test2+" "+test3);

            //CPPTest();

            //IEnumeratorTest();

            //CSharp7Test(out int x, out int y);

            //Person p = new Person();
            //p.ReflectTest();

            //多态测试
            AbstractClassBase abs = new GenericClass<int>(10);
            abs.NewInit(10);
            abs.VirtualInit(10);
            Debug.Log(abs.ID);
        }

        private ref int CSharp7Test(out int x, out int y)
        {
            //out 变量（out variables）
            x = 0;
            y = 0;

            //元组(tuple):一个元组最多只能包含八个元素
            var tuple = (1, "2"); // 未知:这玩意啥类型?

            var tuple2 = ValueTuple.Create(1, 2);
            var tuple3 = new ValueTuple<int, string>(1, "2");

            Tuple<int, string, string> tupleClass = new Tuple<int, string, string>(1, "Steve", "Jobs");
            Tuple<int, bool, string, string> tupleClass2 = Tuple.Create(1, true, "3", "4");

            //元组/对象解构
            var (one, two) = tuple;
            var (name, age) = new Person("Mike", 30, "man");

            //本地函数(local function)
            int localFunction()
            {
                return 0;
            }
            localFunction();

            // ref局部变量和返回ref变量
            // 未知: tuple好像不能ref返回?
            int number = 0;
            ref int n1 = ref number;

            int[] arr4 = { 1, 3, 5, 7 };
            ref int n2 = ref arr4[0];

            //二进制文字、数字分隔符
            var binary = 0b0001;
            decimal pi = 3.141_592_653_589m;

            //拓展表达式体成员(More expression-bodied members)
            //扩展异步返回类型（Generalized async return types)???
            //拓展Throw 表达式（Throw expressions)???

            //switch,is 更新
            if (number is int val)
            {
                switch (number)
                {
                    case 0:                // 常量模式匹配
                        break;
                    case int ival: // 类型模式匹配  啥玩意?
                        break;
                }
            }

            return ref n2;
        }

        public void Update()
        {

        }

        //public static void Main(string[] args)
        //{
        //    CSharpTest cSharpTest = new CSharpTest();
        //    cSharpTest.Init();

        //    Console.WriteLine("Hello CSharp");
        //}

    }

    //未知:泛型问题很大?
    //泛型化接口,避免装箱
    public class PersonList<T> : IEnumerable<T>, IEnumerable //枚举器
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

            ~PeopleEnumerator() //会被编译成finalize()函数
            {
                Dispose(disposing: false);
            }

            //实现这个接口是为了释放资源时不全部释放此对象函数的引用
            public void Dispose()
            {
                Dispose(disposing: true);
                GC.SuppressFinalize(this); // 告诉gc不在执行析构函数
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
                    if (_position == -1 || _position >= peopleList.Count)
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

    public partial class Person{ }

    // 部分类
    //所有部分都必须使用partial 关键字
    //各个部分必须具有相同的可访问性
    //可在不同文件写
    public partial class Person 
    {
        public string pName { get; protected set; }
        public string sex { get; set; }

        public int age { get; set; }
        public int ID;

        public Person()  { }
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

        //解构函数c#7.0,返回值为void,参数为out
        public void Deconstruct(out string name, out int age)
        {
            name = pName;
            age = this.age;
        }


        //反射:Type,Activator,Assembly
        public int ReflectTest(int a) { return a; }
        public void ReflectTest()
        {
            //流程:(Assembly加载程序)->获取Type->Activator创建对象
            //Assembly主要用来加载其他程序集
            //Activator是用于快速实例化对象的类

            Type type = this.GetType(); //=typeof(CSharpTest)

            Person testObj = Activator.CreateInstance(type) as Person;
            Debug.Log(testObj.ID);

            MemberInfo[] infos = type.GetMembers(); //Field+Property
            for (int i = 0; i < infos.Length; i++)
            {
                Debug.Log(infos[i]);
            }

            FieldInfo[] fileldInfos = type.GetFields();
            for (int i = 0; i < fileldInfos.Length; i++)
            {
                Debug.Log(fileldInfos[i]);
            }

            PropertyInfo infoJ = type.GetProperty("age");
            Debug.Log(infoJ);

            ConstructorInfo[] ctors = type.GetConstructors();
            for (int i = 0; i < ctors.Length; i++)
            {
                Debug.Log(ctors[i]);
            }

            MethodInfo[] methods = type.GetMethods();
            for (int i = 0; i < methods.Length; i++)
            {
                Debug.Log(methods[i]);
            }
            //得到ReflectTest(int)函数
            MethodInfo method = type.GetMethod("ReflectTest", new Type[] { typeof(int) });
            Debug.Log(method);

            //如果是静态方法，Invoke中的第一个参数传null即可
            object result = method.Invoke(testObj, new object[] { 10 });
            Debug.Log(result);

            //1、得到枚举：GetEnumName、GetEnumNames
            //2、得到事件：GetEvent、GetEvents
            //3、得到接口：GetInterface、GetInterfaces
            //4、得到属性：GetProperty、GetProtertys
        }

    }
    //静态类,只能有静态方法和静态成员变量
    public static class StaticClass { }

    // 抽象类(abstract class)抽象成员必须加abstract,static不可以加abstract/virtual
    public abstract class AbstractClassBase
    {
        public int ID = -1;

        public AbstractClassBase(int ID) { this.ID = ID; }
        public AbstractClassBase(int ID, int a) { this.ID = ID; }

        public virtual void NewInit(int a) { Debug.Log("Base NewInit"); }
        public virtual void VirtualInit(int a) { Debug.Log("Base VirtualInit"); }
        abstract public void Init();
    }

    public enum EEnumTest { ENUMTEST }

    // 接口(interface)无法创建成员变量,不需要加abstract/virtual,不可以加static,默认public(无法修改访问限制)
    public interface IInterfaceTest
    {
        //object Current { get; }
        //bool MoveNext();
        //void Reset();

        event EventHandler OnDataUpdate;
        event EventHandler InternalOnDataUpdate;
    }

    // 结构体(struct)是数值型,在迭代器里会有各种限制,无法初始化,不能继承,建议存储值类型时使用struct
    public struct StructTest
    {
        int structTest;
    }

    //泛型接口
   // 泛型约束
    //E - 元素，主要由Java集合(Collections)框架使用
    //K - 键，主要用于表示映射中的键的参数类型
    //V - 值，主要用于表示映射中的值的参数类型
    //N - 数字，主要用于表示数字
    //T - 类型，主要用于表示第一类通用型参数
    //S - 类型，主要用于表示第二类通用类型参数
    //U - 类型，主要用于表示第三类通用类型参数
    //V - 类型，主要用于表示第四个通用类型参数
    public interface GenericInterface<E, K, N, T, S, U, V>
        where N : struct                    //约束V必须是值类型
    where T : class                      //约束T必须是引用类型
    where U : T                             //约束U必须是T类型或者T类型的子类
    where S : IComparable            //约束HL必须实现了 IComparable 接口
    where V : class, new()              //约束HE必须是引用类型，且有无参构造函数 { }
    { }

    //泛型类 
    // 命名空间下的class/struct/interface前面只能加public或者不加,类内定义的class /struct/interface无限制
    // 类是(class)引用型
    // sealed密封类/方法
    //面向对象3大特征:封装(encapsulation),继承(extend),多态(Polymorphism)
    public class GenericClass<T> : AbstractClassBase,  IInterfaceTest/*, Person*/ //不能有多个基类,可以有多个接口
    {
        //泛型方法,<T>可不写
        public void GenericFunctionTest<T>(T parameter) { }

        //1. public 访问不受限制
        //2. protected 类内和子类访问
        //3. internal 当前程序集/命名空间内访问
        //4. protected internal 当前程序集/命名空间 类内和子类访问
        //5. private 只能在类内访问
        public readonly RangeFloat rangeTest;
        protected internal static readonly RangeFloat3 range3Test; // readonly修饰任意类型,运行时确定值,无法在inspector显示 //未知:不知道存在哪?
        internal const int testConst = 0;                                       // const只能修饰基本类型 ,编译时确定值,无法在inspector显示

        public static int staticTest;
        protected int protectedTest;
        private int privateTest;

        //接口带event的实现方式
        public event EventHandler OnDataUpdate;

        event EventHandler IInterfaceTest.InternalOnDataUpdate
        {
            add => OnDataUpdate += value;
            remove
            {
                OnDataUpdate -= value;
            }
        }

        //表达式主体成员(expression-bodied-members)
        public void Command() => Init();
        public void Command1(int a) => Debug.Log(a);

        //成员函数没有virtual
        //不管加不加new,当父类引用调用此方法时还是调用的父类
        public new  string ID => "man";
        //public string ID { get; } = "man";         // 和上面写法都相当于只读属性

        public int Age   // 属性:property (带get或set的field)
        {
            //get { return m_age; }
            //set { m_age = value; }

            // c#7.0扩展
            get => m_Age;
            set => m_Age = value;
        }

        private int m_Age; // 字段:field 最好不为public

        //不写构造函数的话,编译器默认会创建空的构造函数
        //每个构造函数都要实现父类构造函数的参数,有多个有参数构造函数实现一个就行?
        public GenericClass(int baseInt) : base(baseInt) { }
        ~GenericClass() { }

        //需要override才能密封
        public sealed override void Init()
        {
            throw new NotImplementedException();
        }

        // overload(重载)是不同参数的同名func
        public void Init(int para)
        {}

        // override(覆盖,隐藏,重写)
        public override void VirtualInit(int a) { Debug.Log(" VirtualInit"); }

        // 不override,当父类引用调用此方法时还是调用的父类
        // c++/c# override: 1.父类需要写virtual,2.子类需要写override, 两者缺一不可
        // c#可以写new来标记无法override(非必须)
        public new void NewInit(int a) { Debug.Log(" NewInit"); }

        public static void staticFunc() { }
    }

    //单例
    //public class Singleton : MonoBehaviour
    //{
    //    public static Singleton Instance { get; private set; }
    //    protected virtual void Awake()
    //    {
    //        Instance = this;
    //    }
    //}
 
    //-------------------------------------建立新语言模板--------------------------------------------
    // 1.环境搭建,注释,语言基本属性,语言基本组成
    // 2.核心类库(控制台调试, 鼠标键盘, io,日期, 数学, 随机)
    // 3.工作需要的框架

    //-------------------------------------语言基本属性-------------------------------------------
    // 编译型语言(compiled language) :c/c++
    // 解释型语言(interpreted languages) :c#,java
    // 脚本语言(script language):js/ts,lua,python

    //1.数据类型:值类型(value type):string,number
    //                 引用类型(ref type):array,object
    //2.数据传递方式: 值传递(pass by value),引用传递(pass by reference)
    //3.定义(define),声明(declare)                  //未知:这玩意还是很疑惑?


    //-------------------------------------语言基本组成/命名方式-----------------------------------------------
    //文件/其他: FileTest,file_test
    //包：com.deamerstudio.xxxtest

    // 类/结构体,接口,枚举：CSharpTest/dSharpTest/d_SharpTest, IInterfaceTest,EEnumTest(ENUM_VALUE)
    // 函数(function/method) : DataTest,dataTest
    //          形参(parameter,函数定义),实参(argument,函数调用)
    //          
    // 回调函数(callback function): 函数指针(function pointer),箭头函数(arrow function)/lambda,delegate

    //变量(variable)：
    //  * 成员变量(member Variable) :属性(property) :MemberTest
    //                                            字段(field):memberTest,m_MemberTest
    //  * 局部变量(local variable):memberTest
    //  * 常量(constant)：CONST_TEST,k_ConstTest
    //  * 静态/全局变量(static): s_Instance,g_Instance

}
