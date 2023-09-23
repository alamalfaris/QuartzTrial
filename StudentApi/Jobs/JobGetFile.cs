using log4net;
using Quartz;
using Renci.SshNet;

namespace StudentApi.Jobs
{
    [DisallowConcurrentExecution]
    public class JobGetFile : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(JobGetFile));
        private readonly string _host;
        private readonly string _username;
        private readonly string _hash;
        private readonly string _baseServerDirectory;
        private readonly string _baseClientDirectory;
        private readonly string _folderMtrLivin;
        private readonly string _folderMtrSmartBranch;

        public JobGetFile(IConfiguration configuration)
        {
            _host = configuration.GetSection("JobGetFile:Host").Value;
            _username = configuration.GetSection("JobGetFile:Username").Value;
            _hash = configuration.GetSection("JobGetFile:Hash").Value;
            _baseServerDirectory = configuration.GetSection("JobGetFile:BaseServerDirectory").Value;
            _baseClientDirectory = configuration.GetSection("JobGetFile:BaseClientDirectory").Value;
            _folderMtrLivin = configuration.GetSection("JobGetFile:FolderMtrLivin").Value;
            _folderMtrSmartBranch = configuration.GetSection("JobGetFile:FolderMtrSmartBranch").Value;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.Info("Job start");
                await GetFilesThenDownload(_folderMtrLivin);
                await GetFilesThenDownload(_folderMtrSmartBranch);
                _logger.Info("Job end");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }

            await Task.CompletedTask;
        }

        public async Task<SftpClient> GetClient()
        {
            var client = new SftpClient(new PasswordConnectionInfo(_host, _username, _hash));
            client.Connect();
            return await Task.FromResult(client);
        }

        public async Task GetFilesThenDownload(string folder)
        {
            // Get files from folder MTR-Livin in server directory
            // Then download to client directory
            using (var client = GetClient().Result)
            {
                var files = client.ListDirectory($"{_baseServerDirectory}{folder}");
                if (files.Any())
                {
                    foreach (var serverFile in files)
                    {
                        using (var stream = File.OpenWrite($"{_baseClientDirectory}{folder}{serverFile.Name}"))
                        {
                            client.DownloadFile(serverFile.FullName, stream);
                            client.DeleteFile(serverFile.FullName);
                        }
                    }
                }
                else
                {
                    _logger.Info($"Folder {folder} empty");
                }
                client.Disconnect();
            }
            await Task.CompletedTask;
        }
    }
}
