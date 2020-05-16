using System;
using Caliburn.Micro;
using System.Windows;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using ScreenBrightness.Views;
using ScreenBrightness.ViewModels;

namespace ScreenBrightness.Install
{
    public class AppBootstrapper : BootstrapperBase
    {
        private IWindsorContainer _container;

        public AppBootstrapper()
        {
            Initialize();

        }

        protected override void Configure()
        {
            _container = new WindsorContainer();

            RegisterBrightnessProvider(_container);

            _container.Register(Component.For<IWindowManager>().ImplementedBy<WindowManager>().LifestyleSingleton());
            _container.Register(Component.For<IEventAggregator>().ImplementedBy<EventAggregator>().LifestyleSingleton());

            _container.Register(Component.For<MainView>().ImplementedBy<MainView>().LifestyleSingleton());
            _container.Register(Component.For<MainViewModel>().ImplementedBy<MainViewModel>().LifestyleSingleton());
        }

        private void RegisterBrightnessProvider(IWindsorContainer container)
        {
            _container.Register(Component.For<IBrightnessDxva2Invoke>().ImplementedBy<BrightnessDxva2Invoke>().LifestyleSingleton());
            _container.Register(Component.For<IBrightnessWmi>().ImplementedBy<BrightnessWmi>().LifestyleSingleton());
            _container.Register(Component.For<IBrightnessUwp>().ImplementedBy<BrightnessUwp>().LifestyleSingleton());
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = _container.Resolve(service);
            if (instance != null)
                return instance;

            throw new InvalidOperationException("Could not locate any instances.");
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            var item = _container.Resolve<MainViewModel>();
            item.Initialize();

            DisplayRootViewFor<MainViewModel>();

        }
    }
}