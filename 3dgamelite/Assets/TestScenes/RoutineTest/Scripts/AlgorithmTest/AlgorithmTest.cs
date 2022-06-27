using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//设计模式(Design Pattern)
//单例,工厂,发布订阅/观察者,mvc,状态机,模板/迭代器

//array
//线性表(linelist) :增加/删除(back)/修改 √ ,  删除/增加(insert)/搜索 ×
//vector(c++)/List(c#)/ArrayList(java)

//增删改查:crud (增加(Create),检索(Retrieve),更新(Update),删除(Delete))
//增加(add/put/push/insert),删除(delete),修改(modify/update)

//搜索(search/index/retrieve) 
//-二分查找(binary search)
//-集合(set) - HashSet(java/c#)/unordered_set(c++) ,TreeSet(java)/set(c++)
//-映射(mapping) - unordered_map(c++)/HashMap(java)/Dictionary(c#)) ,TreeMap(java)/map(c++)(红黑树)

// 排序(sort)
// 比较排序(Comparison Sorting)
//          选择排序(Selection Sort)
//          冒泡排序(Bubble Sort)
//          插入排序(Insertion Sort)
//          ->希尔排序(Shell Sort)
//          归并排序(Merge Sort)    --链表
//          快速排序(Quick Sort)    --数组
// 计数排序(Counting Sort)
// ->桶排序(Bucket Sort)
// ->基数排序(Radix Sort)
// 堆排序(Heap Sort)               --堆

//stack(栈),queue(队列)
//单调队列(monotone-Queue):主要为了解决  区间最值RMQ(range Minimum/Maximum Query)问题 ,用来维护滑动窗口内数据的单调性，从而保证队首元素为当前区间最值
//单调栈(monotome-Stack) :擅长维护最近大于/小于关系，实现检索到前面&后面中最近（第一个）大于/小于 他的元素
//最小栈(min stack)

//递归(深度优先)DFS(Depth First Search)
//循环(广度优先)BFS(Breadth-first search)
//回溯(recall)

//贪婪算法(greedy algorithm)
//动态规划(dp)(dynamic programming)(斐波那契,背包,小偷)

//滑动窗口,双指针,前缀和

//链表(LinkedList):                                                                                //  删除/增加 √
//合并(merge/splice/combine/union)
//双向链表(DoubleLinkedList)
//环形链表(CircularLinkedList)
//双向循环链表(DoubleCircularLinked list) list(c++) LinkedList(c#/java) 

//树(Trees) //猜测:链表实现?                                                                   // 搜索 √
//叶子节点(Leaf Node):没有任何子节点的节点
//树的度(Trees Degree):结点拥有的子树数量称为子树的度
//完美二叉树(Perfect Binary Trees):对称结构
//完全二叉树(Complete Binary Trees):除了最后一层之外的其他每一层都被完全填充,并且所有结点都保持向左对齐.
//(完)满二叉树(Full/Strictly Binary Trees): 其中每个结点恰好有 0 或 2 个子结点

//二叉查找/排序树(BST(Binary Search/Sort Trees))                                        //左子节点必须比父节点小,右子节点必须必比父节点大                  //不占空间,查询快

//自平衡二叉树(Self-Balancing Binary Search Trees)                                    //为了防止二叉搜索树退化成链表
//avl树(以作者名字命名)                                                                 //左右子树高度之差(平衡因子)的绝对值不超过1(-1 / 0 / 1)                            // 旋转操作耗时,小范围数据查找(严格平衡)
//->红黑树(Red-black Trees)                                                        //根节点必为黑色 ,叶子节点都为黑色,null                                        //优化了avl效率 (非严格平衡)
//->线段树/区间树(Segment Trees/Interval Trees)                     ????
//伸展树(Splay Trees)                                                                                   //最近访问的元素很快再次访问

//自平衡多叉树(Self-balancing Multiway Trees/Multi-trees)
//b/b-树,b+树                                                                                                                                                                                     //大范围数据查找(文件系统,数据库)

//前缀树/字典树/单词查找树(Trie/(Prefix Trees))                                           //占空间,查询更快              (常见于26个字母,10个数字)字符串的快速检索/字符串排序/最长公共前缀/自动匹配前缀显示后缀   
//->后缀树(Suffix Trees)(特殊类型的前缀树)
//->基数树(Radix Trees)(前缀树压缩版(compact trie))
//三叉搜索树(Ternary Search Trees)                                                              //省空间,查询快

