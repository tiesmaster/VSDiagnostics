﻿using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoslynTester.Helpers.CSharp;
using VSDiagnostics.Diagnostics.General.NamingConventions;

namespace VSDiagnostics.Test.Tests.General
{
    [TestClass]
    public class NamingConventionsTests : CSharpCodeFixVerifier
    {
        protected override DiagnosticAnalyzer DiagnosticAnalyzer => new NamingConventionsAnalyzer();

        protected override CodeFixProvider CodeFixProvider => new NamingConventionsCodeFix();

        [TestMethod]
        public void NamingConventions_WithPrivateField_AndCapitalStart_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        private int X;
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        private int _x;
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "field", "X", "_x"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithPrivateField_AndCapitalStart_CallsAreUpdated_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        private int X;

        void Method()
        {
            var v = X;
        }
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        private int _x;

        void Method()
        {
            var v = _x;
        }
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "field", "X", "_x"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithPrivateField_AndCapitalStartWithUnderscore_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        private int _X;
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        private int _x;
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "field", "_X", "_x"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithPublicField_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        public int x;
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        public int X;
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "field", "x", "X"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithPublicField_CallsAreUpdated_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        public int x;

        void Method()
        {
            var v = x;
        }
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        public int X;

        void Method()
        {
            var v = X;
        }
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "field", "x", "X"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithProtectedField_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        protected int x;
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        protected int X;
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "field", "x", "X"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithProtectedField_CallsAreUpdated_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        protected int x;

        void Method()
        {
            var v = x;
        }
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        protected int X;

        void Method()
        {
            var v = X;
        }
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "field", "x", "X"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithInternalField_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        internal int x;
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        internal int X;
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "field", "x", "X"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithInternalField_CallsAreUpdated_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        internal int x;

        void Method()
        {
            var v = x;
        }
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        internal int X;

        void Method()
        {
            var v = X;
        }
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "field", "x", "X"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithProtectedInternalField_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        protected internal int x;
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        protected internal int X;
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "field", "x", "X"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithProtectedInternalField_CallsAreUpdated_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        protected internal int x;

        void Method()
        {
            var v = x;
        }
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        protected internal int X;

        void Method()
        {
            var v = X;
        }
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "field", "x", "X"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithProperty_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        public int x { get; set; }
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        public int X { get; set; }
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "property", "x", "X"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithProperty_CallsAreUpdated_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        public int x { get; set; }

        void Method()
        {
            var v = x;
        }
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        public int X { get; set; }

        void Method()
        {
            var v = X;
        }
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "property", "x", "X"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithMethod_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        void method()
        {
        }
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method()
        {
        }
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "method", "method", "Method"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithMethod_CallsAreUpdated_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        void method()
        {
        }

        void Test()
        {
            method();
        }
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method()
        {
        }

