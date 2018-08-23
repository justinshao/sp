using Common.Entities.BaseData;
using System.Collections.Generic;
using System.ServiceModel;

namespace PlatformWcfServers
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IParkDownloadDataCallback
    {

        /// <summary>
        /// 下载信息
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        List<UploadResult> DownloadDataCallBack(string cmd, string list);
    }
}