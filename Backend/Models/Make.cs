using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace vega.Model
{
    public class Make
    {
        public int Id { get; set; }

        public string Name { get; set; }   

        public ICollection<Model> Model { get; set; } 

        public Make()
        {
            Model = new Collection<Model>();
        }
    }
}