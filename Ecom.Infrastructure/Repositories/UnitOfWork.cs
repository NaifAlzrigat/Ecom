using AutoMapper;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;

        private readonly IMapper _mapper;
        private readonly IImageManagementService imageManagementService;

        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IPhotoRepository PhotoRepository { get; }

        public UnitOfWork(AppDBContext context, IMapper mapper, IImageManagementService imageManagementService)
        {
            _context = context;
            _mapper = mapper;
            this.imageManagementService = imageManagementService;

            CategoryRepository = new CategoryRepository(context);
            ProductRepository = new ProductRepository(context,_mapper,imageManagementService);
            PhotoRepository = new PhotoRepository(context);

        }
    }
}
