
﻿/*
This file is part of LazyBot - Copyright (C) 2011 Arutha

    LazyBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    LazyBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with LazyBot.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;

namespace LazyLib.Helpers.Vendor
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class ItemDatabase
    {
        private static readonly SQLiteConnection Db = new SQLiteConnection("Data Source=database.db3");
        public static bool IsOpen { get; private set; }
        /// <summary>
        /// Opens the database and checks schema
        /// </summary>
        public static void Open()
        {
            IsOpen = true;
            Db.Open();
            CheckSchema();
        }

        public static void Close()
        {
            IsOpen = false;
            Db.Close();
        }

        /// <summary>
        /// Checks the database for all the tables,
        /// Creating them when necessary.
        /// </summary>
        /// <returns></returns>
        private static void CheckSchema()
        {
            Query("CREATE TABLE IF NOT EXISTS items (id INTEGER PRIMARY KEY NOT NULL, item_id INTEGER UNIQUE, item_name VARCHAR(255) UNIQUE, item_quality VARCHAR(255));");
        }

        /// <summary>
        /// Used for queries that return nothing
        /// Example. INSERT, DELETE
        /// </summary>
        /// <returns></returns>
        private static void Query(string sql)
        {
            SQLiteCommand query = Db.CreateCommand();
            query.CommandText = sql;
            query.ExecuteNonQuery();
        }

        /// <summary>
        /// Used for inserting into the database
        /// Returns the id of the record just inserted
        /// </summary>
        /// <returns>integer</returns>
        private static int QueryInsert(string sql)
        {
            SQLiteCommand query = Db.CreateCommand();
            query.CommandText = sql + "; SELECT last_insert_rowid() AS RecordID;";
            int newId;
            try
            {
                newId = Convert.ToInt32(query.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Logging.Debug("Exception in QueryInsert: [error] " + ex);
                Logging.Debug("Exception in QueryInsert: [sql] " + sql);
                newId = -1;
            }
            Logging.Debug("QueryInsert: " + newId);
            return newId;
        }

        /// <summary>
        /// Queries the DB for a single row of data
        /// Example. lookup item with id of 1
        /// Returns a DataRow object
        /// </summary>
        /// <returns>DataRow</returns>
        private static DataRow QueryFetchRow(string sql)
        {
            SQLiteCommand query = Db.CreateCommand();
            query.CommandText = sql;

            SQLiteDataAdapter da = new SQLiteDataAdapter(query);
            DataTable dt = new DataTable();
            try
            {
                dt.BeginLoadData();
                da.Fill(dt);
                dt.EndLoadData();
            }
            catch (Exception ex) { Logging.Write("Exception in QueryFetchRow: " + ex); }
            finally { da.Dispose(); }
            return dt.Rows.Cast<DataRow>().FirstOrDefault();

            /*
             * example usage:
            DataRow data = new DataRow();
            row = QueryFetchRow("SELECT * FROM items WHERE id = 1");
            Logging.Write("row: " + row["item_id"].ToString() + " " + row["item_name"].ToString() + "\n");
            */
        }

        /// <summary>
        /// Queries the DB for an Item of the specified name.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataRow GetItem(string id)
        {
            DataRow data = QueryFetchRow(String.Format("SELECT * FROM items WHERE item_id = '{0}'", id));
            return data;
        }

        public static void ClearDatabase()
        {
            Logging.Write("Clearing database");
            Query("DELETE FROM items");
        }

        /// <summary>
        /// Add an item to the DB
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="quality">The quality.</param>
        public static void PutItem(string id, string name, string quality)
        {
            name = name.Replace("'", "''");
            name = name.Replace("\"", "\"\"");
            int itemId = QueryInsert(String.Format("INSERT INTO items (item_id, item_name, item_quality) VALUES ('{0}', '{1}', '{2}')", id, name, quality));
            if (itemId >= 0)
            {
                Logging.Debug("Database: Adding {0} to Database with id = {1}", name, id);
            }
        }
    }
}
