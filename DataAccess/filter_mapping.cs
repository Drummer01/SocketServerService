namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nltu-chat.filter_mapping")]
    public partial class filter_mapping
    {
        public int id { get; set; }

        public int fid { get; set; }

        [Required]
        [StringLength(126)]
        public string original { get; set; }

        [Required]
        [StringLength(126)]
        public string replacement { get; set; }
    }
}
