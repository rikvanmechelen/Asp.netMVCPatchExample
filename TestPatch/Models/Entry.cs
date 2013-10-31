using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace TestPatch.Models
{
    public class Entry : AbstractModel
    {
        [NotPatchable]
        [Key]
        [Required]  
        public int ID { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
    }

    public class EntryDBContext : DbContext
    {
        public DbSet<Entry> Entries { get; set; }
    }
    
}