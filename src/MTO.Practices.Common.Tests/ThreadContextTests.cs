using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTO.Practices.Common.Tests
{
    using System.Collections.Generic;
    using System.Runtime.Remoting.Messaging;
    using System.Threading;
    using System.Threading.Tasks;

    [TestClass]
    public class ThreadContextTests
    {
        [TestMethod]
        public void ThreadContext_Get_Set_TEST()
        {
            var tc = new ThreadContext();
            tc.Set("a", 1);
            Assert.AreEqual(1, tc.Get<int>("a"));
        }

        [TestMethod]
        public void ThreadContext_Get_Set_Guid_TEST()
        {
            var tc = new ThreadContext();
            var guid = Guid.NewGuid();
            tc.Set("b", guid);
            Assert.AreEqual(guid, tc.Get<Guid>("b"));
        }

        /// <summary>
        /// Garante que ThreadContext não compartilha valores entre threads do ThreadPool
        /// </summary>
        [TestMethod]
        public void ThreadContext_Sharing_TEST()
        {
            var tc = new ThreadContext();
            var k = "a";

            var threadTimes = new Dictionary<int, DateTime>();

            var action = new Action(
                () =>
                    {
                        var tk = Thread.CurrentThread.ManagedThreadId;
                        DateTime? now = DateTime.Now;

                        Assert.IsFalse(tc.Get<DateTime?>(k).HasValue);
                        
                        if (!tc.Get<DateTime?>(k).HasValue)
                        {
                            tc.Set(k, now);
                            
                            if (!threadTimes.ContainsKey(tk))
                            {
                                threadTimes[tk] = now.Value;
                            }
                        }

                        Assert.AreEqual(now.Value.Ticks, tc.Get<DateTime?>(k).Value.Ticks);
                    });

            var tasks = new List<Task>();

            for (int i = 0; i < 100000; i++)
            {
                tasks.Add(Task.Factory.StartNew(action));
            }

            Task.WaitAll(tasks.ToArray());
            
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void CallContext_Sharing_TaskPool_TEST()
        {
            var k = "a";

            var action = new Action(
                () =>
                {
                    DateTime? nowagain = DateTime.Now;


                    if (CallContext.GetData(k) == null)
                    {
                        CallContext.SetData(k, nowagain);
                    }

                    Assert.AreEqual(nowagain, CallContext.GetData(k));
                });

            var tasks = new List<Task>();

            for (int i = 0; i < 100000; i++)
            {
                tasks.Add(Task.Factory.StartNew(action));
            }

            Task.WaitAll(tasks.ToArray());

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void CallContext_Sharing_NewTask_TEST()
        {
            var k = "a";
            var now = DateTime.Now;
            CallContext.SetData(k, now);

            var action = new Action(() => Assert.AreNotEqual(now, CallContext.GetData(k)));

            var tasks = new List<Task>();

            for (int i = 0; i < 100000; i++)
            {
                tasks.Add(Task.Factory.StartNew(action));
            }

            Task.WaitAll(tasks.ToArray());

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void CallContext_Sharing_Stack()
        {
            var k = "a";
            var tasks = new List<Task>();

            for (int i = 0; i < 100000; i++)
            {
                var now = DateTime.Now;
                CallContext.SetData(k, now);

                var action = new Action(() =>
                {
                    new Action(() => SetData(k, now)).Invoke();

                    Assert.AreEqual(now, CallContext.GetData(k));
                });

                tasks.Add(Task.Factory.StartNew(action));
            }

            Task.WaitAll(tasks.ToArray());

            Assert.IsTrue(true);
        }

        public static void SetData(string k, object o)
        {
            CallContext.SetData(k, o);
        }
    }
}
