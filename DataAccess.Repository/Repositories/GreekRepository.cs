using DataAccess.Repository.Utilities;
using log4net;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Repositories
{
    public abstract class GreekRepository<T> where T : class
    {
        private readonly IFileHelper _fileHelper;
        private readonly ILog _logger;

        public GreekRepository(IFileHelper fileHelper, ILog logger)
        {
            _fileHelper = fileHelper;
            _logger = logger;
        }

        public List<T> GetDataFromSource(string sourceFilePath, string destinationFilePath, bool processFullFile=false)
        {
            sourceFilePath = string.Format(sourceFilePath, DateTime.Now.ToString("MMdd"));
            destinationFilePath = string.Format(destinationFilePath, DateTime.Now.ToString("MMdd"));
            _fileHelper.CreateDirectoryIfNotExists(destinationFilePath);

            var isNewFile = !_fileHelper.FileExists(destinationFilePath);
            if(_fileHelper.CopyFile(sourceFilePath,destinationFilePath,true))
            {
                _logger.Info($"{typeof(T).GetType().Name}: Processing delta content of file - {destinationFilePath}");
                var columnNames = string.Join(',', typeof(T).GetPropertyNames());
                var content = _fileHelper.ReadAllText(destinationFilePath);
                var fullContent = new StringBuilder();
                fullContent.Append(columnNames + Environment.NewLine);
                fullContent.Append(content);
                var lst = fullContent.ToString().FromCsv<List<T>>();

                var lst1 = new List<dynamic>(lst);
                var finallst = new List<dynamic>();
                if (!isNewFile && !processFullFile)
                {
                    var dtInputFrom = DateTime.Now.AddMinutes(-2);
                    dtInputFrom = dtInputFrom.AddSeconds(dtInputFrom.Second * -1);
                    var dtInputTill = DateTime.Now;
                    dtInputTill = dtInputTill.AddSeconds(dtInputTill.Second * -1);
                    finallst = lst1.Where(i => i.TradeDateTimeVal >= dtInputFrom && i.TradeDateTimeVal <= dtInputTill).ToList();
                    _logger.Info($"{typeof(T).GetType().Name}: Processing delta content of file From: {dtInputFrom.ToString("dd-MM-yyyy HH:mm:ss")} - {dtInputTill.ToString("dd-MM-yyyy HH:mm:ss")}");
                }
                else
                {
                    _logger.Info($"{typeof(T).GetType().Name}: New File has been placed, processing full file - {destinationFilePath}");
                    finallst = lst1.Where(i => i.TradeDateTimeVal >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)).ToList();
                }
                return finallst.Cast<T>().ToList();
            }
            return default(List<T>);
        }
    }
}
