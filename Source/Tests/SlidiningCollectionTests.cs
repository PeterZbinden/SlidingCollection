using FluentAssertions;
using SlidingCollection;

namespace Tests
{
    public class SlidiningCollectionTests
    {
        [Fact]
        public void Ensure_MaxSize_IsRespected()
        {
            // Arrange
            var maxSize = 3;
            var collection = new SlidingEnumerable<int>(maxSize);

            // Act
            collection.Add(1);
            collection.Add(2);
            collection.Add(3);
            collection.Add(4);

            // Assert
            collection.Length.Should().Be(maxSize);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(100)]
        public void Ensure_OldestValues_AreEvicted(int maxSize)
        {
            // Arrange
            var collection = new SlidingEnumerable<int>(maxSize);

            // Act
            for (int i = 0; i < maxSize + 3; i++)
            {
                collection.Add(i);
            }

            // Assert
            collection.Should().NotContain(0);
            collection.Should().NotContain(1);
            collection.Should().NotContain(2);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(-100)]
        public void Ensure_OnlySizesGreaterThanZero_AreAllowed(int maxSize)
        {
            // Arrange
            var func = () => new SlidingEnumerable<int>(maxSize);

            // Act

            // Assert
            func.Should().Throw<ApplicationException>();
        }
    }
}