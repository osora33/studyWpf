using Bogus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;

namespace DummyDataApp
{
    class Program
    {
        public static string MqttBrokerUrl { get; set; }
        public static MqttClient Client { get; set; }
        private static Thread MqttThread { get; set; }
        private static Faker<SensorInfo> SensorData { get; set; }
        private static string CurrValue { get; set; }
        static void Main(string[] args)
        {
            /*while (true)
            {
                var Rooms = new[] { "DINING", "LIVING", "BATH", "BED" };    //식당, 거실, 욕실, 침실
                var sensorDummy = new Faker<SensorInfo>()
                    .RuleFor(r => r.DevId, f => f.PickRandom(Rooms))
                    .RuleFor(r => r.CurrTime, f => f.Date.Past(0))
                    .RuleFor(r => r.Temp, f => f.Random.Float(19.0f, 39.0f))
                    .RuleFor(r => r.Humid, f => f.Random.Float(40.0f, 63.9f));

                var currValue = sensorDummy.Generate();
                Console.WriteLine(JsonConvert.SerializeObject(currValue, Formatting.Indented));

                Thread.Sleep(1000);
            }*/
            InitializeConfig();     //구성초기화
            ConnetMqttBroker();     //브로커에 접속
            StartPublish();         //배포(Publish 발행)
        }

        private static void StartPublish()
        {
            MqttThread = new Thread(() => LoopPublish());
            MqttThread.Start();
        }

        private static void LoopPublish()
        {
            while (true)
            {
                SensorInfo tempValue = SensorData.Generate();
                CurrValue = JsonConvert.SerializeObject(tempValue, Formatting.Indented);
                Client.Publish("home/device/fakedata/", Encoding.Default.GetBytes(CurrValue));
                Console.WriteLine($" : {CurrValue}");
                Thread.Sleep(1000);
            }
        }

        private static void ConnetMqttBroker()
        {
            try
            {
                Client = new MqttClient(MqttBrokerUrl);
                Client.Connect("SMARTHOME01");
                Console.WriteLine("접속성공");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"접속불가 : {ex}");
                Environment.Exit(5);
            }
        }

        private static void InitializeConfig()
        {
            MqttBrokerUrl = "localhost";
            var Rooms = new[] { "DINING", "LIVING", "BATH", "BED" };    //식당, 거실, 욕실, 침실
            SensorData = new Faker<SensorInfo>()
                .RuleFor(r => r.DevId, f => f.PickRandom(Rooms))
                .RuleFor(r => r.CurrTime, f => f.Date.Past(0))
                .RuleFor(r => r.Temp, f => f.Random.Float(19.0f, 39.0f))
                .RuleFor(r => r.Humid, f => f.Random.Float(40.0f, 63.9f));
        }
    }
}
