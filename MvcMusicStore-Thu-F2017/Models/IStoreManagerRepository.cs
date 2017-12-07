using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcMusicStore_Thu_F2017.Models
{
    public interface IStoreManagerRepository
    {
        // used for Unit Testing with Mock Store Manager Album data
        IQueryable<Album> Albums { get;  }
        Album Save(Album album);
        void Delete(Album album);
    }
}