//四叉树(QuadTrees),八叉树(OcTrees)
//二叉空间划分树(Binary space partitioning)        ///????
//->kd树(K-Dimensional Trees)

//优先队列(priority_queue)                        //未知: 好像线段树也可以做这个 ?
//堆(heap):完全二叉树                              //猜测:数组实现??
//小根/大根树(min/max Trees):其中每个节点的值都<=/>=其子节点的值
//二叉堆(binary heap)(大根/大顶堆(max heap),小跟/小顶堆(min heap))              

//可合并堆(meldable heap):
//->二项树(binomial Trees):
//->堆序二项树(heap-ordered binomial Trees):
//->二项堆(binomial heap):
//->斐波那契堆(fibonacci heap)


//并查集/不交集(Disjoint Set) //????

//array2d
//图(graph):邻接表(adjacency list),邻接矩阵(adjacency matrix)
//最小生成树(Minimum Spanning Trees)
    //Kruskal,Prim
//最短路径(shortest path)
    //dijkstra,floyd,astar
//拓扑排序(topological-sort)
    //indegree,dfs


namespace Dalechn
{
    public class AlgorithmTest : MonoBehaviour
    {
        public VisualList list2d;
        public VisualList supList;

        private void Start()
        {
            //StartCoroutine(CanCompleteCircuit());
            //StartCoroutine(UniquePaths());
            //StartCoroutine(Trap());

            //ThreadStart childRef = new ThreadStart(MaxAreaOfIsland);
            //Thread childThread = new Thread(childRef);
            //childThread.Start();

            //StartCoroutine(MaxAreaOfIslandDFSStack());
            StartCoroutine(MaxAreaOfIslandBFS());
        }

        // 题解来自leetcode用户:zhai
        //https://leetcode.cn/problems/gas-station/solution/jia-you-zhan-by-leetcode-solution/
        IEnumerator CanCompleteCircuit()
        {
            int[] gas = list2d.GetArray(0);
            int[] cost = list2d.GetArray(1);

            int n = gas.Length;
            int cur = 0, res = 0, ans = 0;

            List<Info> infoList = new List<Info>();
            infoList.Add(new Info("cur", cur, true));
            infoList.Add(new Info("res", res, true));
            infoList.Add(new Info("ans", ans, true));
            list2d.pointerListLeft.Init(ref infoList, false);
            yield return new WaitForSeconds(0.3f);

            for (int i = 0; i < n; i++)
            {
                cur += gas[i] - cost[i];

                infoList = new List<Info>();
                infoList.Add(new Info("cur", cur));

                if (cur < 0)
                {
                    ans = i + 1;
                    res += cur;
                    cur = 0;

                    infoList.Add(new Info("res", res));
                    infoList.Add(new Info("ans", ans));
                }

                list2d.pointerListLeft.Translate(ref infoList, list2d.dp2dEditor[0][i]);

                yield return new WaitForSeconds(0.3f);
            }

            int index = cur + res >= 0 ? ans : -1;
            Debug.Log(index);
            //return index;
        }

        IEnumerator UniquePaths()
        {
            int row = list2d.row;
            int col = list2d.col;
            int[,] dpList = list2d.GetArray();

            for (int i = 0; i < row; ++i)
            {
                dpList[i, 0] = 1;
                list2d.dp2dEditor[i][0].SetValue(1, ColorUtils.white);

            }
            for (int j = 0; j < col; ++j)
            {
                dpList[0, j] = 1;
                list2d.dp2dEditor[0][j].SetValue(1, ColorUtils.white);

            }
            for (int i = 1; i < row; ++i)
            {
                for (int j = 1; j < col; ++j)
                {
                    dpList[i, j] = dpList[i - 1, j] + dpList[i, j - 1];

                    list2d.dp2dEditor[i][j].SetValue(dpList[i, j], ColorUtils.red);
                    //list2d.pointerListLeft.Translate(list2d.dp2dEditor[i][j]);

                    yield return new WaitForSeconds(0.3f);
                }
            }

            int result = dpList[row - 1, col - 1];
            Debug.Log(result);
        }

