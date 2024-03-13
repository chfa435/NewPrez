using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using NewTiceAI.Data;
using NewTiceAI.Models;

namespace NewTiceAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UploadsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FileUpload>> GetImage(Guid id)
        {
            FileUpload? file = await _context.FileUploads.FirstOrDefaultAsync(i => i.Id == id);

            return file != null ? File(file.Data!, file.Type!) : NotFound();
            //return Ok(file);    
        }
    }
}