        void Test()
        {
            Method();
        }
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "method", "method", "Method"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithClass_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class myClass
    {
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "class", "myClass", "MyClass"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithClass_CallsAreUpdated_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class myClass
    {
    }

    class Test
    {
        myClass _foo = new myClass();
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
    }

    class Test
    {
        MyClass _foo = new MyClass();
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "class", "myClass", "MyClass"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithInterface_WithoutPrefix_AndLowerFirstLetter_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    interface something
    {
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    interface ISomething
    {
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "interface", "something", "ISomething"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithInterface_WithoutPrefix_AndLowerFirstLetter_CallsAreUpdated_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    interface something
    {
    }

    class MyClass
    {
        MyClass(something foo) {}
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    interface ISomething
    {
    }

    class MyClass
    {
        MyClass(ISomething foo) {}
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "interface", "something", "ISomething"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithInterface_WithPrefix_AndLowerSecondLetter_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    interface Isomething
    {
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    interface ISomething
    {
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "interface", "Isomething", "ISomething"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithInterface_WithlowerPrefix_AndCapitalSecondLetter_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    interface iSomething
    {
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    interface ISomething
    {
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "interface", "iSomething", "ISomething"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithInterface_WithoutPrefix_AndCapitalFirstLetter_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    interface Something
    {
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    interface ISomething
    {
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "interface", "Something", "ISomething"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithInterface_WithUnderscore_InMiddle_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    interface Cow_milker
    {
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    interface ICowMilker
    {
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "interface", "Cow_milker", "ICowMilker"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithInterface_WithUnderscore_AtFront_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    interface _Something
    {
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    interface ISomething
    {
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "interface", "_Something", "ISomething"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithInterface_WithUnderscore_AtBack_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    interface Something_
    {
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    interface ISomething
    {
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "interface", "Something_", "ISomething"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithLocalVariable_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method()
        {
            var MyVar = 5;
        }
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method()
        {
            var myVar = 5;
        }
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "local", "MyVar", "myVar"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithLocalVariable_CallsAreUpdated_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method()
        {
            var MyVar = 5;
            var foo = MyVar;
        }
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method()
        {
            var myVar = 5;
            var foo = myVar;
        }
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "local", "MyVar", "myVar"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithParameter_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method(string Param)
        {
        }
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method(string param)
        {
        }
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "parameter", "Param", "param"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithParameter_CallsAreUpdated_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method(string Param)
        {
            var foo = Param;
        }
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method(string param)
        {
            var foo = param;
        }
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "parameter", "Param", "param"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithMultipleFields_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        private int X, Y;
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        private int _x, _y;
    }
}";

            VerifyDiagnostic(original,
                string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "field", "X", "_x"),
                string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "field", "Y", "_y"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithMultipleFields_CallsAreUpdated_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        private int X, Y;

        void Method()
        {
            var i = X;
            var j = Y;
        }
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        private int _x, _y;

        void Method()
        {
            var i = _x;
            var j = _y;
        }
    }
}";

            VerifyDiagnostic(original,
                string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "field", "X", "_x"),
                string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "field", "Y", "_y"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithPrivateField_FollowingConventions_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        private int _x;
    }
}";

            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void NamingConventions_WithProtectedField_FollowingConventions_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        protected int X;
    }
}";

            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void NamingConventions_WithInternalField_FollowingConventions_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        internal int X;
    }
}";

            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void NamingConventions_WithPublicField_FollowingConventions_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        public int X;
    }
}";

            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void NamingConventions_WithInternalProtectedField_FollowingConventions_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        internal protected int X;
    }
}";

            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void NamingConventions_WithProperty_FollowingConventions_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        private int X { get; set; }
    }
}";

            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void NamingConventions_WithMethod_FollowingConventions_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method()
        {
        }
    }
}";

            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void NamingConventions_WithClass_FollowingConventions_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
    }
}";

            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void NamingConventions_WithInterface_FollowingConventions_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    interface ISomething
    {
    }
}";

            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void NamingConventions_WithLocalVariable_FollowingConventions_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method()
        {
            string myVar = string.Empty;
        }
    }
}";

            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void NamingConventions_WithParameter_FollowingConventions_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method(string param)
        {
        }
    }
}";

            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void NamingConventions_WithVerbatimIdentifier_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        public int @class;
    }
}";
            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void NamingConventions_WithEscapedIdentifier_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        public int \u0061ss;
    }
}";
            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void NamingConventions_WithMultipleDifferentTypes_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        internal int x;
        
        void method()
        {
        }
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        internal int X;
        
        void Method()
        {
        }
    }
}";

            VerifyDiagnostic(original,
                string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "field", "x", "X"),
                string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "method", "method", "Method"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithMultipleSimilarTypes_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method()
        {
            int X, Y;
        }
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method()
        {
            int x, y;
        }
    }
}";

            VerifyDiagnostic(original,
                string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "local", "X", "x"),
                string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "local", "Y", "y"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithExclusivelySpecialCharacters_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        protected int ___;
    }
}";
            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void NamingConventions_WithOneLetterPrivateVariable_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        private int x;
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        private int _x;
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "field", "x", "_x"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithPrivateField_WithoutAccessModifier_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        int X;
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        int _x;
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "field", "X", "_x"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithStruct_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    struct myStruct
    {
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    struct MyStruct
    {
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "struct", "myStruct", "MyStruct"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithStruct_CallsAreUpdated_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    struct myStruct
    {
    }

    class MyClass
    {
        myStruct _foo;
    }
}";

            var result = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    struct MyStruct
    {
    }

    class MyClass
    {
        MyStruct _foo;
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "struct", "myStruct", "MyStruct"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void NamingConventions_WithStruct_FollowingConventions_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    struct MyStruct
    {
    }
}";

            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void NamingConventions_WithPrivateField_AsConstant_FollowingConventions_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        private const int X;
    }
}";

            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void NamingConventions_WithPrivateField_AsStatic_FollowingConventions_DoesNotInvokeWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        private static int X;
    }
}";

            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void NamingConventions_WithPrivateField_AsReadonly_InvokesWarning()
        {
            var original = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        private readonly int X;
    }
}";

            var expected = @"
using System;
using System.Text;

namespace ConsoleApplication1
{
    class MyClass
    {
        private readonly int _x;
    }
}";

            VerifyDiagnostic(original, string.Format(NamingConventionsAnalyzer.Rule.MessageFormat.ToString(), "field", "X", "_x"));
            VerifyFix(original, expected);
        }
    }
}