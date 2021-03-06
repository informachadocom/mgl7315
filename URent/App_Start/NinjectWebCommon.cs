[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(URent.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(URent.App_Start.NinjectWebCommon), "Stop")]

namespace URent.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Models.Interfaces;
    using Models.Util;
    using Models.Manager;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch (Exception e)
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ICategoryManager>().To<CategoryManager>().Named("Prod");
            kernel.Bind<IOptionManager>().To<OptionManager>().Named("Prod");
            kernel.Bind<IClientManager>().To<ClientManager>().Named("Prod");
            kernel.Bind<ICarManager>().To<CarManager>().Named("Prod");
            kernel.Bind<IReservationManager>().To<ReservationManager>().Named("Prod");
            kernel.Bind<ISearch>().To<SearchManager>().Named("Prod");
            kernel.Bind<IRentPriceManager>().To<RentPriceManager>().Named("Prod");
            kernel.Bind<IRentManager>().To<RentManager>().Named("Prod");
            kernel.Bind<IUser>().To<UserManager>().Named("Prod");
            kernel.Bind<IHelper>().To<Helper>().Named("Prod");
            kernel.Bind<ICrypt>().To<Crypt>().Named("Prod");
        }
    }
}