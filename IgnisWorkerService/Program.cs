using IgnisWorkerService;
using IgnisWorkerService.Data;
using IgnisWorkerService.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Debugging;
using Serilog.Sinks.PostgreSQL;
using System.Diagnostics;

internal class Program
{
    private static IConfiguration? _configuration = null;
    private static string? _connectionString;

    private static void Main(string[] args)
    {

        // init configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets<Program>(optional: true)
            .Build();

        _configuration = configuration;

        // get connection string
        _connectionString = configuration.GetConnectionString("IgnisDb");


        // init logger
        Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
            .MinimumLevel.Warning()
            .Enrich.FromLogContext()    
            .WriteTo.Console()
            .WriteTo.PostgreSQL(
            connectionString:_connectionString,
            tableName:"AppLogs",
            columnOptions: new Dictionary<string, ColumnWriterBase>
                {
                    { "Message", new RenderedMessageColumnWriter() },
                    { "Level", new LevelColumnWriter(true, NpgsqlTypes.NpgsqlDbType.Text) }, // Forces level to plain text
                    { "Timestamp", new TimestampColumnWriter() },
                    { "Exception", new ExceptionColumnWriter() },
                    { "Properties", new PropertiesColumnWriter() }
                },
            needAutoCreateTable:false
            ).CreateLogger();


        SelfLog.Enable(Console.Out);  // debug serilog logging issues

        try
        {
            var message = $"Hello, Ignis! today is {DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")}";
            Log.Logger.Warning(message);

            // check if the application is run with a specfic command-line argument
            var _consoleMode = args.Contains("--console");

            var isService = !(Debugger.IsAttached || _consoleMode);
            var builder = CreateHostBuilder(args.Where(arg => arg != "--console").ToArray());

            if(isService)
            {
                builder.UseWindowsService();
            }

            if (_consoleMode)
            {
                Log.Warning("Running in console model...");
            }
            else
            {
                Log.Warning("Running in background as a service");
            }

            var host = builder.Build();

            host.Run();

        }
        catch (Exception ex) 
        {
            Log.Fatal(ex, "There was a problem starting the service");
        }
        finally
        {
            Log.CloseAndFlush();
        }



        //var builder = Host.CreateApplicationBuilder(args);
        //builder.Services.AddHostedService<Worker>();
        //var host = builder.Build();
        //host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureAppConfiguration((hostingctx, config) =>
        {
            config.SetBasePath(Directory.GetCurrentDirectory());
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            config.AddEnvironmentVariables();
            config.AddUserSecrets<Program>(optional: true);
        })
        .ConfigureServices((hostctx, services) =>
        {
            IConfiguration config = hostctx.Configuration;


            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(hostctx.Configuration.GetConnectionString("IgnisDb"));
                options.EnableSensitiveDataLogging(false);
            });
            // initialize service workers here
            services.AddHostedService<IgnisWorker>();
            Log.Warning("Starting Worker Services...");
        });
}