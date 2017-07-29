using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuse.Babel
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = Fuse.Scripting.V8.Simple.Context.Create(Handle.Free, Handle.Free);

            var result = context.Evaluate("lol", "2+2", new AutoReleasePool(), x => Console.WriteLine("ERROR"));
            Console.WriteLine(result.AsInt());
        }
    }
}
