using Microsoft.AspNetCore.Mvc;
using SterreWebApi.Models;

namespace SterreWebApi.Repositorys
{
    public class Environment2DRepository
    {
        private static readonly List<Environment2D> _environments = new List<Environment2D>();
        private static int _nextId = 1;

        public IEnumerable<Environment2D> GetAll()
        {
            return _environments;
        }

        public Environment2D? GetById(int id)
        {
            return _environments.FirstOrDefault(environment => environment.Id == id);
        }

        public Environment2D Add(Environment2D environment)
        {
            environment.Id = _nextId++;
            _environments.Add(environment);
            return environment;
        }

        public bool Update(int id, Environment2D updateEnvironment)
        {
            var existing = _environments.FirstOrDefault(environment => environment.Id == id);
            if (existing == null) return false;

            existing.Name = updateEnvironment.Name;
            existing.MaxLength = updateEnvironment.MaxLength;
            existing.MaxHeight = updateEnvironment.MaxHeight;
            return true;
        }

        public bool Delete(int id)
        {
            var environment = _environments.FirstOrDefault(e => e.Id == id);
            if (environment == null) return false;

            _environments.Remove(environment);
            return true;
        }
    }
}
