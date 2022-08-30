using AutoMapper;
using Mango.Services.ProductAPI.DbContexts;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;

        public ProductRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ProductDto> CreateUpdateProductAsync(ProductDto productDto)
        {
            Product product = _mapper.Map<ProductDto, Product>(productDto);
            if (product.Id > 0)
            {
                _db.Products.Update(product);
            }
            else
            {
                _db.Products.Add(product);
            }

            await _db.SaveChangesAsync();

            return _mapper.Map<Product, ProductDto>(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                Product product = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
                
                if (product == null)
                    return false;
                
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            Product product = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            List<Product> productList = await _db.Products.ToListAsync();

            return _mapper.Map<List<ProductDto>>(productList);
        }
    }
}
