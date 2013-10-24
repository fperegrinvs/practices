namespace MTO.Practices.Common.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using MTO.Practices.Common.Funq;
    using MTO.Practices.Common.Logging;
    using MTO.Practices.Common.Unity;

    [TestClass]
    public class Startup
    {
        [AssemblyInitialize]
        public static void OnInit(TestContext ctx)
        {
            // inicializa o unity como failover
            UnityResolver.Init();
            // inicializa o funq como default
            FunqResolver.Init();

            Injector.Register<IContext>(null, () => new Debug.TestContext());
            Injector.Register<ILogger>("Manytoone.Logger", () => new DebugLogger());

            Logger.Instance.ApplicationName = "Unit Tests Practices";
        }
    }
}
