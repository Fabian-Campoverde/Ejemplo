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
         public ProductController(ILogger<ProductController> _logger, AplicationDbContext _db)
        {
            logger = _logger;   
            db = _db;
        }


        [HttpGet]
        public ActionResult< IEnumerable<ProductDTO>> GetProducts()
        {
            logger.LogInformation("Obtener Productos");
            return  Ok(db.Products.ToList());
                
        }
        [HttpGet("id:int",Name ="GetProductId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult< ProductDTO> GetProduct(int id) 
        { 
            if (id == 0)
            {
                logger.LogError("Error con el producto "+id);
                return BadRequest();
            }
            //var prod = ProductStore.lista.FirstOrDefault(p => p.Id == id);
            var prod = db.Products.FirstOrDefault(x => x.Id == id);

            if (prod == null)
            {
                return NotFound();
            }
            return Ok(prod);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ProductDTO> addProduct([FromBody] ProductDTO product)
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            if (db.Products.FirstOrDefault(p=> p.Name.ToLower()==product.Name.ToLower())!=null)
            {
                ModelState.AddModelError("NameExists","The name "+ product.Name+ " exists");
                return BadRequest(ModelState);
            }

            if (product == null)
            {
                return BadRequest(product);

            }

            if (product.Id > 0)
            {
                StatusCode(StatusCodes.Status500InternalServerError);
            }
            //product.Id= ProductStore.lista.OrderByDescending(p => p.Id).FirstOrDefault().Id +1;
            Product modelo = new Product()
            {             
                Name = product.Name,
                Description = product.Description,
                FechaCreacion = DateTime.Now,
                Category = product.Category,
                Proveedor = product.Proveedor,
                Precio = product.Precio,
                Stock = product.Stock,
                imgUrl = product.imgUrl,
                Tipo = ""



            };
            db.Products.Add(modelo);
            db.SaveChanges();
            //product.Id = db.Products.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
            //ProductStore.lista.Add(product);
            return CreatedAtRoute("GetProductId", new {id=product.Id}, product);
        }


        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteProduct(int id)
        {
            if(id==0)
            {
                return BadRequest();
            }
            //var product = ProductStore.lista.FirstOrDefault(p=>p.Id==id);
            var product= db.Products.FirstOrDefault(p=>p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            //ProductStore.lista.Remove(product);
            db.Products.Remove(product);
            db.SaveChanges();
            return NoContent();

        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateProduct(int id, [FromBody] ProductDTO product)
        {
            if(product == null || id!= product.Id)
            {
                return BadRequest();
            }
            //var prod = ProductStore.lista.FirstOrDefault(p => p.Id == id);
            //prod.Name=product.Name;
            //prod.Description=product.Description;
            //prod.Category=product.Category;
            Product modelo = new Product()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                FechaCreacion = DateTime.Now,
                Category = product.Category,
                Proveedor = product.Proveedor,
                Precio = product.Precio,
                Stock = product.Stock,
                imgUrl = product.imgUrl,
                Tipo = ""

            };
            db.Products.Update(modelo);
            db.SaveChanges();

            return NoContent();
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateParcialProduct(int id, JsonPatchDocument<ProductDTO> product)
        {
            if (product == null || id ==0)
            {
                return BadRequest();
            }
            //var prod = ProductStore.lista.FirstOrDefault(p => p.Id == id);
            

            var prod= db.Products.AsNoTracking().FirstOrDefault(p => p.Id == id);
            ProductDTO productDTO = new ProductDTO()
            {
                Id = prod.Id,
                Name = prod.Name,
                Description = prod.Description,
                Category = prod.Category,
                Proveedor = prod.Proveedor,
                Precio = prod.Precio,
                Stock = prod.Stock,
                imgUrl = prod.imgUrl
            };
            if (prod == null) 
            {
                return BadRequest();
            }
            product.ApplyTo(productDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Product modelo = new Product()
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                Description = productDTO.Description,
                FechaCreacion = DateTime.Now,
                Category = productDTO.Category,
                Proveedor = productDTO.Proveedor,
                Precio = productDTO.Precio,
                Stock = productDTO.Stock,
                imgUrl = productDTO.imgUrl,
                Tipo = ""

            };
            db.Products.Update(modelo);
            db.SaveChanges();
            return NoContent();
        }
    }

}
