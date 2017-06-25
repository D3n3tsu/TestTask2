using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using EmpeekTest2.Models;
using System.Threading.Tasks;
using System.Collections;

namespace EmpeekTest2.Infrastructure
{
    public class DBService : IDBService
    {
        string path = "";
        string connectionString = "";
        //this strings are a bit messy to read but prevent errors with table queries
        string tblOwners = "owners";
        string tblPets = "pets";
        string colId = "id";
        string colOwnrName = "owner_name";
        string colPetName = "pet_name";
        string colOwnrId = "owners_id";
        string colNumOfPets = "number_of_pets";

        public DBService()
        {
            //create database file on users desktop
            path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            connectionString = @"Data Source=" + path + @"\testdb.db;Version=3;New=True;";
            try
            {
                using (SQLiteConnection con = new SQLiteConnection(connectionString))
                {
                    con.Open();
                    using (SQLiteTransaction tr = con.BeginTransaction())
                    {
                        using (SQLiteCommand cmnd = con.CreateCommand())
                        {
                            string query = $"CREATE TABLE IF NOT EXISTS {tblOwners} ({colId} INTEGER PRIMARY KEY, {colOwnrName} TEXT UNIQUE);" +
                                $"CREATE TABLE IF NOT EXISTS {tblPets} ({colId} INTEGER PRIMARY KEY, {colPetName} TEXT, {colOwnrId} INTEGER, FOREIGN KEY({colOwnrId}) REFERENCES {tblOwners}({colId}));";
                            cmnd.CommandText = query;
                            cmnd.ExecuteNonQuery();
                        }
                        tr.Commit();
                    }
                    con.Close();
                }
            }
            catch (SQLiteException ex)
            { /*logging*/
              //can't use DB if error while creating
                throw new Exception("Error during DB creation: " + ex.Message);
            }
        }

        public async Task CreateNewOwner(string newOwner)
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();
                using (SQLiteCommand cmnd = con.CreateCommand())
                {
                    string query = $"INSERT INTO {tblOwners} ({colOwnrName}) VALUES ('{newOwner}')";
                    cmnd.CommandText = query;
                    try
                    {
                        await cmnd.ExecuteNonQueryAsync();
                    }
                    catch (SQLiteException ex)
                    { /*logging*/  throw new Exception("Problem in CreateNewOwner method: " + ex.Message); }
                }
                con.Close();
            }
        }

        public async Task CreateNewPet(string petName, int ownerId)
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();
                using (SQLiteCommand cmnd = con.CreateCommand())
                {
                    string query = $"INSERT INTO {tblPets} ({colPetName}, {colOwnrId}) VALUES ('{petName}', {ownerId})";
                    cmnd.CommandText = query;
                    try
                    {
                        await cmnd.ExecuteNonQueryAsync();
                    }
                    catch (SQLiteException ex)
                    { /*logging*/ throw new Exception("Problem in CreateNewPet method: " + ex.Message); }
                }
                con.Close();
            }
        }

        public async Task DeleteOwner(int ownerId)
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();
                using (SQLiteTransaction tr = con.BeginTransaction())
                {
                    using (SQLiteCommand cmnd = con.CreateCommand())
                    {
                        string query = $"DELETE FROM {tblOwners} WHERE {colId} = {ownerId};" +
                            $"DELETE FROM {tblPets} WHERE {colOwnrId} = {ownerId};";
                        cmnd.CommandText = query;
                        try
                        {
                            await cmnd.ExecuteNonQueryAsync();
                        }
                        catch (SQLiteException ex)
                        { /*logging*/  throw new Exception("Problem in DeleteOwner method: " + ex.Message); }
                    }
                    tr.Commit();
                    
                }
                con.Close();
            }
        }

        public async Task DeletePet(int petId)
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();
                using (SQLiteCommand cmnd = con.CreateCommand())
                {
                    string query = $"DELETE FROM {tblPets} WHERE {colId} = {petId}";
                    cmnd.CommandText = query;
                    try
                    {
                        await cmnd.ExecuteNonQueryAsync();
                    }
                    catch (SQLiteException ex)
                    { /*logging*/   throw new Exception("Problem in DeletePet method: " + ex.Message); }
                }
                con.Close();
            }
        }

        public async Task<IEnumerable<Owner>> GetOwners()
        {
            List<Owner> owners = new List<Owner>();
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();
                /*select owners.id, owner_name, count(pets.id) as number_of_pets
                    from owners 
                    join pets on owners.id = owners_id
                    group by owners.id, owner_name*/
                string query = $"SELECT {tblOwners}.{colId}, {colOwnrName}, COUNT({tblPets}.{colId}) AS {colNumOfPets} " +
                    $"FROM {tblOwners} LEFT OUTER JOIN {tblPets} ON {tblOwners}.{colId} = {colOwnrId} " +
                    $"GROUP BY {tblOwners}.{colId}, {colOwnrName}";
                using (SQLiteCommand cmnd = new SQLiteCommand(query,con))
                { 
                    try
                    {
                        SQLiteDataReader dr =(SQLiteDataReader)await cmnd.ExecuteReaderAsync();
                        while (dr.Read())
                        {
                            owners.Add(new Owner
                            {
                                Id = dr.GetInt32(0),
                                Name = dr.GetString(1),
                                NumberOfPets = dr.GetInt32(2)
                            });
                        }
                    }
                    catch (SQLiteException ex)
                    { /*logging*/  throw new Exception("Problem in GetOwners method: " + ex.Message); }

                }
                con.Close();
            }
            return await Task.FromResult(owners);
        }

        public async Task<IEnumerable<Pet>> GetPets(int ownerId)
        {
            List<Pet> pets = new List<Pet>();
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();
                using (SQLiteCommand cmnd = con.CreateCommand())
                {
                    string query = $"SELECT * " +
                        $"FROM {tblPets} WHERE  {colOwnrId} = {ownerId}";
                    cmnd.CommandText = query;

                    try
                    {
                        SQLiteDataReader dr = (SQLiteDataReader)await cmnd.ExecuteReaderAsync();
                        while (dr.Read())
                        {
                            pets.Add(new Pet
                            {
                                Id = int.Parse(dr[colId].ToString()),
                                Name = dr[colPetName].ToString(),
                                OwnerId = int.Parse(dr[colOwnrId].ToString())
                            });
                        }
                    }
                    catch (SQLiteException ex)
                    { /*logging*/ throw new Exception("Problem in GetPets method: " + ex.Message); }

                }
                con.Close();
            }
            return await Task.FromResult(pets);
        }
    }
}