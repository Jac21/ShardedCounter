using NUnit.Framework;
using Shouldly;

namespace ShardedCounter.Core.Tests
{
    /// <summary>
    /// ShardedCounter structure basic operations testing
    /// </summary>
    [TestFixture]
    public class ShardedCounterTest
    {
        [Test]
        public void ShardedCounterIncreaseByOne()
        {
            // arrange
            ShardedCounter shardedCounter = new ShardedCounter();

            // act
            shardedCounter.Increase(1L);

            // assert
            shardedCounter.Count.ShouldBe(1L, "ShardedCounter did not increase by one.");
        }

        [Test]
        public void ShardedCounterIncreaseByOneHundredMillion()
        {
            // arrange
            const long increasedCounterValue = 4999999950000000L;
            ShardedCounter shardedCounter = new ShardedCounter();

            // act
            for (int i = 0; i < 100000000; i++)
            {
                shardedCounter.Increase(i);
            }

            // assert
            shardedCounter.Count.ShouldBe(increasedCounterValue, "ShardedCounter did not increase one hundred million times.");
        }

        [Test]
        public void ShardedCounterDecreaseByOne()
        {
            // arrange
            ShardedCounter shardedCounter = new ShardedCounter();

            // act
            shardedCounter.Decrease(-1L);

            // assert
            shardedCounter.Count.ShouldBe(-1L, "ShardedCounter did not decrease by one.");
        }

        [Test]
        public void ShardedCounterDecreaseByOneHundredMillion()
        {
            // arrange
            const long increasedCounterValue = 4999999950000000L;
            ShardedCounter shardedCounter = new ShardedCounter();

            // act
            shardedCounter.Increase(increasedCounterValue);

            for (int i = 0; i < 100000000; i++)
            {
                shardedCounter.Decrease(i);
            }

            // assert
            shardedCounter.Count.ShouldBe(0L, "ShardedCounter did not decrease one hundred million times.");
        }
    }
}