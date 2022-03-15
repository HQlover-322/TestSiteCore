using TEST.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace TEST.Services
{
    public class HeroImageService
    {
        private readonly EfDBContex dbContex;
        private readonly IWebHostEnvironment hostingEnvironment;
        public HeroImageService(EfDBContex dbContex, IWebHostEnvironment hostingEnvironment)
        {
            this.dbContex = dbContex;
            this.hostingEnvironment = hostingEnvironment;
        }
        public async Task AddNewImage(IFormFile file,Article article)
        {
            using (dbContex)
            {
                if (file.Length > 0)
                {
                    string uploads = Path.Combine(hostingEnvironment.WebRootPath, "MyResFiles");
                    string filePath = Path.Combine(uploads, file.FileName);
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    if (article.HeroImageId != default)
                    {
                        var item = await dbContex.HeroImage.FindAsync(article.HeroImageId);
                        item.Name = file.FileName;
                        item.Path = "\\MyResFiles\\" + file.FileName;
                    }
                    else
                    {
                        var item = new Data.Entities.HeroImage()
                        {
                            ArticleId = article.Id,
                            Name = file.FileName,
                            Path = "\\MyResFiles\\" + file.FileName,
                        };
                        dbContex.HeroImage.Add(item);                   
                    article.HeroImageId = item.Id;
                    dbContex.Articles.Update(article);
                }
                    await dbContex.SaveChangesAsync();
                }
            }
        }
    }
}
