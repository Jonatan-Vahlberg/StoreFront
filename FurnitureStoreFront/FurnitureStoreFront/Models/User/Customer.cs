using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FurnitureStoreFront.Models.User
{
    public class Customer
    {
        public string Firstname { get; }

        public string Lastname { get; }

        public UInt64 id { get; }

        public string Email { get; }

        private string HashedPass { get; }

        private string Salt { get; }

        public Dictionary<string, int> PersonalStatisics = FillStatistics();

        public List<Cart.Cart> Cart = new List<Cart.Cart>();

        public List<Cart.Receipt> previousPurchases = new List<Cart.Receipt>();

        public static Dictionary<string,int> FillStatistics()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            dict.Add("Kitchen", 0);
            dict.Add("Outdoor", 0);
            dict.Add("Bedroom", 0);
            dict.Add("Livingroom", 0);

            return dict;
        }

        public string GetHashedPass()
        {
            return this.HashedPass;
        }

        public string GetSalt()
        {
            return this.Salt;
        }



        public string CreateSalt(int size)
        {
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public string GenerateSHA256Hash(string input,string salt)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input + salt);
            System.Security.Cryptography.SHA256Managed sHA256HastString =
                new System.Security.Cryptography.SHA256Managed();
            byte[] hash = sHA256HastString.ComputeHash(bytes);

            return ByteArrayToHexString(hash);
        }

        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static bool VerifyHashedPassword(string pass,string hashedPass, string salt)
        {
            if((pass.Length < 8))
            {
                return false;
            }

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(pass + salt);
            System.Security.Cryptography.SHA256Managed sHA256HastString =
                new System.Security.Cryptography.SHA256Managed();
            byte[] hash = sHA256HastString.ComputeHash(bytes);
            string tempHashedPass = ByteArrayToHexString(hash);
            if (tempHashedPass.Equals(hashedPass))
            {
                Console.WriteLine("PASSED");
                return true;
            }
            return false;

        }
    }

    
}