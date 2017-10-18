using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Nancy;
using Nancy.Responses.Negotiation;

namespace LondonMovingSouth.CatalogService
{

    public class CatalogModule : NancyModule
    {
        private readonly ICatalogRepository _repository;

        public CatalogModule(ICatalogRepository repository)
        {
            _repository = repository;

            Get["/shopping/{count}/{offset}/{fromDate}/{toDate}"] = args => GetDrinks(args);

            Get["/shopping/{name}"] = args => GetDrink(args);

            Post["/shopping/{name}/{quantity}"] = args => AddDrink(args);

            Put["/shopping/{name}/{quantity}"] = args => UpdateDrink(args);

            Delete["/shopping/{name}"] = args => DeleteDrink(args);
        }

        private Negotiator GetDrinks(dynamic args)
        {
            try
            {
                //Service
                List<Drink> drinkList = _repository.GetDrinksList(args.count, args.offset, args.fromDate, args.toDate);

                if (drinkList.Count == 0)
                {
                    var errorMessage = new ErrorMessage { Message = "NotFound" };
                    return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.NotFound);
                }

                return Negotiate.WithModel(drinkList).WithStatusCode(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                var errorMessage = new ErrorMessage { Message = "InternalServerError" };
                return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.InternalServerError);
            }
        }

        private Negotiator GetDrink(dynamic args)
        {
            try
            {
                var drinkModel = new DrinkModel { Name = args.name, Quantity = args.quantity, DateCreated = DateTime.UtcNow };

                //Model Validation
                var validateRequest = new ValidateRequest(new List<AbstractValidator<DrinkModel>> { new NameValidator() });

                var validationResult = validateRequest.GetResult(drinkModel);

                if (validationResult != "Passed request validations!")
                {
                    var errorMessage = new ErrorMessage { Message = validationResult };
                    return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.BadRequest);
                }

                //Service
                Drink result = _repository.GetDrink(args.name);

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

        private Negotiator AddDrink(dynamic args)
        {
            try
            {
                var drinkModel = new DrinkModel { Name = args.name, Quantity = args.quantity, DateCreated = DateTime.UtcNow };

                //Model Validation
                var validateRequest = new ValidateRequest(new List<AbstractValidator<DrinkModel>> { new NameValidator(), new FormatValidator(), new QuantityValidator() });

                var validationResult = validateRequest.GetResult(drinkModel);

                if (validationResult != "Passed request validations!")
                {
                    var errorMessage = new ErrorMessage { Message = validationResult };
                    return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.BadRequest);
                }

                //Service
                var drink = new Drink { Name = args.name, Quantity = args.quantity, DateCreated = DateTime.Now };
                var result = _repository.AddDrink(drink);

                if (result == false)
                {
                    var errorMessage = new ErrorMessage { Message = "Error adding item" };
                    return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.BadRequest);
                }

                return Negotiate.WithModel(drink).WithStatusCode(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                var errorMessage = new ErrorMessage { Message = "InternalServerError" };
                return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.InternalServerError);
            }
        }

        private Negotiator UpdateDrink(dynamic args)
        {
            try
            {
                //this.RequiresAuthentication();
                var drinkModel = new DrinkModel { Name = args.name, Quantity = args.quantity };

                //Model Validation
                var validateRequest = new ValidateRequest(new List<AbstractValidator<DrinkModel>> { new NameValidator(), new FormatValidator(), new QuantityValidator() });

                var validationResult = validateRequest.GetResult(drinkModel);

                if (validationResult != "Passed request validations!")
                {
                    var errorMessage = new ErrorMessage { Message = validationResult };
                    return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.BadRequest);
                }

                //Service
                var drink = new Drink { Name = args.name, Quantity = args.quantity };
                var result = _repository.UpdateDrink(drink);

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

        private Negotiator DeleteDrink(dynamic args)
        {
            try
            {
                //this.RequiresAuthentication();
                var drinkModel = new DrinkModel { Name = args.name, Quantity = args.quantity };

                //Model Validation
                var validateRequest = new ValidateRequest(new List<AbstractValidator<DrinkModel>> { new NameValidator() });

                var validationResult = validateRequest.GetResult(drinkModel);

                if (validationResult != "Passed request validations!")
                {
                    var errorMessage = new ErrorMessage { Message = validationResult };
                    return Negotiate.WithModel(errorMessage).WithStatusCode(HttpStatusCode.BadRequest);
                }

                //Service
                var result = _repository.DeleteDrink(args.name);

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

    public interface ICatalogRepository
    {
        bool AddDrink(Product drink);
        bool DeleteDrink(string name);
        bool UpdateDrink(Product drink);
        Product GetDrink(string name);
        List<Product> GetDrinksList(string count, string offset, DateTime? fromDate, DateTime? toDate);
    }
}
