using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace FurnitureStoreFront.Models.Files
{
    /// <summary>
    /// Class for handeling all reading and writing with JSON files
    /// Generic to be able to take any class type of Lists or Dictionaries
    /// </summary>
    /// <typeparam name="T">Generic Variable most used in static calls to and from a given JSON file</typeparam>
    public class WorkingWithJSON<T>
        where T : class
    {
        #region Filepaths
        //The Various Filepath used for selecting wich file to read and write to
        public static readonly string StoreListPath = HttpContext.Current.Server.MapPath(@"~\App_Data\_REGISTRY\STORELIST.json");
        public static string UserListPath = HttpContext.Current.Server.MapPath(@"~\App_Data\_REGISTRY\USERLIST.json");
        public static string CARTPath = HttpContext.Current.Server.MapPath(@"~\App_Data\_USER\UserCart");
        public static string ReceiptPath = HttpContext.Current.Server.MapPath(@"~\App_Data\_USER\UserReceipt");
        public static string PersonalPath = HttpContext.Current.Server.MapPath(@"~\App_Data\_USER\Personal");
        #endregion

        /// <summary>
        /// Save data to JSON
        /// </summary>
        /// <param name="list">List to be saved</param>
        /// <param name="Pathchoice">int to decide path</param>
        /// <returns></returns>
        public static bool SaveData(List<T> list,int Pathchoice)
        {
            string AbsolutePath = string.Empty;
            switch (Pathchoice)
            {
                case 1:
                    AbsolutePath = StoreListPath;
                    break;
                case 2:
                    AbsolutePath = UserListPath;
                    break;


            }
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                Formatting = Formatting.Indented
            };

            string json = JsonConvert.SerializeObject(list, settings);
            //Trying to write all of the list
            try
            {
                File.WriteAllText(AbsolutePath, json);
            }
            //Catching wether it is possible or not
            catch (Exception e)
            {
                Console.WriteLine("The file could not be written to:");
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }
        //public static bool SaveData(List<T> list, int Pathchoice)
        //{
        //    string AbsolutePath = string.Empty;
        //    switch (Pathchoice)
        //    {
        //        case 1:
        //            AbsolutePath = StoreListPath;
        //            break;
        //        case 2:
        //            AbsolutePath = UserListPath;
        //            break;

        //    }
        //    JavaScriptSerializer ser = new JavaScriptSerializer();
        //    string json = ser.Serialize(list);

        //    try
        //    {
        //        File.WriteAllText(AbsolutePath, json);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("The file could not be written to:");
        //        Console.WriteLine(e.Message);
        //        return false;
        //    }

        //    return true;
        //}

        //public static List<T> GetData(int Pathchoice)
        //{
        //    string FilePath = string.Empty;
        //    switch (Pathchoice)
        //    {
        //        case 1:
        //            FilePath = StoreListPath;
        //            break;
        //        case 2:
        //            FilePath = UserListPath;
        //            break;

        //    }
        //    JavaScriptSerializer ser = new JavaScriptSerializer();

        //    string json = string.Empty;
        //    List<T> List = new List<T>();

        //    if (File.Exists(FilePath))
        //    {
        //        try
        //        {
        //            using (StreamReader sr = new StreamReader(FilePath))
        //            {
        //                string line;

        //                while ((line = sr.ReadLine()) != null)
        //                {
        //                    json += line;
        //                }
        //            }
        //            List = ser.Deserialize<List<T>>(json);
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine("The file could not be read:");
        //            Console.WriteLine(e.Message);
        //        }
        //    }
        //    else
        //    {
        //        CreateJSON(FilePath);
        //    }

        //    return List;
        //}

        /// <summary>
        /// Saving spesific Cart Data
        /// </summary>
        /// <returns></returns>
        public static bool SaveCartData(List<T> list,int id,int pathchoice = 0)
        {
            
            string AbsolutePath = CARTPath + $"00{id}.json";
            switch (pathchoice)
            {
                case 0:
                    break;
                case 1:
                    AbsolutePath = ReceiptPath + $"00{id}.json";
                    break;
            }
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                Formatting = Formatting.Indented
            };

            string json = JsonConvert.SerializeObject(list, settings);

            try
            {
                File.WriteAllText(AbsolutePath, json);
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be written to:");
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Saving personal data
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool SavePersonalData(Dictionary<string,int> dict, int id)
        {

            string AbsolutePath = PersonalPath + $"00{id}.json";
            
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                Formatting = Formatting.Indented
            };

            string json = JsonConvert.SerializeObject(dict, settings);

            try
            {
                File.WriteAllText(AbsolutePath, json);
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be written to:");
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Getting Data From JSON
        /// </summary>
        /// <param name="Pathchoice">int for choosing path</param>
        /// <returns></returns>
        public static List<T> GetData(int Pathchoice)
        {
            string FilePath = string.Empty;
            switch (Pathchoice)
            {
                case 1:
                    FilePath = StoreListPath;
                    break;
                case 2:
                    FilePath = UserListPath;
                    break;

            }
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                Formatting = Formatting.Indented
            };

            string json = string.Empty;
            List<T> List = new List<T>();

            if (File.Exists(FilePath))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(FilePath))
                    {
                        string line;

                        while ((line = sr.ReadLine()) != null)
                        {
                            json += line;
                        }
                    }
                    List = JsonConvert.DeserializeObject<List<T>>(json, settings);
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                
            }

            return List;
        }

        /// <summary>
        /// Getting cart data from JSON
        /// </summary>
        /// <returns></returns>
        public static List<T> GetCartData(int id,int pathchoice)
        {
            string FilePath = CARTPath + $"00{id}.json";
            switch (pathchoice)
            {
                case 0:
                    break;
                case 1:
                    FilePath = ReceiptPath + $"00{id}.json";
                    break;
            }
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                Formatting = Formatting.Indented
            };

            string json = string.Empty;
            List<T> list = new List<T>();

            if (File.Exists(FilePath))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(FilePath))
                    {
                        string line;

                        while ((line = sr.ReadLine()) != null)
                        {
                            json += line;
                        }
                    }
                    list = JsonConvert.DeserializeObject<List<T>>(json, settings);
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                
            }

            return list;
        }

        /// <summary>
        /// Getting personal data
        /// </summary>
        /// <param name="id"> id of User</param>
        /// <returns></returns>
        public static Dictionary<string,int> GetPersonalData(int id)
        {
            string FilePath = PersonalPath + $"00{id}.json";
            
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                Formatting = Formatting.Indented
            };

            string json = string.Empty;
            Dictionary<string,int> dict = new Dictionary<string, int>();

            if (File.Exists(FilePath))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(FilePath))
                    {
                        string line;

                        while ((line = sr.ReadLine()) != null)
                        {
                            json += line;
                        }
                    }
                    dict = JsonConvert.DeserializeObject<Dictionary<string,int>>(json, settings);
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }
            }
            else
            {

            }

            return dict;
        }

        /// <summary>
        /// Creating JSON for user of id
        /// </summary>
        /// <param name="id">id of user</param>
        public static void CreateJSON(int id)
        {
            string FilePath = CARTPath + $"00{id}.json";
            File.Create(FilePath);
        }

        /// <summary>
        /// Create receipt for user 
        /// </summary>
        /// <param name="id">user id</param>
        public static void CreateReceiptJSON(int id)
        {
            string FilePath = ReceiptPath + $"00{id}.json";
            File.Create(FilePath);
        }

        /// <summary>
        /// Saving Object [OBSOLETE]
        /// </summary>
        /// <returns></returns>
        public static bool SaveObject(T obj,string Filepath)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = ser.Serialize(obj);


            try
            {
                File.WriteAllText(Filepath, json);
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be written to:");
                Console.WriteLine(e.Message);
                return false;
            }

            return true;

        }
        
        /// <summary>
        /// Getting Object [OBSOLETE]
        /// </summary>
        /// <returns></returns>
        public static T GetObject(string FilePath,T obj)
        {
            
            JavaScriptSerializer ser = new JavaScriptSerializer();

            string json = string.Empty;

            if (File.Exists(FilePath))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(FilePath))
                    {
                        string line;

                        while ((line = sr.ReadLine()) != null)
                        {
                            json += line;
                        }
                    }
                    obj = ser.Deserialize<T>(json);
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }
            }
            else
            {

            }

            return obj;
        }

        

    }
}
