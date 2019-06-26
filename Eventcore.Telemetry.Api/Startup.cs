using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Eventcore.Telemetry.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace Eventcore.Telemetry.Api
{
    public class Startup
    {
        private const string ConnectionStringSetting = "ConnectionString";
        private const string DataStoresSection = "DataStores";    
        private const string FileSystemSection = "FileSystem";
        private const string MySqlSection = "MySql";
        private const string StorageDirectorySetting = "StorageDirectory";
        private const string UseStoreSetting = "UseStore";

        // private const string DatabaseUrlSetting = "DatabaseURL";
        // private const string FirebaseSection = "Firebase";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Assembly apiAssembly = Assembly.GetAssembly(typeof(Startup));
            string apiAssemblyDirectory = Path.GetDirectoryName(apiAssembly.Location);

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(apiAssemblyDirectory, "appsettings.json"))
                .Build();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<DirectoryInfo>(new DirectoryInfo(Path.Combine(apiAssemblyDirectory, "Configuration")));
            services.AddSingleton<ITelemetryDataStore<IDictionary<string, object>>>(this.CreateDataStore(configuration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private ITelemetryDataStore<IDictionary<string, object>> CreateDataStore(IConfiguration configuration)
        {
            ITelemetryDataStore<IDictionary<string, object>> dataStore = null;
            IConfigurationSection datastoreSettings = configuration
                .GetSection(Startup.DataStoresSection);

            string useDataStore = datastoreSettings.GetValue<string>(Startup.UseStoreSetting);

            if (useDataStore == Startup.MySqlSection)
            {
                // Use Firebase Data Store
                IConfigurationSection mySqlSettings = datastoreSettings.GetSection(Startup.MySqlSection);
                MySqlConnection connection = new MySqlConnection(mySqlSettings.GetValue<string>(Startup.ConnectionStringSetting));

                dataStore = new SqlTelemetryDataStore<IDictionary<string, object>>(connection);
            }
            else if (useDataStore == Startup.FileSystemSection)
            {
                // Use File System Data Store
                IConfigurationSection fileSystemSettings = datastoreSettings.GetSection(Startup.FileSystemSection);

                dataStore = new FileSystemTelemetryDataStore<IDictionary<string, object>>(
                    new DirectoryInfo(fileSystemSettings.GetValue<string>(Startup.StorageDirectorySetting)));
            }

            return dataStore;
        }
    }
}
