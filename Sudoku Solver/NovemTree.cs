using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver
{
    public class NovemTreeNode<T>
    {
        protected NovemTreeNode<T>[] Children { get; set; } = new NovemTreeNode<T>[9];
        public NovemTreeNode<T> Parent { get; protected set; }
        public T Content { get; set; }

        public NovemTreeNode(T content, NovemTreeNode<T> parent = null)
        {
            this.Content = content;
            this.Parent = parent;
        }

        public NovemTreeNode<T> this[int idx]
        {
            get 
            {
                if (idx < 0 || idx >= 9) throw new IndexOutOfRangeException(string.Format($"{idx} is out of range in Novum Tree Node!"));
                return this.Children[idx];
            }
            set
            {
                if (idx < 0 || idx >= 9) throw new IndexOutOfRangeException(string.Format($"{idx} is out of range in Novum Tree Node!"));
                this.Children[idx] = value;
            }
        }

        public void Deconstruct(out T content, out NovemTreeNode<T>[] children)
        {
            content = this.Content;
            children = this.Children;
        }

    }
}
