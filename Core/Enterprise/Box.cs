namespace Core.Enterprise;

public readonly record struct Box<T>
{
    public Box() { IsEmpty = true; }
    public Box(T? content) { Content = content; IsEmpty = false; }

    public readonly T? Content { get; }
    public readonly bool IsEmpty { get; }

    public static implicit operator Box<T>(T? content) => new(content);
    public static implicit operator T?(Box<T> box) => box.Content;
}
