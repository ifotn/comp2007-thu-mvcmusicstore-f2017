using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// added references
using System.Web;
using System.Data.Entity;

namespace MvcMusicStore_Thu_F2017.Models
{
    public class EFStoreManagerRepository : IStoreManagerRepository
    {
        // db connection
        MusicStoreModel db = new MusicStoreModel();
        
        public IQueryable<Album> Albums {  get { return db.Albums;  } }
        public IQueryable<Artist> Artists { get { return db.Artists; } }
        public IQueryable<Genre> Genres { get { return db.Genres; } }
        public void Delete(Album album)
        {
            db.Albums.Remove(album);
            db.SaveChanges();
        }

        public Album Save(Album album)
        {
            if (album.AlbumId == 0)
            {
                db.Albums.Add(album);
            }
            else
            {
                db.Entry(album).State = EntityState.Modified;
            }
            db.SaveChanges();

            return album;
        }
    }
}
