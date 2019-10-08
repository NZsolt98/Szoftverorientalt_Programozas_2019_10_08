using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Ebay_Server
{
    class Product
    {
        string name, code, user = null;
        int currentPrice;
        public string User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
            }
        }
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public int CurrentPrice
        {
            get
            {
                return currentPrice;
            }
            set
            {
                currentPrice = value;
            }
        }
        public Product(string name, string code, int currentPrice, string user)
        {
            this.Name = name;
            this.Code = code;
            this.CurrentPrice = currentPrice;
            this.User = user;
        }
    }
    class Protocol
    {
        static List<Product> Products = new List<Product>();
        public StreamReader r;
        public StreamWriter w;
        public string user = null;

        public Protocol(TcpClient c)
        {
            this.r = new StreamReader(c.GetStream(), Encoding.UTF8);
            this.w = new StreamWriter(c.GetStream(), Encoding.UTF8);
        }
        void Login(string name, string password)
        {
            //Validálás!
            user = name;
            w.WriteLine("OK");
        }

        void List()
        {
            w.WriteLine("OK*");
            lock (Products)
            {
                foreach (Product item in Products)
                {
                    w.WriteLine("{0}, {1}, {2}, {3}",
                        item.Code,
                        item.Name,
                        item.CurrentPrice,
                        item.User);
                }
            }
            w.WriteLine("OK!");
        }

        void Add(string code, string name, int price)
        {
            if (this.user == null)
            {
                w.WriteLine("ERR|Login please");

            }
            else
            {
                lock (Products)
                {
                    int i = 0;
                    for (i = 0; i < Products.Count && Products[i].Code != code; i++) ;
                    if (i < Products.Count)
                    {
                        w.WriteLine("ERR|Already exists");
                    }
                    else
                    {
                        Product temp = new Product(name, code, price, this.user);
                        Products.Add(temp);
                        w.WriteLine("OK");
                    }
                }
            }
        }

        void Bid(string code, int price)
        {
            if (user == null)
            {
                w.WriteLine("ERR|Login,please!");
            }
            else
            {
                lock (Products)
                {
                    int i = 0;
                    for (i = 0; i < Products.Count && Products[i].Code != code; i++) ;
                    if (i >= Products.Count)
                    {
                        w.WriteLine("ERR|This code is not found!");
                    }
                    else
                    {
                        if (Products[i].CurrentPrice < price)
                        {
                            Products[i].CurrentPrice = price;
                            Products[i].User = user;
                            w.WriteLine("OK");
                        }
                        else
                        {
                            w.WriteLine("ERR|Low price!");
                        }
                    }

                }
            }
        }

        public void Start()
        {
            w.WriteLine("Auction 1.0");
            w.Flush();
            bool ok = true;
            string command, message;
            string[] param;
            while (ok)
            {
                command = r.ReadLine();
                param = message.Split("|");
                command = param[0].ToLower();
                switch (command)
                {

                }
            }
        }
        class Program
        {
            static void Main(string[] args)
            {

            }
        }
    }
}