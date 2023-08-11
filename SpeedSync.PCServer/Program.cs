using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.IO;
using System.Collections;

namespace SpeedSync.PCServer
{
    /// <summary>
    /// ����
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Ӧ�ó��������ڵ�
        /// </summary>
        /// <param name="args">����ִ̨�в���</param>
        public static void Main(string[] args)
        {
            Hashtable htStandard = new Hashtable()
            {
                { "type" , "SpeedSync_Config" },
                { "password" , "none" },
                { "port", "14325" },
                { "syncPath" , "./SyncPath/" }
            };

            Console.WriteLine("��������������ģ��...");
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
        /// ����Mvc
        /// </summary>
        public static void StartMvc()
        {
            Console.WriteLine("�ӱ��ض�ȡ�˿��ļ�...");
            int Port = Convert.ToInt32(File.ReadAllText(@"./Setting/Port.txt"));
            Console.WriteLine("Ӧ�ó���������: *:" + Port);
            Log.SaveLog("Ӧ�ó���˿ںű��趨Ϊ" + Port);
            //ָ��Mvc����ʱ����
            //ʾ���ű�: dotnet xxx.dll --urls http://*:5000
            //URL��Ҫָ��Ϊ*,����ͨ��������ڷ��ʳ�����ܻ�ܾ���������
            string[] PortArg = new string[] { "--urls", "http://*:" + Port };
            try
            {
                CreateHostBuilder(PortArg).Build().Run();
            }
            catch
            {
                return;
            }
            //����Mvcģ��
            //CreateHostBuilder(args).Build().Run();
        }
        /// <summary>
        /// ��Visual Studio���ɵ�ģ��-����Mvc
        /// </summary>
        /// <param name="args">����</param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
