using System;
using EventSourcingTest.Snapshots;
using Xunit;

namespace EventSourcingTest.Tests.Converters
{
    public class StandardTypeSnapshotSerializationTests
    {
        private enum Status
        {
            None,
            Active,
            Inactive
        }

        private class SnapshotModel
        {
            [Persisted]
            private string _title;

            [Persisted]
            private int _count;

            [Persisted]
            private bool _enabled;

            [Persisted]
            private Status _status;

            public string Title => _title;
            public int Count => _count;
            public bool Enabled => _enabled;
            public Status CurrentStatus => _status;

            public SnapshotModel(string title, int count, bool enabled, Status status)
            {
                _title = title;
                _count = count;
                _enabled = enabled;
                _status = status;
            }

            public SnapshotModel() { }
        }

        [Fact]
        public void Should_Serialize_And_Deserialize_StandardTypes_Correctly()
        {
            var original = new SnapshotModel("Test", 42, true, Status.Active);
            var json = SnapshotSerializer.Serialize(original);

            var copy = new SnapshotModel();
            SnapshotSerializer.DeserializeInto(copy, json);

            Assert.Equal("Test", copy.Title);
            Assert.Equal(42, copy.Count);
            Assert.True(copy.Enabled);
            Assert.Equal(Status.Active, copy.CurrentStatus);
        }

        [Fact]
        public void Should_Throw_On_Invalid_Enum()
        {
            var invalidJson = """{ "title": "x", "count": 0, "enabled": true, "status": "InvalidEnum" }""";

            var target = new SnapshotModel();
            Assert.ThrowsAny<Exception>(() =>
            {
                SnapshotSerializer.DeserializeInto(target, invalidJson);
            });
        }
    }
}