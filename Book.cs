namespace GestionBibliotheque.Api;
public class Book
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public DateOnly DatePublic { get; set; }
}