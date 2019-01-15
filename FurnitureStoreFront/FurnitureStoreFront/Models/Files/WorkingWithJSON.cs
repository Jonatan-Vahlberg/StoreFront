using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace FurnitureStoreFront.Models.Files
{
    public class WorkingWithJSON<T>
        where T : class
    {
        public static readonly string StoreListPath = @"C:\Users\jonat\Source\Repos\StoreFront\FurnitureStoreFront\FurnitureStoreFront\App_Data\_REGISTRY\STORELIST.json";
        public static string UserListPath = @"C:\Users\jonat\Source\Repos\StoreFront\FurnitureStoreFront\FurnitureStoreFront\App_Data\_REGISTRY\USERLIST.json";
        

        public static bool SaveData(List<T> list,string AbsolutePath)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = ser.Serialize(list);

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

        public static List<T> GetData(string FilePath)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
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
                    List = ser.Deserialize<List<T>>(json);
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
