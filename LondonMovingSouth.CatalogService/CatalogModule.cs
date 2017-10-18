using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;

namespace LondonMovingSouth.CatalogService
{

    public class CatalogModule : NancyModule
    {
        private readonly ICatalogRepository _repository;

        public CatalogModule(ICatalogRepository repository)
        {
            _repository = repository;

            Get("/catalog/{count}/{offset}/{fromDate}/{toDate}", async args =>
            {
                try
                {
                    //Service
                    IEnumerable<Product> products = await _repository.GetCatalogAsync(args.count, args.offset, args.fromDate, args.toDate);

                    if (products.ToList().Count == 0)
                    {
                        var errorMessage = new ErrorMessage { Message = "NotFound" };
                        return await Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.NotFound);
                    }

                    return await Negotiate.WithModel(products).WithStatusCode(HttpStatusCode.OK);
                }
                catch (Exception)
                {
                    var errorMessage = new ErrorMessage { Message = "InternalServerError" };
                    return await Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.InternalServerError);
                }
            });
            
            Get("/shopping/{name}", async args => await GetProductAsync(args));

            Post("/shopping/{name}/{quantity}", async args => await AddProductAsync(args));

            Put("/shopping/{name}/{quantity}", async args => await UpdateProductAsync(args));

            Delete("/shopping/{name}", async args => await DeleteProductAsync(args));
        }

        private async Task<Negotiator> GetCatalogAsync(dynamic args)
        {
            try
            {
                //Service
                IEnumerable<Product> products = await _repository.GetCatalogAsync(args.count, args.offset, args.fromDate, args.toDate);

                if (products.ToList().Count == 0)
                {
                    var errorMessage = new ErrorMessage { Message = "NotFound" };
                    return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.NotFound);
                }

                return Negotiate.WithModel(products).WithStatusCode(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                var errorMessage = new ErrorMessage { Message = "InternalServerError" };
                return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.InternalServerError);
            }
        }

        private async Task<Negotiator> GetProductAsync(dynamic args)
        {
            try
            {
                var product = new Product { Name = args.name, Summary = args.summary, DateFormatted = DateTime.UtcNow.ToString("dd-MM-yyyyHH:mm:ssZ"), Price = args.price };
                
                Product result = await _repository.GetProductAsync(args.name);

                if (result == null)
                {
                    var errorMessage = new ErrorMessage { Message = "NotFound" };
                    return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.NotFound);
                }

                return Negotiate.WithModel(result).WithStatusCode(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                var errorMessage = new ErrorMessage { Message = "InternalServerError" };
                return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.InternalServerError);
            }
        }

        private async Task<Negotiator> AddProductAsync(dynamic args)
        {
            try
            {
                var product = new Product { Name = args.name, Summary = args.summary, DateFormatted = DateTime.UtcNow.ToString("dd-MM-yyyyHH:mm:ssZ"), Price = args.price};
                
                var result = await _repository.AddProductAsync(product);

                if (result == false)
                {
                    var errorMessage = new ErrorMessage { Message = "Error adding item" };
                    return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.BadRequest);
                }

                return Negotiate.WithModel(product).WithStatusCode(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                var errorMessage = new ErrorMessage { Message = "InternalServerError" };
                return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.InternalServerError);
            }
        }

        private async Task<Negotiator> UpdateProductAsync(dynamic args)
        {
            try
            {
                var product = new Product { Name = args.name, Summary = args.summary, DateFormatted = DateTime.UtcNow.ToString("dd-MM-yyyyHH:mm:ssZ"), Price = args.price};
                
                var result = await _repository.UpdateProductAsync(product);

                if (result == false)
                {
                    var errorMessage = new ErrorMessage { Message = "NotFound" };
                    return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.NotFound);
                }

                var message = new OkResponse { Message = "OK" };
                return Negotiate.WithModel(message).WithStatusCode(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                var errorMessage = new ErrorMessage { Message = "InternalServerError" };
                return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.InternalServerError);
            }
        }

        private async Task<Negotiator> DeleteProductAsync(dynamic args)
        {
            try
            {
                var product = new Product { Name = args.name, Summary = args.summary, DateFormatted = DateTime.UtcNow.ToString("dd-MM-yyyyHH:mm:ssZ"), Price = args.price };
                
                var result = await _repository.DeleteProductAsync(args.name);

                if (result == false)
                {
                    var errorMessage = new ErrorMessage { Message = "NotFound" };
                    return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.NotFound);
                }

                var message = new OkResponse { Message = "OK" };
                return Negotiate.WithModel(message).WithStatusCode(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                var errorMessage = new ErrorMessage { Message = "InternalServerError" };
                return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.InternalServerError);
            }
        }
    }
}
