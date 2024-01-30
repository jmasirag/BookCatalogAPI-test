using AutoMapper;
using BookCatalogApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.Interfaces;
using Repository.Models;
using System.Collections.Generic;

namespace BookCatalogApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public BooksController(IBookRepository bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }


    [HttpGet]
    public async Task<IEnumerable<BookApi>> GetBooksPagedAsync(int pageNumber, int pageSize )
    {
        var books = await _bookRepository.GetBooksPagedAsync(pageNumber, pageSize);

        return _mapper.Map<IEnumerable<BookApi>>(books);
    }


    [HttpGet("{id}", Name = "GetBookById")]
    public ActionResult GetBookById(int id)
    {
        var book =  _bookRepository.GetBookByIdAsync(id).Result;

        if (book == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<BookApi>(book));
    }


    [HttpPost]
    public async Task<IActionResult> AddBookAsync([FromBody] BookApi book)
    {
        var newbook = await _bookRepository.AddBookAsync(_mapper.Map<Book>(book));
        return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBookAsync(int id, [FromBody] BookApi book)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        book.Id = id;

        await _bookRepository.UpdateBookAsync(_mapper.Map<Book>(book));

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBookAsync(int id)
    {
        var existingBook = await _bookRepository.GetBookByIdAsync(id);

        if (existingBook == null)
        {
            return NotFound();
        }

        await _bookRepository.DeleteBookAsync(id);

        return NoContent();
    }

}