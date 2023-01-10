using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UPLOADING_FILE.Models;

namespace UPLOADING_FILE.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            
        }
        public DbSet<FileOnDataBaseModel> FileOnDataBaseModel{get;set;}
        public DbSet<FileOnFileSystem> FileOnFileSystem{get;set;}
        

    }
}