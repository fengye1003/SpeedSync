using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.IO;
using System.Collections;

namespace SpeedSync.PCServer
{
    /// <summary>
    /// 主类
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 应用程序的主入口点
        /// </summary>
        /// <param name="args">控制台执行参数</param>
        public static void Main(string[] args)
        {
            Hashtable htStandard = new Hashtable()
            {
                { "type" , "SpeedSync_Config" },
                { "password" , "none" },
                { "port", "14325" },
                { "syncPath" , "./SyncPath/" }
            };

            Console.WriteLine("尝试运行主程序模块...");
            Thread MvcThread = new Thread(StartMvc)
            {
                Name = "SpeedSync-MVC"
            };
            MvcThread.Start();
            while (true)
            {
                Thread.Sleep(5000);
            }
        }
        
        
        /// <summary>
        /// 启动Mvc
        /// </summary>
        public static void StartMvc()
        {
            Console.WriteLine("从本地读取端口文件...");
            int Port = Convert.ToInt32(File.ReadAllText(@"./Setting/Port.txt"));
            Console.WriteLine("应用程序将运行在: *:" + Port);
            Log.SaveLog("应用程序端口号被设定为" + Port);
            //指定Mvc加载时参数
            //示例脚本: dotnet xxx.dll --urls http://*:5000
            //URL需要指定为*,否则通过其他入口访问程序可能会拒绝连接请求
            string[] PortArg = new string[] { "--urls", "http://*:" + Port };
            try
            {
                CreateHostBuilder(PortArg).Build().Run();
            }
            catch
            {
                return;
            }
            //构建Mvc模块
            //CreateHostBuilder(args).Build().Run();
        }
        /// <summary>
        /// 由Visual Studio生成的模块-加载Mvc
        /// </summary>
        /// <param name="args">操作</param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
