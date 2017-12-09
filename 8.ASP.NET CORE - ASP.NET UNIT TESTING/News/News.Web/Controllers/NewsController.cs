using Microsoft.AspNetCore.Mvc;
using News.Data;
using News.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Web.Controllers
{
    [Route("api/[controller]")]
    public class NewsController : Controller
    {
        private readonly NewsDbContext db;
        public NewsController(NewsDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult GetAllNews()
        {
            return Ok(this.db.News);
        }

        [HttpGet("{id}")]
        public IActionResult GetSingleNew(int id)
        {
            var news = this.db.News.FirstOrDefault(n => n.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            return Ok(news);
        }

        [HttpPost]
        public IActionResult Post([FromBody] NewsEnt model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var news = new NewsEnt
            {
                Title = model.Title,
                Content = model.Content,
                PublishDate = model.PublishDate
            };
            
            this.db.News.Add(news);
            this.db.SaveChanges();

            return CreatedAtAction(nameof(GetSingleNew), new { id = news.Id }, news);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]NewsEnt model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            var oldNews = this.db.News.Find(id);

            if (oldNews == null)
            {
                return BadRequest();
            }

            oldNews.Title = model.Title;
            oldNews.Content = model.Content;
            oldNews.PublishDate = model.PublishDate;

            this.db.News.Update(oldNews);
            this.db.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            var newsToDelete = this.db.News.Find(id);

            if (newsToDelete == null)
            {
                return BadRequest();
            }

            this.db.Remove(newsToDelete);
            this.db.SaveChanges();

            return Ok();
        }

    }
}
