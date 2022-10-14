using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data;
using System.Data.SQLite;
using Dapper;
using Service.Library.Classes;

namespace Service.Library
{
    public class DataBaseService
    {
        private static string GetConnectionString(string name = "SqliteDB")
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public List<ItemModel> GetAllItems()
        {
            var items = new List<ItemModel>();
            var sql = "select * from Item";
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                items = connection.Query<ItemModel>(sql, new DynamicParameters()).ToList();
                items.Select(i => i.Attributes = GetAttributesByItemId(i.Id)).ToList();
                return items;
            }
        }

        private ItemModel GetItemModelById(int Id)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                var item = new ItemModel();
                var sql = @"select * from Item where Id = @Id;
                            select * from Attribute where ItemId = @Id;";

                var parameter = new
                {
                    Id = Id
                };

                using (var multiSqlQuery = connection.QueryMultiple(sql, parameter))
                {
                    item = multiSqlQuery.Read<ItemModel>().FirstOrDefault();
                    if (item != null)
                        item.Attributes = multiSqlQuery.Read<AttributeModel>().ToList();
                }

                return item;
            }
        }

        private DBRequestResult DeleteItemModelById(int Id)
        {
            try
            {
                var itemToDelete = GetItemModelById(Id);
                if (itemToDelete != null)
                {
                    using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
                    {
                        var item = new ItemModel();
                        var sql = @"delete from Item where Id = @Id;
                                    delete from Attribute where ItemId = @Id;";

                        var parameter = new
                        {
                            Id = Id
                        };

                        connection.Execute(sql, parameter);
                        return new DBRequestResult(true);
                    }
                }
                return new DBRequestResult(false);
            }
            catch (Exception ex)
            {
                return new DBRequestResult(ex);
            }

        }

        private List<AttributeModel> GetAttributesByItemId(int ItemId)
        {
            using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                var attributes = connection.Query<AttributeModel>($@"select * from Attribute
                                                                where ItemId = {ItemId}", new DynamicParameters());
                return attributes.ToList();
            }
        }

        public DBRequestResult CreateItem(ItemModel item)
        {
            try
            {
                var existentItem = GetItemModelById(item.Id);

                if (existentItem != null)
                    DeleteItemModelById(item.Id);

                using (IDbConnection connection = new SQLiteConnection(GetConnectionString()))
                {
                    connection.Execute($@"INSERT INTO Item (Id) values ({item.Id})");

                    foreach (var attribute in item.Attributes)
                    {
                        connection.Execute($@"INSERT INTO Attribute (Title, ItemId) values (@Title, @ItemId)", attribute);
                    }
                }
                return new DBRequestResult(true);
            }
            catch (Exception ex)
            {
                return new DBRequestResult(ex);
            }
        }

    }
}
