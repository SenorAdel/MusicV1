using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicV1
{
    public class Model
    {
        public List <MediaObject> medialist = new List <MediaObject> ();

        public static Model instance;

        public Model()
        { }

        public static Model getModel()
        {
            if (instance == null)
                instance = new Model ();
            return instance;
        }
    }
}
