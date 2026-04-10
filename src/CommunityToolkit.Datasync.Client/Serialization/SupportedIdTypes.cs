// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace CommunityToolkit.Datasync.Client.Serialization;

internal static class SupportedIdTypes
{
    private static readonly HashSet<Type> Types = new()
    {
        typeof(Guid),
        typeof(int),
        typeof(long),
        typeof(string)
    };

    public static bool Contains(Type type) =>
        Types.Contains(type);

    public static bool Contains(PropertyInfo idProperty) =>
        Contains(idProperty.PropertyType);
}
