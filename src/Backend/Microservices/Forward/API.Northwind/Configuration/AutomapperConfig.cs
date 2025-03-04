using AutoMapper;
using Common.Domain.Logic.Model;
using Common.UnitOfWork.Collections;
using Elfie.Serialization;
using Northwind.Data.Northwind.Entity;

namespace API.Northwind.Configuration
{
    public class AutomapperConfig : Profile
    {

        public AutomapperConfig()
        {

            CreateMap<CategoriesDto, Categories>().ReverseMap();
            CreateMap<PagedList<CategoriesDto>, PagedList<Categories>>().ReverseMap();

            CreateMap<EmployeesDto, Employees>().ReverseMap();
            CreateMap<PagedList<EmployeesDto>, PagedList<Employees>>().ReverseMap();
        }

    }
}
