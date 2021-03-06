﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace FurnitureStoreFront.Models.User
{
    public class Customer
    {
        #region Object properties
        /// <summary>
        /// First name of the customer
        /// </summary>
        public string Firstname { get;  }
        /// <summary>
        /// Last name of the customer
        /// </summary>
        public string Lastname { get;  }
        /// <summary>
        /// Id of the customer
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Email of the customer
        /// </summary>
        public string Email { get; }
        /// <summary>
        /// The hashed version of the customers password stored for safety
        /// </summary>
        public string HashedPass { get; }
        /// <summary>
        /// Salt for the password of the customer
        /// </summary>
        public string Salt { get;}
        
        /// <summary>
        /// A Dictonary containing filling out the statistics for a Customerr
        /// </summary>
        public Dictionary<string, int> PersonalStatisics = FillStatistics();
        
        /// <summary>
        /// Method for initial filling of the Customers statistics
        /// </summary>
        /// <returns>A initial dictionary</returns>
        public static Dictionary<string,int> FillStatistics()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            dict.Add("Kitchen", 0);
            dict.Add("Outdoors", 0);
            dict.Add("Bedroom", 0);
            dict.Add("Livingroom", 0);

            return dict;
        }

        /// <summary>
        /// These are purchases that have been handeld and processed
        /// </summary>
        public List<Cart.Receipt> previousPurchases = new List<Cart.Receipt>(); 
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor for a Customer
        /// </summary>
        /// <param name="id">Inputted id</param>
        /// <param name="fn">Inputted First name</param>
        /// <param name="ln">Inputted last name</param>
        /// <param name="email">Inputted Email</param>
        /// <param name="hp">The hashed password</param>
        /// <param name="s">the salt of the password</param>
        [JsonConstructor]
        public Customer(int id, string firstname,string lastname, string email,string hashedPass, string salt)
        {
            this.id = id;
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Email = email;
            this.HashedPass = hashedPass;
            this.Salt = salt;
            previousPurchases = Files.WorkingWithJSON<Cart.Receipt>.GetCartData(this.id, 1);
        }
        #endregion


        #region Methods
        /// <summary>
        /// Getter for private Hashed password
        /// </summary>
        /// <returns><see cref="HashedPass"/></returns>
        public string GetHashedPass()
        {
            return this.HashedPass;
        }
        
        /// <summary>
        /// Getter for private Salt for password
        /// </summary>
        /// <returns><see cref="Salt"/></returns>
        public string GetSalt()
        {
            return this.Salt;
        }


        /// <summary>
        /// One time generate a salt of length: size
        /// </summary>
        /// <param name="size">The Length of a given salt standard is 8</param>
        /// <returns>A salted string</returns>
        public static string CreateSalt(int size)
        {
            //Gets a random generator
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            //Set the size of the array
            var buff = new byte[size];
            //Get the array of random chars
            rng.GetBytes(buff);
            //Return the array of random chars as strings
            return Convert.ToBase64String(buff);
        }

        /// <summary>
        /// Generates a SHA256-Hashed Password and returns in Hex
        /// </summary>
        /// <param name="input">The password</param>
        /// <param name="salt">The salt of the password</param>
        /// <returns></returns>
        public static string GenerateSHA256Hash(string input,string salt)
        {
            //Get the bytes of the password and salt combiend
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input + salt);
            
            //Generate a SHA256Managed
            //This part is SUPER complicated for reasons of security as such i can say i understand this part but not inner workings
            System.Security.Cryptography.SHA256Managed sHA256HashString =
                new System.Security.Cryptography.SHA256Managed();

            //return the SHA256 to a byte array
            byte[] hash = sHA256HashString.ComputeHash(bytes);

            //return string in hexadicmal from
            return ByteArrayToHexString(hash);
        }

        /// <summary>
        /// Turns byte array into Hexadecimal string
        /// </summary>
        /// <param name="ba">byte array</param>
        /// <returns></returns>
        public static string ByteArrayToHexString(byte[] ba)
        {
            //Build a string twice the length of the current byte array
            StringBuilder hex = new StringBuilder(ba.Length * 2);

            //Foreach byte in array make the byte into Hex decimal
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);

            //Returns a string in hexadecimal
            return hex.ToString();
        }


        /// <summary>
        /// Verifys the given password aginst the hashed one using the salt generated at object declaration
        /// </summary>
        /// <param name="pass">Password inputed by Customer</param>
        /// <param name="hashedPass">The hashed version of the password</param>
        /// <param name="salt">salt of the password</param>
        /// <returns>Bool value to determin if cleared to move forward in Login</returns>
        public static bool VerifyHashedPassword(string pass,string hashedPass, string salt)
        {
            if(!InitialPasswordCheck(pass))
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

        /// <summary>
        /// Initial fill for list of type customer
        /// </summary>
        /// <returns></returns>
        public static List<Customer> getUsers()
        {
            List<Customer> c = new List<Customer>();
            string salt = CreateSalt(8);
            c.Add(new Customer (1,"ADMIN", "ACC", "Email@provider.com",GenerateSHA256Hash("P4ssword",salt),salt));
            salt = CreateSalt(8);
            c.Add(new Customer (2,"Markus", "Sontag", "Markus@Sontag.com",GenerateSHA256Hash("M4rkassBrown",salt),salt));

            return c;
        }

        /// <summary>
        /// Initial check for login checks if email exsists
        /// </summary>
        /// <param name="email">inputted email</param>
        /// <param name="CustomerList">list to search in</param>
        /// <returns>a object copy of indexed list value</returns>
        public static Customer getCustomerData(string email, List<Customer> CustomerList)
        {
            //Turnary operator for finding a user trough email
            var selected = CustomerList.Where(x => x.Email == email).FirstOrDefault();

            return selected;
        }

        /// <summary>
        /// Itiial password check for registring and login
        /// </summary>
        /// <param name="password">the password in question that the Customer inputed</param>
        /// <returns></returns>
        public static bool InitialPasswordCheck(string password)
        {
            if(password.Length >= 8 )
            {
                if ((password.Any(char.IsUpper)) && (password.Any(char.IsNumber)))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds a items room into the Personal Statistics of a User
        /// </summary>
        /// <param name="dict">Personal Statistics</param>
        /// <param name="id">Id of item in store stock</param>
        /// <param name="list">Storestock</param>
        /// <returns></returns>
        public static Dictionary<string,int> AddToPersonalStatistics(Dictionary<string,int> dict, int id, List<StoreItem.Furniture> list )
        {
            dict[list[id].Room] += 1;

            return dict;
        }
        
        /// <summary>
        /// Gets the prefered room of any User ALGORITHM
        /// </summary>
        /// <param name="Dict">Personal Statistics Dictionary</param>
        /// <returns></returns>
        private static List<string> GetPreferedRoom(Dictionary<string,int> Dict)
        {

            List<String> list = new List<string>();
            //Sorts dictionary inorder of most purchases
            var SortedDict = from entry in Dict orderby entry.Value descending select entry;

            //returns a list of rooms that has a value other than 0 and is therefor pertenant to user
            foreach(KeyValuePair<string,int> entry in SortedDict)
            {
                if(entry.Value != 0)
                {
                    list.Add(entry.Key);
                }
            }

            return list;
                
        }

        /// <summary>
        /// Gets the prefered items using an orderby function ALGORITHM
        /// </summary>
        /// <param name="StoreItems">Store stock</param>
        /// <param name="Dict">A dictionary to send to <see cref="GetPreferedRoom(Dictionary{string, int})">Get prefered room method</see></param>
        /// <returns></returns>
        public static List<StoreItem.Furniture> GetPreferedItems(List<StoreItem.Furniture> StoreItems, Dictionary<string,int> Dict){

            //Returns a list of possible rooms of intrest to user
            var list = GetPreferedRoom(Dict);
            List<StoreItem.Furniture> PersonalItems = new List<StoreItem.Furniture>();

            //Returns items similar to most valued items as user either has no prefrence or is split 4ways on the categories
            if(list.Count == 0 || list.Count == 4)
            {
                PersonalItems = ListOrderBy(StoreItems);
            }
            //User only intrested in one category
            else if(list.Count == 1)
            {
                PersonalItems = StoreItems.FindAll(x => x.Room == list[0]);
                PersonalItems = ListOrderBy(PersonalItems);
            }
            //User only intrested in two categories
            else if (list.Count == 2)
            {
                PersonalItems = StoreItems.FindAll(x => x.Room == list[0] || x.Room == list[1]);
                PersonalItems = ListOrderBy(PersonalItems);
            }
            //User intrested in three categories
            else if (list.Count == 3)
            {
                PersonalItems = StoreItems.FindAll(x => x.Room == list[0] || x.Room == list[1] || x.Room == list[2]);
                PersonalItems = ListOrderBy(PersonalItems);
            }

            //Returns list for output
            return PersonalItems;

        }

        /// <summary>
        /// Simple Orderby method fo items Ordering by decending order
        /// </summary>
        /// <param name="StoreItems">Store stock</param>
        /// <returns></returns>
        public static List<StoreItem.Furniture> ListOrderBy(List<StoreItem.Furniture> StoreItems)
        {
            List<StoreItem.Furniture> PersonalItems = new List<StoreItem.Furniture>();

            var result =
                from o in StoreItems
                orderby o.TotalPurchases descending
                select o;
            foreach (var item in result)
            {
                PersonalItems.Add(item);
            }
            return PersonalItems;
        }
        #endregion
    }

    
}