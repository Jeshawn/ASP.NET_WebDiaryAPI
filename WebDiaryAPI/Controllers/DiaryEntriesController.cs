using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDiaryAPI.Data;
using WebDiaryAPI.Models;

namespace WebDiaryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiaryEntriesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public DiaryEntriesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiaryEntry>>> GetDiaryEntries()
        {
            return await _dbContext.DiaryEntries.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DiaryEntry>> GetDiaryEntry(int id)
        {
            var diaryEntry = await _dbContext.DiaryEntries.FindAsync(id);
            if (diaryEntry == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(diaryEntry);
            }

        }

        [HttpPost]
        public async Task<ActionResult<DiaryEntry>> CreateDiaryEntry(DiaryEntry diaryEntry)
        {
            diaryEntry.Id = 0; //Sets the Id to 0 to ensure database properly increments it
            _dbContext.DiaryEntries.Add(diaryEntry);
            await _dbContext.SaveChangesAsync();

            var resourceURI = Url.Action(nameof(GetDiaryEntry), new { id = diaryEntry.Id });

            return Created(resourceURI, diaryEntry);
        }
    

        [HttpPut]
        public async Task<IActionResult> UpdateDiaryEntry(int id, [FromBody] DiaryEntry diaryEntry)
        {
            if (id != diaryEntry.Id)
            {
                return BadRequest();
            }
                _dbContext.Entry(diaryEntry).State = EntityState.Modified;
                try
                {
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if(!DiaryEntryExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }

                }
            return NoContent();
            

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiaryEntry(int id)
        {
           var diaryEntryToDelete = await _dbContext.DiaryEntries.FindAsync(id);

            if(diaryEntryToDelete == null)
            {
                return NotFound();
            }

            try
            {
                _dbContext.Remove(diaryEntryToDelete);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return NotFound();
            }
            
            return NoContent();
        }

        private bool DiaryEntryExists(int id)
        {
            return _dbContext.DiaryEntries.Any(e => e.Id == id);
        }
    }
}
