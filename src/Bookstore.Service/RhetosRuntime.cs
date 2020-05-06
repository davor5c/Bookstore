﻿/*
    Copyright (C) 2014 Omega software d.o.o.

    This file is part of Rhetos.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as
    published by the Free Software Foundation, either version 3 of the
    License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using Autofac;
using Rhetos.HomePage;
using Rhetos.Logging;
using Rhetos.Security;
using Rhetos.Utilities;
using Rhetos.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

namespace Rhetos
{
    [Export(typeof(IRhetosRuntime))]
    public class RhetosRuntime : IRhetosRuntime
    {
        private readonly bool _isHost;

        public RhetosRuntime() : this(false) { }

        internal RhetosRuntime(bool isHost)
        {
            _isHost = isHost;
        }

        public IConfiguration BuildConfiguration(ILogProvider logProvider, string configurationFolder, Action<IConfigurationBuilder> addCustomConfiguration)
        {
            var configurationBuilder = new ConfigurationBuilder();

            // Main application configuration (usually Web.config).
            if (_isHost)
                configurationBuilder.AddConfigurationManagerConfiguration(); 
            else
                configurationBuilder.AddWebConfiguration(configurationFolder);

            // Rhetos runtime configuration JSON files.
            configurationBuilder.AddRhetosAppEnvironment(configurationFolder); 

            addCustomConfiguration?.Invoke(configurationBuilder);
            return configurationBuilder.Build();
        }

        public IContainer BuildContainer(ILogProvider logProvider, IConfiguration configuration, Action<ContainerBuilder> registerCustomComponents)
        {
            return BuildContainer(logProvider, configuration, registerCustomComponents, () => GetRuntimeAssemblies(logProvider, configuration));
        }

        public string[] GetRuntimeAssemblies(ILogProvider logProvider, IConfiguration configuration)
        {
            var rhetosAppOptions = configuration.GetOptions<RhetosAppOptions>();
            var legacyPaths = configuration.GetOptions<LegacyPathsOptions>();

            if (string.IsNullOrEmpty(legacyPaths.PluginsFolder))
            {
                // Application with Rhetos CLI.
                return Directory.GetFiles(rhetosAppOptions.GetAssemblyFolder(), "*.dll", SearchOption.TopDirectoryOnly);
            }
            else
            {
                // Application With DeployPackages.
                return new[] { rhetosAppOptions.GetAssemblyFolder(), legacyPaths.PluginsFolder, rhetosAppOptions.AssetsFolder }
                    .Where(folder => Directory.Exists(folder))
                    .SelectMany(folder => Directory.GetFiles(folder, "*.dll", SearchOption.TopDirectoryOnly))
                    .Distinct()
                    .ToArray();
            }
        }

        private IContainer BuildContainer(ILogProvider logProvider, IConfiguration configuration, Action<ContainerBuilder> registerCustomComponents, Func<IEnumerable<string>> pluginAssemblies)
        {
            var builder = new RhetosContainerBuilder(configuration, logProvider, pluginAssemblies);

            builder.AddRhetosRuntime();

            if (_isHost)
            {
                // WCF-specific component registrations.
                // Can be customized later by plugin modules.
                builder.RegisterType<WcfWindowsUserInfo>().As<IUserInfo>().InstancePerLifetimeScope();
                builder.RegisterType<RhetosService>().As<RhetosService>().As<IServerApplication>();
                builder.RegisterType<Rhetos.Web.GlobalErrorHandler>();
                builder.RegisterType<WebServices>();
                builder.GetPluginRegistration().FindAndRegisterPlugins<IService>();
            }

            builder.AddPluginModules();

            if (_isHost)
            {
                // HomePageServiceInitializer must be register after other core services and plugins to allow routing overrides.
                builder.RegisterType<HomePageService>().InstancePerLifetimeScope();
                builder.RegisterType<HomePageServiceInitializer>().As<IService>();
                builder.GetPluginRegistration().FindAndRegisterPlugins<IHomePageSnippet>();
            }

            registerCustomComponents?.Invoke(builder);

            return builder.Build();
        }
    }
}