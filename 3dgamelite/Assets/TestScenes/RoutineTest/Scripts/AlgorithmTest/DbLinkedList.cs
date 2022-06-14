using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Dalechn
{
    public class DbNode<T>
    {
        private T _data; 
        private DbNode<T> _prev;
        private DbNode<T> _next; 

        public T Data
        {
            get { return this._data; }
            set { this._data = value; }
        }

        public DbNode<T> Prev
        {
            get { return this._prev; }
            set { this._prev = value; }
        }

        public DbNode<T> Next
        {
            get { return this._next; }
            set { this._next = value; }
        }

        public DbNode(T data, DbNode<T> prev, DbNode<T> next)
        {
            this._data = data;
            this._prev = prev;
            this._next = next;
        }

        public DbNode(T data, DbNode<T> prev)
        {
            this._data = data;
            this._prev = prev;
            this._next = null; //结尾处的node
        }

        //public DbNode(T data, DbNode<T> next)
        //{
        //    this._data = data;
        //    this._prev = null; //开头处的node
        //    this._next = next;
        //}

        public DbNode(DbNode<T> next)
        {
            this._data = default(T);
            this._next = next;
            this._prev = null;
        }

        public DbNode(T data)
        {
            this._data = data;
            this._prev = null;
            this._next = null;
        }

        public DbNode()
        {
            this._data = default(T);
            this._prev = null;
            this._next = null;
        }
    }

    public class DbLinkedList<T>
    {

        private DbNode<T> _head;

        public DbNode<T> Head
        {
            get { return this._head; }
            set { this._head = value; }
        }

        public DbLinkedList()
        {
            Head = null;
        }

        public T this[int index]
        {
            get
            {
                return this.GetItemAt(index);
            }
        }

        public bool IsEmpty()
        {
            return Head == null;
        }

        public T GetItemAt(int i)
        {
            if (IsEmpty())
            {
                Debug.Log("The double linked list is empty.");
                return default(T);
            }

            DbNode<T> p = new DbNode<T>();
            p = Head;

            if (0 == i)
            {
                return p.Data;
            }

            int j = 0;
            while (p.Next != null && j < i)//移动j的指针到i的前一个node
            {
                j++;
                p = p.Next;
            }

            if (j == i)
            {
                return p.Data;
            }
            else
            {
                Debug.Log("The node dose not exist.");
                return default(T);
            }
        }

        public int Count()
        {
            DbNode<T> p = Head;
            int length = 0;
            while (p != null)
            {
                length++;
                p = p.Next;
            }
            return length;
        }

        public void Clear()
        {
            this.Head = null;
        }

        public void AddAfter(T item, int i)
        {
            if (IsEmpty() || i < 0)
            {
                Debug.Log("The double linked list is empty or the position is uncorrect.");
                return;
            }

            if (0 == i) //在头之后插入元素
            {
                DbNode<T> newNode = new DbNode<T>(item);
                newNode.Next = Head.Next;
                Head.Next.Prev = newNode;
                Head.Next = newNode;
                newNode.Prev = Head;
                return;
            }

            DbNode<T> p = Head; //p指向head
            int j = 0;

            while (p != null && j < i)
            {
                p = p.Next;
                j++;
            }

            if (j == i)
            {
                DbNode<T> newNode = new DbNode<T>(item);
                newNode.Next = p.Next;
                if (p.Next != null)
                {
                    p.Next.Prev = newNode;
                }
                newNode.Prev = p;
                p.Next = newNode;
            }
            else
            {
                Debug.Log("The position is uncorrect.");
            }

        }

        public void AddBefore(T item, int i)
        {
            if (IsEmpty() || i < 0)
            {
                Debug.Log("The double linked list is empty or the position is uncorrect.");
                return;
            }

            if (0 == i) //在头之前插入元素
            {
                DbNode<T> newNode = new DbNode<T>(item);
                newNode.Next = Head; //把头改成第二个元素
                Head.Prev = newNode;
                Head = newNode; //把新元素设置为头
                return;
            }

            DbNode<T> n = Head;
            DbNode<T> d = new DbNode<T>();
            int j = 0;

            while (n.Next != null && j < i)
            {
                d = n; //把d设置为头
                n = n.Next;
                j++;
            }

            if (n.Next == null) //在最后节点后插入，即AddLast
            {
                DbNode<T> newNode = new DbNode<T>(item);
                n.Next = newNode;
                newNode.Prev = n;
                newNode.Next = null;//尾节点指向空
            }
            else //插到中间
            {
                if (j == i)
                {
                    DbNode<T> newNode = new DbNode<T>(item);
                    d.Next = newNode;
                    newNode.Prev = d;
                    newNode.Next = n;
                    n.Prev = newNode;
                }
            }
        }

        public void AddLast(T item)
        {
            DbNode<T> newNode = new DbNode<T>(item);
            DbNode<T> p = new DbNode<T>();

            if (Head == null)
            {
                Head = newNode;
                return;
            }
            p = Head; //如果head不为空，head就赋值给第一个节点
            while (p.Next != null)
            {
                p = p.Next;
            }
            p.Next = newNode;
            newNode.Prev = p;
        }

        public T RemoveAt(int i)
        {
            if (IsEmpty() || i < 0)
            {
                Debug.Log("The double linked list is empty or the position is uncorrect.");
                return default(T);
            }

            DbNode<T> q = new DbNode<T>();
            if (0 == i)
            {
                q = Head;
                Head = Head.Next;
                Head.Prev = null;//删除掉了第一个元素
                return q.Data;
            }

            DbNode<T> p = Head;
            int j = 0;

            while (p.Next != null && j < i)
            {
                j++;
                q = p;
                p = p.Next;
            }

            if (i == j) //?
            {
                p.Next.Prev = q;
                q.Next = p.Next;
                return p.Data;
            }
            else
            {
                Debug.Log("The position is uncorrect.");
                return default(T);
            }
        }

        public int IndexOf(T value)
        {
            if (IsEmpty())
            {
                Debug.Log("The list is empty.");
                return -1;
            }

            DbNode<T> p = new DbNode<T>();
            p = Head;
            int i = 0;
            while (p.Next != null && !p.Data.Equals(value))//查找value相同的item
            {
                p = p.Next;
                i++;
            }
            return i;
        }

        public void Reverse()
        {//遍历当前链表的元素，逐个插入到另一个临时链表中AddBefore
         //这样，得到的新链表的顺序正好和原链表是相反的

            DbLinkedList<T> tmpList = new DbLinkedList<T>();
            DbNode<T> p = this.Head;
            tmpList.Head = new DbNode<T>(p.Data);
            p = p.Next;

            while (p != null)
            {
                tmpList.AddBefore(p.Data, 0);
                p = p.Next;
            }

            this.Head = tmpList.Head;
            tmpList = null;
        }

        private DbNode<T> GetNodeAt(int i)
        {
            if (IsEmpty())
            {
                Debug.Log("The list is empty.");
                return null;
            }

            DbNode<T> p = new DbNode<T>();
            p = this.Head;

            if (0 == i)
            {
                return p;
            }

            int j = 0;
            while (p.Next != null && j < i)
            {
                j++;
                p = p.Next;
            }
            if (j == i)
            {
                return p;
            }
            else
            {
                Debug.Log("The node does not exist.");
                return null;
            }
        }

        public T Fisrt()
        {
            return this.GetItemAt(0);
        }

        public T Last()
        {
            return this.GetItemAt(this.Count() - 1);
        }

        /// <summary>
        /// 根据Prev指针从最后一个节点开始倒叙遍历输出
        /// </summary>
        /// <returns></returns>
        public string ReverseByPrev()
        {
            //取得最后一个节点
            DbNode<T> tail = GetNodeAt(Count() - 1);
            StringBuilder sb = new StringBuilder();
            sb.Append(tail.Data.ToString() + ",");
            while (tail.Prev != null)
            {
                sb.Append(tail.Prev.Data + ",");
                tail = tail.Prev;
            }
            return sb.ToString().TrimEnd(',');
        }

        /// <summary>
        /// 打印双向链表的每个元素
        /// </summary>
        public void Print()
        {
            DbNode<T> current = new DbNode<T>();
            current = this.Head;
            Debug.Log(current.Data + ",");
            while (current.Next != null)
            {
                Debug.Log(current.Next.Data + ",");
                current = current.Next;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            DbNode<T> n = this.Head;
            sb.Append(n.Data.ToString() + ",");
            while (n.Next != null)
            {
                sb.Append(n.Next.Data.ToString() + ",");
                n = n.Next;
            }

            return sb.ToString().TrimEnd(',');
        }
    }
}

