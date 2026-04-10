// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;

namespace CommunityToolkit.Datasync.Client.Serialization;

internal static class KeyConverter
{
    private static readonly ConcurrentDictionary<Type, Func<string, object[]>> ConverterCache = new();

    private static readonly Dictionary<Type, Func<string, object[]>> KeyConverters = new()
    {
        { typeof(Guid), GetGuidKey },
        { typeof(int), GetIntKey },
        { typeof(long), GetLongKey },
        { typeof(string), GetStringKey }
    };

    /// <summary>
    /// Creates a new <see cref="object"/> <see cref="Array"/> containing syncId parsed to the entity's Id type.
    /// </summary>
    /// <param name="entityType">The type of entity whose Id type is used to parse.</param>
    /// <param name="syncId">The value to parse.</param>
    /// <returns></returns>
    public static object[] GetKey(Type entityType, string? syncId)
    {
        try
        {
            return GetConverter(entityType).Invoke(syncId!);
        }
        catch (ArgumentNullException ae)
        {
            throw new DatasyncException($"Null is an invalid value for {entityType.FullName}.Id.", ae);
        }
        catch (FormatException fe)
        {
            throw new DatasyncException($"'{syncId}' is an invalid value for {entityType.FullName}.Id.", fe);
        }
    }

    private static Func<string, object[]> GetConverter(Type entityType) =>
        ConverterCache.GetOrAdd(entityType, BuildConverter);

    private static Func<string, object[]> BuildConverter(Type entityType)
    {
        Type idType = EntityResolver.GetEntityPropertyInfo(entityType).IdPropertyInfo.PropertyType;

        if (!KeyConverters.TryGetValue(idType, out Func<string, object[]>? converter))
        {
            throw new DatasyncException($"{idType.FullName} is not a supported Id type.");
        }

        return converter;
    }

    private static object[] GetGuidKey(string syncId) =>
        [Guid.Parse(syncId)];

    private static object[] GetIntKey(string syncId) =>
        [int.Parse(syncId)];

    private static object[] GetLongKey(string syncId) =>
        [long.Parse(syncId)];

    private static object[] GetStringKey(string syncId) =>
        [syncId];
}
