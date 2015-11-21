namespace Ignite.Entities
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Reflection;

    public class BaseEntity : IBaseEntity
    {
        public BaseEntity()
        {
            SetSerializableProperties(string.Empty);
        }

        /// <summary> Gets or sets the primary key. </summary>
        /// <value> The primary key value. </value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary> Gets or sets the version. </summary>
        /// <value> The version. </value>
        [DefaultValue(1)]
        public int Version { get; set; } = 1;

        /// <summary> Gets or sets the creation date. </summary>
        /// <value> The creation date. </value>       
        public virtual DateTime CreationDate { get; set; } = DateTime.Now;

        /// <summary> Gets or sets the modification date. </summary>
        /// <value> The modification date. </value>
        public virtual DateTime? ModificationDate { get; set; }

        /// <summary> Gets or sets the serializable properties. </summary>
        /// <value> The serializable properties. </value>        
        [NotMapped]
        [JsonIgnore]
        public List<string> serializableProperties { get; set; }

        /// <summary> Sets serializable properties. </summary>
        /// <remarks> Jonathan Ruckert, 12/06/2015. </remarks>
        /// <param name="fields"> The fields. </param>
        public virtual void SetSerializableProperties(string fields)
        {
            if (!string.IsNullOrEmpty(fields))
            {
                string[] returnFields = fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                serializableProperties = returnFields.ToList();
                return;
            }

            serializableProperties = new List<string>();
            serializableProperties.AddRange(GetAllSerializableProperties());
        }

        public virtual List<string> GetAllSerializableProperties()
        {
            var members = this.GetType().GetRuntimeProperties().Where(x => x.Name != "serializableProperties");
            return members.Select(x => x.Name).ToList();
        }        
    }
}
