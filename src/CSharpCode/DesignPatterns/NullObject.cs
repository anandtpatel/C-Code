using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpCode.DesignPatterns.NullObject
{
    public interface IShape
    {
        void Draw();
    }
    public class Square : IShape
    {
        public void Draw()
        {
            Console.WriteLine("Drawing a Square...");
        }
    }
    public class NullShape : IShape
    {
        public void Draw()
        {
            // Do Nothing
        }
    }
    public class ShapeService
    {
        public IShape GetShapeById(int id)
        {
            var square = GetShapeFromDatabase(id);
            return square ?? new NullShape();
        }
        private IShape GetShapeFromDatabase(int id)
        {
            if(id % 2 == 0)
                return new Square();

            return null;
        }
    }
    public class Program
    {
        public static void Main()
        {
            var shapeService = new ShapeService();
            var shape = shapeService.GetShapeById(3);

            shape.Draw(); // No need to check for null
        }
    }
}
