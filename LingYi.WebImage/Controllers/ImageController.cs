using Cicada;
using Cicada.DI;
using Cicada.Configuration;
using Cicada.FileSystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using LingYi.DataTransfer.Models.Message;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Text;
using LY.UserAuthorize;
using Im.Rpc.Mysql;

namespace LingYi.WebImage.Controllers
{
    public class ImageController : ApiController
    {
        private readonly IFileSystem _fileService;
        private readonly MysqlService.Iface _mysqlService;
        private readonly UserInfoService.Iface _userService;
        public ImageController(IFileSystem fileService, MysqlService.Iface mysqlService, UserInfoService.Iface userService)
        {
            _userService = userService;
            _fileService = fileService;
            _mysqlService = mysqlService;
        }
        [HttpPost]
        [NonAction]
        [UserAuthorization]
        public async Task<HttpResponseMessage> Upload(int fileType)
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            var provider = new MultipartMemoryStreamProvider();
            List<tbl_FileManager> fileList = new List<tbl_FileManager>();
            //开始上传文件
            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var file in provider.Contents)
            {
                var FileName = file.Headers.ContentDisposition.FileName;
                var FileExtensionName = Path.GetExtension(FileName.Replace("\"", string.Empty)).Replace(".", "");
                var FileDirectory = _fileService.Upload(await file.ReadAsByteArrayAsync(), FileExtensionName);
                if (null != file.Headers.ContentType)
                {
                    if (file.Headers.ContentType.MediaType.IndexOf("video") >= 0)
                    {
                        var config = CicadaFacade.Container.Resolve<IConfigurationDataRespository>();
                        string basePath = "c:\\MediaTool\\";
                        string ffmpeg_exe = basePath + "ffmpeg.exe";
                        string temp_files = basePath + Guid.NewGuid() + ".jpg";
                        string arguments = " -i " + config.Get("Cicada.FileSystem.LocalUrlPrefix") + "/fs/" + FileDirectory;
                        ProcessStartInfo startInfo = new ProcessStartInfo(ffmpeg_exe);
                        startInfo.CreateNoWindow = true;
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo.UseShellExecute = false;
                        startInfo.Arguments = arguments + " -y -f image2 -ss 00:00:01 -t 0.001 -vf transpose=1 " + temp_files;
                        try
                        {
                            using (Process proc = Process.Start(startInfo))
                            {
                                proc.WaitForExit();
                                if (File.Exists(temp_files))
                                {
                                    var bytes = File.ReadAllBytes(temp_files);
                                    using (var fileStream = new MemoryStream(bytes))
                                    {
                                        FileDirectory += ";" + _fileService.Upload(fileStream.ToArray(), "jpg");
                                    }
                                    File.Delete(temp_files);
                                }
                            }
                        }
                        catch { }
                    }
                }
                var FileModel = new tbl_FileManager
                {
                    FileID = Guid.NewGuid().ToString(),
                    FileDirectory = FileDirectory,
                    FileDate = DateTime.Now.ToString("yyyyMMddHHmmss")
                };
                _mysqlService.FileInsert(JsonConvert.SerializeObject(FileModel));
                fileList.Add(FileModel);
            }
            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    Data = fileList.Select(s=>s.FileID).ToList(),
                    Status = 200,
                    Message = "成功加载数据"
                }), Encoding.GetEncoding("UTF-8"))
            };
            return resp;
        }

        [HttpGet]
        public HttpResponseMessage UserImage(string userID)
        {
            var config = CicadaFacade.Container.Resolve<IConfigurationDataRespository>();
            var ImageBase = config.Get("Cicada.FileSystem.UrlPrefix");
            var UserInfo=_userService.GetUserInfo(userID);
            var ImageUrl = UserInfo.UserInfo.UserImage.Replace(ImageBase, "");
            var ImageByte = _fileService.Download(ImageUrl);
            var MediaType = "image/" + Path.GetExtension(ImageUrl).TrimStart('.');
            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ImageByte)
            };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaType);
            return resp;
        }

        [HttpGet]
        public HttpResponseMessage BussFiles(string fileID)
        {
            var FileManager  = _mysqlService.GetFileByID(fileID);
            var FileInfo = JsonConvert.DeserializeObject<tbl_FileManager>(FileManager);
            var ImageByte = _fileService.Download(FileInfo.FileDirectory);
            var MediaType = "image/" + Path.GetExtension(FileInfo.FileDirectory).TrimStart('.');
            if (FileInfo.FileType != 1)
                MediaType = "application/octet-stream";
            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ImageByte)
            };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaType);
            return resp;
        }
    }
}
