﻿namespace BooksApi.Models
{
    public class BookstoreDatabaseSettings : IBookstoreDatabaseSettings
    {
        public string BooksCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
    /*The preceding BookstoreDatabaseSettings class is used to store the 
     * appsettings.json file's BookstoreDatabaseSettings property values. 
     * The JSON and C# property names are named identically to ease the mapping process.*/

    public interface IBookstoreDatabaseSettings
    {
        string BooksCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}