using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ent.Framework.Log.Log4NetService
{
    public class Log4netImp
    {
        public string RepositoryName
        {
            get { return _repositoryName; }
        }

        private string _repositoryName = "";
        private string _logDir = "";

        private ILoggerRepository _repository;
        public void InitLog()
        {
            _repository = LogManager.CreateRepository("NETCoreRepository");
            _repositoryName = _repository.Name;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4NetService", "log4net.config");
            XmlConfigurator.Configure(_repository, new FileInfo(path));

            var data = _repository.GetAppenders();
            foreach (var item in data)
            {
                if (item is RollingFileAppender fa)
                {
                    _logDir = Path.GetDirectoryName(fa.File);
                }
            }

        }

        public string GetLogDir
        {
            get { return _logDir; }
        }
    }
}
