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
            var config = PropertiesHelper.AutoCheck(htStandard, "./config.properties");
            if ((string)config["type"] != "SpeedSync_Config")
            {
                Log.SaveLog("����:���ص�config.properties����SpeedSync�������ļ�!��������Ӧ�ó�������...");
                File.Delete("./config.properties");
                config = PropertiesHelper.AutoCheck(htStandard, "./config.properties");
            }
            if ((string)config["password"] == "none")
            {
                Log.SaveLog("��ӭʹ��SpeedSync!��������Ӧ�ó���!\n\n");
                Log.SaveLog("��һ��:�����÷�������\nΪ�˰�ȫ���,ÿ���豸��������ʱ����ҪУ�����ķ�������,��ȡ�÷�����Կ.��������Ӧ���ǲ�����6λ����ĸ+���ֵ����");
                Console.Write("���������벢���»س���:");
                var passwd = Console.ReadLine();
                Console.Write("���ٴ��������벢���»س���:");
                if (passwd != Console.ReadLine()) 
                {
                    Log.SaveLog("������������벻һ��!��˶Ժ��ٴ�����!!");
                    Main(args);
                    return;
                }
                config["password"] = passwd;
                Console.Clear();
                Log.SaveLog("�ڶ���:���÷���˿�\nSpeedSync����֧���û��Զ������˿�,Ĭ��ֵΪ14325,����˶˿��Ѿ���ռ�û���ϣ��ʹ�������˿�,�����·�����.���û�д�������߲�֪������ʲô,��ֱ�Ӱ��»س�.");
                Console.Write("�������Ҫ�޸Ķ˿�,������˿ں�:");
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
                    Log.SaveLog($"�����쳣:{ex}");
                    Log.SaveLog("����������Ķ˿ں�!���������޷���ʶ��Ϊ����!���ó������ж�.");
                    Main(args);
                    return;
                }


            }
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
