using API.Data;
using DomainModels.Dto;
using DomainModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
	private readonly AppDbContext _dbContext;
	public NotesController(AppDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	[HttpGet]
	public async Task<ActionResult<List<DeploymentNoteDto>>> GetNotes()
	{
		var notes = await _dbContext.DeploymentNotes
			.OrderByDescending(note => note.CreatedAt)
			.Select(note => new DeploymentNoteDto
			{
				Id = note.Id,
				Title = note.Title,
				Content = note.Content,
				Category = note.Category,
				CreatedAt = note.CreatedAt
			})
			.ToListAsync();
		return Ok(notes);
	}

	[HttpGet("{id:int}")]
	public async Task<ActionResult<DeploymentNoteDto>> GetNoteById(int id)
	{
		var note = await _dbContext.DeploymentNotes
			.Where(note => note.Id == id)
			.Select(note => new DeploymentNoteDto
			{
				Id = note.Id,
				Title = note.Title,
				Content = note.Content,
				Category = note.Category,
				CreatedAt = note.CreatedAt
			})
			.FirstOrDefaultAsync();

		if (note is null)
		{
			return NotFound();
		}

		return Ok(note);
	}

	[HttpPost]
	public async Task<ActionResult<DeploymentNoteDto>> CreateNote(CreateDeploymentNoteDto createDto)
	{
		var note = new DeploymentNoteVm
		{
			Title = createDto.Title,
			Content = createDto.Content,
			Category = createDto.Category,
			CreatedAt = DateTime.UtcNow
		};

		_dbContext.DeploymentNotes.Add(note);
		await _dbContext.SaveChangesAsync();

		var result = new DeploymentNoteDto
		{
			Id = note.Id,
			Title = note.Title,
			Content = note.Content,
			Category = note.Category,
			CreatedAt = note.CreatedAt
		};

		return CreatedAtAction(nameof(GetNoteById), new { id = note.Id }, result);
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> DeleteNote(int id)
	{
		var note = await _dbContext.DeploymentNotes.FindAsync(id);
		if (note is null)
		{
			return NotFound();
		}
		_dbContext.DeploymentNotes.Remove(note);
		await _dbContext.SaveChangesAsync();
		return NoContent();
	}
}
