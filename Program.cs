using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fuse.Scripting.V8.Simple;

namespace Fuse.Babel
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = Fuse.Scripting.V8.Simple.Context.Create(Handle.Free, Handle.Free);
            var babelsrc = new System.IO.StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Fuse.Babel.babel.min.js")).ReadToEnd();
            var pool = new AutoReleasePool();
            Action<JSScriptException> errorHandler = (JSScriptException e) =>
            {
                Console.WriteLine("ERROR");
            };
            context.Evaluate("(babel)", babelsrc, pool, errorHandler);

            var evalFunc = context.Evaluate("(babel)", "(function(code) { return Babel.transform(code, { presets: ['es2015'] }).code })", pool, errorHandler).AsFunction();

            var code = "import foo from 'bar'";

            var res = evalFunc.Call(context, V8SimpleExtensions.Null().AsObject(), new JSValue[] { V8SimpleExtensions.NewString(context, code, pool).AsValue() }, pool, errorHandler).AsString().ToStr(context);

            Console.WriteLine(res);
        }
    }
}
