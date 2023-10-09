using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Datos;
using WebApi.Model;
using WebApi.Model.DTO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> logger;
        private readonly AplicationDbContext db;
        private readonly IMapper mapper;
         public ProductController(ILogger<ProductController> _logger, AplicationDbContext _db, IMapper mapp)
        {
            logger = _logger;   
            db = _db;
            mapper = mapp;
        }


        [HttpGet]
        public async Task< ActionResult< IEnumerable<ProductDTO>>> GetProducts()
        {
            logger.LogInformation("Obtener Productos");
            IEnumerable<Product> productList = await db.Products.ToListAsync();
            return  Ok(mapper.Map<IEnumerable<ProductDTO>>(productList));
                
        }
        [HttpGet("id:int",Name ="GetProductId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task< ActionResult< ProductDTO>> GetProduct(int id) 
        { 
            if (id == 0)
            {
                logger.LogError("Error con el producto "+id);
                return BadRequest();
            }
            //var prod = ProductStore.lista.FirstOrDefault(p => p.Id == id);
            var prod = await db.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (prod == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<ProductDTO>(prod));
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task< ActionResult<ProductDTO>> addProduct([FromBody] ProductCreateDTO createproduct)
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            if (await db.Products.FirstOrDefaultAsync(p=> p.Name.ToLower()== createproduct.Name.ToLower())!=null)
            {
                ModelState.AddModelError("NameExists","The name "+ createproduct.Name+ " exists");
                return BadRequest(ModelState);
            }

            if (createproduct == null)
            {
                return BadRequest(createproduct);

            }


            //product.Id= ProductStore.lista.OrderByDescending(p => p.Id).FirstOrDefault().Id +1;
            //Product modelo = new Product()
            //{             
            //    Name = product.Name,
            //    Description = product.Description,
            //    FechaCreacion = DateTime.Now,
            //    Category = product.Category,
            //    Proveedor = product.Proveedor,
            //    Precio = product.Precio,
            //    Stock = product.Stock,
            //    imgUrl = product.imgUrl,
            //    Tipo = ""



            //};

            Product modelo = mapper.Map<Product>(createproduct);
            await db.Products.AddAsync(modelo);
           await  db.SaveChangesAsync();
            //product.Id = db.Products.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
            //ProductStore.lista.Add(product);
            return CreatedAtRoute("GetProductId", new {id=modelo.Id}, modelo);
        }


        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task< IActionResult> DeleteProduct(int id)
        {
            if(id==0)
            {
                return BadRequest();
            }
            //var product = ProductStore.lista.FirstOrDefault(p=>p.Id==id);
            var product= await db.Products.FirstOrDefaultAsync(p=>p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            //ProductStore.lista.Remove(product);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return NoContent();

        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task< IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDTO updateproduct)
        {
            if(updateproduct == null || id!= updateproduct.Id)
            {
                return BadRequest();
            }
            //var prod = ProductStore.lista.FirstOrDefault(p => p.Id == id);
            //prod.Name=product.Name;
            //prod.Description=product.Description;
            //prod.Category=product.Category;
            //Product modelo = new Product()
            //{
            //    Id = product.Id,
            //    Name = product.Name,
            //    Description = product.Description,
            //    FechaCreacion = DateTime.Now,
            //    Category = product.Category,
            //    Proveedor = product.Proveedor,
            //    Precio = product.Precio,
            //    Stock = product.Stock,
            //    imgUrl = product.imgUrl,
            //    Tipo = ""

            //};
            Product modelo= mapper.Map<Product>(updateproduct);
            db.Products.Update(modelo);
            await db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task< IActionResult> UpdateParcialProduct(int id, JsonPatchDocument<ProductUpdateDTO> product)
        {
            if (product == null || id ==0)
            {
                return BadRequest();
            }
            //var prod = ProductStore.lista.FirstOrDefault(p => p.Id == id);
            

            var prod= await db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            ProductUpdateDTO productDTO= mapper.Map<ProductUpdateDTO>(prod);
            //ProductUpdateDTO productDTO = new ProductUpdateDTO()
            //{
            //    Id = prod.Id,
            //    Name = prod.Name,
            //    Description = prod.Description,
            //    Category = prod.Category,
            //    Proveedor = prod.Proveedor,
            //    Precio = prod.Precio,
            //    Stock = prod.Stock,
            //    imgUrl = prod.imgUrl
            //};
            if (prod == null) 
            {
                return BadRequest();
            }
            product.ApplyTo(productDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Product modelo = new Product()
            //{
            //    Id = productDTO.Id,
            //    Name = productDTO.Name,
            //    Description = productDTO.Description,
            //    FechaCreacion = DateTime.Now,
            //    Category = productDTO.Category,
            //    Proveedor = productDTO.Proveedor,
            //    Precio = productDTO.Precio,
            //    Stock = productDTO.Stock,
            //    imgUrl = productDTO.imgUrl,
            //    Tipo = ""

            //};
            Product modelo = mapper.Map<Product>(productDTO);
            db.Products.Update(modelo);
            await db.SaveChangesAsync();
            return NoContent();
        }
    }

}
