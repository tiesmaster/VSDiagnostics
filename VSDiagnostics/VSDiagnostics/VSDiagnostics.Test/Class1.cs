using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        internal int SomeProperty { get; private set; }

        MyClass()
        {
            SomeProperty = 42;
            this.SomeProperty = 10;
        }

        void SomeMethod()
        {
            Console.WriteLine("awesome");
        }
    }
}