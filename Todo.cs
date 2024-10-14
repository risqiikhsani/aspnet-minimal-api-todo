// public class Todo
// {
//     public int Id { get; set; }
//     public string? Name { get; set; }
//     public bool IsComplete { get; set; }
// }

// A DTO can be used to:

//     Prevent over-posting.
//     Hide properties that clients aren't supposed to view.
//     Omit some properties to reduce payload size.
//     Flatten object graphs that contain nested objects. Flattened object graphs can be more convenient for clients.

// To demonstrate the DTO approach, update the Todo class to include a secret field:

public class Todo
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    public string? Secret { get; set; }
}   

// The secret field needs to be hidden from this app, but an administrative app could choose to expose it.

// Verify you can post and get the secret field.