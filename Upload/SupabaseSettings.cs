using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Backend.Upload
{
    public class SupabaseSettings
    {
        public const string SectionName = "Supabase";
        public string Url { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string Bucket { get; set; } = "image";        
        
    }
}