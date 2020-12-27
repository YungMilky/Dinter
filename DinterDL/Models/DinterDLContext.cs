using System;
using System.Collections.Generic;
using System.Text;
using DinterDL.Models;
using Microsoft.EntityFrameworkCore;

namespace DinterDL.Models
{
    public class DinterDLContext : DbContext
    {
        public DinterDLContext(DbContextOptions<DinterDLContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        //public DbSet<Post> Posts { get; set; }
        //public DbSet<Friendship> Friendships { get; set; }
        //public DbSet<Files> Files { get; set; }
    }
}
