using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project.Entity.LogModel
{
    public class LogDetails
    {
        public Object? Model { get; set; }
        public Object? Controller { get; set; }
        public Object? Action { get; set; }
        public Object? Id { get; set; }
        public Object? Created {  get; set; }
        public LogDetails()
        {
            Created = DateTime.UtcNow;
        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
