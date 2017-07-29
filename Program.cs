using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fuse.Scripting.V8.Simple;

namespace Fuse.Babel
{
    public class Transpiler
    {
        JSContext _context;
        AutoReleasePool _pool;
        JSFunction _transform;

        public Transpiler()
        {
            _context = Fuse.Scripting.V8.Simple.Context.Create(Handle.Free, Handle.Free);
            var babelsrc = new System.IO.StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Fuse.Babel.babel.min.js")).ReadToEnd();
            _pool = new AutoReleasePool();
            _context.Evaluate("(babel)", babelsrc, _pool, OnError);
            _transform = _context.Evaluate("(babel)", "(function(code) { return Babel.transform(code, { presets: ['es2015'] }).code })", _pool, OnError).AsFunction();
        }

        void OnError(JSScriptException e)
        {
            Console.WriteLine("ERROR");
        }

        public string Transpile(string code)
        {
            return _transform.Call(_context, V8SimpleExtensions.Null().AsObject(), new JSValue[] { V8SimpleExtensions.NewString(_context, code, _pool).AsValue() }, _pool, OnError).AsString().ToStr(_context);
        }
    }
}
