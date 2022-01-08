using System;
using System.Collections.Generic;
using System.Linq;

class Node : IEqualityComparer<Node> ,IComparable<Node>
{
    public List<Item> items;
    public Node left, right;
    public int currentCapacity;
    public int totalValue;
    public static int capacity;

    public Node()
    {
        items = new List<Item>();
        left = null;
        right = null;
        update();
    }

    public Node(List<Item> items,Item item)
    {
        this.items = new List<Item>();
        if(items.Count > 0 )
        {
            for (int i = 0; i < items.Count; i++)
            {
                this.items.Add(items[i]);
            }
        }
        this.items.Add(item);
        left = null;
        right = null;
        update();
    }

    public Node(List<Item> items)
    {
        this.items = new List<Item>();
        if (items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                this.items.Add(items[i]);
            }
        }
        left = null;
        right = null;
        update();
    }

    public void addLeft(Item item)
    {
        if(currentCapacity + item.getWeight() <= capacity)
        {
            left = new Node(items, item);
            currentCapacity += item.getWeight();
        }
        else
        {
            left = new Node(items);
        }
    }

    public void addRight()
    {
        right = new Node(items);

    }

    public void printItems()
    {
        foreach(Item item in items)
        {
            Console.Write("{0} ", item.getNum());
        }
        Console.WriteLine();
    }

    public void printTree(Node root)
    {
        if (root == null)
            return;
        printTree(root.left);
        root.printItems();
        printTree(root.right);
    }

    public void getLeaves(Node root ,List<Node> leaves)
    {
        if (root != null)
        {
            if (root.left == null && root.right == null)
            {
                leaves.Add(root);
            }
            if(root.left != null)
            {
                getLeaves(root.left, leaves);
            }
            if(root.right != null)
            {
                getLeaves(root.right, leaves);
            }
        }
    }

    public static void setCapacity(int size)
    {
        capacity = size;
    }

    public static int getCapacity()
    {
        return capacity;
    }

    public void setcapacity(int n)
    {
        setCapacity(n);
    }

    public int getcapacity()
    {
        return getCapacity();
    }

    private void update()
    {
        foreach(Item item in items)
        {
            currentCapacity += item.getWeight();
            totalValue += item.getVal();
        }
    }

    public bool Equals(Node x ,Node y)
    {
        return x.getItems() == x.getItems();
    }

    public int GetHashCode(Node obj)
    {
        return obj.getItems().GetHashCode();
    }

    public int CompareTo(Node node)
    {
        if (node == null)
            return 1;
        else
            return totalValue.CompareTo(node.totalValue);
    }

    public String getItems()
    {
        String s = String.Empty;
        if (items.Count == 0)
            return "-";
        foreach (Item item in items)
        {
            s = String.Concat(s, " ", item.getNum().ToString());

        }
        return s.Trim();
    }

    public String toString()
    {
        return String.Format("Total Weight\t:{0}\nTotal Value\t:{1}\nItem numbers\t:{2}", currentCapacity, totalValue, getItems());
    }
}

class Items
{
    public List<Item> items = new List<Item>();

    public void addItem(Item item)
    {
        items.Add(item);
    }

    public void printItems()
    {
        foreach(Item item in items)
        {
            Console.WriteLine("{0}", item.toString());
        }
    }
}

class Item
{
    private int itemNum { get; set; }
    private int weight { get; set; }
    private int value { get; set; }

    public Item(int itemNum ,int weight ,int value)
    {
        this.itemNum = itemNum;
        this.weight = weight;
        this.value = value;
    }

    public int getVal()
    {
        return value;
    }

    public int getWeight()
    {
        return weight;
    }

    public int getNum()
    {
        return itemNum;
    }

    public String toString()
    {
        return String.Format("Item Num\t:{0}\nItem Weight\t:{1}\nItem Value\t:{2}\n", itemNum, weight, value);
    }
}


namespace knapsack
{
    class Program
    {
        static void Main(string[] args)
        {
            Items items = new Items();
            Node root = new Node();
            int numOfItems;
            Console.Write("Enter the number of items to generate.");
            numOfItems = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            int N = numOfItems;
            int capacity = new Random().Next(N, (N * N) + 1);
            createItems(items ,numOfItems ,capacity);
            root.setcapacity(capacity);
            items.printItems();
            createTree(root, items);
            List<Node> leaves = new List<Node>();
            root.getLeaves(root, leaves);
            var result = leaves.Distinct(new Node());//Using LINQ distinct function to prevent use of replicate sets.
            List<Node> res = result.ToList<Node>();
            res.Sort(); //Sorted from minValue to maxValue
            res.Reverse(); //To have maxValue at index 0 we reverse the list.
            res.RemoveAt(res.Count - 1); //Deleting last element which has 0 items.
            Console.WriteLine("KNAPSACK CAPACITY IS : {0}\n", capacity);
            Console.WriteLine("OPTIMAL SOLUTION");
            Console.WriteLine("{0}\n", res[0].toString());
            Console.WriteLine("POSSIBLE SOLUTIONS");
            foreach (Node node in res)
            {
                Console.WriteLine("{0}\n", node.toString());
            }
        }

        static void createTree(Node root,Items items)
        {
            List<Node> leaves = new List<Node>();
            root.getLeaves(root, leaves);
            for (int i = 0; i < items.items.Count; i++)
            {
                foreach (Node node in leaves)
                {
                    addChildren(node, items.items[i]);
                }
                leaves.Clear();
                root.getLeaves(root, leaves);
            }
        }

        static void addChildren(Node node ,Item item)
        {
            node.addLeft(item);
            node.addRight();
        }

        static void createItems(Items items ,int num ,int capacity) //Creates random values according the parameters and restrictions.
        {
            var rnd = new Random(); 
            for(int i=1;i<=num;i++)
            {
                items.addItem(new Item(i, rnd.Next(1, capacity + 1), rnd.Next(1, (num * num) + 1)));
            }
        }
    }
}
