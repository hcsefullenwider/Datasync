// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Datasync.Client.Serialization;

namespace CommunityToolkit.Datasync.Client.Test.Serialization;

[ExcludeFromCodeCoverage]
public class KeyConverter_Tests
{
    [Theory]
    [InlineData(typeof(TypeGuid), "55199C61-740C-44F1-8693-153B10A2A632")]
    [InlineData(typeof(TypeInt), "84531")]
    [InlineData(typeof(TypeLong), "6854178435151")]
    [InlineData(typeof(TypeString), "keykeykey")]
    public void SupportedTypes(Type type, string id)
    {
        object[] key = KeyConverter.GetKey(type, id);
        key.Should().HaveCount(1);
    }

    [Theory]
    [InlineData(typeof(TypeDouble), "2.154")]
    public void UnsupportedTypes(Type type, string id)
    {
        Action act = () => _ = KeyConverter.GetKey(type, id);
        act.Should().Throw<DatasyncException>();
    }

    [Theory]
    [InlineData(typeof(TypeGuid), "")]
    [InlineData(typeof(TypeInt), "")]
    [InlineData(typeof(TypeLong), "")]
    public void EmptyStrings(Type type, string id)
    {
        Action act = () => _ = KeyConverter.GetKey(type, id);
        act.Should().Throw<DatasyncException>();
    }

    [Theory]
    [InlineData(typeof(TypeGuid), null)]
    [InlineData(typeof(TypeInt), null)]
    [InlineData(typeof(TypeLong), null)]
    public void NullStrings(Type type, string id)
    {
        Action act = () => _ = KeyConverter.GetKey(type, id);
        act.Should().Throw<DatasyncException>();
    }

    [Theory]
    [InlineData(typeof(TypeGuid), "keykeykey")]
    [InlineData(typeof(TypeInt), "keykeykey")]
    [InlineData(typeof(TypeLong), "keykeykey")]
    public void IncompatibleTypes(Type type, string id)
    {
        Action act = () => _ = KeyConverter.GetKey(type, id);
        act.Should().Throw<DatasyncException>();
    }

    class TypeDouble
    {
        public double Id { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? Version { get; set; }
        public bool Deleted { get; set; }
    }

    class TypeGuid
    {
        public Guid Id { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? Version { get; set; }
        public bool Deleted { get; set; }
    }

    class TypeInt
    {
        public int Id { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? Version { get; set; }
        public bool Deleted { get; set; }
    }

    class TypeLong
    {
        public long Id { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? Version { get; set; }
        public bool Deleted { get; set; }
    }

    class TypeString
    {
        public string Id { get; set; } = string.Empty;
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? Version { get; set; }
        public bool Deleted { get; set; }
    }
}
