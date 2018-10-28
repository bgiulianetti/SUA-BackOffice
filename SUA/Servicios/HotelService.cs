using SUA.Models;
using SUA.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Servicios
{
    public class HotelService
    {
        public ESRepositorio Repository { get; set; }

        public HotelService()
        {
            var node = new UriBuilder("localhost");
            node.Port = 9200;
            var settings = new ESSettings(node);
            Repository = new ESRepositorio(settings, ESRepositorio.ContentType.hotel.ToString());
        }

        public List<Hotel> GetHoteles()
        {
            return Repository.GetHoteles();
        }
        public Hotel GetHotelById(string id)
        {
            return Repository.GetHotelById(id);
        }
        public Hotel GetHotelByNombre(string nombre)
        {
            return Repository.GetHotelByNombre(nombre);
        }
        public void AddHotel(Hotel hotel)
        {
            Repository.AddHotel(hotel);
        }
        public void AddBulkHotel(List<Hotel> hoteles)
        {
            Repository.AddBulkHotel(hoteles);
        }
        public void UpdateHotel(Hotel hotel)
        {
            Repository.UpdateHotel(hotel);
        }
        public string GetHotelInnerIdById(string id)
        {
            return Repository.GetHotelInnerIdById(id);
        }
        public void DeleteHotel(string id)
        {
            Repository.DeleteHotel(id);
        }
    }
}