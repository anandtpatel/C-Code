using CSharpCode.DesignPatterns.NullObject;
using CSharpCode.DesignPatterns.PipeFilter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var shapeService = new ShapeService();
            var shape = shapeService.GetShapeById(3);

            shape.Draw(); // No need to check for null
        }
    }
}
