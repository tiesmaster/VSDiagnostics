using System;
using System.Text;

namespace ConsoleApplication1
{
    public class MyClass
    {
        public int SomeProperty { get; private set; }

        MyClass()
        {
            this.SomeProperty = 42;
        }

        private class NestedClass
        {
            internal int SomeProperty { get; set; }

            void Foo()
            {
                SomeProperty = 10;
            }
        }
    }
}