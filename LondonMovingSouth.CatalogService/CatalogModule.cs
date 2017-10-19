﻿using System;
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

            Get("/api/products/{count}/{offset}/{fromDate}/{toDate}", async args => await GetCatalogAsync(args));
            
            Get("/api/products/{id}", async args => await GetProductAsync(args));

            Post("/api/products/{id}", async args => await AddProductAsync(args));

            Put("/api/products/{id}", async args => await UpdateProductAsync(args));

            Delete("/api/products/{id}", async args => await DeleteProductAsync(args));
        }

        private async Task<Negotiator> GetCatalogAsync(dynamic args)
        {
            try
            {
                //Service
                IEnumerable<Product> products = await _repository.GetCatalogAsync(args.count, args.offset, args.fromDate, args.toDate);

                if (products.ToList().Count != 0) return Negotiate.WithModel(products).WithStatusCode(HttpStatusCode.OK);

                var errorMessage = new ErrorMessage { Message = "NotFound" };
                return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.NotFound);
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
                Product result = await _repository.GetProductAsync(args.name);

                if (result != null) return Negotiate.WithModel(result).WithStatusCode(HttpStatusCode.OK);

                var errorMessage = new ErrorMessage { Message = "NotFound" };
                return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.NotFound);
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
                Product product = ProductBuilder(args);
                
                var result = await _repository.AddProductAsync(product);

                if (result) return Negotiate.WithModel(product).WithStatusCode(HttpStatusCode.OK);

                var errorMessage = new ErrorMessage { Message = "Error adding item" };
                return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.BadRequest);
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
                var product = ProductBuilder(args);
                
                var result = await _repository.UpdateProductAsync(product);

                if (result)
                {
                    var message = new OkResponse {Message = "OK"};
                    return Negotiate.WithModel(message).WithStatusCode(HttpStatusCode.OK);
                }

                var errorMessage = new ErrorMessage { Message = "NotFound" };
                return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.NotFound);
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
                var result = await _repository.DeleteProductAsync(args.name);

                if (result)
                {
                    var message = new OkResponse {Message = "OK"};
                    return Negotiate.WithModel(message).WithStatusCode(HttpStatusCode.OK);
                }

                var errorMessage = new ErrorMessage { Message = "NotFound" };
                return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                var errorMessage = new ErrorMessage { Message = "InternalServerError" };
                return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.InternalServerError);
            }
        }

        private static Product ProductBuilder(dynamic args)
        {
            var product = new Product
            {
                Id = args.id,
                DetailId = args.detailId,
                Category = args.category,
                Summary = args.summary,
                Price = args.price,
                CreatedDate = DateTime.UtcNow.ToString("dd-MM-yyyyHH:mm:ssZ"),
                ModifiedDate = DateTime.UtcNow.ToString("dd-MM-yyyyHH:mm:ssZ")
            };
            return product;
        }
    }
}
