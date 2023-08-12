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
            var config = PropertiesHelper.AutoCheck(htStandard, "./config.properties");
            if ((string)config["type"] != "SpeedSync_Config")
            {
                Log.SaveLog("错误:加载的config.properties不是SpeedSync的配置文件!正在重置应用程序配置...");
                File.Delete("./config.properties");
                config = PropertiesHelper.AutoCheck(htStandard, "./config.properties");
            }
            if ((string)config["password"] == "none")
            {
                Log.SaveLog("欢迎使用SpeedSync!请先设置应用程序!\n\n");
                Log.SaveLog("第一步:请设置访问密码\n为了安全起见,每次设备进行连接时都需要校验您的访问密码,并取得访问密钥.您的密码应当是不少于6位的字母+数字的组合");
                Console.Write("请输入密码并按下回车键:");
                var passwd = Console.ReadLine();
                Console.Write("请再次输入密码并按下回车键:");
                if (passwd != Console.ReadLine()) 
                {
                    Log.SaveLog("两次输入的密码不一致!请核对后再次输入!!");
                    Main(args);
                    return;
                }
                config["password"] = passwd;
                Console.Clear();
                Log.SaveLog("第二步:设置服务端口\nSpeedSync程序支持用户自定义服务端口,默认值为14325,如果此端口已经被占用或您希望使用其他端口,请在下方输入.如果没有此需求或者不知道这是什么,请直接按下回车.");
                Console.Write("如果您需要修改端口,请输入端口号:");
                var userPort = Console.ReadLine();
                int userPortNum = 14325;
                try
                {
                    if (userPort != "") 
                    {
                        userPortNum = Convert.ToInt32(userPort);
                    }
                    else
                    {
                        userPort = "14325";
                    }
                    
                }
                catch (Exception ex)
                {
                    Log.SaveLog($"引发异常:{ex}");
                    Log.SaveLog("请检查您输入的端口号!您的输入无法被识别为数字!设置程序已中断.");
                    Main(args);
                    return;
                }


            }
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
