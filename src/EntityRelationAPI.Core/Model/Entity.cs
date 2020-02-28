using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityRelationAPI.Core.Model
{
    public class Entity : AbstractEntity
    {
        public long? ParentId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        public string Code { get; set; }

        public virtual Entity Parent { get; set; }

        public virtual ICollection<Entity> Children { get; set; } = new List<Entity>();
    }
}
