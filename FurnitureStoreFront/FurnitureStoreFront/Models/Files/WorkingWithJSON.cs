using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace FurnitureStoreFront.Models.Files
{
    public class WorkingWithJSON<T>
        where T : class
    {
        public static readonly string StoreListPath = @"C:\Users\jonat\Source\Repos\StoreFront\FurnitureStoreFront\FurnitureStoreFront\App_Data\_REGISTRY\STORELIST.json";
        public static string UserListPath = @"C:\Users\jonat\Source\Repos\StoreFront\FurnitureStoreFront\FurnitureStoreFront\App_Data\_REGISTRY\USERLIST.json";
        

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
                CreateJSON(FilePath);
            }

            return List;
        }

        public static void CreateJSON(string FilePath)
        {
            
            File.Create(FilePath);
        }

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
