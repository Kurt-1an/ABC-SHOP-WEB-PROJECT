using Serilog;

// namespace PSEConnect.Client.AppsSettingsHelper;
public static class AppSettingsHelper {
    private static IConfiguration _config;
        public static void SetConfig(IConfiguration configuration)
        {
            _config = configuration;
        }

    #region SERILOG
    public static void EnableLogger()
    {
        try
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File(_config["LogPath"] + "/log_.txt", rollOnFileSizeLimit: true, shared: true,
            fileSizeLimitBytes: 10000000, rollingInterval: RollingInterval.Day).CreateLogger();
            Log.Information("START PROCESS");
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
        } 
    }
    #endregion

    #region SQL DB CONNECTIONS
    public static string AbcDbConnection => _config["ConnectionStrings:DefaultConnection"];
    #endregion
    
}