        public IEnumerator Trap()
        {
            int[] height = list2d.GetArray(0);

            int ans = 0;
            int left = 0, right = height.Length - 1;
            int leftMax = 0, rightMax = 0;

            List<Info> infoList = new List<Info>();
            infoList.Add(new Info("leftMax", leftMax, true));
            infoList.Add(new Info("ans", ans, true));
            list2d.pointerListLeft.Init(ref infoList, false, ColorUtils.red);

            infoList = new List<Info>();
            infoList.Add(new Info("rightMax", rightMax, true));
            infoList.Add(new Info("ans", ans, true));
            list2d.pointerListRight.Init(ref infoList, true, ColorUtils.green);

            yield return new WaitForSeconds(0.3f);

            while (left < right)
            {
                leftMax = Mathf.Max(leftMax, height[left]);
                rightMax = Mathf.Max(rightMax, height[right]);

                if (height[left] < height[right])
                {
                    ans += leftMax - height[left];
                    
                    infoList = new List<Info>();
                    infoList.Add(new Info("leftMax", leftMax));
                    infoList.Add(new Info("ans", ans));
                    list2d.pointerListLeft.Translate(ref infoList, list2d.dp2dEditor[0][left], true, ColorUtils.red);

                    ++left;

                    yield return new WaitForSeconds(0.3f);
                }
                else
                {
                    ans += rightMax - height[right];

                    infoList = new List<Info>();
                    infoList.Add(new Info("leftMax", rightMax));
                    infoList.Add(new Info("ans", ans));
                    list2d.pointerListRight.Translate(ref infoList, list2d.dp2dEditor[0][right], true, ColorUtils.green);

                    --right;

                    yield return new WaitForSeconds(0.3f);
                }
            }

            Debug.Log(ans);
        }

        // X(1)...X(n)
        //中位数: 当n为奇数时,m(0.5) = X(n+1)/2
        //			   当n为偶数时,m(0.5) = (X(n/2)+X(n/2+1))/2


        // 3位数阿姆斯特朗数(Armstrong Number)叫水仙花数(Narcissistic Number):1^3 + 5^3+ 3^3 = 153
        // 回文数(Palindrome Number)正读与反读都一样的数: 1，11，121，1221...
        bool IsArmstrongNumber(int num)
        {
            int tempNum = 0;

            //回文数需要数据
            int originNum = num;

            // 水仙花数需要数据
            //int minNum = 0;

            while (num != 0)
            {
                //水仙花数需要数据
                //minNum = num % 10;
                //tempNum += minNum * minNum*minNum;

                //回文数需要数据
                tempNum = tempNum * 10 + originNum % 10;

                num /= 10;
            }

            return tempNum == originNum;
        }

        //素数:除了 1 和它本身以外,不能被任何整数整除的数 2,3,5,7
        //合数:除了素数和1以外的自然数 4,6,8,9,10
        //		 能整除的数叫因子(factor),除了自身和1以外的叫真因子
        //完全数(Complete number):一个数恰好等于它的所有的真因子之和

        //偶数:能被2整除的整数
        //奇数:不能被2整除的的整数
        bool Prime(int num)
        {
            int factor = 0;
            for (int i = 2; i < num; i++)
            {
                if (num % i == 0)
                    factor += i;
            }

            bool isCompleteNumber = (factor == num);

            if (num <= 1)
            {
                return false;
            }
            else if (num == 2) // 用了return 其实用不用else都一样,不用return 只有前面判断失败了才会执行下面的
            {
                return true;
            }

            //switch (num)
            //{
            //    case 1:
            //        break;
            //    case 2:
            //        break;
            //    default:
            //        break;
            //}

            int k = (int)Mathf.Sqrt(num);

            for (int i = 2; i <= k; i++)
            {
                if (num % i == 0)
                    return false;
            }


            return true;
        }

        // 每一项都是前两项的和0,1,1,2,3,5,8,13....
        long FibonacciRecursion(int n)
        {
            return n < 2 ? n : FibonacciRecursion(n - 1) + FibonacciRecursion(n - 2);
        }

        long Fibonacci(int n)
        {
            int[] result = { 0, 1 };
            if (n < 2)
                return result[n];

            long fibNOne = 1, fibNTwo = 0, fibN = 0;
            for (int i = 1; i < n; i++)
            {
                fibN = fibNOne + fibNTwo;
                fibNTwo = fibNOne;
                fibNOne = fibN;
            }
            return fibN;
        }

        //n!
        long FactorialRecursion(int n)
        {
            if (n == 1)
                return 1;
            return n * FactorialRecursion(n - 1); //阶乘
                                                  //return n + FactorialRecursion(n - 1); //求和
        }

