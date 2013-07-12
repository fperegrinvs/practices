namespace MTO.Practices.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using MTO.Practices.Common.JobManager;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class JobManagerTests
    {
        [TestMethod]
        public void JobManager_Executa_Action_Sem_Retorno_TEST()
        {
            var id = TestJobQueue.Instance.Enqueue(
                "op1",
                this.OnOperacao,
                "master");

            var job = JobManager.Instance.FindJob(id);
            Assert.IsFalse(job.Done);

            for (int i = 0; i < 10000000; i++)
            {
                var j = i;
            }

            Assert.IsTrue(job.Done);
        }

        [TestMethod]
        public void JobManager_Executa_Funcao_Com_Retorno_TEST()
        {
            var id = TestJobQueue.Instance.Enqueue(
                "op1",
                () => this.OnOperacaoFuncao1(100),
                "master");

            var job = JobManager.Instance.FindJob(id);
            Assert.IsFalse(job.Done);

            for (int i = 0; i < 10000000; i++)
            {
                var j = i;
            }

            Assert.IsTrue(job.Done);
            Assert.AreEqual(10000, job.Result);
        }

        [TestMethod]
        public void JobManager_Executa_Metodo_Com_Erro_TEST()
        {
            var id = TestJobQueue.Instance.Enqueue(
                "op-err",
                () => { Thread.Sleep(10); throw new Exception("ERRO SOCORRO"); },
                "master");

            var job = JobManager.Instance.FindJob(id);
            Assert.IsFalse(job.Done);

            Thread.Sleep(100);

            Assert.IsTrue(job.Done);
            Assert.IsNotNull(job.Error);
            Assert.AreEqual("ERRO SOCORRO", job.Error.Message);
        }

        [TestMethod]
        public void JobManager_Executa_Metodos_Em_Sequencia_TEST()
        {
            var idList = new List<Guid>();
            for (int i = 0; i < 20; i++)
            {
                var a = i;
                idList.Add(TestJobQueue.Instance.Enqueue(
                "op-err" + i,
                () => { Thread.Sleep(10); throw new Exception("ERRO SOCORRO" + a); },
                "master"));
            }

            var jobs = idList.Select(x => JobManager.Instance.FindJob(x)).ToList();

            while (jobs.Any(x => !x.Done))
            {
                Thread.Sleep(10);
            }

            var idx = 0;
            foreach (var job in jobs.OrderBy(x => x.StartDate))
            {
                Assert.IsTrue(job.Done);
                Assert.IsNotNull(job.Error);
                Assert.AreEqual(jobs[idx], job);
                Assert.AreEqual("ERRO SOCORRO" + idx, job.Error.Message);
                idx++;
            }
        }

        [TestMethod]
        public void JobManager_Mantem_Tamanho_Historico_TEST()
        {
            var idList = new List<Guid>();
            for (int i = 0; i < 20; i++)
            {
                var a = i;
                idList.Add(TestJobQueue.Instance.Enqueue(
                "op-err" + i,
                () => { Thread.Sleep(10); throw new Exception("ERRO SOCORRO" + a); },
                "master"));
            }

            var jobs = idList.Select(x => JobManager.Instance.FindJob(x)).ToList();

            while (jobs.Any(x => !x.Done))
            {
                Thread.Sleep(10);
            }

            var historySize = JobManager.Instance.GetQueues().SelectMany(x => x.GetDoneOrDoing().Where(y => y.Done)).Count();
            Assert.AreEqual(5, historySize);
        }

        [TestMethod]
        public void JobManager_Mantem_Apenas_Mais_Recentes_TEST()
        {
            var idList = new List<Guid>();
            for (int i = 0; i < 20; i++)
            {
                var a = i;
                idList.Add(TestJobQueue.Instance.Enqueue(
                "op-err" + i,
                () => { Thread.Sleep(10); throw new Exception("ERRO SOCORRO" + a); },
                "master"));
            }

            var jobs = idList.Select(x => JobManager.Instance.FindJob(x)).ToList();

            while (jobs.Any(x => !x.Done))
            {
                Thread.Sleep(10);
            }

            var history = JobManager.Instance.GetQueues().SelectMany(x => x.GetDoneOrDoing().Where(y => y.Done));

            var idx = 19;
            foreach (var job in history.OrderByDescending(x => x.StartDate))
            {
                Assert.AreEqual("ERRO SOCORRO" + idx, job.Error.Message);
                idx--;
            }
        }

        [TestMethod]
        public void JobManager_Loga_Mensagens_No_Objeto_TEST()
        {
            var idList = new List<Guid>();
            for (int i = 0; i < 20; i++)
            {
                var a = i;
                idList.Add(TestJobQueue.Instance.Enqueue(
                "op-err" + i,
                () => { Thread.Sleep(10); Logger.Instance.LogError(new Exception("ERRO SOCORRO" + a)); },
                "master"));
            }

            var jobs = idList.Select(x => JobManager.Instance.FindJob(x)).ToList();

            while (jobs.Any(x => !x.Done))
            {
                Thread.Sleep(10);
            }

            var history = JobManager.Instance.GetQueues().SelectMany(x => x.GetDoneOrDoing().Where(y => y.Done));

            var idx = 19;
            foreach (var job in history.OrderByDescending(x => x.StartDate))
            {
                Assert.IsNotNull(job.Log);
                Assert.AreEqual(1, job.Log.Count);
                Assert.IsTrue(job.Log[0].Contains("ERRO SOCORRO" + idx));
                idx--;
            }
        }

        private void OnOperacao()
        {
            for (int i = 0; i < 900000; i++)
            {
                var j = i;
            }
        }

        private int OnOperacaoFuncao1(int numero)
        {
            for (int i = 0; i < 100000; i++)
            {
                var j = i;
            }

            return numero * 100;
        }
    }
}
