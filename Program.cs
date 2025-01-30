using GestionBibliotheque.Api;
using GestionBibliotheque.Api.DTOs;
//using Microsoft.AspNetCore.Builder;
// using AutoMapper;
// using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var books = new List<BookDto>
{
    new BookDto {
        Id = 1,
        Title = "Champ d'Ombre",
        Author = "Senghor",
        DatePublic = new DateOnly(1960,10,23)
    },
    new BookDto {
        Id = 2,
        Title = "Contemplations",
        Author = "Hugo",
        DatePublic = new DateOnly(1856,05,14)
    }
};


app.MapGet("/", () => "Hello World!");

//endpoint pour recuperer la liste des livres
app.MapGet("/books",() => books);
//endpoint permettant de recuperer un book par son ID 
app.MapGet("/books/{id}",(int id )=>{
    var book =  books.FirstOrDefault(book=>book.Id == id);
    if ( book is null) {
        return Results.NotFound(new {Message = $"Le book avec l'ID {id} n'existe pas" });
    } 
    return Results.Ok(book);

});
app.MapPost("/books",(BookDto newBook) => {
    int newId = books.Any() ? books.Max(book=>book.Id) + 1 : 1;
    var book = new BookDto {
        Id = newId,
        Title = newBook.Title,
        Author = newBook.Author,
        DatePublic =newBook.DatePublic
    };
    books.Add(book);
    return Results.Created($"/books/{book.Id}", book);
});

app.MapPut("/books/{id}",(int id,UpdateBookDto newBook) => {
    var book = books.FirstOrDefault(b=>b.Id == id);
     if ( book is null ) {
       return Results.NotFound();
     }
     book.Title = newBook.Title ?? book.Title;
     book.Author = newBook.Author ?? book.Author;
     book.DatePublic =newBook.DatePublic !=default ? newBook.DatePublic : book.DatePublic;
     //return Results.NoContent();
     return Results.Ok(new {
     Message = $"Book mis a jour avec succes",
     UpdateBookDto = book
      });
});

app.MapDelete("/books/{id}",(int id) => {
var book = books.FirstOrDefault(b=>b.Id == id);
if ( book is null ) {
    return Results.NotFound();
}
   books.Remove(book);
   return Results.Ok("Livre supprimée avec succes");
});
app.Run();


