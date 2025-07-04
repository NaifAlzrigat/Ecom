using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDBContext dbContext;
        private readonly IMapper _mapper;
        private readonly IImageManagementService imageManagementService;
        public ProductRepository(AppDBContext dbContext, IMapper mapper, IImageManagementService imageManagementService) : base(dbContext)
        {
            this.dbContext = dbContext;
            _mapper = mapper;
            this.imageManagementService = imageManagementService;
        }

        public async Task<bool> AddAsync(AddProductDTO productDTO)
        {
            if (productDTO == null) return false;
            var product =_mapper.Map<Product>(productDTO);
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            var imagePath = await imageManagementService.AddImagesAsync(productDTO.Photos,productDTO.Name);

            var photo = imagePath.Select(path => new Photo { ImageName = path, ProductId = product.Id }).ToList();

            await dbContext.Photos.AddRangeAsync(photo);
            await dbContext.SaveChangesAsync();
            return true;

        }

        public async Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO)
        {
            if (updateProductDTO is null)
            {
                return false;
            }

            var findProduct= await dbContext.Products.Include(x=>x.Category)
                .Include(x=>x.Photos).FirstOrDefaultAsync(x=>x.Id==updateProductDTO.id); 

            if(findProduct is null)
            {
                return false;
            }

            _mapper.Map(updateProductDTO, findProduct);

            var findPhotos = await dbContext.Photos.Where(x=>x.ProductId==updateProductDTO.id).ToListAsync();

            foreach (var photo in findPhotos)
            {
                imageManagementService.DeleteImageAsync(photo.ImageName);
            }

            dbContext.Photos.RemoveRange(findPhotos);

            var paths = await imageManagementService.AddImagesAsync(updateProductDTO.Photos, updateProductDTO.Name);

            var listPhotos = paths.Select(x => new Photo { ImageName = x, ProductId = updateProductDTO.id }).ToList();
            await dbContext.Photos.AddRangeAsync(listPhotos);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
