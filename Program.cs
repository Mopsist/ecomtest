using Newtonsoft.Json.Linq;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace EcomTest
{
    class PaymentInfo
    {
        public string payment_currency { get; set; }
        public string payment_amount { get; set; }
        public string payment_id { get; set; }
        public string project_id { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string json = @"{
                    payment_id : 'FFCD12-35',
                    project_id : '1341',
                    payment_currency : 'USD',
                    payment_amount : '10'                
                   }";

            JObject o = JObject.Parse(json);
            //Console.WriteLine(o + "\n");

            //Приводим свойства объекта к нужному формату строк и сортируем
            List<string> props = new List<string>();
            foreach(var item in o)
            {
                props.Add(item.Key + ":" + item.Value);
            }
            props.Sort();
            //Объединяем в одну строку с разделителем ";"
            string oneString = String.Join(';', props);
            Console.WriteLine(oneString);

            string secret = "7efc48eaba4f540db1daed87b937c0e7cb87196715fdebf63295285fa1873a236aafd440166e1e88d1641559f17a5ef873b31703532a8debb8a5458d75821412";
            var key = Encoding.UTF8.GetBytes(secret);

            //Первый способ 
            Console.WriteLine("1)");
            byte[] hashValue = { };
            using (HMACSHA512 hmac = new HMACSHA512(key))
            {
                hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(oneString));
                
                hashValue.ToList().ForEach(i => Console.Write(i));
            }
            Console.WriteLine();
            var base64 = Convert.ToBase64String(hashValue);
            Console.WriteLine(base64);

            //Второй способ (непонятно куда засунуть secret_key)
            Console.WriteLine("2)");
            var alg = SHA512.Create();
            alg.ComputeHash(Encoding.UTF8.GetBytes(oneString));
            alg.Hash.ToList().ForEach(i => Console.Write(i));
            Console.WriteLine();
            Console.WriteLine(Convert.ToBase64String(alg.Hash));

            //Третий способ (???)
            Console.WriteLine("3)");
            var hash = BitConverter.ToString(alg.Hash);
            Console.WriteLine(hash);

            //Четвертый способ
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.PersistKeyInCsp = false;
                
                return rsa.SignData(data, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
            }

            Console.ReadLine();
        }
       
        public static string GetSignature(JObject obj)
        {
            return "";
        }
    }
}
