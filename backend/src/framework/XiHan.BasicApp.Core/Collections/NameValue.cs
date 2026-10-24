// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace XiHan.BasicApp.Core;

/// <summary>
/// 名称/字符串值对（承接 4.x）
/// </summary>
public class NameValue : NameValue<string>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public NameValue()
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="value">值</param>
    public NameValue(string name, string value)
    {
        Name = name;
        Value = value;
    }
}

/// <summary>
/// 名称/值对（承接 4.x）
/// </summary>
/// <typeparam name="T">值类型</typeparam>
public class NameValue<T>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public NameValue()
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="value">值</param>
    public NameValue(string name, T value)
    {
        Name = name;
        Value = value;
    }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 值
    /// </summary>
    public T Value { get; set; } = default!;
}
