using BigBook.Tests.BaseClasses;
using Xunit;

namespace BigBook.Tests
{
    public class GraphTests : TestBaseClass<Graph<int>>
    {
        public GraphTests()
        {
            TestObject = new Graph<int>();
            var Vertex1 = TestObject.AddVertex(1);
            var Vertex2 = TestObject.AddVertex(2);
            var Vertex3 = TestObject.AddVertex(3);
            var Edge1 = TestObject.AddEdge(Vertex1, Vertex2);
            var Edge2 = TestObject.AddEdge(Vertex1, Vertex3);
            var Edge3 = TestObject.AddEdge(Vertex2, Vertex3);
        }

        [Fact]
        public void Creation()
        {
            var TestObject = new Graph<int>();
            var Vertex1 = TestObject.AddVertex(1);
            var Vertex2 = TestObject.AddVertex(2);
            var Vertex3 = TestObject.AddVertex(3);
            var Edge1 = TestObject.AddEdge(Vertex1, Vertex2);
            var Edge2 = TestObject.AddEdge(Vertex1, Vertex3);
            var Edge3 = TestObject.AddEdge(Vertex2, Vertex3);
            Assert.Equal(3, TestObject.Vertices.Count);
            Assert.Equal(Edge1, Vertex1.OutgoingEdges[0]);
            Assert.Equal(Edge2, Vertex1.OutgoingEdges[1]);
            Assert.Equal(Edge3, Vertex2.OutgoingEdges[0]);
        }

        [Fact]
        public void RemoveVertex()
        {
            var TestObject = new Graph<int>();
            var Vertex1 = TestObject.AddVertex(1);
            var Vertex2 = TestObject.AddVertex(2);
            var Vertex3 = TestObject.AddVertex(3);
            var Edge1 = TestObject.AddEdge(Vertex1, Vertex2);
            var Edge2 = TestObject.AddEdge(Vertex1, Vertex3);
            var Edge3 = TestObject.AddEdge(Vertex2, Vertex3);
            TestObject.RemoveVertex(Vertex3);
            Assert.Equal(2, TestObject.Vertices.Count);
            Assert.Equal(Edge1, Vertex1.OutgoingEdges[0]);
        }
    }
}