        //1!+2!+...+n! //阶乘求和
        long FactorialSum(int n)
        {

            //int sum = 0;
            //int res = 1;
            //for (int i = 1; i <= n; i++) {
            //	res *= i;  // 阶乘

            //	sum += res; //求和
            //}

            /*int sum = 0;
            for (int i = 1; i <= n; i++) {
                int tmp = 1;
                for (int j = 1; j <= i; j++) {
                    tmp *= j;
                }
                sum += tmp;
            }*/


            int i = 1;
            int sum = 0;
            while (i <= n)
            {
                int tmp = 1;

                int j = 1;
                do //不管怎样都会执行一次这里其实不太合适只是恰巧j=1
                {
                    tmp *= j;
                    j++;
                } while (j <= i);

                sum += tmp;

                i++;
            }

            return sum;

        }

        //需要升序排序后使用
        int BinarySearch(ref List<int> vec, int target)
        {

            int low = 0, mid = 0;
            int height = vec.Count - 1;

            while (low <= height)
            {
                mid = (low + height) / 2;

                if (vec[mid] < target)
                    low = mid + 1;
                else if (vec[mid] > target)
                    height = mid - 1;
                else
                    return vec[mid];
            }

            return -1;
        }

        void SelectSort(ref List<int> vec)
        {
            int size = vec.Count;

            for (int i = 0; i < size; i++)
            {
                int min = i;
                for (int j = i + 1; j < size; j++)
                {
                    if (vec[j] < vec[min])
                    {
                        min = j;
                    }
                }

                //swap
                if (min != i)
                {
                    int __tmp = vec[min];
                    vec[min] = vec[i];
                    vec[i] = __tmp;
                }
            }
        }

        // 快速排序
        public void Sort(int[] list, int low, int high)
        {
            int pivot;
            int l, r;
            int mid;
            if (high <= low)
                return;
            else if (high == low + 1)
            {
                if (list[low] > list[high])
                    Swap(ref list[low], ref list[high]);
                return;
            }
            mid = (low + high) >> 1;
            pivot = list[mid];
            Swap(ref list[low], ref list[mid]);
            l = low + 1;
            r = high;
            do
            {
                while (l <= r && list[l] < pivot)
                    l++;
                while (list[r] >= pivot)
                    r--;
                if (l < r)
                    Swap(ref list[l], ref list[r]);
            } while (l < r);
            list[low] = list[r];
            list[r] = pivot;
            if (low + 1 < r)
                Sort(list, low, r - 1);
            if (r + 1 < high)
                Sort(list, r + 1, high);
        }


        void Swap<_Tp>(ref _Tp __a, ref _Tp __b)
        {
            _Tp __tmp = __a;
            __a = __b;
            __b = __tmp;
        }

        IEnumerator MaxAreaOfIslandDFSStack()
        {
            List<List<int>> grid = list2d.GetArr();

            int ans = 0;
            for (int i = 0; i != grid.Count; ++i)
            {
                for (int j = 0; j != grid[0].Count; ++j)
                {
                    int cur = 0;
                    Queue<int> queuei = new Queue<int>();
                    Queue<int> queuej = new Queue<int>();
                    queuei.Enqueue(i);
                    queuej.Enqueue(j);


                    yield return new WaitForSeconds(0.3f);
                    List<Info> infoList = new List<Info>();
                    infoList.Add(new Info("ans", ans));
                    list2d.pointerListUp.Translate(ref infoList, list2d.dp2dEditor[i][j], false, ColorUtils.white);
                    supList.AddLast(0, i+ "," + j);


                    while (queuei.Count > 0)
                    {
                        int cur_i = queuei.Dequeue();
                        int cur_j = queuej.Dequeue();


                        yield return new WaitForSeconds(0.3f);
                        supList.DeleteFront();
                        if (list2d.ValidIndex(cur_i, cur_j))
                        {
                            list2d.pointerListUp.Translate(ref infoList, list2d.dp2dEditor[cur_i][cur_j], false, ColorUtils.blue);
                        }


                        if (cur_i < 0 || cur_j < 0 || cur_i == grid.Count || cur_j == grid[0].Count || grid[cur_i][cur_j] != 1)
                        {
                            continue;
                        }

                        list2d.dp2dEditor[cur_i][cur_j].SetValue(0, ColorUtils.red);

                        ++cur;
                        grid[cur_i][cur_j] = 0;
                        int[] di = { 0, 0, 1, -1 };
                        int[] dj = { 1, -1, 0, 0 };
                        for (int index = 0; index != 4; ++index)
                        {
                            int next_i = cur_i + di[index], next_j = cur_j + dj[index];
                            queuei.Enqueue(next_i);
                            queuej.Enqueue(next_j);

                            supList.AddLast(0, next_i + ","+ next_j);

                        }
                    }
                    ans = Mathf.Max(ans, cur);
                }
            }

            Debug.Log(ans);
            //return ans;
        }

