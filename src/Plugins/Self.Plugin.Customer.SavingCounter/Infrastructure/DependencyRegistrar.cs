using Autofac;
using Self.Plugin.Customer.SavingCounter.Services;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;

namespace Self.Plugin.Customer.SavingCounter.Infrastructure
{
    public class DependancyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<SavingCounterService>().As<ISavingCounterService>().InstancePerLifetimeScope();
        }
    }
}
