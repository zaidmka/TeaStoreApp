using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaStoreApp.Models;

namespace TeaStoreApp.Services
{
    public class BookmarkItemService
    {
        private readonly SQLiteConnection _database;
        public BookmarkItemService()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "entities.db");
            _database = new SQLiteConnection(dbPath);
            _database.CreateTable<BookmarkProduct>();

        }

        public BookmarkProduct Read(int id)
        {
            return _database.Table<BookmarkProduct>()
                .Where(p => p.ProductId == id)
                .FirstOrDefault();
        }

        public List<BookmarkProduct> ReadAll()
        {
            return _database.Table<BookmarkProduct>()
                .ToList();
        }

        public void Create(BookmarkProduct bookmarkProduct)
        {
            _database.Insert(bookmarkProduct);
        }

        public void Delete(BookmarkProduct bookmarkProduct)
        {
            _database.Delete(bookmarkProduct);
        }




    }
}
