using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data;
using System.Data.SQLite;
using Dapper;
using System.Text;
using System.Threading.Tasks;
using Service.Classes;

namespace Service
{
    public class Service
    {
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public List<Item> GetAllItems()
        {
            var items = new List<Item>();
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                items = connection.Query<Item>("select * from Item", new DynamicParameters()).ToList();
                items.Select(i => i.Attributes = GetAttributesByItemId(i.Id)).ToList();
                return items;
            }
        }

        private List<Item> GetItemById(int Id)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var attributes = connection.Query<Classes.Item>($@"select * from Item
                                                                where Id = {Id}", new DynamicParameters());
                return attributes.ToList();
            }
        }

        private List<Classes.Attribute> GetAttributesByItemId(int ItemId)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var attributes = connection.Query<Classes.Attribute>($@"select * from Attribute
                                                                where ItemId = {ItemId}", new DynamicParameters());
                return attributes.ToList();
            }
        }

        private DBRequestResult DeleteAttributesByItemId(int ItemId)
        {
            try
            {
                using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
                {
                    connection.Query<Classes.Attribute>($"delete from Attribute where ItemId = {ItemId}", new DynamicParameters());
                    connection.Query<Item>($"delete from Item where Id = {ItemId}", new DynamicParameters());
                }
                return new DBRequestResult(true);
            }
            catch (Exception)
            {
                return new DBRequestResult(false);
            }

        }

        public DBRequestResult CreateItem(Classes.Item item)
        {
            try
            {
                var existentItems = GetItemById(item.Id);
                var existentAttributes = GetAttributesByItemId(item.Id);

                if (existentItems.Any() || existentAttributes.Any())
                    DeleteAttributesByItemId(item.Id);

                using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
                {
                    connection.Execute($@"INSERT INTO Item (Id) 
                                     values ({item.Id})");

                    foreach (var attribute in item.Attributes)
                    {
                        connection.Execute($@"INSERT INTO Attribute (Title, ItemId) 
                                     values (@Title, @ItemId)", attribute);
                    }
                }
                return new DBRequestResult(true);
            }
            catch (Exception)
            {
                return new DBRequestResult(false);
            }
        }


    }




}
