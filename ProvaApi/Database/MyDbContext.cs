using Microsoft.EntityFrameworkCore;

namespace ProvaApi.Database
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }  // Tabella utenti nel database (tabella Users)  

        public DbSet<UserModel> UserModels { get; set; }  // Tabella utenti nel database (tabella Users)

    }
}