        IEnumerator MaxAreaOfIslandBFS()
        {
            List<List<int>> grid = list2d.GetArr();

            int ans = 0;
            for (int i = 0; i != grid.Count; ++i)
            {
                for (int j = 0; j != grid[0].Count; ++j)
                {
                    int cur = 0;
                    Stack<int> stacki = new Stack<int>();
                    Stack<int> stackj = new Stack<int>();
                    stacki.Push(i);
                    stackj.Push(j);


                    yield return new WaitForSeconds(0.3f);
                    List<Info> infoList = new List<Info>();
                    infoList.Add(new Info("ans", ans));
                    list2d.pointerListUp.Translate(ref infoList, list2d.dp2dEditor[i][j], false, ColorUtils.white);
                    supList.AddLast(0, i + "," + j);


                    while (stacki.Count > 0)
                    {
                        int cur_i = stacki.Pop();
                        int cur_j = stackj.Pop();


                        yield return new WaitForSeconds(0.3f);
                        supList.DeleteLast();
                        if (list2d.ValidIndex(cur_i, cur_j))
                        {
                            list2d.pointerListUp.Translate(ref infoList, list2d.dp2dEditor[cur_i][cur_j], false, ColorUtils.blue);
                        }


                        if (cur_i < 0 || cur_j < 0 || cur_i == grid.Count || cur_j == grid[0].Count || grid[cur_i][cur_j] != 1)
                        {
                            continue;
                        }

                        list2d.dp2dEditor[cur_i][cur_j].SetValue(0, ColorUtils.red);

                        ++cur;
                        grid[cur_i][cur_j] = 0;
                        int[] di = { 0, 0, 1, -1 };
                        int[] dj = { 1, -1, 0, 0 };
                        for (int index = 0; index != 4; ++index)
                        {
                            int next_i = cur_i + di[index], next_j = cur_j + dj[index];
                            stacki.Push(next_i);
                            stackj.Push(next_j);

                            supList.AddLast(0, next_i + "," + next_j);

                        }
                    }
                    ans = Mathf.Max(ans, cur);
                }
            }

            Debug.Log(ans);
            //return ans;
        }

        private void Update()
        {
            //if (list2d.validIndex(i, j))
            //{
            //    if (setColor)
            //    {
            //        list2d.dp2dEditor[i][j].pointer.SetValue(0, Color.red);
            //    }

            //    List<Info> infoList = new List<Info>();
            //    infoList.Add(new Info("ans", ans));
            //    list2d.pointerListUp.Translate(ref infoList, list2d.dp2dEditor[i][j], false, currentColor);
            //}

        }

        int i, j, ans;
        int currentColor = ColorUtils.white;
        bool setColor;
        int dfs(ref List<List<int>> grid, int cur_i, int cur_j)
        {
            if (cur_i < 0 || cur_j < 0 || cur_i == grid.Count || cur_j == grid[0].Count || grid[cur_i][cur_j] != 1)
            {
                Thread.Sleep(100);

                return 0;
            }

            grid[cur_i][cur_j] = 0;
            int[] di = { 0, 0, 1, -1 };
            int[] dj = { 1, -1, 0, 0 };
            int ans = 1;
            for (int index = 0; index != 4; ++index)
            {
                int next_i = cur_i + di[index], next_j = cur_j + dj[index]; //分别向四个方向递归(右,下,左,上)

                currentColor = ColorUtils.blue;
                setColor = true;
                i = cur_i;
                j = cur_j;
                Thread.Sleep(300);
                setColor = false;
                i = next_i;
                j = next_j;

                ans += dfs(ref grid, next_i, next_j);
            }

            return ans;
        }

        void MaxAreaOfIsland()
        {
            List<List<int>> grid = list2d.GetArr();

            int ans = 0;
            Thread.Sleep(1000);

            for (int i = 0; i != grid.Count; ++i)
            {
                for (int j = 0; j != grid[0].Count; ++j)
                {
                    currentColor = ColorUtils.white;
                    setColor = false;
                    this.i = i;
                    this.j = j;
                    Thread.Sleep(300);                  //延迟可视化还需要更多思考??

                    ans = Mathf.Max(ans, dfs(ref grid, i, j));
                    this.ans = ans;
                }
            }

            Debug.Log(ans);
            //return ans;
        }

    }
}

